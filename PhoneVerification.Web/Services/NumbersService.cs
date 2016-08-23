using System.Data.Entity;
using System.Threading.Tasks;
using PhoneVerification.Web.Models;

namespace PhoneVerification.Web.Services
{
    public interface INumbersService
    {
        Task<Number> FindByPhoneNumberAsync(string phoneNumber);
        Task<int> CreateAsync(Number number);
        Task<int> DeleteAsync(Number number);
    }

    public class NumbersService : INumbersService
    {
        private readonly PhoneVerificationDbContext _context;

        public NumbersService(PhoneVerificationDbContext context)
        {
            _context = context;
        }

        public async Task<Number> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Numbers.FirstOrDefaultAsync(n => n.PhoneNumber == phoneNumber);
        }

        public async Task<int> CreateAsync(Number number)
        {
            _context.Numbers.Add(number);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Number number)
        {
            _context.Entry(number).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
    }
}