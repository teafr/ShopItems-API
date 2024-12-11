using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ShopItems_API.Models;
using DataLibrary;

namespace ShopItems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDataAccess _dataAccess;
        private readonly string _connectionString;

        private List<Item> _items = new List<Item>();

        public ItemController(IConfiguration configuration) 
        { 
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("default")!;
            _dataAccess = new DataAccess();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllItems()
        {
            string sql = "select * from item";
            _items = await _dataAccess.LoadData<Item, dynamic>(sql, new { }, _connectionString);
            return Ok(_items);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                string sql = "select * from item where Id = @Id";
                Item item = (await _dataAccess.LoadData<Item, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;

                if (item == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode = 500,
                        message = ex.Message
                    });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateItem([FromBody] Item newItem)
        {
            try
            {
                if (newItem == null)
                    return BadRequest(new { StatusCode = 400, message = "Item wasn't described properly" });

                string sql = "select * from item";
                _items = await _dataAccess.LoadData<Item, dynamic>(sql, new { }, _connectionString);
                newItem.Id = _items.Count != 0 ? _items.Max(o => o.Id) + 1 : 1;

                sql = "insert into item (Id, Name, Description, Price) values (@Id, @Name, @Description, @Price)";
                await _dataAccess.SaveData<dynamic>(sql, new { newItem.Id, newItem.Name, newItem.Description, newItem.Price }, _connectionString);
                
                return CreatedAtAction(nameof(GetItemById), new { id = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode = 500,
                        message = ex.Message
                    });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItemById(int id, [FromBody] Item newItem)
        {
            try
            {
                if (newItem == null)
                    return BadRequest(new { StatusCode = 400, message = "Item wasn't described properly" });

                string sql = "select * from item where Id = @Id";
                Item item = (await _dataAccess.LoadData<Item, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;
                
                if (item == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                sql = "update item set Name = @Name, Description = @Description, Price = @Price where Id = @Id";
                await _dataAccess.SaveData<dynamic>(sql, new { newItem.Name, newItem.Description, newItem.Price, Id = id }, _connectionString);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode = 500,
                        message = ex.Message
                    });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            try
            {
                string sql = "select * from item where Id = @Id";
                Item item = (await _dataAccess.LoadData<Item, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;

                if (item == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                sql = "delete from item where Id = @Id";
                await _dataAccess.SaveData<dynamic>(sql, new { Id = id }, _connectionString);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        statusCode = 500,
                        message = ex.Message
                    });
            }
        }
    }
}
