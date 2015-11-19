using System;
using System.Collections.Generic;
using System.Net;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface ITrackResponse<TConsignment>
    {
        HttpStatusCode StatusCode { get; set; }
        Exception Exception { get; set; }
        ICollection<TConsignment> Consignments { get; }
        void AppendConsignments(ITrackResponse<TConsignment> trackResponse);
    }
}