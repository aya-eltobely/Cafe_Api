using Cafe.Helpers;
using Cafe.Models;

namespace Cafe.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        PageResult<Order> GetAll(int pagenumber, int pagesize, string includeProperties, string search);

    }
}
