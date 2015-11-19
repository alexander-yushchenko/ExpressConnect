using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public abstract class TrackRequestBase<TConsignment> : ITrackRequest<TConsignment>
    {
        #region Request Builder Members

        private static string GetBasicAuthorizationValue(string userName, string password)
        {
            var bytes = Encoding.UTF8.GetBytes(string.Concat(userName, ":", password));
            return Convert.ToBase64String(bytes);
        }

        protected virtual XElement GetSearchCriteriaElement()
        {
            var element = new XElement("SearchCriteria",
                new XAttribute("marketType", MarketType));

            if (!string.IsNullOrWhiteSpace(OriginCountry))
                element.Add(
                    new XAttribute("originCountry", OriginCountry));

            return element;
        }

        private XElement GetLevelOfDetailElement()
        {
            var levelOfDetail = new XElement("LevelOfDetail");

            if (_levelOfDetail == LevelOfDetail.Summary)
            {
                levelOfDetail.Add(new XElement("Summary"));
                return levelOfDetail;
            }

            var completeLevel = new XElement("Complete");

            if (!string.IsNullOrWhiteSpace(Locale))
                completeLevel.Add(
                    new XAttribute("locale", Locale));
            if (ExtraDetails.Contains(ExtraDetail.OriginAddress))
                completeLevel.Add(
                    new XAttribute("originAddress", true));
            if (ExtraDetails.Contains(ExtraDetail.DestinationAddress))
                completeLevel.Add(
                    new XAttribute("destinationAddress", true));
            if (ExtraDetails.Contains(ExtraDetail.Shipment))
                completeLevel.Add(
                    new XAttribute("shipment", true));
            if (ExtraDetails.Contains(ExtraDetail.Package))
                completeLevel.Add(
                    new XAttribute("package", true));
            if (ExtraDetails.Contains(ExtraDetail.PodImage))
            {
                completeLevel.Add(
                    new XAttribute("podImage", true));
                levelOfDetail.Add(
                    new XElement("POD",
                        new XAttribute("format", "URL")));
            }

            levelOfDetail.Add(completeLevel);
            return levelOfDetail;
        }

        private string GetRequestString()
        {
            var searchCriteria = GetSearchCriteriaElement();
            var levelOfDetail = GetLevelOfDetailElement();
            var continuationKey = !string.IsNullOrWhiteSpace(ContinuationKey)
                ? new XElement("ContinuationKey", ContinuationKey)
                : null;

            var request = new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement("TrackRequest",
                    new XAttribute("locale", "en_US"),
                    new XAttribute("version", "3.1"),
                    searchCriteria,
                    levelOfDetail,
                    continuationKey));

            return string.Concat("xml_in=", request.Declaration, request);
        }

        private HttpContent GetHttpContent()
        {
            var contentString = GetRequestString();
            var content = new StringContent(contentString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

        #endregion

        protected readonly ITrackCredentials Credentials;
        private readonly LevelOfDetail _levelOfDetail;
        private readonly ITrackResponseParser<TConsignment> _trackResponseParser;

        public string Locale { get; set; }
        
        public MarketType MarketType { get; set; }

        public string OriginCountry { get; set; }
        
        public ICollection<ExtraDetail> ExtraDetails { get; private set; }
        
        public string ContinuationKey { get; protected set; }
        
        public HttpRequestMessage GetHttpRequestMessage(string requestUri)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
            message.Headers.Authorization = 
                new AuthenticationHeaderValue("Basic",
                    GetBasicAuthorizationValue(Credentials.UserName, Credentials.Password));
            
            if (Credentials.Account != null)
            {
                message.Headers.Add("accountCountry", Credentials.Account.CountryCode);
                message.Headers.Add("accountList", Credentials.Account.Number);
            }

            message.Content = GetHttpContent();
            return message;
        }

        public ITrackResponse<TConsignment> GetTrackResponse(HttpResponseMessage responseMessage, ITrackResponse<TConsignment> response = null)
        {
            string continuationKey;
            var trackResponse = _trackResponseParser.Parse(responseMessage, out continuationKey);

            ContinuationKey = continuationKey;
            trackResponse.AppendConsignments(response);

            return trackResponse;
        }

        public override string ToString()
        {
            return GetRequestString();
        }

        protected TrackRequestBase(ITrackCredentials credentials, LevelOfDetail levelOfDetail, ITrackResponseParser<TConsignment> trackResponseParser)
        {
            if (credentials == null) 
                throw new ArgumentNullException("credentials");

            if (trackResponseParser == null)
                throw new ArgumentNullException("trackResponseParser");

            Credentials = credentials;
            _levelOfDetail = levelOfDetail;
            _trackResponseParser = trackResponseParser;
            ExtraDetails = new List<ExtraDetail>();
        }
    }
}
