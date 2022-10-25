using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Repositories.ProductRepository;
using Project.Entity.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET=> www.mysite.com/api/products
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products=_productRepository.GetProductsWithCategory();
            var productsDto=_mapper.Map<List<ProductWithCategoryDto>>(products);
            return Ok(productsDto);
        }



    }
}
