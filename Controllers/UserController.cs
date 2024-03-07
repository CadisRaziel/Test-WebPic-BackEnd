using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApiWebPic.Domain;
using UserApiWebPic.Infra.Data;

[Produces("application/json")]
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {            
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante a busca de todos os usuários.");
        }
    }
      
    [HttpGet("pagination")] //-> Get paginado apenas para amostra de projeto.
    public async Task<IActionResult> GetAllpagination(int page = 1, int pageSize = 10)
    {
        try
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Quantidade de página ou linhas inválidos.");

            int skip = (page - 1) * pageSize;

            var totalUsers = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                TotalUsers = totalUsers,
                TotalPages = totalPages,
                Users = users
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante a busca de todos os usuários com paginação.");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("ID inválido.");

            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound("Usuário não encontrado.");
            

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante a busca do usuário por ID.");
        }
    }


    [HttpPost()]  
    public async Task<IActionResult> Post([FromBody] User user) //-> o mais recomendado seria um DTO, mas como é algo simples e tinha pouco tempo resolvi usar o objeto do domain direto
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                        
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == user.Cpf || u.Rg == user.Rg);
            if (existingUser != null)
                return Conflict("Já existe um usuário com o mesmo CPF ou RG.");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante o cadastro do usuário.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] User user)
    {
        try
        {           

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            if (user is null)
                return BadRequest("O objeto não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante a atualização do usuário.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("ID inválido.");

            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound("Usuário não encontrado.");
           

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user); 
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
            return StatusCode(500, "Ocorreu um erro durante a exclusão do usuário.");
        }
    }


}
