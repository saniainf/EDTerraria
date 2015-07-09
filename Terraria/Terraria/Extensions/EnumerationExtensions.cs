/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;

namespace Extensions
{
    public static class EnumerationExtensions
    {
        public static T Include<T>(this Enum value, T append)
        {
            Type type = value.GetType();
            object obj1 = value;
            EnumerationExtensions._Value obj2 = new EnumerationExtensions._Value(append, type);
            if (obj2.Signed.HasValue)
                obj1 = (Convert.ToInt64(value) | obj2.Signed.Value);
            else if (obj2.Unsigned.HasValue)
                obj1 = (ulong)((long)Convert.ToUInt64(value) | (long)obj2.Unsigned.Value);
            return (T)Enum.Parse(type, obj1.ToString());
        }

        public static T Remove<T>(this Enum value, T remove)
        {
            Type type = value.GetType();
            object obj1 = value;
            EnumerationExtensions._Value obj2 = new EnumerationExtensions._Value(remove, type);
            if (obj2.Signed.HasValue)
                obj1 = (Convert.ToInt64((object)value) & ~obj2.Signed.Value);
            else if (obj2.Unsigned.HasValue)
                obj1 = (ulong)((long)Convert.ToUInt64(value) & ~(long)obj2.Unsigned.Value);
            return (T)Enum.Parse(type, obj1.ToString());
        }

        public static bool Has<T>(this Enum value, T check)
        {
            Type type = value.GetType();
            EnumerationExtensions._Value obj = new EnumerationExtensions._Value(check, type);
            if (obj.Signed.HasValue)
                return (Convert.ToInt64(value) & obj.Signed.Value) == obj.Signed.Value;
            if (obj.Unsigned.HasValue)
                return ((long)Convert.ToUInt64(value) & (long)obj.Unsigned.Value) == (long)obj.Unsigned.Value;
            return false;
        }

        public static bool Missing<T>(this Enum obj, T value)
        {
            return !EnumerationExtensions.Has<T>(obj, value);
        }

        private class _Value
        {
            private static Type _UInt64 = typeof(ulong);
            private static Type _UInt32 = typeof(long);
            public long? Signed;
            public ulong? Unsigned;

            public _Value(object value, Type type)
            {
                if (!type.IsEnum)
                    throw new ArgumentException("Value provided is not an enumerated type!");
                Type underlyingType = Enum.GetUnderlyingType(type);
                if (underlyingType.Equals(EnumerationExtensions._Value._UInt32) || underlyingType.Equals(EnumerationExtensions._Value._UInt64))
                    Unsigned = new ulong?(Convert.ToUInt64(value));
                else
                    Signed = new long?(Convert.ToInt64(value));
            }
        }
    }
}
