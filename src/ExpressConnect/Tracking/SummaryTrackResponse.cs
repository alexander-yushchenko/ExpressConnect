using System;
using System.Collections.Generic;
using System.Net;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class SummaryTrackResponse : ITrackResponse<IConsignmentSummary>
    {
        public HttpStatusCode StatusCode { get; set; }

        public Exception Exception { get; set; }

        public ICollection<IConsignmentSummary> Consignments { get; private set; }

        public void AppendConsignments(ITrackResponse<IConsignmentSummary> trackResponse)
        {
            if (trackResponse == null || 
                trackResponse.Consignments == null ||
                trackResponse.Consignments.Count == 0) return;

            foreach (var consignment in trackResponse.Consignments)
                Consignments.Add(consignment);
        }

        public SummaryTrackResponse()
        {
            Consignments = new List<IConsignmentSummary>();
        }
    }
}