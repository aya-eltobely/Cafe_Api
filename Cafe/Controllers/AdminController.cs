using Cafe.DTOs;
using Cafe.Helpers;
using Cafe.Implementations;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using Image = Cafe.Models.Image;

namespace Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = WebSiteRoles.SiteAdmin)]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageService imageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDBContext context;

        public AdminController(IUnitOfWork _unitOfWork , IImageService _imageService , UserManager<ApplicationUser> _userManager, ApplicationDBContext _context) 
        {
            unitOfWork = _unitOfWork;
            imageService = _imageService;
            userManager = _userManager;
            context = _context;
        }

        /// *********************************** Category ************************** ///
        #region Category

        //******************************************** Create
        [HttpPost("Category")]
        public IActionResult CreateCategory( [FromBody] CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var category = new Category() { Name = categoryDTO.Name };
                Category cat = unitOfWork.Category.Create(category);
                if (cat != null)
                {
                    return Ok( new { message = "Category Created" } );
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }

            }
            return BadRequest(ModelState);
        }

        //******************************************** Edit
        [HttpPut("Category/{id}")]
        public IActionResult EditCategory( int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var category = unitOfWork.Category.GetById(id);

                if (category != null)
                {
                    category.Name = categoryDTO.Name;
                    bool res = unitOfWork.Category.Update(category);
                    if (res)
                    {
                        return Ok(new { message = "Category Updated" });
                    }
                    else
                    {
                        return BadRequest("SomeThing Wrong");
                    }
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }

            }
            return BadRequest(ModelState);
        }

        //******************************************** Delete
        [HttpDelete("Category/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = unitOfWork.Category.GetById(id);

            if (category != null)
            {
                bool res = unitOfWork.Category.Delete(category);
                if (res)
                {
                    return Ok(new { message = "Category Deleted" });
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }
            }
            else
            {
                return BadRequest("SomeThing Wrong");
            }
        }

        //******************************************** Get All
        [HttpGet("Category")]
        public IActionResult GetAllCategory()
        {
            var categories = unitOfWork.Category.GetAll(null, query => query.OrderByDescending(x => x.Id), "");
            List<GetCategoryDTO> catDto = new List<GetCategoryDTO>();
            foreach (var cat in categories)
            {
                catDto.Add(new GetCategoryDTO() { Id = cat.Id, Name = cat.Name });
            }
            return Ok(catDto);
        }


        #endregion


        /// *********************************** SubCategory ************************** ///
        #region SubCategory

        //******************************************** Create
        [HttpPost("SubCategory")]
        public IActionResult CreateSubCategory([FromBody] SubCategoryDTO subCategoryDTO)
        {

            if (ModelState.IsValid)
            {
                //image
                Image subCategoryImg = new Image() { FileName = subCategoryDTO.FileName, ContentType = subCategoryDTO.ContentType, ImageData = subCategoryDTO.ImageData };
                Image subcatimgRes = unitOfWork.Image.Create(subCategoryImg);


                if (subcatimgRes != null)
                {
                    var subCategory = new SubCategory() { Name = subCategoryDTO.Name, CategoryId = subCategoryDTO.CategoryId, ImageId = subcatimgRes.Id };

                    SubCategory subCat = unitOfWork.SubCategory.Create(subCategory);

                    if (subCat != null)
                    {
                        return Ok(new { message = "SubCategory Created" });
                    }
                    else
                    {
                        return BadRequest("SomeThing Wrong");
                    }

                }
                else
                {
                    return BadRequest("Image Not Found");
                }

            }
            else
            {
                return BadRequest(ModelState);

            }

        }

        //******************************************** Edit
        [HttpPut("SubCategory/{id}")]
        public IActionResult EditSubCategory(int id, [FromBody] SubCategoryDTO subCategoryDTO)
        {

            if (ModelState.IsValid)
            {
                
                var subCat = unitOfWork.SubCategory.GetById(id);

                if (subCat != null)
                {
                    //get image
                    var image = unitOfWork.Image.GetById(subCat.ImageId);
                    if (image != null)
                    {
                        image.FileName = subCategoryDTO.FileName;
                        image.ImageData = subCategoryDTO.ImageData;
                        image.ContentType = subCategoryDTO.ContentType;

                        bool imgres  = unitOfWork.Image.Update(image);

                        if(imgres)
                        {
                            subCat.Name = subCategoryDTO.Name;
                            subCat.CategoryId = subCategoryDTO.CategoryId;
                            subCat.ImageId = image.Id;

                            bool res = unitOfWork.SubCategory.Update(subCat);

                            if (res)
                            {
                                return Ok(new { message = "Sub Category Updated" });
                            }
                            else
                            {
                                return BadRequest("SomeThing Wrong");
                            }
                        }
                        else
                        {
                            return BadRequest("SomeThing wrong in Image");
                        }
                    }
                    else
                    {
                        return BadRequest("Image not found");
                    }

                    ///

                }
                else
                {
                    return BadRequest("Sub Category Not Found");
                }



            }
            return BadRequest(ModelState);


        }

        //******************************************** Delete
        [HttpDelete("SubCategory/{id}")]
        public IActionResult DeleteSubCategory(int id)
        {

            using var transaction = context.Database.BeginTransaction();

            try
            {
                var subCat = unitOfWork.SubCategory.GetById(id);
                unitOfWork.SubCategory.Delete(subCat);
                var image = unitOfWork.Image.GetById(subCat.ImageId);
                unitOfWork.Image.Delete(image);



                context.SaveChanges();
                transaction.Commit();

                return Ok(new { message = "SubCategory Deleted" });

            }
            catch (Exception)
            {

                transaction.Rollback();
                return BadRequest("Something Wrong");
            }
        }

        //******************************************** Get All
        [HttpGet("SubCategory")]
        public IActionResult GetAllSubCategory()
        {
            var subCategories = unitOfWork.SubCategory.GetAll(null, query => query.OrderByDescending(x => x.Id), "");

            //get image 

            List<GetSubCategoryDTO> subCatDto = new List<GetSubCategoryDTO>();
            foreach (var cat in subCategories)
            {
                //ImageDTO imgDto = imageService.GetImage(cat.ImageId);
                var imgDto = unitOfWork.Image.GetById(cat.ImageId);
                subCatDto.Add(new GetSubCategoryDTO() { Id = cat.Id, Name = cat.Name,CategoryName = cat.Category.Name, CategoryId = cat.CategoryId, FileName = imgDto.FileName, ImageData = imgDto.ImageData, ContentType = imgDto.ContentType });
            }
            return Ok(subCatDto);
        }



        //[HttpPost("SubCategory")]
        //public IActionResult CreateSubCategory( [FromBody] SubCategoryDTO subCategoryDTO, IFormFile file)
        //{


        //    int imageId = imageService.UploadImage(file);

        //    if (imageId != 0)
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            var subCategory = new SubCategory() { Name = subCategoryDTO.Name,CategoryId= subCategoryDTO.CategoryId, ImageId = imageId };

        //            SubCategory subCat = unitOfWork.SubCategory.Create(subCategory);

        //            if (subCat != null)
        //            {
        //                return Ok(new { message = "SubCategory Created" });
        //            }
        //            else
        //            {
        //                return BadRequest("SomeThing Wrong");
        //            }

        //        }
        //        return BadRequest(ModelState);
        //    }
        //    else
        //    {
        //        return BadRequest("Image Not Found");
        //    }

        //}

        //[HttpPut("SubCategory/{id}")]
        //public IActionResult EditSubCategory(int id, [FromBody] SubCategoryDTO subCategoryDTO, IFormFile file)
        //{
        //    int imageId = imageService.UploadImage(file);

        //    if (imageId != 0)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var subCat = unitOfWork.SubCategory.GetById(id);

        //            if (subCat != null)
        //            {
        //                subCat.Name = subCategoryDTO.Name;
        //                subCat.CategoryId = subCategoryDTO.CategoryId;
        //                subCat.ImageId = imageId;

        //                bool res = unitOfWork.SubCategory.Update(subCat);

        //                if (res)
        //                {
        //                    return Ok(new { message = "Sub Category Updated" });
        //                }
        //                else
        //                {
        //                    return BadRequest("SomeThing Wrong");
        //                }
        //            }
        //            else
        //            {
        //                return BadRequest("Sub Category Not Found");
        //            }



        //        }
        //        return BadRequest(ModelState);
        //    }
        //    else
        //    {
        //        return BadRequest("Image Not Found");
        //    }

        //}
        #endregion


        /// *********************************** Delivery ************************** ///  
        #region Delivery

        //-------------------------------------------- Create
        [HttpPost("Delivery")]
        public async Task<IActionResult> CreateDelivery (RegisterUserDTO delivery )
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = delivery.UserName,
                    Email = delivery.Email,
                    FirstName = delivery.FirstName,
                    LastName = delivery.LastName,
                    PhoneNumber = delivery.PhoneNumber
                };
                IdentityResult createRes = await userManager.CreateAsync(user, delivery.Password);

                if(createRes.Succeeded)
                {
                    //asign role to delivery
                    IdentityResult roleRes = userManager.AddToRoleAsync(user, WebSiteRoles.SiteDelivery).GetAwaiter().GetResult();
                    if(roleRes.Succeeded)
                    {
                        //create delivery
                        Delivery newDelivery = new Delivery() { AppUserId = user.Id };
                        var deliv = unitOfWork.Delivery.Create(newDelivery);
                        if(deliv!=null)
                        {
                            return Ok(new { message = "Delivery Created" });
                        }
                        else
                        {
                            return BadRequest("Something Wrong");

                        }
                    }
                    else
                    {
                        return BadRequest(roleRes.Errors);
                    }

                }
                else
                {
                    return BadRequest(createRes.Errors);
                    //forloop to show all errors
                    //foreach (var item in createRes.Errors)
                    //{
                    //    return BadRequest(item);
                    //}

                }             
            }
            return BadRequest(ModelState);
        }


        //--------------------------------------------- Delete
        [HttpDelete("Delivery/{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                //get delivery
                var delivery = unitOfWork.Delivery.GetById(id);
                //delete delivery from delivery table
                unitOfWork.Delivery.Delete(delivery);
                //get delivery from user table
                var user = await userManager.FindByIdAsync(delivery.AppUserId);
                //delete delivery from userRoles 
                await userManager.RemoveFromRoleAsync(user, WebSiteRoles.SiteDelivery);
                //delete delivery from user table
                unitOfWork.AppUser.Delete(user);

                context.SaveChanges();
                transaction.Commit();

                return Ok(new { message = "Delivery Deleted" });

            }
            catch (Exception)
            {

                transaction.Rollback();
                return BadRequest("Something Wrong");
            }       

        }


        //--------------------------------------------- Get All
        [HttpGet("Delivery")]
        public IActionResult GetAllDelivery()
        {
            List<Delivery> deliveries = unitOfWork.Delivery.GetAll( null, query => query.OrderByDescending(x => x.Id), "").ToList();
            List<GetDeliveryDTO> deliveryDto = new List<GetDeliveryDTO>();
            foreach (var delivery in deliveries)
            {
                deliveryDto.Add(new GetDeliveryDTO() { DeliveryId = delivery.Id,UserId=delivery.AppUserId , FirstName = delivery.AppUser.FirstName, LastName = delivery.AppUser.LastName, Email = delivery.AppUser.Email, PhoneNumber = delivery.AppUser.PhoneNumber, UserName = delivery.AppUser.UserName });
            }

            return Ok(deliveryDto);


        }


        //[HttpDelete("Delivery/{id}")]
        //public async Task<IActionResult> DeleteDelivery( int id )
        //{
        //    var delivery = unitOfWork.Delivery.GetById(id);

        //    if (delivery == null)
        //    {
        //        return BadRequest("User not found");
        //    }
        //    //delete delivery from delivery 
        //    bool res = unitOfWork.Delivery.Delete(delivery);
        //    if (res)
        //    {
        //        var user = await userManager.FindByIdAsync(delivery.AppUserId);
        //        //delete delivery from userRoles 
        //        var userRoleRes = await userManager.RemoveFromRoleAsync(user, WebSiteRoles.SiteDelivery);

        //        if(!userRoleRes.Succeeded)
        //        {
        //            return BadRequest("Something Wrong");
        //        }

        //        //delete delivery from user 

        //        bool res2 = unitOfWork.AppUser.Delete(user);
        //        if(res2)
        //        {
        //            return Ok(new { message = "Delivery Deleted" });
        //        }
        //        else
        //        {
        //            return BadRequest("Something Wrong");
        //        }

        //    }
        //    else
        //    {
        //        return BadRequest("Something Wrong");
        //    }

        //}

        #endregion


        /// *********************************** User ************************** ///
        #region User

        //-------------------------------------------- Get All
        [HttpGet("NormalUser")]
        public IActionResult GetAllNormalUser(string? search = null)
        {
            var normalUsers = unitOfWork.AppUser.GetAll(search);


                List<GetUserDTO> userDto = new List<GetUserDTO>();
                foreach (var user in normalUsers)
                {
                    userDto.Add(new GetUserDTO() 
                    { UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, UserName = user.UserName, Activate = (user.IsActivate==0)? "activate" : "deactive" });
                }

                return Ok(userDto);
        }

        //------------------------------------------- Deactivate 
        [HttpGet("NormalUser/DeactivatelUser/{id}")]
        public async Task<IActionResult> ToggleUserStatus(string id) // 0 => active , 1 => deactive
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if(user.IsActivate == 0 )
            {
                user.IsActivate = 1;
            }
            else if(user.IsActivate == 1)
            {
                user.IsActivate = 0;
            }


            bool res = unitOfWork.AppUser.Update(user);
            if(res)
            {
                if(user.IsActivate == 0)
                {
                    return Ok(new { message = "Account Activated" });
                }
                else
                {
                    return Ok(new { message = "Account Deactivated" });
                }
            }
            else
            {
                return BadRequest("SomeThing Wrong");
            }

        }



        ////////////////////////////////////////////////// get all with pagination
        //[HttpGet("NormalUser")]
        //public IActionResult GetAllNormalUser(int pagenumber, int pagesize, string? search = null)
        //{
        //    var normalUsers = unitOfWork.AppUser.GetAll(pagenumber, pagesize, null, search);


        //    if (normalUsers.Data.Count() > 0)
        //    {
        //        List<GetUserDTO> userDto = new List<GetUserDTO>();
        //        foreach (var user in normalUsers.Data)
        //        {
        //            userDto.Add(new GetUserDTO() { UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, UserName = user.UserName });
        //        }

        //        return Ok(new PageResult<GetUserDTO>()
        //        {
        //            Data = userDto,
        //            PageNumber = pagenumber,
        //            PageSize = pagesize,
        //            TotalItem = normalUsers.TotalItem
        //        });
        //        //return Ok(userDto);
        //    }
        //    else
        //    {
        //        return BadRequest("SomeThing Wrong");
        //    }
        //}

        //////////////////////////////////////////////////
        //[HttpGet("NormalUser")]
        //public async Task<IActionResult> GetAllNormalUser()
        //{
        //    var normalUsers = await userManager.GetUsersInRoleAsync(WebSiteRoles.SiteUser);


        //    if (normalUsers.Count() > 0)
        //    {
        //        List<GetUserDTO> userDto = new List<GetUserDTO>();
        //        foreach (var user in normalUsers)
        //        {
        //            userDto.Add(new GetUserDTO() {UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, UserName = user.UserName });
        //        }
        //        return Ok(userDto);
        //    }
        //    else
        //    {
        //        return BadRequest("SomeThing Wrong");
        //    }
        //}


        #endregion


        /// *********************************** Product ************************** ///
        #region Product

        //------------------------------------------------ Create
        [HttpPost("Product")]
        public IActionResult CreateProduct( [FromBody] ProductDTO productDTO )
        {
            if (ModelState.IsValid)
            {

                using var transaction = context.Database.BeginTransaction();

                try
                {
                    //image
                    Image productImg = new Image() { FileName = productDTO.FileName, ContentType = productDTO.ContentType, ImageData = productDTO.ImageData };
                    unitOfWork.Image.Create(productImg);


                    var product = new Product() { Name = productDTO.Name, Price = productDTO.Price, Description = productDTO.Description, SubCategoryId = productDTO.subCategoryId, ImageId = productImg.Id };
                    unitOfWork.Product.Create(product);

                    context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "Product Created" });

                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return BadRequest("Something Wrong");
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        //------------------------------------------------ Edit
        [HttpPut("Product/{id}")]
        public IActionResult EditProduct(int id, [FromBody] ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                using var transaction = context.Database.BeginTransaction();

                try
                {
                    var product = unitOfWork.Product.GetById(id);
                    //image
                    var image = unitOfWork.Image.GetById(product.ImageId);
                    image.FileName = productDTO.FileName;
                    image.ContentType = productDTO.ContentType;
                    image.ImageData = productDTO.ImageData;
                    unitOfWork.Image.Update(image);


                    product.Name = productDTO.Name;
                    product.Price = productDTO.Price;
                    product.Description = productDTO.Description;
                    product.SubCategoryId = productDTO.subCategoryId;
                    product.ImageId = image.Id;
                    unitOfWork.Product.Update(product);


                    context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "Product updated" });

                }
                catch (Exception)
                {

                    transaction.Rollback();
                    return BadRequest("Something Wrong");
                }
                
            }
            return BadRequest(ModelState);

        }

        //------------------------------------------------ Delete
        [HttpDelete("Product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var product = unitOfWork.Product.GetById(id);
                unitOfWork.Product.Delete(product);
                var image = unitOfWork.Image.GetById(product.ImageId);
                unitOfWork.Image.Delete(image);



                context.SaveChanges();
                transaction.Commit();

                return Ok(new { message = "Product Deleted" });

            }
            catch (Exception)
            {

                transaction.Rollback();
                return BadRequest("Something Wrong");
            }


        }

        //------------------------------------------------ Get All
        [HttpGet("product")]
        public IActionResult GetAllproduct()
        {
            var products = unitOfWork.Product.GetAll();

            List<GetProductDTO> productDTOs = new List<GetProductDTO>();

            foreach (var prod in products)
            {
                var img = unitOfWork.Image.GetById(prod.ImageId);

                productDTOs.Add(
                new GetProductDTO()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    Description = prod.Description,
                    subCategoryName = prod.SubCategory.Name,
                    subCategoryId = prod.SubCategory.Id,
                    FileName = img.FileName,
                    ContentType = img.ContentType,
                    ImageData = img.ImageData
                });
            }
            return Ok(productDTOs);

        }


        //[HttpGet("product")]
        //public IActionResult GetAllproduct(int pagenumber, int pagesize, string? search = null)
        //{
        //    PageResult<Product> products = unitOfWork.Product.GetAll(pagenumber, pagesize, null, search);

        //    List<GetProductDTO> productDTOs = new List<GetProductDTO>();

        //    foreach (var prod in products.Data)
        //    {
        //        ImageDTO imgDto = imageService.GetImage(prod.ImageId);

        //        productDTOs.Add(
        //        new GetProductDTO()
        //        {
        //            Id = prod.Id,
        //            Name = prod.Name,
        //            Price = prod.Price,
        //            Description = prod.Description,
        //            SubCatName = prod.SubCategory.Name,
        //            FileName = imgDto.FileName,
        //            ContentType = imgDto.ContentType,
        //            ImageData = imgDto.ImageData
        //        });
        //    }
        //    return Ok(productDTOs);


        //}

        #endregion


        /// *********************************** Order ************************** ///
        #region Order

        //--------------------------------------------------------
        [HttpGet("Order")]
        public IActionResult GetAllOrder(string? search = null) //searc by userid
        {
            var orders = unitOfWork.Order.GetAll(null,null,"AppUser,Address,Delivery");
            List<GetOrderDTO> orderDTOs = new List<GetOrderDTO>();

            foreach (var order in orders)
            {
                orderDTOs.Add(
                new GetOrderDTO()
                {
                    Id = order.Id,
                    Date = order.Date,
                    Status = Enum.GetName(typeof(ProductStatusEnum), order.Status),
                    deliveryStatus = Enum.GetName(typeof(DeliveryStatusEnum), order.DeliveryStatus) ,
                    OrderTotals = order.Total,
                    UserFullName = order.Address.Name ,
                    DeliveryFullName = order.Delivery?.AppUser?.FirstName + " " + order.Delivery?.AppUser?.LastName,
                    Address = order.Address.State + " , " + order.Address.City + " , " + order.Address.Street
                });
            }
            return Ok(orderDTOs);
        }


        //--------------------------------------------------------
        [HttpGet("Order/reject/{id}")]
        public IActionResult RejectOrder(int id)
        {
           
            Order order = unitOfWork.Order.GetById(id);

            if(order != null )
            {
                if (order.Status == ProductStatusEnum.Pendding)
                {
                    order.Status = ProductStatusEnum.Rejected;
                    bool res = unitOfWork.Order.Update(order);
                    if (res)
                    {
                        return Ok(new { message = "Order Rejected" });
                    }
                    else
                    {
                        return BadRequest("SomeTing Wrong");
                    }
                }
                else
                {
                    return BadRequest("Cant Reject Order");
                }
            }
            else
            {
                return BadRequest("Order Not Found");
            }
           
        }


        //--------------------------------------------------------
        [HttpGet("Order/approved/{id}")]
        public IActionResult ApproveOrder(int id)
        {

            Order order = unitOfWork.Order.GetById(id);

            if (order != null)
            {
                if (order.Status == ProductStatusEnum.Pendding)
                {
                    order.Status = ProductStatusEnum.Approved;
                    bool res = unitOfWork.Order.Update(order);
                    if (res)
                    {
                        return Ok(new { message = "Order Approved" });
                    }
                    else
                    {
                        return BadRequest("SomeTing Wrong");
                    }
                }
                else
                {
                    return BadRequest("Cant Approve Order");
                }
            }
            else
            {
                return BadRequest("Order Not Found");
            }

        }


        //--------------------------------------------------------
        [HttpDelete("Order/{id}")]
        public IActionResult DeleteOrder(int id)
        {

            Order order = unitOfWork.Order.GetById(id);

            if (order != null)
            {
                if (order.Status == ProductStatusEnum.Pendding || order.Status == ProductStatusEnum.Rejected)
                {
                    bool res = unitOfWork.Order.Delete(order);
                    if (res)
                    {
                        return Ok(new { message = "Order Deleted" });
                    }
                    else
                    {
                        return BadRequest("SomeTing Wrong");
                    }
                }
                else
                {
                    return BadRequest("Cant Delete Order");
                }
            }
            else
            {
                return BadRequest("Order Not Found");
            }

        }


        //--------------------------------------------------------
        [HttpPut("Order/assignToDelivery/{id}")]
        public IActionResult AssignOrder(int id, [FromBody] int deliveryId)
        {
            Order order = unitOfWork.Order.GetById(id);

            Delivery delivery = unitOfWork.Delivery.GetById(deliveryId);

            if (order != null && delivery !=null)
            {
                if (order.Status == ProductStatusEnum.Approved)
                {
                    //assign ==> deliveryId in Order Table 
                    order.DeliveryId = deliveryId;
                    order.DeliveryStatus = DeliveryStatusEnum.Pendding;
                    bool res = unitOfWork.Order.Update(order);
                    if (res)
                    {
                        return Ok(new { message = "Order Assigned To Delivery" });
                    }
                    else
                    {
                        return BadRequest("SomeTing Wrong");
                    }
                }
                else
                {
                    return BadRequest("Cant Assign Order");
                }
            }
            else
            {
                return BadRequest("Order or Delivery Not Found");
            }

        }

        //--------------------------------------------------------
        [HttpPost("Order/getNumsOfOrders/{filterBy}")]
        public IActionResult GetNumbersOrders(int filterBy = 0 , [FromBody] Date date = default  ) // [  1 ==> filter daily , 2 ==> monthly , 3==>yearly ]
        {
            if( date == default )
            {
                date.date = DateTime.Now.ToString();
            }

            DateTime newDate = DateTime.ParseExact(date.date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            // Extract year, month, and day
            int year = newDate.Year;
            int month = newDate.Month;
            int day = newDate.Day;

            var totalOrders = 0;

            switch (filterBy)
            {
                case 1: // Daily
                    totalOrders = context.Orders.Where(o => o.Date == newDate).Count();
                    break;
                case 2: // monthly
                    totalOrders = context.Orders.Where(o => o.Date.Month == month && o.Date.Year == year).Count();
                    break;
                case 3: // Yearly
                    totalOrders = context.Orders.Where(o => o.Date.Year == year).Count();
                    break;
                default:
                    totalOrders = context.Orders.Where(o => o.Date == newDate).Count();
                    break;
            }

            return Ok(totalOrders);
        }


        //--------------------------------------------------------
        [HttpPost("Order/getTotalOfOrders/{filterBy}")]
        public IActionResult GetTotalOrders(int filterBy = 0, [FromBody] Date date = default) // [  1 ==> filter daily , 2 ==> monthly , 3==>yearly ]
        {
            if (date == default)
            {
                date.date = DateTime.Now.ToString();
            }

            DateTime newDate = DateTime.ParseExact(date.date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            // Extract year, month, and day
            int year = newDate.Year;
            int month = newDate.Month;
            int day = newDate.Day;

            var totalOrders = 0;

            switch (filterBy)
            {
                case 1: // Daily
                    totalOrders = context.Orders.Where(o => o.Date == newDate).Sum(o => o.Total);
                    break;
                case 2: // monthly
                    totalOrders = context.Orders.Where(o => o.Date.Month == month && o.Date.Year == year).Sum(o => o.Total);
                    break;
                case 3: // Yearly
                    totalOrders = context.Orders.Where(o => o.Date.Year == year).Sum(o => o.Total);
                    break;
                default:
                    totalOrders = context.Orders.Where(o => o.Date == newDate).Sum(o => o.Total);
                    break;
            }

            return Ok(totalOrders);
        }

        //--------------------------------------------------------
        [HttpPost("Order/getOrdersStatus/{filterBy}")]
        public IActionResult GetOrdersStatus(int filterBy = 0, [FromBody] Date date = default) // [  1 ==> filter daily , 2 ==> monthly , 3==>yearly ]
        {
            if (date == default)
            {
                date.date = DateTime.Now.ToString();
            }

            DateTime newDate = DateTime.ParseExact(date.date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            // Extract year, month, and day
            int year = newDate.Year;
            int month = newDate.Month;
            int day = newDate.Day;

            OrderStatusDTO orderStatusDTO = new OrderStatusDTO();

            switch (filterBy)
            {
                case 1: // Daily
                default:
                    orderStatusDTO.Pendding = context.Orders.Where(o => o.Date == newDate && o.Status == ProductStatusEnum.Pendding).Count();
                    orderStatusDTO.Rejected = context.Orders.Where(o => o.Date == newDate && o.Status == ProductStatusEnum.Rejected).Count();
                    orderStatusDTO.Completed = context.Orders.Where(o => o.Date == newDate && o.Status == ProductStatusEnum.Approved).Count();
                    break;
                case 2: // monthly
                    orderStatusDTO.Pendding = context.Orders.Where(o => o.Date.Month == month && o.Date.Year == year && o.Status == ProductStatusEnum.Pendding).Count();
                    orderStatusDTO.Rejected = context.Orders.Where(o => o.Date.Month == month && o.Date.Year == year && o.Status == ProductStatusEnum.Rejected).Count();
                    orderStatusDTO.Completed = context.Orders.Where(o => o.Date.Month == month && o.Date.Year == year && o.Status == ProductStatusEnum.Approved).Count();
                    break;
                case 3: // Yearly
                    orderStatusDTO.Pendding = context.Orders.Where(o => o.Date.Year == year && o.Status == ProductStatusEnum.Pendding).Count();
                    orderStatusDTO.Rejected = context.Orders.Where(o => o.Date.Year == year && o.Status == ProductStatusEnum.Rejected).Count();
                    orderStatusDTO.Completed = context.Orders.Where(o => o.Date.Year == year && o.Status == ProductStatusEnum.Approved).Count();
                    break;
            }

            return Ok(orderStatusDTO);
        }


        //--------------------------------------------------------
        [HttpPost("Order/getOrdersInYear")]
        public IActionResult GetOrdersInYear([FromBody] Date date = default) 
        {
            if (date == default)
            {
                date.date = DateTime.Now.ToString();
            }

            DateTime newDate = DateTime.ParseExact(date.date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

          
            int year = newDate.Year;

            var monthlyOrderCounts = new int[12];

          
            var orders = context.Orders
                .Where(o => o.Date.Year == year)
                .ToList();

            foreach (var order in orders)
            {
                int month = order.Date.Month;
                monthlyOrderCounts[month - 1]++; 
            }

            return Ok(monthlyOrderCounts);
        }

        #endregion
    }




}
