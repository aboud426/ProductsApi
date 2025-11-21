using ProductsApi.Repositories;
using ProductsApi.Services;
using FluentValidation;
using ProductsApi.DTOs;
using ProductsApi.Middlewares;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<CreateProductDTO>, CreateProductDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDTO>, UpdateProductDTOValidator>();

// Register data store as singleton so in-memory store is preserved during app lifetime
builder.Services.AddSingleton<IDataStore, InMemoryDataStore>();

// Register UnitOfWork as scoped (one per request)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register service
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseAuthorization();

app.MapControllers();

app.Run();
