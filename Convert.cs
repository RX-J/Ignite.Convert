namespace Ignite {
    internal static class Convert {
        public static T To<T> (this object? value)
            => Primitive<T> (value);
        public static T Primitive<T> (object? value) {
            if (value == null && System.Nullable.GetUnderlyingType (typeof (T)) == null)
                throw new System.ArgumentNullException (nameof (value));

            var type = value!.GetType ().Name.Replace ("IntPtr", "nint").Replace ("Unint", "nuint");
            var targetType = typeof (T);
            var targetTypeCode = System.Type.GetTypeCode (targetType);

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition () == typeof (System.Nullable<>)) {
                if (value == null)
                    return default!;

                targetType = System.Nullable.GetUnderlyingType (targetType);
                targetTypeCode = System.Type.GetTypeCode (targetType);
            }

            try {
                return targetType == typeof (nint)
                    ? value switch {
                        bool x when x == true => (T)(object)(nint)1,
                        bool x when x == false => (T)(object)(nint)0,
                        char x => (T)(object)(nint)x,
                        _ => (T)(object)nint.Parse (value!.ToString ()!)
                    }
                    : targetType == typeof (nuint)
                        ? value switch {
                            bool x when x == true => (T)(object)(nuint)1,
                            bool x when x == false => (T)(object)(nuint)0,
                            char x => (T)(object)(nuint)x,
                            _ => (T)(object)nuint.Parse (value!.ToString ()!)
                        }
                        : targetTypeCode switch {
                            System.TypeCode.Byte when value is nint x => (T)(object)(byte)x,
                            System.TypeCode.UInt16 when value is nint x => (T)(object)(ushort)x,
                            System.TypeCode.UInt32 when value is nint x => (T)(object)(uint)x,
                            System.TypeCode.UInt64 when value is nint x => (T)(object)(ulong)x,

                            System.TypeCode.SByte when value is nint x => (T)(object)(sbyte)x,
                            System.TypeCode.Int16 when value is nint x => (T)(object)(short)x,
                            System.TypeCode.Int32 when value is nint x => (T)(object)(int)x,
                            System.TypeCode.Int64 when value is nint x => (T)(object)(long)x,

                            System.TypeCode.Single when value is nint x => (T)(object)(float)x,
                            System.TypeCode.Double when value is nint x => (T)(object)(double)x,
                            System.TypeCode.Decimal when value is nint x => (T)(object)(decimal)x,

                            System.TypeCode.Boolean when value is nint x && x == 1 => (T)(object)true,
                            System.TypeCode.Boolean when value is nint x && x == 0 => (T)(object)false,
                            System.TypeCode.Char when value is nint x => (T)(object)(char)x,

                            System.TypeCode.Byte when value is nuint x => (T)(object)(byte)x,
                            System.TypeCode.UInt16 when value is nuint x => (T)(object)(ushort)x,
                            System.TypeCode.UInt32 when value is nuint x => (T)(object)(uint)x,
                            System.TypeCode.UInt64 when value is nuint x => (T)(object)(ulong)x,

                            System.TypeCode.SByte when value is nuint x => (T)(object)(sbyte)x,
                            System.TypeCode.Int16 when value is nuint x => (T)(object)(short)x,
                            System.TypeCode.Int32 when value is nuint x => (T)(object)(int)x,
                            System.TypeCode.Int64 when value is nuint x => (T)(object)(long)x,

                            System.TypeCode.Single when value is nuint x => (T)(object)(float)x,
                            System.TypeCode.Double when value is nuint x => (T)(object)(double)x,
                            System.TypeCode.Decimal when value is nuint x => (T)(object)(decimal)x,

                            System.TypeCode.Boolean when value is nuint x && x == 1 => (T)(object)true,
                            System.TypeCode.Boolean when value is nuint x && x == 0 => (T)(object)false,
                            System.TypeCode.Char when value is nuint x => (T)(object)(char)x,

                            System.TypeCode.Single when value is char x => (T)(object)(float)x,
                            System.TypeCode.Double when value is char x => (T)(object)(double)x,
                            System.TypeCode.Decimal when value is char x => (T)(object)(decimal)x,

                            System.TypeCode.Byte => (T)(object)System.Convert.ToByte (value),
                            System.TypeCode.UInt16 => (T)(object)System.Convert.ToUInt16 (value),
                            System.TypeCode.UInt32 => (T)(object)System.Convert.ToUInt32 (value),
                            System.TypeCode.UInt64 => (T)(object)System.Convert.ToUInt64 (value),

                            System.TypeCode.SByte => (T)(object)System.Convert.ToSByte (value),
                            System.TypeCode.Int16 => (T)(object)System.Convert.ToInt16 (value),
                            System.TypeCode.Int32 => (T)(object)System.Convert.ToInt32 (value),
                            System.TypeCode.Int64 => (T)(object)System.Convert.ToInt64 (value),

                            System.TypeCode.Single => (T)(object)System.Convert.ToSingle (value),
                            System.TypeCode.Double => (T)(object)System.Convert.ToDouble (value),
                            System.TypeCode.Decimal => (T)(object)System.Convert.ToDecimal (value),

                            System.TypeCode.Boolean => value switch {
                                char x when (byte)x == 1 => (T)(object)true,
                                char x when (byte)x == 0 => (T)(object)false,
                                _ => (T)(object)System.Convert.ToBoolean (value)
                            },

                            System.TypeCode.Char => value switch {
                                float x => (T)(object)(char)x,
                                double x => (T)(object)(char)x,
                                decimal x => (T)(object)(char)x,
                                bool x when x == true => (T)(object)(char)1,
                                bool x when x == false => (T)(object)(char)0,
                                _ => (T)(object)System.Convert.ToChar (value)
                            },

                            System.TypeCode.String => (T)(object)value!.ToString ()!,

                            System.TypeCode.Object => (T)value!,

                            _ => throw new System.InvalidCastException ()
                        };
            } catch {
                throw new System.InvalidCastException ($"Unsupported conversion from type '{type}' to type '{targetType!.Name}'.");
            }
        }

        public static T[] Array<T> (System.Collections.Generic.IEnumerable<T> collection)
            => System.Linq.Enumerable.ToArray (collection);
        public static System.Collections.Generic.List<T> List<T> (System.Collections.Generic.IEnumerable<T> collection)
            => System.Linq.Enumerable.ToList (collection);
        public static System.Collections.Generic.Stack<T> Stack<T> (System.Collections.Generic.IEnumerable<T> collection)
            => new (collection);
        public static System.Collections.Generic.Queue<T> Queue<T> (System.Collections.Generic.IEnumerable<T> collection)
            => new (collection);
    }
}