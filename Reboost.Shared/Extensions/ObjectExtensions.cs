using System;
using System.Collections.Generic;
using System.Text;

namespace Reboost.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetProp(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
