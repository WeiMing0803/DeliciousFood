namespace AI.DeliciousFood.Core.Common.ManageModel.UserManager
{
    public class UserManagerModel
    {
        public required long Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateOnly MembershipExpireAt { get; set; }
        public required DateTime CreateDateTime { get; set; }
        public required string RoleId { get; set; }
        public required string Status { get; set; }
    }

    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public required string ControllerName { get; set; }
        public required string ActionName { get; set; }
    }
}
