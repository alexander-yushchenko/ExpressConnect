using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AY.TNT.ExpressConnect.Tracking;

namespace AY.TNT.ExpressConnect
{
    public static class Extensions
    {
        public static Task<ITrackResponse<TConsignment>> TrackConsignmentsAsync<TConsignment>(this HttpClient client, string requestUri, ITrackRequest<TConsignment> request)
        {
            return TrackConsignmentsAsync(client, requestUri, request, CancellationToken.None);
        }

        public static Task<ITrackResponse<TConsignment>> TrackConsignmentsAsync<TConsignment>(this HttpClient client, string requestUri, ITrackRequest<TConsignment> request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<ITrackResponse<TConsignment>>();

            Task.Run(async () =>
            {
                ITrackResponse<TConsignment> response = null;

                var completed = false;

                while (!completed)
                {
                    var httpRequestMessage = request.GetHttpRequestMessage(requestUri);

                    try
                    {
                        var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            tcs.TrySetCanceled();
                            return;
                        }

                        response = request.GetTrackResponse(httpResponseMessage, response);
                        completed = string.IsNullOrWhiteSpace(request.ContinuationKey);
                        tcs.TrySetResult(response);
                        if (!completed) Thread.Sleep(1000);
                    }
                    catch (OperationCanceledException)
                    {
                        completed = true;
                        tcs.TrySetCanceled();
                    }
                    catch (Exception exception)
                    {
                        completed = true;
                        tcs.TrySetException(exception);
                    }
                }

            }, cancellationToken);

            return tcs.Task;
        }
    }
}