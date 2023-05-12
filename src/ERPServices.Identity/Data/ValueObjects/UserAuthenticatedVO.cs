namespace ERPServices.Identity.Data.ValueObjects
{
    public class UserAuthenticatedVO
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
