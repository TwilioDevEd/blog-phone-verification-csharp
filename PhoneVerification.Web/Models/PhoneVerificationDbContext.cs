using System.Data.Entity;

namespace PhoneVerification.Web.Models
{
    public class PhoneVerificationDbContext : DbContext
    {
        public PhoneVerificationDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Number> Numbers { get; set; }
    }
}