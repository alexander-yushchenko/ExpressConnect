using System;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface IShipmentSummary
    {
        Payment TermsOfPayment { get; set; }
        DateTime DueDate { get; set; }
        string Service { get; set; }
    }
}