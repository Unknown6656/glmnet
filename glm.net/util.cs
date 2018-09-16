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

        internal static IEnumerable<float> AllNumbers()
        {
            float s = 0;

            while (true)
            {
                yield return s;

                s = (1 - Math.Sign(s)) * (Math.Abs(s) + float.Epsilon * float.Epsilon);
            }
        }

        internal static IEnumerable<float> OpenInterval(float start, bool ascending = true)
        {
            const float ε = float.Epsilon * float.Epsilon;

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
