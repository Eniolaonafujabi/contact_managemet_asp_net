using contactManagement.data.model;
using contactManagement.data.repo;
using contactManagement.services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<MongoClient>(sp => new MongoClient("mongodb://localhost:27017"));

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IContactInterface, ContactImplementation>();
builder.Services.AddScoped<IContactRepo, ContactRepo>();
builder.Services.AddScoped<User>();

builder.Services.AddScoped<Contact>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();