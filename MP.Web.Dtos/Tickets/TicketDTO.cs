using MP.Web.Dtos.Purchases;
using MP.Web.Dtos.Shows;
using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Tickets
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public string Qr { get; set; }
        public ShowDTO Show { get; set; }
        public PurchaseDTO Purchase { get; set; }
    }
}
