using AI.DeliciousFood.Core.Common.Model;
using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;
using AI.DeliciousFood.Web.Client.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace AI.DeliciousFood.Core.Server
{
    public interface IAccountRepository
    {
        Task<List<MemberPrice>> GetMenberPriceAsync(CancellationToken cancellationToken = default);
    }

    public class AccountRepository(GenericRepository<FoodDbContext> dbContext) : IAccountRepository
    {
        public async Task<List<MemberPrice>> GetMenberPriceAsync(CancellationToken cancellationToken)
        {
            List<MemberPrice> memberPrices = dbContext.GetQueryable<MemberPrice>().ToList();
            return null;
        }
    }
}