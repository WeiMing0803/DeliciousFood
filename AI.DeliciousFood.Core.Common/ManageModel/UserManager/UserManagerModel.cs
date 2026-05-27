namespace AI.DeliciousFood.Core.Common.ManageModel.UserManager
{
    public class UserManagerModel
    {
        public required long Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateOnly MembershipExpireAt { get; set; }
        public required string CreateDateTime { get; set; }
        public required string RoleId { get; set; }
        public required string Status { get; set; }
    }

    public class SaveUserModel
    {
        public long Id { get; set; }
        public DateOnly MembershipExpireAt { get; set; }
        public string RoleId { get; set; }
    }
}
