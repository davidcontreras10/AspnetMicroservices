using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext _context;

		public ProductRepository(ICatalogContext context)
		{
			_context = context;
		}

		public async Task CreateProductAsync(Product product)
		{
			await _context.Products.InsertOneAsync(product);
		}

		public async Task<bool> DeleteProductAsync(string id)
		{
			var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
			var deleteResult = await _context
				.Products
				.DeleteOneAsync(filter);
			return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
		}

		public async Task<Product> GetProductAsync(string id)
		{
			return await _context
				.Products
				.Find(p=>p.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			return await _context
				.Products
				.Find(Builders<Product>.Filter.Empty)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
		{
			var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
			return await _context
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
			return await _context
				.Products
				.Find(filter)
				.ToListAsync();
		}

		public async Task<bool> UpdateProductAsync(Product product)
		{
			var updateResult = await _context
				.Products
				.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}
	}
}
