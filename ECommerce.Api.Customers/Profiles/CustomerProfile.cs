using AutoMapper;
using ECommerce.Api.Customers.Db.Entities;
using ECommerce.Api.Customers.Models;

namespace ECommerce.Api.Customers.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>();
        }
    }
}
