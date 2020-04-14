using DocumentFormat.OpenXml.Drawing;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Resources;
namespace HospitalSys.App_Code
{
    public static class EnumExtensions
    {
        // This method can be made private if you don't use it elsewhere
        public static TAttribute GetEnumAttribute<TAttribute>(this Enum enumValue)
          where TAttribute : Attribute
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());
            if (memberInfo.Length == 0)
                return null;
            return memberInfo[0].GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().FirstOrDefault();
        }

        public static string ToDescription(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetEnumAttribute<DisplayAttribute>();

            return displayAttribute == null ? enumValue.ToString().Replace("_", " ") : new ResourceManager(displayAttribute.ResourceType).GetString(displayAttribute.Description, CultureInfo.CurrentUICulture);
        }

        public static string ToName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetEnumAttribute<DisplayAttribute>();
            if (displayAttribute == null)
                return string.Empty;
            return displayAttribute == null ? enumValue.ToString().Replace("_", " ") : new ResourceManager(displayAttribute.ResourceType).GetString(displayAttribute.Name, CultureInfo.CurrentUICulture);
        }
        public static void AddFormattedText(this Run run, string textToAdd)
        {
            var texts = textToAdd.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < texts.Length; i++)
            {
                if (i > 0)
                    run.Append(new Break());
                Text text = new Text();
                text.Text = texts[i];
                run.Append(text);
            }
        }
    }

}
