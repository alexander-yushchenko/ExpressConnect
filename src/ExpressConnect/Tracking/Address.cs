namespace AY.TNT.ExpressConnect.Tracking
{
    public class Address : IAddress
    {
        public AddressParty Party { get; set; }
        public string Name { get; set; }
        public string[] AddressLines { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Postcode { get; set; }
        public ICountry Country { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string AccountNumber { get; set; }
        public string VatNumber { get; set; }
    }
}