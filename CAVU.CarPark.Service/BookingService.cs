using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAVU.CarPark.Abstraction;
using CAVU.CarPark.Abstraction.Data;
using CAVU.CarPark.Abstraction.Service;
using Microsoft.Extensions.Logging;

namespace CAVU.CarPark.Service
{
    public class BookingService: IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly IBookingData _bookingData;
        private readonly int _totalSpacesAvailable = 10;

        //Per day prices
        private readonly int _summerWeekdayPrice = 8;

        private readonly int _summerWeekendPrice = 10;

        private readonly int _winterWeekdayPrice = 6;

        private readonly int _winterWeekendPrice = 8;

        public BookingService(
            ILogger<BookingService> logger,
            IBookingData bookingData)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._bookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
        }

        public async Task<bool> CheckAvailability(DateTime from, DateTime to)
        {
            return await this.SafeExecuteAsync(async () =>
            {
                var bookingCounts = await this._bookingData.GetBookingCount();
                var loopCount = (to - from).Days;
                for (var i = 0; i <= loopCount; i++)
                {
                    var key = from.AddDays(i).Date;
                    if (bookingCounts.ContainsKey(key) && bookingCounts[key] >= this._totalSpacesAvailable)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        public async Task<decimal> GetPrice(DateTime from, DateTime to)
        {
            return await this.SafeExecuteAsync(async () =>
            {
                var loopCount = (to - from).Days;
                decimal totalPrice = 0;
                for (var i = 0; i <= loopCount; i++)
                {
                    totalPrice += this.GetDatePrice(from.AddDays(i));
                }

                return totalPrice;
            });
        }

        

        public async Task<int> GetSpaceAvailablePerDate(DateTime date)
        {
            return await this.SafeExecuteAsync(async () =>
            {
                var bookingCounts = await this._bookingData.GetBookingCount();
                var key = date.Date;
                return bookingCounts.ContainsKey(key)
                    ? this._totalSpacesAvailable - bookingCounts[key]
                    : this._totalSpacesAvailable;
            });
        }

        public async Task<List<Booking>> GetBookings()
        {
            return await this.SafeExecuteAsync(async () => await this._bookingData.Get());
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            return await this.SafeExecuteAsync(async () =>
            {
                //Do validations for booking
                await this.ValidateBooking(booking);

                booking.TotalPrice = await this.GetPrice(booking.From, booking.To);

                return await this._bookingData.Create(booking);

            });
        }

        public async Task<Booking> GetBooking(Guid bookingId)
        {
            return await this.SafeExecuteAsync(async () => await this._bookingData.Get(bookingId));
        }

        
        public async Task<bool> CancelBooking(Guid bookingId)
        {
            return await this.SafeExecuteAsync(async () => await this._bookingData.Delete(bookingId));
        }

        public async Task<Booking> UpdateBooking(Booking booking)
        {
            return await this.SafeExecuteAsync(async () =>
            {
                await this.ValidateBooking(booking);

                booking.TotalPrice = await this.GetPrice(booking.From, booking.To);

                return await this._bookingData.Update(booking);

            });
        }


        private async Task ValidateBooking(Booking booking)
        {
            if (string.IsNullOrEmpty(booking.CarRegistration))
            {
                throw new Exception("Please provide car registration number");
            }
            if (booking.From > booking.To)
            {
                throw new Exception("From date should be less than To date");
            }
            if ((booking.To - booking.From).Days > this._totalSpacesAvailable)
            {
                throw new Exception("Total bookings days cannot execeed 10");
            }

            if (!await this.CheckAvailability(booking.From, booking.To))
            {
                throw new Exception("Spaces are not available for the provided dates");
            }
        }

        private decimal GetDatePrice(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            if (date.Month > 9 || date.Month < 4)
            {

                return (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) ? _winterWeekendPrice : _winterWeekdayPrice;
            }
            else
            {
                return (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) ? _summerWeekendPrice : _summerWeekdayPrice;
            }

        }

        private async Task<ReturnType> SafeExecuteAsync<ReturnType>(Func<Task<ReturnType>> runnable)
        {
            try
            {
                return await runnable();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);

                throw new Exception(ex.Message);
            }
        }
    }
}
