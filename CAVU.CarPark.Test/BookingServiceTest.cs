using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CAVU.CarPark.Abstraction;
using CAVU.CarPark.Abstraction.Data;
using CAVU.CarPark.Abstraction.Service;
using CAVU.CarPark.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CAVU.CarPark.Test
{
    public class BookingServiceTest
    {
        private Mock<IBookingData> _mockBookingData;
        private Mock<ILogger<BookingService>> _mockLogger;
        private IBookingService _bookingService;
        [SetUp]
        public void Setup()
        {
            _mockBookingData = new Mock<IBookingData>();
            _mockLogger = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(_mockLogger.Object, _mockBookingData.Object);
        }

        [Test]
        public async Task NoOfSpaceAvailableTest()
        {
            _mockBookingData
                .Setup(m => m.GetBookingCount())
                .ReturnsAsync(new Dictionary<DateTime, int>(){{DateTime.Now.AddDays(1).Date, 1}, { DateTime.Now.AddDays(2).Date, 5} });

            var noOfSpaceAvailable = await this._bookingService.GetSpaceAvailablePerDate(DateTime.Now.AddDays(1));

            Assert.IsNotNull(noOfSpaceAvailable);
            Assert.AreEqual(9, noOfSpaceAvailable);
        }

        [Test]
        public async Task CheckAvailableTest()
        {
            _mockBookingData
                .Setup(m => m.GetBookingCount())
                .ReturnsAsync(new Dictionary<DateTime, int>() { { DateTime.Now.AddDays(1).Date, 1 }, { DateTime.Now.AddDays(2).Date, 10 } });

            var isSpaceAvailable = await this._bookingService.CheckAvailability(DateTime.Now.AddDays(2), DateTime.Now.AddDays(6));
            
            Assert.IsFalse(isSpaceAvailable);
        }


        [Test]
        public async Task GetPricingTest()
        {
            var fromDate = new DateTime(2023, 06, 16);
            var toDate = new DateTime(2023, 06, 18);

            var price = await this._bookingService.GetPrice(fromDate, toDate);

            Assert.AreEqual(28, price);
        }

        [Test]
        public async Task CreateBookingTest()
        {
            var booking = new Booking(){
                From = new DateTime(2023, 06, 15),
                To = new DateTime(2023,06,20),
                CarRegistration = "AB12CDE"
           };

            _mockBookingData
                .Setup(m => m.GetBookingCount())
                .ReturnsAsync(new Dictionary<DateTime, int>() { { DateTime.Now.AddDays(1).Date, 1 }, { DateTime.Now.AddDays(2).Date, 7 } } );

            _mockBookingData 
                .Setup(m => m.Create(booking))
                .ReturnsAsync(new Booking()
                {
                    BookedDateTime = DateTime.Now,
                    CarRegistration = booking.CarRegistration,
                    From = booking.From, To = booking.To,
                    TotalPrice = await this._bookingService.GetPrice(booking.From, booking.To)
                });


            var newBooking = await this._bookingService.CreateBooking(booking);
           
            Assert.IsNotNull(newBooking);
            Assert.IsNotNull(newBooking.Id);

        }
    }
    
}
