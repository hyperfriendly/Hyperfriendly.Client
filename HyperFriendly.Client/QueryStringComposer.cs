using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Tavis.UriTemplates;

namespace HyperFriendly.Client
{
    public class QueryStringComposer
    {
        public string Compose(string href, object arguments)
        {
            var properties = arguments != null ? arguments.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public) : new PropertyInfo[0];

            var values = new Dictionary<string, string>();

            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(arguments, null);
                values.Add(propertyInfo.Name.ToCamelCase(), value != null ? ConvertToString(value) : null);
            }

            var url = ResolveTemplateUrl(href, values);

            return url;
        }

        private static string ResolveTemplateUrl(string href, Dictionary<string, string> values)
        {
            var template = new UriTemplate(href);

            foreach (var templateValue in values)
                template.SetParameter(templateValue.Key, templateValue.Value);

            return template.Resolve();
        }

        private static string ConvertToString(object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
                return stringValue;

            if (value is char)
                return ((char)value).ToString(CultureInfo.InvariantCulture);
            if (value is int)
                return ((int)value).ToString(CultureInfo.InvariantCulture);
            if (value is long)
                return ((long)value).ToString(CultureInfo.InvariantCulture);
            if (value is double)
                return ((double)value).ToString(CultureInfo.InvariantCulture);
            if (value is Decimal)
                return ((Decimal)value).ToString(CultureInfo.InvariantCulture);
            if (value is DateTime)
                return ((DateTime)value).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            if (value is Guid)
                return ((Guid)value).ToString();

            throw new QueryStringException("Input value of type '" + value.GetType() + "' is not supported.");
        }
    }
}