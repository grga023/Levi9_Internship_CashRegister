using Microsoft.OpenApi.Models;
using CashRegister.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Infrastructure.Repositories;
using FluentAssertions.Common;
using CashRegister.Application.Interfaces;
using CashRegister.Application.Services;
using System.Text.Json.Serialization;
using CashRegister.Infrastructure.Intrface;
using CashRegister.Infrastructure.Interfaces;
using CashRegister.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CashRegister.API", Version = "v1" });
});

builder.Services.AddDbContext<CashRegisterDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CashRegisterDBConnection")
    ).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));


builder.Services.AddTransient<IBillRepository, BillRepository>();
builder.Services.AddTransient<IBillService, BillService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<IProductBillService, ProductBillService>();
builder.Services.AddTransient<IProductBillRepository<ProductBill>, ProductBillRepository>();
builder.Services.AddTransient<IExchangeService, ExchangeService>();
builder.Services.AddTransient<IExchangeRepository, ExchangeRepository>();


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
