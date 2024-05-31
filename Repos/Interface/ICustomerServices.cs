using Restaurant_WebApp.Models;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface ICustomerServices
    {
        Task<List<Customer>> GetAllCustomersAsync();

        Task<Customer> AddCustomerAsync(Customer customer);

        Task<bool> UpdateCustomerAsync(Customer customer);
        
        Task<Customer> GetCustomerByIdAsync(int id);

        Task<Customer> DeleteUserAsync(int id);


    }
}
