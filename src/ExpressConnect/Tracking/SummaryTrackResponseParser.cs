using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class SummaryTrackResponseParser : TrackResponseParserBase<IConsignmentSummary>
    {
        protected override ITrackResponse<IConsignmentSummary> TrackResponseFactory()
        {
            return new SummaryTrackResponse();
        }

        protected override ICollection<IConsignmentSummary> GetConsignments(XDocument document)
        {
            if (document == null || document.Root == null) 
                return new List<IConsignmentSummary>();

            return document.Root.Elements("Consignment")
                .Select(TrackResponseParserHelpers.GetConsignmentSummary)
                .ToList();
        }
    }
}