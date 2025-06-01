using DataTracking.Data;
using DataTracking.DTO;
using DataTracking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace DataTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<productController> _logger;
        public productController(DatabaseContext db,ILogger<productController> logger)
        {
            _db = db;        
            _logger = logger;   
        }
        [HttpGet]
        public async Task<IActionResult> getData()
        {
            try
            {             
                var data = await _db.products.ToListAsync();            
                _logger.LogInformation("Retrieved all products.{@Products}", data);
                return Ok(data);
             
            }
            catch
            {
                return BadRequest("error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> addData(productDTO DTO) {
            try
            {
                var product = new Products { name = DTO.name, price = DTO.price, stock = DTO.stock };
                _db.products.Add(product);
                await _db.SaveChangesAsync();
                //[command]: ("{@_val}",val) = object | ("{_val}",val) = string
                _logger.LogInformation("Create new product {@Product}",product);
                return Ok("success");
            }
            catch(Exception ex)
            {              
                return BadRequest("error");
            }
        }
        [HttpPut]
        public async Task<IActionResult> updateData(Guid id,productDTO DTO)
        {
            try
            {
                var product = _db.products.Find(id);
                if (product is null) { return NotFound(); }           
                _db.Entry(product).CurrentValues.SetValues(DTO);                   
                await _db.SaveChangesAsync();               
                return Ok("success");
            }
            catch(Exception ex)
            {             
                return BadRequest("error");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> deleteData(Guid id)
        {
            try
            {
                var product = _db.products.Find(id);
                if (product is null) return NotFound("not found");
                _db.products.Remove(product);
                await _db.SaveChangesAsync();
                return Ok("success");
            }
            catch
            {
                return BadRequest("error");
            }
        }

    }
}
