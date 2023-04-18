using ChatGPT.Net;
using Dor;
using Dor.FileChatHistory;
using dotenv.net;
using ChatGpt = Dor.ChatGpt.ChatGpt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// add history
var chatHistory = new FileChatHistory("C:\\dev\\temp\\history.json");
builder.Services.AddSingleton<IChatHistory>(chatHistory);

DotEnv.Load();

var chatGpt = new ChatGpt(new ChatGPT.Net.ChatGpt( DotEnv.Read()["CHATGPTKEY"]!));

// add chat ai
// builder.Services.AddSingleton<IAiChat>(chatGpt);
builder.Services.AddSingleton<IAiChat, ChatGpt>((sp) => chatGpt);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(4002, configure =>
    {
        configure.UseConnectionLogging();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();