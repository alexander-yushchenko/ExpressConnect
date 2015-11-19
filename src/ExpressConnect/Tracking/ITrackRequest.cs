using System.Collections.Generic;
using System.Net.Http;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface ITrackRequest<TConsignment>
    {
        string Locale { get; set; }
        MarketType MarketType { get; set; }
        string OriginCountry { get; set; }
        ICollection<ExtraDetail> ExtraDetails { get; }
        string ContinuationKey { get; }
        HttpRequestMessage GetHttpRequestMessage(string requestUri);
        ITrackResponse<TConsignment> GetTrackResponse(HttpResponseMessage responseMessage, ITrackResponse<TConsignment> response = null);
    }
}