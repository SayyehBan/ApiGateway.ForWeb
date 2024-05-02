using ApiGateway.ForWeb.Models.DiscountServices;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDiscountService, RDiscountService>();
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).
    AddJsonFile("ocelot.json").
    AddOcelot(builder.Environment).AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration).
    AddPolly().
    AddCacheManager(x => { x.WithDictionaryHandle(); });


var app = builder.Build();

// Configure the HTTP request pipeline.
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
}
);

app.UseOcelot().Wait();
app.Run();
