using System;
using System.Linq;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public static class TrackResponseParserHelpers
    {
        private static ConsignmentAccess GetConsignmentAccess(string access)
        {
            return access != null && access.Equals("full")
                ? ConsignmentAccess.Full
                : ConsignmentAccess.Public;
        }

        private static DeliverySummary GetDeliverySummary(string code)
        {
            if (code == null) return DeliverySummary.NotFound;

            switch (code)
            {
                case "EXC": return DeliverySummary.Exception;
                case "INT": return DeliverySummary.InTransit;
                case "DEL": return DeliverySummary.Delivered;
                default: return DeliverySummary.NotFound;
            }
        }

        private static WeightUnit GetWeightUnit(string unit)
        {
            return unit != null && unit.Equals("lbs")
                ? WeightUnit.Pounds
                : WeightUnit.Kilograms;
        }

        private static Payment GetTermsOfPayment(string payer)
        {
            return payer != null && payer.Equals("Receiver")
                ? Payment.Receiver
                : Payment.Sender;
        }

        private static AddressParty GetAddressParty(string party)
        {
            if (party == null) return AddressParty.Sender;

            switch (party)
            {
                case "Receiver": return AddressParty.Receiver;
                case "Collection": return AddressParty.Collection;
                case "Delivery": return AddressParty.Delivery;
                default: return AddressParty.Sender;
            }
        }

        private static ICountry GetCountry(XContainer countryContainer)
        {
            if (countryContainer == null) return null;

            return new Country
            {
                Code = countryContainer.Element("CountryCode").GetTextValue(),
                Name = countryContainer.Element("CountryName").GetTextValue()
            };
        }

        private static IAccount GetAccount(XContainer accountContainer)
        {
            if (accountContainer == null) return null;

            return new Account
            {
                Number = accountContainer.Element("Number").GetTextValue(),
                CountryCode = accountContainer.Element("CountryCode").GetTextValue()
            };
        }

        private static IStatusData GetStatusData(XContainer statusDataContainer)
        {
            if (statusDataContainer == null) return null;

            return new StatusData
            {
                StatusCode = statusDataContainer.Element("StatusCode").GetTextValue(),
                StatusDescription = statusDataContainer.Element("StatusDescription").GetTextValue(),
                LocalEventDate = statusDataContainer.Element("LocalEventDate").GetDateTimeValue("yyyyMMdd"),
                LocalEventTime = statusDataContainer.Element("LocalEventTime").GetTimeSpanValue("hhmm"),
                Depot = statusDataContainer.Element("Depot").GetTextValue(),
                DepotName = statusDataContainer.Element("DepotName").GetTextValue()
            };
        }

        private static IAddress GetAddress(XElement addressElement)
        {
            if (addressElement == null) return null;

            return new Address
            {
                Party = GetAddressParty(addressElement.Attribute("addressParty").GetTextValue()),
                Name = addressElement.Element("Name").GetTextValue(),
                AddressLines = addressElement.Elements("AddressLine")
                    .Select(element => element.GetTextValue())
                    .Where(line => line != null)
                    .ToArray(),
                City = addressElement.Element("City").GetTextValue(),
                Province = addressElement.Element("Province").GetTextValue(),
                Postcode = addressElement.Element("Postcode").GetTextValue(),
                Country = GetCountry(addressElement.Element("Country")),
                PhoneNumber = addressElement.Element("PhoneNumber").GetTextValue(),
                ContactName = addressElement.Element("ContactName").GetTextValue(),
                ContactPhoneNumber = addressElement.Element("ContactPhoneNumber").GetTextValue(),
                AccountNumber = addressElement.Element("AccountNumber").GetTextValue(),
                VatNumber = addressElement.Element("VATNumber").GetTextValue()
            };
        }

        private static IPackageSummary GetPackageSummary(XContainer packageSummaryContainer)
        {
            if (packageSummaryContainer == null) return null;

            var weightElement = packageSummaryContainer.Element("Weight");
            var invoiceAmountElement = packageSummaryContainer.Element("InvoiceAmount");

            return new PackageSummary
            {
                NumberOfPieces = packageSummaryContainer.Element("NumberOfPieces").GetUInt16Value(),
                Weight = weightElement.GetDoubleValue(),
                WeightUnit = weightElement != null
                    ? GetWeightUnit(weightElement.Attribute("units").GetTextValue())
                    : WeightUnit.Kilograms,
                PackageDescription = packageSummaryContainer.Element("PackageDescription").GetTextValue(),
                GoodsDescription = packageSummaryContainer.Element("GoodsDescription").GetTextValue(),
                InvoiceAmount = invoiceAmountElement.GetDoubleValue(),
                InvoiceCurrency = invoiceAmountElement != null
                    ? invoiceAmountElement.Attribute("currency").GetTextValue()
                    : string.Empty
            };
        }

        private static IShipmentSummary GetShipmentSummary(XContainer shipmentSummaryContainer)
        {
            if (shipmentSummaryContainer == null) return null;

            return new ShipmentSummary
            {
                TermsOfPayment = GetTermsOfPayment(shipmentSummaryContainer.Element("TermsOfPayment").GetTextValue()),
                DueDate = shipmentSummaryContainer.Element("DueDate").GetDateTimeValue("yyyyMMdd"),
                Service = shipmentSummaryContainer.Element("Service").GetTextValue()
            };
        }

        private static void SetSummaryValues(XElement consignmentElement, IConsignmentSummary consignment)
        {
            if (consignmentElement == null)
                throw new ArgumentNullException("consignmentElement");
            if (consignment == null)
                throw new ArgumentNullException("consignment");

            var elementName = consignmentElement.Name.LocalName;
            if (!elementName.Equals("Consignment"))
                throw new ArgumentException("Only Consignment node is allowed");

            consignment.Access = GetConsignmentAccess(consignmentElement.Attribute("access").GetTextValue());
            consignment.ConsignmentNumber = consignmentElement.Element("ConsignmentNumber").GetTextValue();
            consignment.AlternativeConsignmentNumber = consignmentElement.Element("AlternativeConsignmentNumber").GetTextValue();
            consignment.CustomerReference = consignmentElement.Element("CustomerReference").GetTextValue();
            consignment.Summary = GetDeliverySummary(consignmentElement.Element("SummaryCode").GetTextValue());

            if (consignment.Summary == DeliverySummary.NotFound) return;

            consignment.CollectionDate = consignmentElement.Element("CollectionDate").GetDateTimeValue("yyyyMMdd");
            consignment.DeliveryTown = consignmentElement.Element("DeliveryTown").GetTextValue();
            consignment.DeliveryDate = consignmentElement.Element("DeliveryDate").GetNullableDateTimeValue("yyyyMMdd");
            consignment.DeliveryTime = consignmentElement.Element("DeliveryTime").GetNullableTimeSpanValue("hhmm");
            consignment.Signatory = consignmentElement.Element("Signatory").GetTextValue();
            consignment.PieceQuantity = consignmentElement.Element("PieceQuantity").GetUInt16Value();
            consignment.TermsOfPaymentAccount = GetAccount(consignmentElement.Element("TermsOfPaymentAccount"));
        }

        private static void SetCompleteValues(XElement consignmentElement, IConsignmentComplete consignment)
        {
            SetSummaryValues(consignmentElement, consignment);

            if (consignment.Summary == DeliverySummary.NotFound) return;

            consignment.OriginDepot = consignmentElement.Element("OriginDepot").GetTextValue();
            consignment.OriginDepotName = consignmentElement.Element("OriginDepotName").GetTextValue();
            consignment.DestinationCountry = GetCountry(consignmentElement.Element("DestinationCountry"));
            consignment.OriginCountry = GetCountry(consignmentElement.Element("OriginCountry"));
            consignment.SenderAccount = GetAccount(consignmentElement.Element("SenderAccount"));

            consignment.Statuses =
                consignmentElement.Elements("StatusData")
                .Select(GetStatusData)
                .Where(statusData => statusData != null)
                .ToList();

            var addressesElement = consignmentElement.Element("Addresses");
            if (addressesElement != null)
                consignment.Addresses =
                    addressesElement.Elements("Address")
                    .Select(GetAddress)
                    .Where(address => address != null)
                    .ToList();

            consignment.PackageSummary = GetPackageSummary(consignmentElement.Element("PackageSummary"));
            consignment.ShipmentSummary = GetShipmentSummary(consignmentElement.Element("ShipmentSummary"));

            var podUriString = consignmentElement.Element("POD").GetTextValue();
            var podValid = Uri.IsWellFormedUriString(podUriString, UriKind.Absolute);
            if (!podValid) return;
            consignment.Pod = new Uri(podUriString, UriKind.Absolute);
        }

        public static IConsignmentSummary GetConsignmentSummary(XElement consignmentElement)
        {
            var consignment = new ConsignmentSummary();
            SetSummaryValues(consignmentElement, consignment);
            return consignment;
        }

        public static IConsignmentComplete GetConsignmentComplete(XElement consignmentElement)
        {
            var consignment = new ConsignmentComplete();
            SetCompleteValues(consignmentElement, consignment);
            return consignment;
        }
    }
}