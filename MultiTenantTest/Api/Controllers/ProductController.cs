using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantTest.Servicios.Common;
using MultiTenantTest.Servicios.Product;
using MultiTenantTest.Servicios.Product.Dtos;

namespace MultiTenantTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet("{slugTenant}/productos")]
        public async Task<IActionResult> GetProductos(string slugTenant)
        {
            var result = ValidateToken(slugTenant);
            if (!result.Success)
                return BadRequest(new ServiceResponse<IEnumerable<ProductDto>>
                {
                    Message = result.Message
                });

            var resultProcess = await _productServices.GetProducts(slugTenant);

            if (resultProcess.Success)
            {
                return Ok(resultProcess);
            }
            return BadRequest(resultProcess);
        }

        [HttpPost("{slugTenant}/productos")]
        public async Task<IActionResult> Insert(string slugTenant, ProductDto product)
        {
            var result = ValidateToken(slugTenant);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            var resultProcess = await _productServices.Insert(product, slugTenant);

            if (resultProcess.Success)
            {
                return Ok(resultProcess);
            }
            return BadRequest(resultProcess);
        }


        [HttpPut("{slugTenant}/productos")]
        public async Task<IActionResult> Update(string slugTenant, ProductDto product)
        {
            var result = ValidateToken(slugTenant);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            var resultProcess = await _productServices.Update(product, slugTenant);
            if (resultProcess.Success)
            {
                return Ok(resultProcess);
            }
            return BadRequest(resultProcess);

        }

        private ServiceResponse<bool> ValidateToken(string slugTenant)
        {
            var claims = User.Claims.ToList();
            var tieneElMismoSlug = claims.FirstOrDefault(x => x.Type == "tenant");
            if (string.IsNullOrWhiteSpace(tieneElMismoSlug?.Value) || tieneElMismoSlug.Value != slugTenant)
            {
                return new ServiceResponse<bool>
                {
                    Message = "El token correspondiente no pertenece al Slugtenant ingresado"
                };
            }
            return new ServiceResponse<bool>
            {
                Data = true,
                Success = true,
            };
        }
    }
}