using System;
using System.Collections.Generic;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here         
        {
            _ctx = context;
        }
        [Route("{email}")]
   
        [HttpGet]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> carts; ;
            CustomerDAO cDao = new(_ctx);
            Customer? cartOwner = await cDao.GetByEmail(email);
            OrderDAO oDao = new(_ctx);
            carts = await oDao.GetAll(cartOwner!.Id);
            return carts;
        }
        [Route("{orderid}/{email}")]
       
        [HttpGet]
        public async Task<ActionResult<List<OrderDetailsHelper>>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new(_ctx);
            return await dao.GetOrderDetails(orderid, email);
        }
        /* [HttpGet("{orderid}/{email}")]
         public ActionResult<List<OrderDetailsHelper>> GetCartDetails(int orderid, string email)
         {
             OrderDAO dao = new OrderDAO(_ctx);
             return dao.GetOrderDetails(orderid, email);
         }*/
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal;
            try
            {
                CustomerDAO uDao = new(_ctx);
                Customer? orderOwner = await uDao.GetByEmail(helper.Email);
                OrderDAO oDao = new(_ctx);
                int orderId = await oDao.AddOrder(orderOwner!.Id, helper.selections!);
                retVal = orderId > 0
                ? "Order " + orderId + " Created! Goods backordered!"
                : "Order not created";
            }
            catch (Exception ex)
            {
                retVal = "Order not created " + ex.Message;
            }
            return retVal;
        }
    }
}
