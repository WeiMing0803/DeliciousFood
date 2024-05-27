using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;

namespace AI.DeliciousFood.Core.Server
{
    public interface IAlipayRepository
    {
        Task<Dictionary<Guid, MemberPrice>> GetMenberPriceAsync(CancellationToken cancellationToken = default);
        Task<MemberPrice> GetMenberPriceByIdAsync(Guid memberPriceGuid, CancellationToken cancellationToken = default);
    }

    public class AlipayRepository(GenericRepository<FoodDbContext> dbContext) : IAlipayRepository
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

        public async Task<MemberPrice> GetMenberPriceByIdAsync(Guid memberPriceGuid, CancellationToken cancellationToken)
        {
            if (MemberPrices == null)
            {
                MemberPrices = await GetMenberPriceAsync(cancellationToken);
            }

            if (MemberPrices.ContainsKey(memberPriceGuid))
            {
                return MemberPrices[memberPriceGuid];
            }

            return null;
        }
    }
}