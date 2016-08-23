namespace PhoneVerification.Web.Models
{
    public class Number
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public bool Verified { get; set; }
    }
}