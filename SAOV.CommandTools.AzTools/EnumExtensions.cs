namespace SAOV.CommandTools.AzTools
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    internal static class EnumExtensions
    {
        internal static T? GetAttribute<T>(this Enum value) where T : Attribute
        {
            Type type = value.GetType();
            MemberInfo[] memberInfo = type.GetMember(value.ToString());
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo.First().GetCustomAttributes(typeof(T), false);
                if (attributes.Length > 0)
                {
                    return (T)attributes.First();
                }
            }
            return null;
        }

        internal static string? ToName(this Enum value)
        {
            DisplayAttribute? displayAttribute = value.GetAttribute<DisplayAttribute>();
            return displayAttribute != null ? displayAttribute.Name : value.ToString();
        }
    }
}
