using AutoMapper;
using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InnoGotchiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authOptions = new AuthOptions(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
builder.Services.AddTransient<IMapper>(x => new Mapper(config));
builder.Services.AddTransient<InnoGotchiUnitOfWork>();
builder.Services.AddTransient<FarmService>();
builder.Services.AddTransient<PetService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RequestService>();
builder.Services.AddTransient<PictureService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
