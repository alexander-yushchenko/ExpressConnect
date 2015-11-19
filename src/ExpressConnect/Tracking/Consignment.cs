using System;
using System.Collections.Generic;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class ConsignmentSummary : IConsignmentSummary
    {
        public ConsignmentAccess Access { get; set; }
        public string ConsignmentNumber { get; set; }
        public string AlternativeConsignmentNumber { get; set; }
        public string CustomerReference { get; set; }
        public DateTime CollectionDate { get; set; }
        public string DeliveryTown { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public TimeSpan? DeliveryTime { get; set; }
        public string Signatory { get; set; }
        public DeliverySummary Summary { get; set; }
        public IAccount TermsOfPaymentAccount { get; set; }
        public ushort PieceQuantity { get; set; }
    }

    public class ConsignmentComplete : ConsignmentSummary, IConsignmentComplete
    {
        public string OriginDepot { get; set; }
        public string OriginDepotName { get; set; }
        public ICountry DestinationCountry { get; set; }
        public ICountry OriginCountry { get; set; }
        public IAccount SenderAccount { get; set; }
        public ICollection<IStatusData> Statuses { get; set; }
        public ICollection<IAddress> Addresses { get; set; }
        public IPackageSummary PackageSummary { get; set; }
        public IShipmentSummary ShipmentSummary { get; set; }
        public Uri Pod { get; set; }

        public ConsignmentComplete()
        {
            Statuses = new List<IStatusData>();
            Addresses = new List<IAddress>();
        }
    }
}