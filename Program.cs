using HelloBlazor.Components;
using HelloBlazor.Data;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// --- MongoDB Configuration ---
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") 
                            ?? "mongodb://localhost:27017";
var mongoDatabaseName = builder.Configuration["MongoDatabase"] ?? "HelloBlazorDb";

// Register MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConnectionString));

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

// Register your service that works with MongoDB
builder.Services.AddSingleton(new TodoService(
    mongoConnectionString, // MongoDB connection string
    mongoDatabaseName      // Database name
));

// --- Add Razor Components (Blazor) ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// --- Configure middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// --- Map Blazor App ---
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();