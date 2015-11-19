using System;
using System.Collections.Generic;
using System.Net;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class CompleteTrackResponse : ITrackResponse<IConsignmentComplete>
    {
        public HttpStatusCode StatusCode { get; set; }

        public Exception Exception { get; set; }

        public ICollection<IConsignmentComplete> Consignments { get; private set; }

        public void AppendConsignments(ITrackResponse<IConsignmentComplete> trackResponse)
        {
            if (trackResponse == null ||
                trackResponse.Consignments == null ||
                trackResponse.Consignments.Count == 0) return;

            foreach (var consignment in trackResponse.Consignments)
                Consignments.Add(consignment);
        }

        public CompleteTrackResponse()
        {
            Consignments = new List<IConsignmentComplete>();
        }
    }
}