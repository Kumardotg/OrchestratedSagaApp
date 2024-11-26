using Microsoft.EntityFrameworkCore;
using MassTransit;
using OrchestratedSagaApp;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit((busConfigurator) =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumers(typeof(Program).Assembly);

    busConfigurator.AddSagaStateMachine<NewsLetterOnBoardSaga, NewsLetterOnBoardSagaData>()
    .EntityFrameworkRepository(r =>
    {
        r.ExistingDbContext<AppDbContext>();
        r.UseSqlServer();
    });

    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", c =>
            {
                c.Username("guest");
                c.Password("guest");
            });
        cfg.UseInMemoryOutbox(context);
        cfg.ConfigureEndpoints(context);

    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (Guid.NewGuid(),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/weatherforecase", async (WeatherForecast weatherForecast, AppDbContext context) =>
{
    context.WeatherForecast.Add(new WeatherForecast
    (
        Guid.NewGuid(),
        weatherForecast.TemperatureC,
        weatherForecast.Summary
    ));
    await context.SaveChangesAsync();

}).WithName("PostWeatherForecast")
.WithOpenApi();

app.MapPost("/AddSubscriber", async ([FromBody] string Email, IBus bus) =>
{
    await bus.Publish(new SubscribeToNewsLetter(Email));
    return Results.Accepted();

}).WithName("AddSubscriber")
.WithOpenApi();



app.Run();

