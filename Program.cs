using Cart.Api.Application.Services;
using Cart.Api.EntityFramework;
using Cart.Api.EventHandlers;
using Cart.Api.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.SetKebabCaseEndpointNameFormatter(); //order-created-event

    busConfig.AddConsumer<OrderCreatedEventHandler>();
    
    busConfig.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });
        
        config.ReceiveEndpoint("order-created-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedEventHandler>(context);
        });

        
        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddTransient<IPdfService, PdfService>();

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // This sets Swagger as the default route
    });
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();
