namespace AI.DeliciousFood.Core.Model
{

    /// <summary>
    /// 定义会员价格表
    /// </summary>
    public class MemberPrice
    {
        /// <summary>
        /// GUID
        /// </summary>
        public Guid MemberPriceGuid { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
