using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAVU.CarPark.Abstraction.Data
{
    public interface IBookingData
    {
        Task<List<Booking>> Get();

        Task<Booking> Get(Guid bookingId);

        Task<Booking> Create(Booking booking);

        Task<bool> Delete(Guid bookingId);

        Task<Booking> Update(Booking booking);

        Task<Dictionary<DateTime, int>> GetBookingCount();
    }
}