using ReSQL.Data;
using System;
using System.Collections;
using System.Reflection;

namespace ReSQL.Utils
{
    internal class Json
    {
        internal static bool IsJsonNeeded(PropertyInfo property)
        {
            return !property.PropertyType.IsPrimitive && !property.PropertyType.Equals(typeof(string));
            /*
            Console.WriteLine(property.PropertyType.Assembly.FullName);
            foreach (var s in SystemNames.systemNames)
            {
                if (property.PropertyType.Assembly.FullName.StartsWith(s))
                {
                    return false;
                }
            }
            return true;*/
        }
    }
}