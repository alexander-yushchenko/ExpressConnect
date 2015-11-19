using System;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class ShipmentSummary : IShipmentSummary
    {
        public Payment TermsOfPayment { get; set; }
        public DateTime DueDate { get; set; }
        public string Service { get; set; }
    }
}