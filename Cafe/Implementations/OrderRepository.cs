using Cafe.Helpers;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Implementations
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {

        private readonly UserManager<ApplicationUser> userManager;

        public OrderRepository(ApplicationDBContext Context, UserManager<ApplicationUser> _userManager) : base(Context)
        {
            userManager = _userManager;
        }


        public PageResult<Order> GetAll(int pagenumber, int pagesize, string includeProperties, string search)
        {
            var data = Context.Orders.ToList();
            int totalCount;
            try
            {
                var query = data.AsQueryable();
                //var query = Set.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); //.Skip(ExcludedData).Take(pagesize);
                    }
                }


                data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.UserId.Contains(search)).ToList();
                }

                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Order>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }

    }
}
