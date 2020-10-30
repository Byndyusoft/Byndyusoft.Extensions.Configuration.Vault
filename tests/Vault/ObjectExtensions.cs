namespace Byndyusoft.Extensions.Configuration.Vault.Vault
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object values)
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(values))
            {
                dict.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(values));
            }

            return dict;
        }
    }
}