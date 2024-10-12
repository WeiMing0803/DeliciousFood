namespace AI.DeliciousFood.Core.Common.ManageModel.UserManager
{
    public class UserManagerModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime MembershipExpireAt { get; set; }
        public string Status { get; set; }
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
