using System;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class StatusData : IStatusData
    {
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public DateTime LocalEventDate { get; set; }
        public TimeSpan LocalEventTime { get; set; }
        public string Depot { get; set; }
        public string DepotName { get; set; }
    }
}