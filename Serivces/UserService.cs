using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApiWebPic.Domain;
using UserApiWebPic.Infra.Data;
using UserApiWebPic.Serivces;

namespace UserApiWebPic.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .ToListAsync();

                return new OkObjectResult(users);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ocorreu um erro durante a busca de todos os usuários.");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetAllpagination(int page = 1, int pageSize = 10)
        {
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

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestObjectResult("ID inválido.");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new NotFoundObjectResult("Usuário não encontrado.");

            return new OkObjectResult(user);
        }

        public async Task<IActionResult> Post(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == user.Cpf || u.Rg == user.Rg);
            if (existingUser != null)
                return new ConflictObjectResult("Já existe um usuário com o mesmo CPF ou RG.");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<IActionResult> Put(Guid id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return new NotFoundObjectResult("Usuário não encontrado.");

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestObjectResult("ID inválido.");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new NotFoundObjectResult("Usuário não encontrado.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }
    }
}
