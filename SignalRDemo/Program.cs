using Microsoft.EntityFrameworkCore;
using SignalRDemo;
using SignalRDemo.HubConfig;

var builder = WebApplication.CreateBuilder(args);

//Add SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddDbContext<SignalrContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

///////////////////////

// Add services to the container.

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

//Add SignalR

app.UseCors("AllowAllHeaders");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MyHub>("/toastr");
});
///////////////////////

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
