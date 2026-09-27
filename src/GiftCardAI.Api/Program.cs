using GiftCardAI.Application.Image;
using GiftCardAI.Infrastructure.Image;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins(builder.Configuration["UI:URL"]!);
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddHttpClient(
    builder.Configuration["AI:ImageHttp"]!,
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["AI:ImageAPI"]!);
        client.Timeout = TimeSpan.FromMinutes(5); // Larger timeouts for image gen
    });

// SERVICES
builder.Services.AddTransient<IImageGenService, PythonImageGenService>();

var app = builder.Build();

app.UseCors("ReactClient");
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();