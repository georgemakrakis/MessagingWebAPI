using System.Collections.Generic;
using System.Linq;
using MessagesWebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MessagesWebApi.Controllers
{
    [Route("api/messages")]
    public class MessagesController : Controller
    {
        private readonly MessageContext _context;

        // Here we define an empty controller class, use Dependency Injection to inject the database context
        // and add an item to the in-memory database if one doesn't exist.
        public MessagesController(MessageContext context)
        {
            _context = context;

            if (_context.Messages.Count() <= 0)
            {
                _context.Messages.Add(new MessageItem() { Message = "Hello world!"});
                _context.SaveChanges();
            }
        }

        // The GetAll method returns an IEnumerable.MVC automatically serializes the object to JSON and writes the JSON into the body of the response message
        [HttpGet]
        public IEnumerable<MessageItem> GetAll()
        {
            return _context.Messages.ToList();
            
        }

        // The GetById method returns the more general IActionResult type, which represents a wide range of return types
        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult GetById(long id)
        {
            var item = _context.Messages.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        // This is an HTTP POST method, indicated by the [HttpPost] attribute. The [FromBody] attribute tells MVC to get the value of the to-do item 
        // from the body of the HTTP request.
        [HttpPost]
        public IActionResult Create([FromBody] MessageItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Messages.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetMessage", new { id = item.Id }, item);
        }

        // Update is similar to Create, but uses HTTP PUT
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] MessageItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var message = _context.Messages.FirstOrDefault(t => t.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            message.IsSeen = item.IsSeen;
            message.Message = item.Message;

            _context.Messages.Update(message);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // Our delete method
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var message = _context.Messages.FirstOrDefault(t => t.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
