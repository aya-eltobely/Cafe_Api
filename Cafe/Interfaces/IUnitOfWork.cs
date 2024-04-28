using Cafe.Implementations;
using Cafe.Models;

namespace Cafe.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IBaseRepository<Address> Address { get; }
        INormalUserRepository AppUser { get; }
        IBaseRepository<Category> Category { get; }
        IBaseRepository<Image> Image { get; }

        IOrderRepository Order { get; }
        IBaseRepository<OrderItem> OrderItem { get; set; }
        IProductRepository Product { get; set; }
        IBaseRepository<SubCategory> SubCategory { get; set; }
        IBaseRepository<Delivery> Delivery { get; set; }


        void Save();

    }
}
