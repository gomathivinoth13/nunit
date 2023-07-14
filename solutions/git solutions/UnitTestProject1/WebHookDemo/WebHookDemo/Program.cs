using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/webhook", async context =>
{
    var requestBody = await context.Request.ReadFromJsonAsync<WebhookPayload>();
    Console.WriteLine($"Header:{requestBody?.Header},Body:{requestBody?.Body}");
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Webhook Impl");
});

app.Run();
public record WebhookPayload(string Header,string Body);
