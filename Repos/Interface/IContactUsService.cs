﻿using Restaurant_WebApp.Models;
using System.Threading.Tasks;

namespace Restaurant_WebApp.Repos.Interface
{
    public interface IContactUsService 
    {
        Task<List<ContactUs>> GetAllMessagesAsync();

        Task<ContactUs> DeleteMessageAsync(int id);

        Task<ContactUs> GetMessageByIdAsync(int id);

        Task AddMessageAsync(ContactUs contact);
        Task<int> GetMessagesCountAsync();


    }
}
