using LibrarySystem;
using LibrarySystem.Infrastructure;
using LibrarySystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceRegistration(builder.Configuration).AddInfrastructureDependencies().AddServiceDependencies();


builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.UseWebSockets();
app.MapControllers();
app.MapHub<ChatHub>("chat-hub");
app.Run();
