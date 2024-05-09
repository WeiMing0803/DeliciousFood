using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;

namespace AI.DeliciousFood.Core.Server
{
    public interface IAccountRepository
    {
        Task<Dictionary<Guid, MemberPrice>> GetMenberPriceAsync(CancellationToken cancellationToken = default);
    }

    public class AccountRepository(GenericRepository<FoodDbContext> dbContext) : IAccountRepository
    {
        public static Dictionary<Guid, MemberPrice> MemberPrices = null;

        public async Task<Dictionary<Guid, MemberPrice>> GetMenberPriceAsync(CancellationToken cancellationToken)
        {
            if (MemberPrices == null)
            {
                MemberPrices = dbContext.GetQueryable<MemberPrice>().ToDictionary(k => k.MemberPriceGuid, v => v);
            }
            return MemberPrices;
        }
    }
}