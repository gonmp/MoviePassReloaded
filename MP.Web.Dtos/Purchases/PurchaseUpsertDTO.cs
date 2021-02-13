using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Web.Dtos.Purchases
{
    public class PurchaseUpsertDTO
    {
        public int NumberOfTickets { get; set; }
        public int ShowId { get; set; }
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvc { get; set; }
    }
}
