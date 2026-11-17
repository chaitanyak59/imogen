using Amazon;
using Amazon.BedrockRuntime;
using ChayGiftCardGen.Infrastructure.ImageGeneration.Bedrock;
using GiftCardAI.Application.Image;
using GiftCardAI.Application.Image.Validator;
using GiftCardAI.Infrastructure.Image;
using GiftCardAI.Infrastructure.Image.Bedrock;

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
        policy.SetPreflightMaxAge(TimeSpan.FromHours(1));
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
// builder.Services.AddSingleton<IAmazonBedrockRuntime>(_ => new AmazonBedrockRuntimeClient(RegionEndpoint.GetBySystemName("us-west-2")));
builder.Services.AddKeyedTransient<IImageGenService, PythonImageGenService>("Python");
builder.Services.AddKeyedTransient<IImageGenService, BedrockImageGenerationService>("AWS");
builder.Services.AddTransient<IImageGenFactory, ImageGenFactory>();

builder.Services.AddHttpClient<BedrockImageClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var region = configuration["AWS:Region"];
    client.BaseAddress = new Uri($"https://bedrock-runtime.{region}.amazonaws.com/");
    client.Timeout = TimeSpan.FromMinutes(3);
});

builder.Services.AddSingleton<GenImageValidator>();
builder.Services.AddSingleton<GenImagePromptBuilder>();

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