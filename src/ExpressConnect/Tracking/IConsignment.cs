using System;
using System.Collections.Generic;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface IConsignmentSummary
    {
        ConsignmentAccess Access { get; set; }

        // Summary
        string ConsignmentNumber { get; set; }
        string AlternativeConsignmentNumber { get; set; }
        string CustomerReference { get; set; }
        DateTime CollectionDate { get; set; }
        string DeliveryTown { get; set; }
        DateTime? DeliveryDate { get; set; }
        TimeSpan? DeliveryTime { get; set; }
        string Signatory { get; set; }
        DeliverySummary Summary { get; set; }
        IAccount TermsOfPaymentAccount { get; set; }
        ushort PieceQuantity { get; set; }
    }

    public interface IConsignmentComplete : IConsignmentSummary
    {
        // Complete
        string OriginDepot { get; set; }
        string OriginDepotName { get; set; }
        ICountry DestinationCountry { get; set; }
        ICountry OriginCountry { get; set; }
        IAccount SenderAccount { get; set; }
        ICollection<IStatusData> Statuses { get; set; }
        
        //Extra Details
        ICollection<IAddress> Addresses { get; set; }
        IPackageSummary PackageSummary { get; set; }
        IShipmentSummary ShipmentSummary { get; set; }
        Uri Pod { get; set; }
    }
}