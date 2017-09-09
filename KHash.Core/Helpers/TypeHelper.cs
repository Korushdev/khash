using KHash.Core.Spec;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Helpers
{
    public static class TypeHelper
    {
        public static object CastByString( string type, object value )
        {
            switch( type )
            {
                case Definition.Types.Int:
                    return Convert.ToInt32( value );
                case Definition.Types.Float:
                    return (float)Convert.ToDouble( value );
                case Definition.Types.Double:
                    return Convert.ToDouble( value );
                case Definition.Types.Decimal:
                    return Convert.ToDecimal( value );
                case Definition.Types.Bool:
                    return Convert.ToBoolean( value );
                case Definition.Types.String:
                    return Convert.ToString( value );
                default:
                    return value;
            }
        }
    }
}
