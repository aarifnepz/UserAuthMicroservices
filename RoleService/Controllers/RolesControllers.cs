using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using RoleService;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public RolesController(RoleContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    // A model to represent the user from UserService
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }

    // POST: api/roles/assign
    [HttpPost("assign")]
    public async Task<ActionResult<Role>> AssignRole(Role role)
    {
        var httpClient = _httpClientFactory.CreateClient("UserService");
        var user = await httpClient.GetFromJsonAsync<User>($"api/users/{role.UserId}");

        if (user == null || !user.IsActive)
        {
            return BadRequest("User not found or is not active.");
        }

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction("CheckRole", new { userId = role.UserId, role = role.RoleName }, role);
    }

    // GET: api/roles/check?userid={id}&role={role}
    [HttpGet("check")]
    public async Task<IActionResult> CheckRole([FromQuery] int userId, [FromQuery] string role)
    {
        var httpClient = _httpClientFactory.CreateClient("UserService");
        var user = await httpClient.GetFromJsonAsync<User>($"api/users/{userId}");

        if (user == null || !user.IsActive)
        {
            return BadRequest("User not found or is not active.");
        }

        var hasRole = await _context.Roles.AnyAsync(r => r.UserId == userId && r.RoleName == role);

        if (hasRole)
        {
            return Ok($"User {userId} has the role '{role}'.");
        }
        else
        {
            return NotFound($"User {userId} does not have the role '{role}'.");
        }
    }
}