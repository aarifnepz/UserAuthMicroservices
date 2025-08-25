using Microsoft.EntityFrameworkCore;
// Make sure to include the using statement for your User and UserContext classes
// e.g. using UserService.Models; or whatever namespace you use

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add DbContext with an in-memory database.
builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UserList"));
// Add caching
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.UseUrls("http://localhost:5000"); //open on fixed port 5000

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
    var context = scope.ServiceProvider.GetRequiredService<UserContext>();
    if (!context.Users.Any())
    {
        context.Users.Add(new User { Id = 1, Username = "aarif", Email = "aarif@aarif.com", FullName = "Mahammad Aarif", IsActive = true });
        context.Users.Add(new User { Id = 2, Username = "john", Email = "john@doe.com", FullName = "john Doe", IsActive = false });
        context.SaveChanges();
    }
}

app.Run();