using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Tickets
{
    public class TicketUpsertDTO
    {
        public string Qr { get; set; }
        public int ShowId { get; set; }
        public int PurchaseId { get; set; }
    }
}
