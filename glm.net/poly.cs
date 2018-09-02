using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GlmNet
{
    /// <summary>
    /// Represents a polynomial
    /// </summary>
    public readonly struct poly
        : IEnumerable<float>
    {
        private readonly float[] _coeff;

        // ⁰¹²³⁴⁵⁶⁷⁸⁹


        /// <summary>
        /// The polynomial's degree
        /// </summary>
        public int Degree => _coeff.Length;

        /// <summary>
        /// The polynomial's coefficient in ascending order
        /// </summary>
        public ReadOnlyCollection<float> Coefficients => new ReadOnlyCollection<float>(_coeff);

        /// <summary>
        /// Evaluates the polynomial at the given X value
        /// </summary>
        /// <param name="x">X value</param>
        /// <returns>Polynomial value at X</returns>
        public float this[float x]
        {
            get
            {
                float res = 0;
                float _x = 1;

                for (int i = 0; i < Degree; ++i)
                {
                    res += _coeff[i] * _x;
                    _x *= x;
                }

                return res;
            }
        }

        public poly Derivative => (this >> 1).Zip(Enumerable.Range(0, Degree - 1), (f, i) => f * i).ToArray();


        /// <summary>
        /// Creates a new polynomial using the given coefficients in ascending exponential order
        /// </summary>
        /// <param name="c">Polynomial coefficients</param>
        public poly(params float[] c) =>
            _coeff = (c ?? new float[0])
                    .Reverse()
                    .SkipWhile(glm.is_zero)
                    .Reverse()
                    .ToArray();

        /// <inheritdoc />
        public poly(IEnumerable<float> c)
            : this(c?.ToArray())
        {
        }

        /// <inheritdoc />
        IEnumerator<float> IEnumerable<float>.GetEnumerator() => Coefficients.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<float>).GetEnumerator();

        public override bool Equals(object obj) => obj is poly p && _coeff.ZipOuter(p._coeff, glm.@is).All(b => b);

        public override int GetHashCode() => this.Aggregate(-Degree, (h, f) => h ^ f.GetHashCode());

        public override string ToString() => string.Join(" + ", this.Select((c, i) => c.is_zero() ? "" : $"{c}⋅x{glm.ToSuperScript(i)}").Where(s => s.Length > 0).Reverse());


        public static implicit operator poly(float f) => new poly(f);

        public static implicit operator poly(float[] c) => new poly(c);

        public static poly operator +(poly p) => p;

        public static poly operator -(poly p) => p.Select(c => -c).ToArray();

        public static poly operator +(poly p1, poly p2) => p1._coeff.ZipOuter(p2._coeff, (c1, c2) => c1 + c2).ToArray();

        public static poly operator -(poly p1, poly p2) => p1 + -p2;

        /// <summary>
        /// <b>Increaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Increacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator <<(poly p, int a) => a < 0 ? p >> -a : a == 0 ? p : new[] { 0f }.Concat(p).ToArray();

        /// <summary>
        /// <b>Decreaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^-a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Decreacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator >>(poly p, int a) => a < 0 ? p << -a : a == 0 ? p : p.Take(Math.Max(p.Degree - 1, 0)).ToArray();
    }
}
