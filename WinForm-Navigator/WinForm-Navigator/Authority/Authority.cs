namespace Navigator
{
    [Flags]
    public enum Authority
    {
        ROOT = 0b10000,
        ADMIN = 0b01000,
        VIP = 0b00100,
        USER = 0b00010,
        VISITOR = 0b00001
    }

    public class AuthorityMismatchedEventArgs : EventArgs
    {
        public Authority MyAuthority { get; set; }
        public Authority PageRequired { get; set; }
    }
}
