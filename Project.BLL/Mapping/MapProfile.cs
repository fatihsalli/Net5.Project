using AutoMapper;
using Project.Entity.DTOs;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductWithCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

        }

    }
}
