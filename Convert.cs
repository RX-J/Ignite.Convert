namespace Ignite {
    public static class Convert {
        public static T To<T> (this object? value)
            => Convert_Primitive<T> (in value);
        public static T To<T> (this object? value, T exception)
            => Convert_Primitive_Exception (in value, in exception);

        public static T Primitive<T> (in object? value)
            => Convert_Primitive<T> (in value);
        public static T Primitive<T> (in object? value, T exception)
            => Convert_Primitive_Exception (in value, in exception);

        public static void Primitive<T> (in object? value, out T result)
            => result = Convert_Primitive<T> (in value);
        public static void Primitive<T> (in object? value, T exception, out T result)
            => result = Convert_Primitive_Exception (in value, in exception);

        public static void Primitive<T> (ref T result, in object? value)
            => result = Convert_Primitive<T> (in value);
        public static void Primitive<T> (ref T result, in object? value, T exception)
            => result = Convert_Primitive_Exception (in value, in exception);

        public static T[] Array<T> (in System.Collections.Generic.IEnumerable<T> collection)
            => System.Linq.Enumerable.ToArray (collection);
        public static System.Collections.Generic.List<T> List<T> (in System.Collections.Generic.IEnumerable<T> collection)
            => System.Linq.Enumerable.ToList (collection);
        public static System.Collections.Generic.Stack<T> Stack<T> (in System.Collections.Generic.IEnumerable<T> collection)
            => new (collection);
        public static System.Collections.Generic.Queue<T> Queue<T> (in System.Collections.Generic.IEnumerable<T> collection)
            => new (collection);

        public static void Array<T> (in System.Collections.Generic.IEnumerable<T> collection, out T[] result)
            => result = System.Linq.Enumerable.ToArray (collection);
        public static void List<T> (in System.Collections.Generic.IEnumerable<T> collection, out System.Collections.Generic.List<T> result)
            => result = System.Linq.Enumerable.ToList (collection);
        public static void Stack<T> (in System.Collections.Generic.IEnumerable<T> collection, out System.Collections.Generic.Stack<T> result)
            => result = new (collection);
        public static void Queue<T> (in System.Collections.Generic.IEnumerable<T> collection, out System.Collections.Generic.Queue<T> result)
            => result = new (collection);

        public static void Array<T> (ref T[] result, in System.Collections.Generic.IEnumerable<T> collection)
            => result = System.Linq.Enumerable.ToArray (collection);
        public static void List<T> (ref System.Collections.Generic.List<T> result, in System.Collections.Generic.IEnumerable<T> collection)
            => result = System.Linq.Enumerable.ToList (collection);
        public static void Stack<T> (ref System.Collections.Generic.Stack<T> result, in System.Collections.Generic.IEnumerable<T> collection)
            => result = new (collection);
        public static void Queue<T> (ref System.Collections.Generic.Queue<T> result, in System.Collections.Generic.IEnumerable<T> collection)
            => result = new (collection);

        public static T[] Array<T> (in System.Collections.IEnumerable collection)
            => [.. List<T> (collection)];
        public static System.Collections.Generic.List<T> List<T> (in System.Collections.IEnumerable collection) {
            var result = new System.Collections.Generic.List<T> ();

            foreach (var item in collection)
                result.Add (Convert_Primitive<T> (in item));

            return result;
        }
        public static System.Collections.Generic.Stack<T> Stack<T> (in System.Collections.IEnumerable collection)
            => new (List<T> (collection));
        public static System.Collections.Generic.Queue<T> Queue<T> (in System.Collections.IEnumerable collection)
            => new (List<T> (collection));

        public static void Array<T> (in System.Collections.IEnumerable collection, out T[] result)
            => result = [.. List<T> (collection)];
        public static void List<T> (in System.Collections.IEnumerable collection, out System.Collections.Generic.List<T> result)
            => result = List<T> (in collection);
        public static void Stack<T> (in System.Collections.IEnumerable collection, out System.Collections.Generic.Stack<T> result)
            => result = new (List<T> (collection));
        public static void Queue<T> (in System.Collections.IEnumerable collection, out System.Collections.Generic.Queue<T> result)
            => result = new (List<T> (collection));

        public static void Array<T> (ref T[] result, in System.Collections.IEnumerable collection)
            => result = [.. List<T> (collection)];
        public static void List<T> (ref System.Collections.Generic.List<T> result, in System.Collections.IEnumerable collection)
            => result = List<T> (in collection);
        public static void Stack<T> (ref System.Collections.Generic.Stack<T> result, in System.Collections.IEnumerable collection)
            => result = new (List<T> (collection));
        public static void Queue<T> (ref System.Collections.Generic.Queue<T> result, in System.Collections.IEnumerable collection)
            => result = new (List<T> (collection));

        private static T Convert_Primitive<T> (in object? value) {
            if (value == null) {
                return typeof (T).IsValueType && System.Nullable.GetUnderlyingType (typeof (T)) == null
                    ? throw new System.ArgumentNullException (nameof (value), "Cannot convert null to a non-nullable value type.")
                    : default!;
            }

            var target = System.Nullable.GetUnderlyingType (typeof (T)) ?? typeof (T);

            if (IsTuple (in target))
                return (T)ConvertTuple (in value, in target);

            try {
                return (T)ToPrimitive (in value, in target);
            } catch (System.InvalidCastException) {
                throw new System.InvalidOperationException ($"Cannot convert the value '{value}' of type {value.GetType ()} to type {typeof (T)}.");
            } catch (System.FormatException) {
                throw new System.InvalidOperationException ($"Cannot convert the value '{value}' of type {value.GetType ()} to type {typeof (T)} due to format issue.");
            }

            static bool IsTuple (in System.Type type) {
                if (!type.IsGenericType)
                    return false;

                var definition = type.GetGenericTypeDefinition ();

                return definition == typeof (System.ValueTuple<>) ||
                       definition == typeof (System.ValueTuple<,>) ||
                       definition == typeof (System.ValueTuple<,,>) ||
                       definition == typeof (System.ValueTuple<,,,>) ||
                       definition == typeof (System.ValueTuple<,,,,>) ||
                       definition == typeof (System.ValueTuple<,,,,,>) ||
                       definition == typeof (System.ValueTuple<,,,,,,>) ||
                       (definition == typeof (System.ValueTuple<,,,,,,,>) &&
                       System.Linq.Enumerable.Last (type.GetGenericArguments ()).IsGenericType &&
                       IsTuple (System.Linq.Enumerable.Last (type.GetGenericArguments ())));
            }

            static object ConvertTuple (in object value, in System.Type target) {
                if (value == null)
                    throw new System.ArgumentNullException (nameof (value), "Cannot convert null to a tuple.");

                var type = value.GetType ();

                if (!IsTuple (in type))
                    throw new System.InvalidOperationException ("Input value is not a tuple.");

                var arguments = target.GetGenericArguments ();
                var length = arguments.Length;

                if (length != type.GetGenericArguments ().Length && (length != 8 || !IsTuple (System.Linq.Enumerable.Last (arguments))))
                    throw new System.InvalidOperationException ("Tuple lengths do not match.");

                var items = new object[length];

                for (var i = 0; i < length - 1; i++)
                    items[i] = ConvertItem (type.GetField ($"Item{i + 1}")?.GetValue (value)!, arguments[i]);

                if (length == 8) {
                    items[7] = ConvertTuple (type.GetField ("Rest")?.GetValue (value)!, System.Linq.Enumerable.Last (arguments));
                } else {
                    items[length - 1] = ConvertItem (type.GetField ($"Item{length}")?.GetValue (value)!, System.Linq.Enumerable.Last (arguments));
                }

                return System.Activator.CreateInstance (target, items)!;
            }

            static object ConvertItem (in object value, in System.Type target) {
                if (value == null)
                    return System.Nullable.GetUnderlyingType (target) != null
                        ? null!
                        : throw new System.InvalidOperationException ("Null object cannot be converted to a non-nullable value type.");

                var underlying = System.Nullable.GetUnderlyingType (target) ?? target;

                return IsTuple (in underlying)
                    ? ConvertTuple (in value, in underlying)
                    : ToPrimitive (in value, in underlying)!;
            }

            static object ToPrimitive (in object value, in System.Type target) {
                try {
                    return target switch {
                        var type when type == typeof (nint) => ToNInt (in value),
                        var type when type == typeof (nuint) => ToNUInt (in value),
                        _ => ToTargetType (in value, System.Type.GetTypeCode (target))
                    };
                } catch {
                    throw new System.InvalidCastException ($"Unsupported conversion from type '{value.GetType ().Name.Replace ("IntPtr", "nint").Replace ("UIntPtr", "nuint")}' to type '{target.Name}'.");
                }
            }

            static object ToNInt (in object value) => value switch {
                byte type => type,
                ushort type => type,
                uint type => (nint)type,
                ulong type => (nint)type,

                sbyte type => type,
                short type => type,
                int type => type,
                long type => (nint)type,

                nint type => type,
                nuint type => (nint)type,

                float type => (nint)type,
                double type => (nint)type,
                decimal type => (nint)type,

                bool type when type => 1,
                bool type when !type => 0,
                char type => type,
                _ => nint.Parse (value.ToString ()!)
            };

            static object ToNUInt (in object value) => value switch {
                byte type => type,
                ushort type => type,
                uint type => type,
                ulong type => (nuint)type,

                sbyte type => (nuint)type,
                short type => (nuint)type,
                int type => (nuint)type,
                long type => (nuint)type,

                nint type => (nuint)type,
                nuint type => type,

                float type => (nuint)type,
                double type => (nuint)type,
                decimal type => (nuint)type,

                bool type when type => (nuint)1,
                bool type when !type => (nuint)0,
                char type => type,
                _ => nuint.Parse (value.ToString ()!)
            };

            static object ToTargetType (in object value, in System.TypeCode target) => target switch {
                System.TypeCode.Byte => System.Convert.ToByte (value),
                System.TypeCode.UInt16 => System.Convert.ToUInt16 (value),
                System.TypeCode.UInt32 => System.Convert.ToUInt32 (value),
                System.TypeCode.UInt64 => System.Convert.ToUInt64 (value),
                System.TypeCode.SByte => System.Convert.ToSByte (value),
                System.TypeCode.Int16 => System.Convert.ToInt16 (value),
                System.TypeCode.Int32 => System.Convert.ToInt32 (value),
                System.TypeCode.Int64 => System.Convert.ToInt64 (value),
                System.TypeCode.Single => System.Convert.ToSingle (value),
                System.TypeCode.Double => System.Convert.ToDouble (value),
                System.TypeCode.Decimal => System.Convert.ToDecimal (value),
                System.TypeCode.Boolean => ToBoolean (in value),
                System.TypeCode.Char => ToChar (in value),
                System.TypeCode.String => value.ToString ()!,
                System.TypeCode.Object => value,
                _ => throw new System.InvalidCastException ()
            };

            static object ToBoolean (in object value) => value switch {
                char type when (byte)type == 1 => true,
                char type when (byte)type == 0 => false,
                _ => System.Convert.ToBoolean (value)
            };

            static object ToChar (in object value) => value switch {
                float type => (char)type,
                double type => (char)type,
                decimal type => (char)type,
                bool type when type => (char)1,
                bool type when !type => (char)0,
                _ => System.Convert.ToChar (value)
            };
        }
        private static T Convert_Primitive_Exception<T> (in object? value, in T exception) {
            try {
                return Convert_Primitive<T> (in value);
            } catch {
                return exception;
            }
        }
    }
}