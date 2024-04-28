using Cafe.DTOs;
using Cafe.Helpers;
using Cafe.Implementations;
using Cafe.Interfaces;
using Cafe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = WebSiteRoles.SiteDelivery)]

    public class DeliveryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDBContext context;

        public DeliveryController(IUnitOfWork _unitOfWork,  UserManager<ApplicationUser> _userManager, ApplicationDBContext _context)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            context = _context;
        }


        [HttpGet("Order/{selectedOrder}")]
        public IActionResult GetAllOrder(int selectedOrder) 
        {
            if (User.Identity.IsAuthenticated)
            {
                // Retrieve the user ID from the ClaimsPrincipal
                string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var delivery = unitOfWork.Delivery.GetAll(d => d.AppUserId == userId, null, "").FirstOrDefault();

                var orders = new List<Order>();

                if (selectedOrder == 1)
                {
                     orders = (List<Order>)unitOfWork.Order.GetAll(o => o.DeliveryId == delivery.Id && o.DeliveryStatus != DeliveryStatusEnum.Done, null, "AppUser,Address");
                }
                else if(selectedOrder == 2)
                {
                     orders = (List<Order>)unitOfWork.Order.GetAll(o => o.DeliveryId == delivery.Id && o.DeliveryStatus == DeliveryStatusEnum.Done, null, "AppUser,Address");
                }

                List<GetOrderDTO> orderDTOs = new List<GetOrderDTO>();

                foreach (var order in orders)
                {
                    orderDTOs.Add(
                    new GetOrderDTO()
                    {
                        Id = order.Id,
                        Date = order.Date,
                        deliveryStatus = Enum.GetName(typeof(DeliveryStatusEnum), order.DeliveryStatus),
                        OrderTotals = order.Total,
                        UserFullName = order.AppUser.FirstName + " " + order.AppUser.LastName,
                        Address = order.Address.State + " , " + order.Address.City + " , " + order.Address.Street,
                        Phone = order.Address.Phone.ToString()
                    });
                }
                return Ok(orderDTOs);

                

                
            }

            return BadRequest("Unotheraize");

            
        }


        [HttpGet("Order/onWay/{id}")]
        public IActionResult OnWayOrder(int id)
        {

            Order order = unitOfWork.Order.GetById(id);

            if (order != null)
            {
                if (order.DeliveryStatus == DeliveryStatusEnum.Pendding)
                {
                    order.DeliveryStatus = DeliveryStatusEnum.OnWay;
                    bool res = unitOfWork.Order.Update(order);
                    if (res)
                    {
                        return Ok(new { message = "Order OnWay Mode" });
                    }
                    else
                    {
                        return BadRequest("Order can’t be onWay");
                    }
                }
                else
                {
                    return BadRequest("SomeTing Wrong");
                }
            }
            else
            {
                return BadRequest("Order Not Found");
            }

        }

        [HttpGet("Order/done/{id}")]
        public IActionResult DoneOrder(int id)
        {

            Order order = unitOfWork.Order.GetById(id);

            if (order != null)
            {
                if (order.DeliveryStatus == DeliveryStatusEnum.OnWay)
                {
                    order.DeliveryStatus = DeliveryStatusEnum.Done;
                    bool res = unitOfWork.Order.Update(order);
                    if (res)
                    {
                        return Ok(new { message = "Order Done Mode" });
                    }
                    else
                    {
                        return BadRequest("SomeTing Wrong");
                    }
                }
                else
                {
                    return BadRequest("Order can’t be Done");
                }
            }
            else
            {
                return BadRequest("Order Not Found");
            }

        }


    }
}
