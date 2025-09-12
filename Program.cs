using HelloBlazor.Components;
using HelloBlazor.Data;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") 
                            ?? "mongodb://localhost:27017";
var mongoDatabaseName = builder.Configuration["MongoDatabase"] ?? "HelloBlazorDb";

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConnectionString));

// Register database (optional if you want direct access elsewhere)
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

builder.Services.AddScoped<EmailService>();

// Register TodoService
builder.Services.AddSingleton<TodoService>(sp =>
    new TodoService(mongoConnectionString, mongoDatabaseName));

// Add Razor Components (Blazor)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Map Blazor App
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();