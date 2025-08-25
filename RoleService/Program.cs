using Microsoft.EntityFrameworkCore;
using RoleService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext for roles
builder.Services.AddDbContext<RoleContext>(opt => opt.UseInMemoryDatabase("RoleList"));

// Register HttpClient to communicate with UserService
builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000"); // fixed port of UserService
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RoleContext>();
    if (!context.Roles.Any())
    {
        context.Roles.Add(new Role { Id = 1, UserId = 1, RoleName = "admin" });
        context.Roles.Add(new Role { Id = 2, UserId = 2, RoleName = "user" });
        context.SaveChanges();
    }
}

app.Run();