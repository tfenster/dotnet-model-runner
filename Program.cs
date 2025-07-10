using System.ClientModel;
using dotnet_model_runner.Components;
using OpenAI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton(sp =>
{
    var baseUrl = Environment.GetEnvironmentVariable("SMOLLM2_URL")
        ?? throw new InvalidOperationException("SMOLLM2_URL environment variable is not set.");
    var endpoint = baseUrl.Replace("/v1", "/llama.cpp/v1");
    var options = new OpenAIClientOptions
    {
        Endpoint = new Uri(endpoint),
    };
    return new OpenAIClient(new ApiKeyCredential("unused"), options);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
