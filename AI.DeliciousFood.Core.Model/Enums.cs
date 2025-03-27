using System.ComponentModel;

namespace AI.DeliciousFood.Core.Common.Model;
public enum Roles
{
    ADMIN = 1,
    USER = 2,
    VIPUSER = 3
}

public enum StatusEnum
{
    [Description("草稿")]
    Draft = 1,
    [Description("审核中")]
    UnderReview = 2,
    [Description("已发布")]
    Approved = 3,
    [Description("审核未通过")]
    NotApproved = 4
}