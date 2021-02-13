using System;
using System.Collections.Generic;
using System.Text;

namespace MP.DataAccess.EntityModels
{
    public class Ticket
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public string Qr { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}
