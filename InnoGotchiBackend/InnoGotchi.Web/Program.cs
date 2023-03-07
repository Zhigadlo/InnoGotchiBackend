using AutoMapper;
using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Models;
using InnoGotchi.BLL.Services;
using InnoGotchi.Web.Extensions;
using InnoGotchi.Web.Mapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<TokenSettings>(options => builder.Configuration.GetSection(nameof(TokenSettings)).Bind(options));
builder.Services.AddDbContext<InnoGotchiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtTokenAuthentication(builder.Configuration);

var config = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile> { new MapperProfile(), new ViewModelProfile() }));
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
