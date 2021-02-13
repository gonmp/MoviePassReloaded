using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public decimal TotalNoDiscount { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public decimal Discount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumberOfTickets { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
