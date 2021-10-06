using System;
using System.Collections.Generic;

namespace ECommerce.Api.Orders.Db.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }
}
