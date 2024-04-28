using Cafe.Helpers;
using Cafe.Models;

namespace Cafe.Interfaces
{
    public interface INormalUserRepository : IBaseRepository<ApplicationUser>
    {
        PageResult<ApplicationUser> GetAll(int pagenumber, int pagesize, string includeProperties, string search);

        public List<ApplicationUser> GetAll(string search);
    }
}
