using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public abstract class CustomerReferenceTrackRequest<TConsignment> : TrackRequestBase<TConsignment>
    {
        private readonly ICollection<string> _customerReferences;

        protected override XElement GetSearchCriteriaElement()
        {
            var element = new XElement("SearchCriteria",
                new XAttribute("marketType", MarketType));

            if (!string.IsNullOrWhiteSpace(OriginCountry))
                element.Add(
                    new XAttribute("originCountry", OriginCountry));

            foreach (var reference in _customerReferences)
                element.Add(
                    new XElement("CustomerReference", reference));

            return element;
        }

        protected CustomerReferenceTrackRequest(ITrackCredentials credentials, LevelOfDetail levelOfDetail,
            ITrackResponseParser<TConsignment> trackResponseParser, ICollection<string> customerReferences) 
            : base(credentials, levelOfDetail, trackResponseParser)
        {
            if (customerReferences == null)
                throw new ArgumentNullException("customerReferences");

            if (customerReferences.Count == 0)
                throw new ArgumentOutOfRangeException(
                    "customerReferences",
                    "The minimum number of customer references, that may be submitted, is 1");

            if (customerReferences.Count > 50)
                throw new ArgumentOutOfRangeException(
                    "customerReferences", customerReferences.Count,
                    "The maximum number of customer references, that may be submitted, is 50");

            _customerReferences = customerReferences;
        }
    }

    public class CustomerReferenceSummaryTrackRequest : CustomerReferenceTrackRequest<IConsignmentSummary>
    {
        public CustomerReferenceSummaryTrackRequest(ITrackCredentials credentials, ICollection<string> customerReferences)
            : base(credentials, LevelOfDetail.Summary, new SummaryTrackResponseParser(), customerReferences)
        {
        }
    }

    public class CustomerReferenceCompleteTrackRequest : CustomerReferenceTrackRequest<IConsignmentComplete>
    {
        public CustomerReferenceCompleteTrackRequest(ITrackCredentials credentials, ICollection<string> customerReferences)
            : base(credentials, LevelOfDetail.Complete, new CompleteTrackResponseParser(), customerReferences)
        {
        }
    }
}