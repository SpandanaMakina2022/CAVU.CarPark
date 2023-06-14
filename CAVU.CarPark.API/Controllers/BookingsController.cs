using CAVU.CarPark.Abstraction;
using CAVU.CarPark.Abstraction.Service;
using CAVU.CarPark.API.Utilities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CAVU.CarPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            ILogger<BookingsController> logger,
            IBookingService bookingService)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }


        // GET api/<BookingsController>/checkAvailable/20-06-2023/25-06-2023
        [HttpGet("checkAvailable/{fromDate}/{toDate}")]
        public async Task<ActionResult<bool>> CheckAvailability(DateTime fromDate, DateTime toDate)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.CheckAvailability(fromDate, toDate));
        }

        // GET api/<BookingsController>/getPrice/20-06-2023/25-06-2023
        [HttpGet("getPrice/{fromDate}/{toDate}")]
        public async Task<ActionResult<decimal>> GetPrice(DateTime fromDate, DateTime toDate)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.GetPrice(fromDate, toDate));
        }

        // GET api/<BookingsController>/20-06-2023
        [HttpGet("noOfSpaces/{date}")]
        public async Task<ActionResult<int>> Get(DateTime date)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.GetSpaceAvailablePerDate(date));
        }

        // GET api/<BookingsController>/guidId
        [HttpGet()]
        public async Task<ActionResult<List<Booking>>> Get()
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.GetBookings());
        }

        // GET api/<BookingsController>/guidId
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> Get(Guid id)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.GetBooking(id));
        }

        // POST api/<BookingsController>
        [HttpPost]
        public async Task<ActionResult<Booking>> Post([FromBody] Booking booking)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.CreateBooking(booking));
        }

        // PUT api/<BookingsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Booking>> Put(Guid id, [FromBody] Booking booking)
        {
            booking.Id = id;
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.UpdateBooking(booking));
        }

        // DELETE api/<BookingsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            return await this.ExecuteAsync(this._logger, this._bookingService, async (client) => await client.CancelBooking(id));
        }
    }
}
