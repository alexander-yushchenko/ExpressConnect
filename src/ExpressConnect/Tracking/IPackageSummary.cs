namespace AY.TNT.ExpressConnect.Tracking
{
    public interface IPackageSummary
    {
        ushort NumberOfPieces { get; set; }
        double Weight { get; set; }
        WeightUnit WeightUnit { get; set; }
        string PackageDescription { get; set; }
        string GoodsDescription { get; set; }
        double InvoiceAmount { get; set; }
        string InvoiceCurrency { get; set; }
    }
}