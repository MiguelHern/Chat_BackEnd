using Core.Domain.Entities;
using Core.Domain.Services;
using Core.Features.Usuario.Command;
using Core.Infraestructure;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Presentation;
using Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddMediatR(typeof(Usuario).Assembly);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddPresentationServices(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

/* builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", builder =>
    {
        builder.WithOrigins("https://nxmessage.netlify.app") // Reemplaza con el origen de tu aplicación Vue.js
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Permitir credenciales
    });
});*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

//Database
const string connectionName = "ConexionMaestra";
var connectionString = builder.Configuration.GetConnectionString(connectionName);
builder.Services.AddDbContext<ChatContext>(options => options.UseMySQL(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); 
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/message").RequireCors("SignalRPolicy");
    endpoints.MapControllers();
});*/
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/message").RequireCors("AllowAllOrigins");
    endpoints.MapControllers();
});

app.Run();
