namespace AY.TNT.ExpressConnect.Tracking
{
    public interface ITrackCredentials
    {
        IAccount Account { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}