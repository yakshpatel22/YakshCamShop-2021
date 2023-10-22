using Casestudy.DAL.DomainClasses;
using Casestudy.DAL.DAO;
using Casestudy.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Casestudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders!.Where(order => order.CustomerId == id).ToListAsync<Order>();
        }
        public async Task<List<OrderDetailsHelper>> GetOrderDetails(int oid, string email)
        {
            Customer? customer = _db.Customers!.FirstOrDefault(customer => customer.Email == email);
            List<OrderDetailsHelper> allDetails = new();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Orders
                          join oi in _db.OrderLineItems! on o.Id equals oi.OrderId
                          join p in _db.Products! on oi.ProductId equals p.id
                          where (o.CustomerId == customer!.Id && o.Id == oid)
                          select new OrderDetailsHelper
                          {
                              OrderId = o.Id,
                              CustomerID = customer!.Id,
                              ProductName = p.ProductName,
                              QtyOrdered = oi.QtyOrdered,
                              QtySold = oi.QtySold,
                              QtyBackOrdered = oi.QtyBackOrdered,
                              SellingPrice = p.MSRP,
                              ProductId = oi.ProductId,
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = await results.ToListAsync();
            return allDetails;
        }
        /*public List<OrderDetailsHelper> GetOrderDetails(int cid, string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(customer => customer.Email == email);

            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();// LINQ way of doing INNER JOINS 
            var results = from o in _db.Orders
                          join oi in _db.OrderLineItems on o.Id equals oi.OrderId
                          join pi in _db.Products on oi.ProductId equals pi.id
                          where (o.CustomerId == customer.Id && o.Id == cid)
                          select new OrderDetailsHelper //create a new helperdetails for the details of a previous order
                          {
                              OrderId = o.Id,
                              CustomerId = customer.Id,
                              ProductId = pi.id,
                              Description = pi.Description,
                              ProductName = pi.ProductName,
                              CostMSRP = (float)pi.MSRP,
                              QTYSold = oi.QtySold,
                              QTYOrdered = oi.QtyOrdered,
                              QTYBacked = oi.QtyBackOrdered,
                              Subtotal = o.OrderAmount,
                              QTY = oi.QtyOrdered,
                              Tax = o.OrderAmount * (decimal)0.13,
                              OrderTotal = o.OrderAmount + (o.OrderAmount * (decimal)0.13),
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = results.ToList<OrderDetailsHelper>();
            return allDetails;
        }*/
        public async Task<int> AddOrder(int CustomerId, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            // we need a transaction as multiple entities involved
            using (var _trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    Order order = new();
                    order.CustomerId = CustomerId;
                    order.OrderDate = System.DateTime.Now;
                    order.OrderAmount = 0.0M;
                    // calculate the totals and then add the order row to the table
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        order.OrderAmount += selection.Item!.MSRP * selection.Qty;
                    }

                    await _db.Orders!.AddAsync(order);
                    await _db.SaveChangesAsync();
                    // then add each item to the orderitems table
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        OrderLineItem oItem = new();
                        oItem.Product = selection.Item!;
                        oItem.ProductId = selection.Item!.id;
                        oItem.SellingPrice = selection.Item!.MSRP * (decimal)selection.Qty;
                        oItem.OrderId = order.Id;
                        if (selection.Qty <= oItem.Product!.QtyOnHand)
                        {
                            oItem.Product!.QtyOnHand -= selection.Qty;
                            oItem.QtySold = selection.Qty;
                            oItem.QtyBackOrdered = 0;
                            oItem.QtyOrdered = selection.Qty;
                        }
                        else
                        {
                            oItem.QtySold = oItem.Product!.QtyOnHand;
                            oItem.Product!.QtyOnBackOrder += (selection.Qty - oItem.Product!.QtyOnHand);
                            oItem.QtyBackOrdered += (selection.Qty - oItem.Product!.QtyOnHand);
                            oItem.Product!.QtyOnHand = 0;
                            oItem.QtyOrdered = selection.Qty;
                        }

                        await _db.OrderLineItems!.AddAsync(oItem);
                        _db.Products!.Update(oItem.Product!);
                        await _db.SaveChangesAsync();
                    }
                    // test trans by uncommenting out these 3 lines
                    //int x = 1;
                    //int y = 0;
                    //x = x / y;
                    await _trans.CommitAsync();
                    orderId = order.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await _trans.RollbackAsync();
                }
            }
            return orderId;
        }
    }
}