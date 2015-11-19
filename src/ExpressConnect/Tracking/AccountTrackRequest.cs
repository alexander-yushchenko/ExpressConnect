using System;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public abstract class AccountTrackRequest<TConsignment> : TrackRequestBase<TConsignment>
    {
        private readonly DateTime _dateFrom;
        private readonly ushort _numberOfDays;

        protected override XElement GetSearchCriteriaElement()
        {
            var element = new XElement("SearchCriteria",
                new XAttribute("marketType", MarketType));

            if (!string.IsNullOrWhiteSpace(OriginCountry))
                element.Add(
                    new XAttribute("originCountry", OriginCountry));

            element.Add(
                new XElement("Account",
                    new XElement("Number", Credentials.Account.Number),
                    new XElement("CountryCode", Credentials.Account.CountryCode)));

            element.Add(
                new XElement("Period",
                    new XElement("DateFrom", _dateFrom.ToString("yyyyMMdd")),
                    new XElement("NumberOfDays", _numberOfDays)));

            return element;
        }

        protected AccountTrackRequest(ITrackCredentials credentials, LevelOfDetail levelOfDetail,
            ITrackResponseParser<TConsignment> responseParser, DateTime dateFrom, ushort numberOfDays)
            : base(credentials, levelOfDetail, responseParser)
        {
            if (credentials == null || credentials.Account == null)
                throw new ArgumentNullException("credentials");

            if (numberOfDays > 4)
                throw new ArgumentOutOfRangeException(
                    "numberOfDays", numberOfDays,
                    "The maximum number of days, that may be submitted, is 4");

            _dateFrom = dateFrom;
            _numberOfDays = numberOfDays;
        }
    }

    public class AccountSummaryTrackRequest : AccountTrackRequest<IConsignmentSummary>
    {
        public AccountSummaryTrackRequest(ITrackCredentials credentials, DateTime dateFrom, ushort numberOfDays) 
            : base(credentials, LevelOfDetail.Summary, new SummaryTrackResponseParser(), dateFrom, numberOfDays)
        {
        }
    }

    public class AccountCompleteTrackRequest : AccountTrackRequest<IConsignmentComplete>
    {
        public AccountCompleteTrackRequest(ITrackCredentials credentials, DateTime dateFrom, ushort numberOfDays) 
            : base(credentials, LevelOfDetail.Complete, new CompleteTrackResponseParser(), dateFrom, numberOfDays)
        {
        }
    }
}