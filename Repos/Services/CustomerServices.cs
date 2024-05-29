using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Restaurant_WebApp.Data;
using Restaurant_WebApp.Models;
using Restaurant_WebApp.Repos.Interface;

namespace Restaurant_WebApp.Repos.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ApplicationDbContext _db;

        public CustomerServices(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Customer> AddCustomerAsync(Customer newCustomer, int orderId)
        {
            _db.Customers.Add(newCustomer);
            await _db.SaveChangesAsync();

            if (orderId != 0)
            {
                var order = await _db.Orders.FindAsync(orderId);
                if (order != null) 
                {
                    if (newCustomer.Orders == null)
                    {
                        newCustomer.Orders = new List<Order>();
                    }
                    newCustomer.Orders.Add(order);
                    order.CustomerId = newCustomer.Id;
                    await _db.SaveChangesAsync();
                }

            }
            return newCustomer;
        }

        public async Task<Customer> DeleteUserAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);

            if (customer == null) 
            {
                throw new Exception("Customer not found");
            }
            _db.Customers.Remove(customer); 
            await _db.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customerList = await _db.Customers.Include(c => c.Orders).ToListAsync();
            return customerList;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
           var customer = await _db.Customers.Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) 
            {
                throw new Exception("Customer not found");
            }

            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                var custInDb = await _db.Customers.FirstOrDefaultAsync(c => c.Id ==  customer.Id);
                if (custInDb == null) 
                {
                    throw new Exception("Customer not found");
                }
                // Update properties
                custInDb.Name = customer.Name;
                custInDb.Email = customer.Email;
                custInDb.PhoneNumber = customer.PhoneNumber;
                custInDb.DiscountPercentage = customer.DiscountPercentage;
                custInDb.Orders = customer.Orders;
                custInDb.RewardPoints = customer.RewardPoints;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                // Handle concurrency exception
                return false;
            }

        }
    }
}
