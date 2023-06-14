using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAVU.CarPark.Abstraction.Service
{
    public interface IBookingService
    {
        Task<bool> CheckAvailability(DateTime from, DateTime to);

        Task<decimal> GetPrice(DateTime from, DateTime to);

        Task<int> GetSpaceAvailablePerDate(DateTime date);

        Task<List<Booking>> GetBookings();

        Task<Booking> GetBooking(Guid bookingId);

        Task<Booking> CreateBooking(Booking booking);

        Task<bool> CancelBooking(Guid bookingId);

        Task<Booking> UpdateBooking(Booking booking);
    }
}