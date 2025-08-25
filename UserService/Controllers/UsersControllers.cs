using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;
    private readonly IMemoryCache _cache;

    public UsersController(UserContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        string cacheKey = $"user-{id}";

        // Declare cachedUser as a nullable type to fix the first warning
        if (_cache.TryGetValue(cacheKey, out User? cachedUser))
        {
            // Check for null before returning
            if (cachedUser != null)
            {
                return cachedUser;
            }
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _cache.Set(cacheKey, user, TimeSpan.FromMinutes(1)); // Cache for 1 minute

        return user;
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }
}