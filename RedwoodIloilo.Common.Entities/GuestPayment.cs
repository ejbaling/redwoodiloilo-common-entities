using System;

namespace RedwoodIloilo.Common.Entities
{
    public class GuestPayment
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
