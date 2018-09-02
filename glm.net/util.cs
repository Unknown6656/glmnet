using System.Collections.Generic;
using System.Linq;
using System;


namespace GlmNet
{
    public static partial class glm
    {
        internal static IEnumerable<V> ZipOuter<T, U, V>(this T[] a, U[] b, Func<T, U, V> func)
        {
            int cmp = a.Length.CompareTo(b.Length);

            if (cmp < 0)
                Array.Resize(ref a, b.Length);
            else if (cmp > 0)
                Array.Resize(ref b, a.Length);

            return a.Zip(b, func);
        }

        internal static string ToSuperScript(this long l) => new string(l.ToString().Trim('+', ' ').Select(c => c == '-' ? '⁻' : "⁰¹²³⁴⁵⁶⁷⁸⁹"[c - '0']).ToArray());
    }
}
