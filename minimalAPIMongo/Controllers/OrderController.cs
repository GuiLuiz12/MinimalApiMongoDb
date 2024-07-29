using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
                //var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
                return order is not null ? Ok(order) : NotFound();
                //return Ok(filter);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Order order)
        {
            try
            {
                await _order.InsertOneAsync(order);
                return StatusCode(201, order);
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
                var filter = Builders<Order>.Filter.Eq("Id", id);

                if (filter != null)
                {
                    await _order.DeleteOneAsync(filter);

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
        public async Task<ActionResult> Update(Order o)
        {
            try
            {
                //buscar por id
                var filter = Builders<Order>.Filter.Eq(x => x.Id, o.Id);

                if (filter != null)
                {
                    //substituindo o objeto buscado pelo novo objeto
                    await _order.ReplaceOneAsync(filter, o);

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
