namespace AY.TNT.ExpressConnect.Tracking
{
    public class TrackCredentials : ITrackCredentials
    {
        public IAccount Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public TrackCredentials()
        {
            Account = new Account();
        }
    }
}