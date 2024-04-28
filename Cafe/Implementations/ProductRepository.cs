using Cafe.Helpers;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Implementations
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProductRepository(ApplicationDBContext Context, UserManager<ApplicationUser> _userManager) : base(Context)
        {
            userManager = _userManager;
        }


        public PageResult<Product> GetAll(int pagenumber, int pagesize, string includeProperties, string search)
        {
            var data = Context.Products.ToList();
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
                    data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
                }

                totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Product>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }


        public PageResult<Product> GetAll(int subCategoryId,int pagenumber, int pagesize, string includeProperties, string search)
        {
            var data = Context.Products.ToList();
            int totalCount;
            try
            {
                var query = data.Where(p=>p.SubCategoryId==subCategoryId).AsQueryable();
                //var query = Set.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); //.Skip(ExcludedData).Take(pagesize);
                    }
                }

                totalCount = query.ToList().Count;

                data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
                }

            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<Product>()
            {
                Data = data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }


    }
}
