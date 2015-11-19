using System;
using System.Collections.Generic;
using System.Net.Http;
using AY.TNT.ExpressConnect.Tracking;

namespace AY.TNT.ExpressConnect.Samples
{
    internal class Program
    {
        private const string TntTrackingUriString = "https://express.tnt.com/expressconnect/track.do?version=3";
        private const string TntTestUserName = "CITTEST1UA";
        private const string TntTestPassword = "CITTEST1UA";

        private static void Main()
        {
            TrackByConsignmentNumbers();
            TrackByCustomerReferences();
            TrackByAccount();

            Console.ReadLine();
        }

        private static async void TrackByConsignmentNumbers()
        {
            var credentials = new TrackCredentials
            {
                UserName = TntTestUserName,
                Password = TntTestPassword
            };

            var httpClient = new HttpClient();

            var consignmentNumbers = new List<string>
            {
                "123456785"
            };

            var request = new ConsignmentNumberCompleteTrackRequest(credentials, consignmentNumbers)
            {
                OriginCountry = "UA",
                MarketType = MarketType.International,
                Locale = "RU"
            };
            
            request.ExtraDetails.Add(ExtraDetail.OriginAddress);
            request.ExtraDetails.Add(ExtraDetail.DestinationAddress);
            request.ExtraDetails.Add(ExtraDetail.Shipment);
            request.ExtraDetails.Add(ExtraDetail.Package);
            request.ExtraDetails.Add(ExtraDetail.PodImage);

            Console.WriteLine("Consignment numbers tracking. Request string:\n\n{0}\n\n", request);

            var result = await httpClient.TrackConsignmentsAsync(TntTrackingUriString, request);

            var statusCode = result.StatusCode;
            var exception = result.Exception;
            var consignments = result.Consignments;
        }

        private static async void TrackByCustomerReferences()
        {
            var credentials = new TrackCredentials
            {
                UserName = TntTestUserName,
                Password = TntTestPassword
            };

            var httpClient = new HttpClient();

            var references = new List<string>
            {
                "test1", "test2"
            };

            var request = new CustomerReferenceCompleteTrackRequest(credentials, references)
            {
                OriginCountry = "UA",
                MarketType = MarketType.International,
                Locale = "RU"
            };

            Console.WriteLine("Customer references tracking. Request string:\n\n{0}\n\n", request);

            var result = await httpClient.TrackConsignmentsAsync(TntTrackingUriString, request);

            var statusCode = result.StatusCode;
            var exception = result.Exception;
            var consignments = result.Consignments;
        }

        private static async void TrackByAccount()
        {
            var credentials = new TrackCredentials
            {
                UserName = TntTestUserName,
                Password = TntTestPassword,
                Account = new Account
                {
                    Number = "1",
                    CountryCode = "UA"
                }
            };

            var httpClient = new HttpClient();

            var request = new AccountCompleteTrackRequest(credentials: credentials, dateFrom: DateTime.Today.AddDays(-14), numberOfDays: 3)
            {
                OriginCountry = "UA",
                MarketType = MarketType.Domestic
            };

            Console.WriteLine("Account tracking. Request string:\n\n{0}\n\n", request);

            var result = await httpClient.TrackConsignmentsAsync(TntTrackingUriString, request);

            var statusCode = result.StatusCode;
            var exception = result.Exception;
            var consignments = result.Consignments;
        }
    }
}
