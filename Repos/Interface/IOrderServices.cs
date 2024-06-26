using Restaurant_WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrdersAsync();  // Hämta alla beställningar
        Task<Order> GetOrderByIdAsync(int id); // Hämta en specifik beställning baserad på ID
        Task UpdateOrderAsync(Order order);    // Uppdatera en befintlig beställning
        Task DeleteOrderAsync(int id);         // Ta bort en beställning baserad på ID
        bool OrderExists(int id);              // Kontrollera om en beställning med ett visst ID existerar
    }
}
