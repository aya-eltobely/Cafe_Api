using Cafe.DTOs;
using Cafe.Helpers;
using Cafe.Implementations;
using Cafe.Interfaces;
using Cafe.Migrations;
using Cafe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = WebSiteRoles.SiteUser)]

    public class UserController : ControllerBase
    {
        private  IUnitOfWork unitOfWork;
        private  IImageService imageService;
        private  UserManager<ApplicationUser> userManager;
        private  ApplicationDBContext context;

        public UserController(IUnitOfWork _unitOfWork, IImageService _imageService, UserManager<ApplicationUser> _userManager,ApplicationDBContext _context)
        {
            unitOfWork = _unitOfWork;
            imageService = _imageService;
            userManager = _userManager;
            context = _context ;
        }

        #region Order

        ///////////////////////////////////////////////////////////////////////////
        ///

        [AllowAnonymous]
        [HttpGet("product")]
        public IActionResult GetAllproduct(int subcategoryId,int pagenumber, int pagesize, string? search = null)
        {
            PageResult<Product> products = unitOfWork.Product.GetAll(subcategoryId,pagenumber, pagesize, null, search);

            List<GetProductDTO> productDTOs = new List<GetProductDTO>();

            foreach (var prod in products.Data)
            {
                Image img = unitOfWork.Image.GetById(prod.ImageId);

                productDTOs.Add(
                new GetProductDTO()
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Price = prod.Price,
                    Description = prod.Description,
                    subCategoryName = prod.SubCategory.Name,
                    subCategoryId = prod.SubCategoryId,
                    FileName = img.FileName,
                    ContentType = img.ContentType,
                    ImageData = img.ImageData
                });
            }
            //return Ok(productDTOs);
            return Ok(new PageResult<GetProductDTO>() {
                Data = productDTOs,
                PageNumber = products.PageNumber,
                PageSize = products.PageSize,
                TotalItem = products.TotalItem

            });


        }


        [AllowAnonymous]

        [HttpGet("product/{id}")]
        public IActionResult GetProductById(int id)
        {
            Product prod = unitOfWork.Product.GetAll(p => p.Id == id, null, "Image,SubCategory").SingleOrDefault();

            //Image img = unitOfWork.Image.GetById(prod.ImageId);


            GetProductDTO productDTO = new GetProductDTO()
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                Description = prod.Description,
                subCategoryName = prod.SubCategory.Name,
                subCategoryId = prod.SubCategoryId,
                FileName = prod.Image.FileName,
                ContentType = prod.Image.ContentType,
                ImageData = prod.Image.ImageData
            };

            //return Ok(productDTOs);
            return Ok(productDTO);


        }

        [AllowAnonymous]

        //// get all categories
        [HttpGet("Category")]
        public IActionResult GetAllCategory()
        {
            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            List<GetCategoryDTO> categoriesDTO = new List<GetCategoryDTO>();

            foreach (var category in categories)
            {
                categoriesDTO.Add(new GetCategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDTO);
        }

        [AllowAnonymous]

        // get all subcategory for spesific category
        [HttpGet("subCategory/{categoryId}")]
        public IActionResult GetAllSubCategory(int categoryId)
        {
            var subCategories = unitOfWork.SubCategory.GetAll(s => s.CategoryId == categoryId);
            List<GetSubCategoryDTO> subCategoryDTOs = new List<GetSubCategoryDTO>();

            foreach (var subcat in subCategories)
            {
                subCategoryDTOs.Add(new GetSubCategoryDTO()
                {
                    Id = subcat.Id,
                    Name = subcat.Name,
                    CategoryId = subcat.CategoryId,
                    CategoryName = subcat.Category.Name,
                    FileName = subcat.Image.FileName,
                    ImageData = subcat.Image.ImageData,
                    ContentType = subcat.Image.ContentType
                });
                
            }
            return Ok(subCategoryDTOs);
        }


        // place order
        [HttpPost("order")]
        public IActionResult placeOrder([FromBody] orderDTO orders)
        {
            if (ModelState.IsValid)
            {

                using var transaction = context.Database.BeginTransaction();

                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    //save in orders table
                    Order order = new Order()
                    {
                        Date = DateTime.Now,
                        Status = ProductStatusEnum.Pendding,
                        DeliveryStatus = DeliveryStatusEnum.Waiting,
                        Total = orders.total,
                        UserId = userId,
                        //AddressId = address.Id,
                        DeliveryId = null
                    };
                    unitOfWork.Order.Create(order);

                    //save in address table
                    Address address = new Address()
                    {
                        Name = orders.name,
                        Email = orders.email,
                        City = orders.city,
                        State = orders.state,
                        Street = orders.street,
                        Phone = orders.phone,
                        OrderId = order.Id

                    };
                    unitOfWork.Address.Create(address);

                    

                    //save in orderItems table
                    List<OrderItem> orderItems = new List<OrderItem>();
                    foreach (var item in orders.items)
                    {
                        orderItems.Add(
                            new OrderItem()
                            {
                                ProductId = item.productId,
                                SubTotal = item.subTotal,
                                Quantity = item.quantity,
                                OrderId = order.Id
                            });
                    };
                    unitOfWork.OrderItem.CreateRange(orderItems);


                    context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "Order Created" });

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


        // get all current and completed orders
        [HttpGet("currCompOrder")]
        public IActionResult getAllCurrentAndCompletedOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var productStatusNames = Enum.GetNames(typeof(ProductStatusEnum));
            var deliveryStatusNames = Enum.GetNames(typeof(DeliveryStatusEnum));

            var ordersQuery = context.Orders
                .Where(o => o.UserId == userId && (o.Status == ProductStatusEnum.Pendding || o.Status == ProductStatusEnum.Approved))
                .Select(o => new
                {
                    OrderId = o.Id,
                    OrderDate = o.Date.Date,
                    OrderTotal = o.Total,
                    OrderStatus = productStatusNames[(int)o.Status],
                    Deliverystatus = deliveryStatusNames[(int)o.DeliveryStatus],
                    Products = o.OrderItems.Select(oi => new
                    {
                        ProductName = oi.Product.Name,
                        ImageFileName = oi.Product.Image.FileName,
                        ImageContentType = oi.Product.Image.ContentType,
                        ImageDataa = oi.Product.Image.ImageData,
                        TotalQuantity = oi.Quantity,
                        TotalSubTotal = oi.SubTotal
                    }).ToList()
                }).OrderByDescending(o => o.OrderId);

            return Ok(ordersQuery.ToList());
        }


        // get all Rejected  orders
        [HttpGet("rejectedOrder")]
        public IActionResult getAllRejectedOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var productStatusNames = Enum.GetNames(typeof(ProductStatusEnum));
            var deliveryStatusNames = Enum.GetNames(typeof(DeliveryStatusEnum));

            var ordersQuery = context.Orders
                .Where(o => o.UserId == userId && (o.Status == ProductStatusEnum.Rejected || o.DeliveryStatus == DeliveryStatusEnum.DeclineByUser))
                .Select(o => new
                {
                    OrderId = o.Id,
                    OrderDate = o.Date.Date,
                    OrderTotal = o.Total,
                    OrderStatus = productStatusNames[(int)o.Status],
                    Deliverystatus = deliveryStatusNames[(int)o.DeliveryStatus],
                    Products = o.OrderItems.Select(oi => new
                    {
                        ProductName = oi.Product.Name,
                        ImageFileName = oi.Product.Image.FileName,
                        ImageContentType = oi.Product.Image.ContentType,
                        ImageDataa = oi.Product.Image.ImageData,
                        TotalQuantity = oi.Quantity,
                        TotalSubTotal = oi.SubTotal
                    }).ToList()
                }).OrderByDescending(o=>o.OrderId);

            return Ok(ordersQuery.ToList());
        }


        // Decline Order bu user
        [HttpPut("declineOrder")]
        public IActionResult declineOrder( [FromBody] int orderId)
        {

            var order = unitOfWork.Order.GetById(orderId);
            if(order.Status == ProductStatusEnum.Pendding)
            {
                order.Status = ProductStatusEnum.Rejected;
                order.DeliveryStatus = DeliveryStatusEnum.DeclineByUser;
                unitOfWork.Order.Update(order);
                return Ok(new { message = "Order Declined" });

            }
            return BadRequest("Cant Decline Order");



        }
        #endregion

    }
}









