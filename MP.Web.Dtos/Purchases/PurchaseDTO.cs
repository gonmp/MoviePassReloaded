using MP.Web.Dtos.Tickets;
using MP.Web.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Purchases
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public decimal TotalNoDiscount { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public decimal Discount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumberOfTickets { get; set; }
        public UserDto User { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
