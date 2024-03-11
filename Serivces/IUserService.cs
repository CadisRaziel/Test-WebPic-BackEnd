using Microsoft.AspNetCore.Mvc;
using UserApiWebPic.Domain;

namespace UserApiWebPic.Serivces
{
    public interface IUserService
    {
        public Task<IActionResult> GetAll();
        public Task<IActionResult> GetAllpagination(int page = 1, int pageSize = 10);
        public Task<IActionResult> GetById(Guid id);
        public Task<IActionResult> Post(User user);
        public Task<IActionResult> Put(Guid id, User user);
        public Task<IActionResult> Delete(Guid id);
    }
}
