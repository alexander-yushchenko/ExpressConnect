using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AY.TNT.ExpressConnect.Tracking
{
    public class TrackError
    {
        public ushort Code { get; private set; }
        public string Message { get; private set; }

        public TrackError(ushort code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    [Serializable]
    public class TrackRequestException : Exception
    {
        private readonly IReadOnlyCollection<TrackError> _errors;

        public TrackRequestException(IList<TrackError> errors) 
            : base("See errors collection for details")
        {
            _errors = new ReadOnlyCollection<TrackError>(errors);
        }

        public IReadOnlyCollection<TrackError> GetErrors()
        {
            return _errors;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TrackErrors", _errors);
        }
    }
}