using Microsoft.EntityFrameworkCore;
using Stargate.Server.Business.Commands;
using Stargate.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder => builder.WithOrigins("https://localhost:62088")
     .AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StargateContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));


builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddSingleton<ILoggerProvider, SQLiteLoggerProvider>();
builder.Services.Configure<SQLiteLoggerConfiguration>(config =>
{
    config.LogLevel = LogLevel.Information;
});

var app = builder.Build();
app.UseDefaultFiles();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; var dbContext = services.GetService<StargateContext>();
    var modelBuilder = new ModelBuilder(); 
    StargateContext.SeedData(modelBuilder);
}
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
