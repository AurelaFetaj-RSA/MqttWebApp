using MqttWebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<MqttService>();
builder.Services.AddSignalR();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5000") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Serve static files
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Map SignalR hub
app.MapHub<MqttHub>("/mqttHub");

// Default route
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("MQTT Web App is running...");
});

// Run the application
app.Run();
