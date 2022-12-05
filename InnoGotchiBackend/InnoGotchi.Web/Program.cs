using AutoMapper;
using InnnoGotchi.DAL.EF;
using InnnoGotchi.DAL.Interfaces;
using InnnoGotchi.DAL.Respositories;
using InnoGotchi.BLL.Mapper;
using InnoGotchi.BLL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InnoGotchiContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = new MapperConfiguration(cnf => cnf.AddProfile<MapperProfile>());
builder.Services.AddTransient<IMapper>(x => new Mapper(config));
builder.Services.AddTransient<IUnitOfWork, InnoGotchiUnitOfWork>();
builder.Services.AddTransient<FarmService>();
builder.Services.AddTransient<PetService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
