using Cafe.Helpers;
using Cafe.Models;

namespace Cafe.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        PageResult<Product> GetAll(int pagenumber, int pagesize, string includeProperties, string search);
        PageResult<Product> GetAll(int subCategoryId,int pagenumber, int pagesize, string includeProperties, string search);

    }
}
