using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAVU.CarPark.Abstraction;
using CAVU.CarPark.Abstraction.Data;

namespace CAVU.CarPark.Data
{
    public class BookingData: IBookingData
    {
        private readonly List<Booking> _bookings = new List<Booking>();

        //Holds Booking date to number of bookings done on a given date
        private readonly Dictionary<DateTime,int> _bookingsCountDictionary = new Dictionary<DateTime,int>();

        public async Task<List<Booking>> Get()
        {
            return this._bookings;
        }

        public async Task<Booking> Get(Guid bookingId)
        {
            return this._bookings.FirstOrDefault(f => f.Id == bookingId);
        }

        public async Task<Dictionary<DateTime, int>> GetBookingCount()
        {
            return this._bookingsCountDictionary;
        }

        public async Task<Booking> Create(Booking booking)
        {
            this._bookings.Add(booking);
            await this.UpdateSpacesPerDay(booking);
            return booking;
        }
        

        public async Task<bool> Delete(Guid bookingId)
        {
            var index = this._bookings.FindIndex(b => b.Id == bookingId);
            if (index == -1) throw new Exception($"No booking found for id {bookingId}");
            await this.RemoveSpacesPerDay(this._bookings[index]);
            this._bookings.RemoveAt(index);
            return true;

        }

        public async Task<Booking> Update(Booking booking)
        {
            var index = this._bookings.FindIndex(b => b.Id == booking.Id);
            if (index == -1) throw new Exception($"No booking found for id {booking.Id}");
            await this.RemoveSpacesPerDay(this._bookings[index]);
            this._bookings[index] = booking;
            await this.UpdateSpacesPerDay(booking);
            return booking;

        }

        private async Task UpdateSpacesPerDay(Booking booking)
        {
            var days = (booking.To - booking.From).Days;
            for (var i = 0; i <= days; i++)
            {
                var key = booking.From.AddDays(i).Date;
                if (this._bookingsCountDictionary.ContainsKey(key))
                {
                    this._bookingsCountDictionary[key] += 1;
                }
                else
                {
                    this._bookingsCountDictionary.Add(key, 1);
                }
            }
        }

        private async Task RemoveSpacesPerDay(Booking booking)
        {
            var days = (booking.To - booking.From).Days;
            for (var i = 0; i <= days; i++)
            {
                var key = booking.From.AddDays(i).Date;
                if (this._bookingsCountDictionary.ContainsKey(key))
                {
                    this._bookingsCountDictionary[key] -= 1;
                }
            }
        }
    }
}
