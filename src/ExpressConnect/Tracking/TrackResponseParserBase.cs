using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect.Tracking
{
    public abstract class TrackResponseParserBase<TConsignment> : ITrackResponseParser<TConsignment>
    {
        private static TrackRequestException GetTrackRequestException(XDocument document)
        {
            if (document == null || document.Root == null) return null;

            var errors = document.Root.Elements("Error").ToList();
            if (errors.Count == 0) return null;

            var list = (from error in errors
                        let code = error.Element("Code").GetUInt16Value()
                        let message = error.Element("Message").GetTextValue()
                        select new TrackError(code, message)).ToList();

            return new TrackRequestException(list);
        }

        private static string GetContinuationKey(XDocument document)
        {
            if (document == null || document.Root == null) return null;
            return document.Root.Element("ContinuationKey").GetTextValue();
        }

        protected abstract ITrackResponse<TConsignment> TrackResponseFactory();

        protected virtual ICollection<TConsignment> GetConsignments(XDocument document)
        {
            return new List<TConsignment>();
        }

        public ITrackResponse<TConsignment> Parse(HttpResponseMessage responseMessage, out string continuationKey)
        {
            if (responseMessage == null)
                throw new ArgumentNullException("responseMessage");

            var trackResponse = TrackResponseFactory();
            trackResponse.StatusCode = responseMessage.StatusCode;

            if (trackResponse.StatusCode != HttpStatusCode.OK)
            {
                continuationKey = string.Empty;
                return trackResponse;
            }

            try
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                var document = XDocument.Parse(content);
                trackResponse.Exception = GetTrackRequestException(document);
                if (trackResponse.Exception != null)
                {
                    continuationKey = string.Empty;
                    return trackResponse;
                }

                continuationKey = GetContinuationKey(document);
                var consignments = GetConsignments(document);

                if (consignments == null ||
                    consignments.Count == 0)
                    return trackResponse;

                foreach (var consignment in consignments)
                    trackResponse.Consignments.Add(consignment);

                return trackResponse;
            }
            catch (Exception exception)
            {
                trackResponse.Exception = exception;
                continuationKey = string.Empty;
                return trackResponse;
            }
        }
    }
}