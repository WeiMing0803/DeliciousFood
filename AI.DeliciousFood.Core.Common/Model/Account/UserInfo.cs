namespace AI.DeliciousFood.Core.Common.Model.Account;


public class GetUserInfoModel
{
    public required UserInfoModel UserInfoModel {  get; set; }
    public required List<MenuSummary> Draft { get; set; }
    public required List<MenuSummary> UnderReviewOrNotApproved { get; set; }
    public required List<MenuSummary> Approved { get; set; }
}

public class MenuSummary
{
    public required Guid Guid { get; set; }
    public required string RecipeName { get; set; }
    public required string RecipeDescription { get; set; }
    public required string FileName { get; set; }
    public required string ImageUrl { get; set; }
}

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
