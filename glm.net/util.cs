using System.Collections.Generic;
using System.Linq;
using System;

#if DOUBLE_PRECISION
using scalar = System.Double;
#else
using scalar = System.Single;
#endif


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

        internal static IEnumerable<scalar> AllNumbers()
        {
            scalar s = 0;

            while (true)
            {
                yield return s;

                s = (1 - Math.Sign(s)) * (Math.Abs(s) + scalar.Epsilon * scalar.Epsilon);
            }
        }

        internal static IEnumerable<scalar> OpenInterval(scalar start, bool ascending = true)
        {
            const scalar ε = scalar.Epsilon * scalar.Epsilon;

            while (true)
            {
                yield return start;

                if (ascending)
                    start += ε;
                else
                    start -= ε;
            }
        }

        internal static string ToSuperScript(this long l) => new string(l.ToString().Trim('+', ' ').Select(c => c == '-' ? '⁻' : "⁰¹²³⁴⁵⁶⁷⁸⁹"[c - '0']).ToArray());
    }
}
