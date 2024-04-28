using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Identity;

namespace Cafe.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDBContext Context;
        private readonly UserManager<ApplicationUser> userManager;

        public IBaseRepository<Address> Address { get; set; }
        public INormalUserRepository AppUser { get; set; }
        public IBaseRepository<Category> Category { get; set; }
        public IBaseRepository<Image> Image { get; set; }

        public IOrderRepository Order { get; set; }
        public IBaseRepository<OrderItem> OrderItem { get; set; }
        public IProductRepository Product { get; set; }
        public IBaseRepository<SubCategory> SubCategory { get; set; }
        public IBaseRepository<Delivery> Delivery { get; set; }


        public UnitOfWork(ApplicationDBContext context, UserManager<ApplicationUser> _userManager)
        {
            Context = context;
            userManager = _userManager;
            Address = new BaseRepository<Address>(Context);
            AppUser = new NormalUserRepository(Context, userManager);
            Category = new BaseRepository<Category>(Context);
            Image = new BaseRepository<Image>(Context);
            Order = new OrderRepository(Context, userManager);
            OrderItem = new BaseRepository<OrderItem>(Context);
            Image = new BaseRepository<Image>(Context);
            Product = new ProductRepository(Context,userManager);
            SubCategory = new BaseRepository<SubCategory>(Context);
            Delivery = new BaseRepository<Delivery>(Context);
        }


         public void Dispose()
         {
            Context.Dispose();
         }

        public void Save()
        {
            Context.SaveChanges();
        }

      
    }
}
