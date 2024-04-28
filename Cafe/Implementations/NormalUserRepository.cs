using Cafe.Helpers;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Implementations
{
    public class NormalUserRepository : BaseRepository<ApplicationUser>,  INormalUserRepository
    {

        private readonly UserManager<ApplicationUser> userManager;

        public NormalUserRepository(ApplicationDBContext Context, UserManager<ApplicationUser> _userManager) : base(Context)
        {
            userManager = _userManager;
        }


        public PageResult<ApplicationUser> GetAll(int pagenumber, int pagesize, string includeProperties, string search)
        {
            
            var data = userManager.GetUsersInRoleAsync(WebSiteRoles.SiteUser).GetAwaiter().GetResult();
            int totalCount;
            try
            {
                var query = data.AsQueryable();

                var ExcludedData = (pagenumber * pagesize) - pagesize;

                if (!string.IsNullOrWhiteSpace(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp); 
                    }
                }


                data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.Id.Contains(search)).ToList();
                }

                totalCount = data.ToList().Count;
                //totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return new PageResult<ApplicationUser>()
            {
                Data = (List<ApplicationUser>)data,
                PageNumber = pagenumber,
                PageSize = pagesize,
                TotalItem = totalCount
            };
        }

        public List<ApplicationUser> GetAll(string search)
        {

            var data = userManager.GetUsersInRoleAsync(WebSiteRoles.SiteUser).GetAwaiter().GetResult();
            
            try
            {
                var query = data.AsQueryable();

                //var ExcludedData = (pagenumber * pagesize) - pagesize;

                //if (!string.IsNullOrWhiteSpace(includeProperties))
                //{
                //    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        query = query.Include(includeProp);
                //    }
                //}


                //data = query.Skip(ExcludedData).Take(pagesize).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(d => d.UserName.ToLower().Contains(search.ToLower())).ToList();
                }

                //totalCount = data.ToList().Count;
                //totalCount = Set.ToList().Count;
            }

            catch (Exception)
            {

                throw;
            }

            return (List<ApplicationUser>)data;
        }
    }
}
