using System.ComponentModel;

namespace AI.DeliciousFood.Core.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo != null)
        {
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
            {
                return attributes.First().Description;
            }
        }
        return enumValue.ToString(); // 如果没有找到 Description，则返回枚举名
    }
}
