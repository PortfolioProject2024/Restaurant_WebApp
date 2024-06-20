using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;
using Restaurant_WebApp.Repos.Services;

namespace Restaurant_WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ApplicationDbContext _db;

        public OrderController(IOrderServices orderServices, ApplicationDbContext db)
        {
            _orderServices = orderServices;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems) 
                    .ThenInclude(oi => oi.FoodItems) 
                .ToListAsync();

            return View(orders);
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.FoodItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
           
            var order = await _db.Orders.FindAsync(id);
            return View(order);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, OrderDate, TotalPrice, Comment, CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(order);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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

            var order = await _db.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _db.Orders.Any(e => e.Id == id);
        }

    }
}
