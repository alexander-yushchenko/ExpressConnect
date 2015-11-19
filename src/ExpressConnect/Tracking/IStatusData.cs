using System;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface IStatusData
    {
        string StatusCode { get; set; }
        string StatusDescription { get; set; }
        DateTime LocalEventDate { get; set; }
        TimeSpan LocalEventTime { get; set; }
        string Depot { get; set; }
        string DepotName { get; set; }
    }
}