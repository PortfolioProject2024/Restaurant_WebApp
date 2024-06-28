using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Controllers
{
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IOrderServices _orderServices;

        public AdminOrderController(IOrderServices orderServices, ApplicationDbContext db)
        {
            _orderServices = orderServices;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderServices.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderServices.GetOrderByIdAsync(id.Value);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderServices.GetOrderByIdAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, OrderDate, TotalPrice, SpecialComment")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderServices.UpdateOrderAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_orderServices.OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderServices.GetOrderByIdAsync(id.Value);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderServices.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult AddAdminComment(int orderId, string adminComment)
        {
            
            var order = _db.Orders.Find(orderId);
            if (order != null)
            {
                order.SpecialComment = adminComment;
                _db.SaveChanges();
            }

            return RedirectToAction("Index"); 
        }
        
        [HttpPost]
        public IActionResult MarkCompleted(int orderId)
        {
            var order = _db.Orders.Find(orderId);
            if (order != null)
            {
                order.IsCompleted = true;
                order.CompletedTimestamp = DateTime.Now;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}
