namespace AI.DeliciousFood.Core.Common.Model;
public enum Roles
{
    ADMIN = 1,
    USER = 2,
    VIPUSER = 3
}

public enum StatusEnum
{
    /// <summary>
    /// 草稿
    /// </summary>
    Draft = 1,
    /// <summary>
    /// 审核中
    /// </summary>
    UnderReview = 2,
    /// <summary>
    /// 审核通过
    /// </summary>
    Approved = 3,
    /// <summary>
    /// 审核未通过
    /// </summary>
    NotApproved = 4
}