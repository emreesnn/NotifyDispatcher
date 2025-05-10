using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotifyDispatcher.Data;
using NotifyDispatcher.Dispatchers;
using NotifyDispatcher.Jobs;
using NotifyDispatcher.Notifiers;
using NotifyDispatcher.ProductWarcherService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ProductWatcherService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();
builder.Services.AddScoped<NotificationDispatcher>();
builder.Services.AddScoped<TelegramNotifier>();
builder.Services.AddScoped<ProductCheckJob>();


//Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Telegram options
var telegramConfig = builder.Configuration.GetSection("Telegram");
string botToken = telegramConfig["BotToken"];
string chatId = telegramConfig["ChatId"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

RecurringJob.AddOrUpdate<ProductCheckJob>(
    "product-checker",
    job => job.RunAsync(),
    Cron.Minutely);

app.Run();
