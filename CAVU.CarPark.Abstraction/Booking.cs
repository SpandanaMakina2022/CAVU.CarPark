using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CAVU.CarPark.Abstraction
{
    public class Booking
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime BookedDateTime { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDateTime { get; set; }

        public decimal TotalPrice { get; set; }
        public string CarRegistration { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    
}
