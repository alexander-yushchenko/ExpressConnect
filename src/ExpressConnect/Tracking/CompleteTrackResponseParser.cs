using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class CompleteTrackResponseParser : TrackResponseParserBase<IConsignmentComplete>
    {
        protected override ITrackResponse<IConsignmentComplete> TrackResponseFactory()
        {
            return new CompleteTrackResponse();
        }

        protected override ICollection<IConsignmentComplete> GetConsignments(XDocument document)
        {
            if (document == null || document.Root == null)
                return new List<IConsignmentComplete>();

            return document.Root.Elements("Consignment")
                .Select(TrackResponseParserHelpers.GetConsignmentComplete)
                .ToList();
        }
    }
}