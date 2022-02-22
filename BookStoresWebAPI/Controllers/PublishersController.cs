using BookStoresWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoresWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublishersController(BookStoresDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return Ok(await _context.Publishers.ToListAsync());
        }

        [HttpGet("GetPublisherDetails/{id}")]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublisherDetails(int id)
        {
            // Eager loading
            //var publisher = _context.Publishers
            //                                              .Include(pub => pub.Books)
            //                                                    .ThenInclude(book => book.Sales)
            //                                                .Include(pub => pub.Users)
            //                                                    .ThenInclude(user => user.Job)
            //                                                .Where(pub => pub.PubId == id)
            //                                                .FirstOrDefault();

            // Explicit loading
            var publisher = await _context.Publishers.SingleAsync(pub => pub.PubId == id);

            _context.Entry(publisher)
                .Collection(pub => pub.Users)
                .Query()
                .Where(usr => usr.EmailAddress.Contains("Karin"))
                .Load();

            _context.Entry(publisher)
                .Collection(pub => pub.Books)
                .Query()
                .Include(book => book.Sales)
                .Load();

            // ont to ont
            //var user = await _context.Users.SingleAsync(usr => usr.UserId == 1);
            //_context.Entry(user)
            //    .Reference(usr => usr.Role)
            //    .Load();

            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        [HttpGet("PostPublisherDetails/")]
        public async Task<ActionResult<IEnumerable<Publisher>>> PostPublisherDetails()
        {
            var publisher = new Publisher();
            publisher.PublisherName = "Harper & Brothers";
            publisher.City = "New York City";
            publisher.State = "NY";
            publisher.Country = "USA";


            //int id = publisher.PubId;

            Book book1 = new Book();
            //book1.PubId = id;
            book1.Title = "Good night Moon-1";
            book1.PublishedDate = DateTime.Now;

            Book book2 = new Book();
            //book2.PubId = id;
            book2.Title = "Good night Moon-2";
            book2.PublishedDate = DateTime.Now;



            //int book1id = book1.BookId;
            //int book2id = book2.BookId;

            Sale sale1 = new Sale();
            //sale1.BookId = book1id;
            sale1.Quantity = 2;
            sale1.StoreId = "8042";
            sale1.OrderNum = "XYZ";
            sale1.PayTerms = "Net 30";
            sale1.OrderDate = DateTime.Now;

            Sale sale2 = new Sale();
            //sale2.BookId = book2id;
            sale2.Quantity = 2;
            sale2.StoreId = "7131";
            sale2.OrderNum = "QA879";
            sale2.PayTerms = "Net 20";
            sale2.OrderDate = DateTime.Now;


            book1.Sales.Add(sale1);
            book2.Sales.Add(sale2);

            publisher.Books.Add(book1);
            publisher.Books.Add(book2);

            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            var publishers = _context.Publishers
                                                          .Include(pub => pub.Books)
                                                                .ThenInclude(book => book.Sales)
                                                            .Include(pub => pub.Users)
                                                                .ThenInclude(user => user.Job)
                                                            .Where(pub => pub.PubId == publisher.PubId)
                                                            .FirstOrDefault();
            if (publishers == null)
            {
                return NotFound();
            }
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.Where(x => x.PubId == id).ToListAsync();
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        [HttpPost]
        public async Task<ActionResult> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); } catch { }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publish = await _context.Publishers.FindAsync(id);
            if (publish == null) { return NotFound(); }
            _context.Publishers.Remove(publish);
            await _context.SaveChangesAsync();
            return Ok(publish);
        }
    }
}