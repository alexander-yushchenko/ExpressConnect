namespace AY.TNT.ExpressConnect.Tracking
{
    public interface IAddress
    {
        AddressParty Party { get; set; }
        string Name { get; set; }
        string[] AddressLines { get; set; }
        string City { get; set; }
        string Province { get; set; }
        string Postcode { get; set; }
        ICountry Country { get; set; }
        string PhoneNumber { get; set; }
        string ContactName { get; set; }
        string ContactPhoneNumber { get; set; }
        string AccountNumber { get; set; }
        string VatNumber { get; set; }
    }
}