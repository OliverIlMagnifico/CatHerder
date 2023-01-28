using CatHerder;
using CatHerder.Data;
using CatHerder.Mediatr;
using Microsoft.AspNetCore.Components.Server.Circuits;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddLogging(l => l.AddConsole());
builder.Services.AddSingleton<ILogger>(c =>
    c.GetService<ILoggerFactory>()?.CreateLogger("base")
);
Injection.Register(builder.Services);
builder.Services.AddSingleton<MongoConfiguration>(i => {
    var mongoConfig = new MongoConfiguration();
    builder.Configuration.GetSection("mongo").Bind(mongoConfig);
    return mongoConfig;
}
);

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<ICrud, MongoCrud>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
