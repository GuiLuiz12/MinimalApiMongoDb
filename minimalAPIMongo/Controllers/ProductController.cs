using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();
                //var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
                return product is not null ? Ok(product) : NotFound();
                //return Ok(filter);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);                
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    product.Id = ObjectId.GenerateNewId().ToString();
                }
                await _product.InsertOneAsync(product);
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq("Id", id);
                
                if (filter != null)
                {
                    await _product.DeleteOneAsync(filter);

                    return Ok();
                }
                return NotFound();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product p)
        {
            try
            {
                //buscar por id
                var filter = Builders<Product>.Filter.Eq(x => x.Id, p.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _product.ReplaceOneAsync(filter,p);

                    return Ok();
                }

                return NotFound();
              
            }
            catch (Exception)
            {
                return BadRequest();                
            }
        }
    }
}
