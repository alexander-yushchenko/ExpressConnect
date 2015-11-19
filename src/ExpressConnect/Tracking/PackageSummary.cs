namespace AY.TNT.ExpressConnect.Tracking
{
    public class PackageSummary : IPackageSummary
    {
        public ushort NumberOfPieces { get; set; }
        public double Weight { get; set; }
        public WeightUnit WeightUnit { get; set; }
        public string PackageDescription { get; set; }
        public string GoodsDescription { get; set; }
        public double InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
    }
}