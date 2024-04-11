using GuestApi.Commands;
using GuestApi.Handlers;
using GuestApi.Models;
using GuestApi.Queries;

var generatedApiKey = ApiKeyGenerator.GenerateApiKey();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<Guest>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSingleton<ICommandHandler<AddGuestCommand>, AddGuestCommandHandler>();
builder.Services.AddSingleton<ICommandHandler<AddPhoneCommand>, AddPhoneCommandHandler>();
builder.Services.AddSingleton<IQueryHandler<GetGuestByIdQuery, Guest>, GetGuestByIdQueryHandler>();
builder.Services.AddSingleton<IQueryHandler<GetAllGuestsQuery, List<Guest>>, GetAllGuestsQueryHandler>();

var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     if (!context.Request.Headers.TryGetValue("X-API-Key", out var apiKey))
//     {
//         context.Response.StatusCode = 401;
//         await context.Response.WriteAsync("Missing API Key");
//         return;
//     }
//     if (apiKey != generatedApiKey)
//     {
//         context.Response.StatusCode = 403;
//         await context.Response.WriteAsync("Invalid API Key");
//         return;
//     }

//     await next();
// });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();