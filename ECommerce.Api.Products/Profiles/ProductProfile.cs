using AutoMapper;
using ECommerce.Api.Products.Db.Entities;
using ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
        }
    }
}
