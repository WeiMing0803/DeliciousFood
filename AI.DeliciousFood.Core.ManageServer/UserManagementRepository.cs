using AI.DeliciousFood.Core.Data;
using AI.DeliciousFood.Core.Model;

namespace AI.DeliciousFood.Core.ManageServer
{
    public interface IUserManagementRepository
    {
        Task<IEnumerable<FoodUser>> GetUserListAsync(CancellationToken cancellationToken = default);
    }

    public class UserManagementRepository(GenericRepository<FoodDbContext> dbContext) : IUserManagementRepository
    {
        public async Task<IEnumerable<FoodUser>> GetUserListAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<FoodUser> result = await dbContext.GetAllAsync<FoodUser>(cancellationToken);
            return result;
        }
    }
}
