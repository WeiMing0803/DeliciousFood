namespace AI.DeliciousFood.Core.Common.Model.Account;

public class UserInfoModel
{
    public required long Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string MembershipExpireAt { get; set; }
    public required string CreateDateTime { get; set; }
    public required string RoleId { get; set; }
}

public class SaveUserInfoModel
{
    public required long Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}
