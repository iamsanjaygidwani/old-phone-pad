using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OldPhonePad.Lib;

var builder = WebApplication.CreateBuilder(args);

// Logging and services
builder.Services.AddLogging();
builder.Services.AddSingleton<IKeyMapping, DefaultKeyMapping>();
builder.Services.AddSingleton<IInputTokenizer, SimpleTokenizer>();
builder.Services.AddSingleton<IGroupResolver, GroupResolver>();
builder.Services.AddSingleton<IOldPhonePadService, OldPhonePadService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/decode", (DecodeRequest req, IOldPhonePadService svc) =>
{
    if (req?.Input is null) return Results.BadRequest(new { error = "Missing input" });
    try
    {
        var output = svc.Decode(req.Input);
        return Results.Ok(new { input = req.Input, output });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

public record DecodeRequest(string Input);
