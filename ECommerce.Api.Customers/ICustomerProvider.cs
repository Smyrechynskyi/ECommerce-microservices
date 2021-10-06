using ECommerce.Api.Customers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers
{
    public interface ICustomerProvider
    {
        Task<(bool IsSuccess, CustomerModel Customer, string ErrorMessage)> GetCustomerAsync(int id);

        Task<(bool IsSuccess, IEnumerable<CustomerModel> Customers, string ErrorMessage)> GetCustomersAsync();
    }
}