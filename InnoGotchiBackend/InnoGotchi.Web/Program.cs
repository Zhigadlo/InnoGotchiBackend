using AutoMapper;
using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Interfaces;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InnoGotchiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
builder.Services.AddTransient<IMapper>(x => new Mapper(config));
//builder.Services.AddTransient<IUnitOfWork, InnoGotchiUnitOfWork>();
builder.Services.AddTransient<InnoGotchiUnitOfWork>();
builder.Services.AddTransient<FarmService>();
builder.Services.AddTransient<PetService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RequestService>();
builder.Services.AddTransient<PictureService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
