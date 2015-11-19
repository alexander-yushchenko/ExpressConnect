using System.Net.Http;

namespace AY.TNT.ExpressConnect.Tracking
{
    public interface ITrackResponseParser<TConsignment>
    {
        ITrackResponse<TConsignment> Parse(HttpResponseMessage responseMessage, out string continuationKey);
    }
}