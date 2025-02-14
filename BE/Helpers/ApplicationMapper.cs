using AutoMapper;
using BE.Data;
using BE.Models;

namespace BE.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}