﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	[ApiController()]
	[Route("api/v1/[controller]")]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly ILogger<CatalogController> _logger;

		public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productRepository.GetProductsAsync();
			return Ok(products);
		}

		[HttpGet("{id:length(24)}", Name = "GetProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<Product>> GetProductById(string id)
		{
			var product = await _productRepository.GetProductAsync(id);
			if(product == null)
			{
				_logger.LogError($"Product with id: {id}, not found");
				return NotFound();
			}
			return Ok(product);
		}

		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
		{
			var products = await _productRepository.GetProductsByCategoryAsync(category);
			return Ok(products);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _productRepository.CreateProductAsync(product);
			return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			return Ok(await _productRepository.UpdateProductAsync(product));
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteProductById(string id)
		{
			return Ok(await _productRepository.DeleteProductAsync(id));
		}
	}
}
