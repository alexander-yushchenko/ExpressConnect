using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public abstract class ConsignmentNumberTrackRequest<TConsignment> : TrackRequestBase<TConsignment>
    {
        private readonly ICollection<string> _consignmentNumbers;

        protected override XElement GetSearchCriteriaElement()
        {
            var element = new XElement("SearchCriteria",
                new XAttribute("marketType", MarketType));

            if (!string.IsNullOrWhiteSpace(OriginCountry))
                element.Add(
                    new XAttribute("originCountry", OriginCountry));

            foreach (var number in _consignmentNumbers)
                element.Add(
                    new XElement("ConsignmentNumber", number));

            return element;
        }

        protected ConsignmentNumberTrackRequest(ITrackCredentials credentials, LevelOfDetail levelOfDetail, 
            ITrackResponseParser<TConsignment> responseParser, ICollection<string> consignmentNumbers)
            : base(credentials, levelOfDetail, responseParser)
        {
            if (consignmentNumbers == null)
                throw new ArgumentNullException("consignmentNumbers");

            if (consignmentNumbers.Count == 0)
                throw new ArgumentOutOfRangeException(
                    "consignmentNumbers",
                    "The minimum number of consignment numbers, that may be submitted, is 1");

            if (consignmentNumbers.Count > 50)
                throw new ArgumentOutOfRangeException(
                    "consignmentNumbers", consignmentNumbers.Count,
                    "The maximum number of consignment numbers, that may be submitted, is 50");

            _consignmentNumbers = consignmentNumbers;
        }
    }

    public class ConsignmentNumberSummaryTrackRequest : ConsignmentNumberTrackRequest<IConsignmentSummary>
    {
        public ConsignmentNumberSummaryTrackRequest(ITrackCredentials credentials, ICollection<string> consignmentNumbers) 
            : base(credentials, LevelOfDetail.Summary, new SummaryTrackResponseParser(), consignmentNumbers)
        {
        }
    }

    public class ConsignmentNumberCompleteTrackRequest : ConsignmentNumberTrackRequest<IConsignmentComplete>
    {
        public ConsignmentNumberCompleteTrackRequest(ITrackCredentials credentials, ICollection<string> consignmentNumbers)
            : base(credentials, LevelOfDetail.Complete, new CompleteTrackResponseParser(), consignmentNumbers)
        {
        }
    }
}