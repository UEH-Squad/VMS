namespace VMS.Domain.Configurations
{
    public class CacheConfiguration
    {
        public int AbsoluteExpirationInHours { get; set; }
        public int SlidingExpirationInMinutes { get; set; }
        public bool IsCacheEnabled { get; set; }
    }
}