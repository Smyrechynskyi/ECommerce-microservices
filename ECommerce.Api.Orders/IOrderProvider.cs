using ECommerce.Api.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders
{
    public interface IOrderProvider
    {
        Task<(bool IsSuccess, IEnumerable<OrderModel> Orders, string ErrorMessage)> GetOrdersByCustomerIdAsync(int customerId);
        Task<(bool IsSuccess, IEnumerable<OrderModel> Orders, string ErrorMessage)> GetOrdersAsync();
        Task<(bool IsSuccess, OrderModel Order, string ErrorMessage)> GetOrderAsync(int id);
    }
}
