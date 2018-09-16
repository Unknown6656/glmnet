using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a polynomial
    /// </summary>
    public readonly struct poly
        : IEnumerable<float>
    {
        private readonly float[] _coeff;
        

        /// <summary>
        /// The polynomial's degree
        /// </summary>
        public int Degree => _coeff.Length - 1;

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

                for (int i = 0; i < _coeff.Length; ++i)
                {
                    res += _coeff[i] * _x;
                    _x *= x;
                }

                return res;
            }
        }

        public poly Derivative => (this << 1).Zip(Enumerable.Range(1, _coeff.Length), (f, i) => f * i).ToArray();


        /// <summary>
        /// The constant zero polynomial
        /// </summary>
        public static poly Zero { get; } = 0;

        /// <summary>
        /// The constant one polynomial
        /// </summary>
        public static poly One { get; } = 1;

        /// <summary>
        /// The polynomial 'x'
        /// </summary>
        public static poly X { get; } = One >> 1;


        /// <summary>
        /// Creates a new polynomial using the given coefficients in ascending exponential order
        /// </summary>
        /// <param name="c">Polynomial coefficients</param>
        public poly(params float[] c)
        {
            _coeff = (c ?? new float[0])
                    .Reverse()
                    .SkipWhile(glm.is_zero)
                    .Reverse()
                    .ToArray();

            if (_coeff.Length == 0)
                _coeff = new[] { 0f };
        }

        /// <inheritdoc />
        public poly(IEnumerable<float> c)
            : this(c?.ToArray())
        {
        }

        /// <inheritdoc />
        IEnumerator<float> IEnumerable<float>.GetEnumerator() => Coefficients.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<float>).GetEnumerator();

        /// <summary>
        /// Solves the current polynomial for any given y-value
        /// </summary>
        /// <param name="y">y-value</param>
        /// <returns>X-values</returns>
        public IEnumerable<float> Solve(float y)
        {
            if (Degree == 0)
            {
                if (_coeff[0].@is(y))
                    foreach (float f in glm.AllNumbers())
                        yield return f;
            }
            else if (Degree == 1)
                yield return (y - _coeff[0]) / _coeff[1];
            else if (Degree == 2)
            {
                double a = _coeff[2];
                double b = _coeff[1];
                double c = _coeff[0];
                double q = b * b - 4 * a * c;

                if (q > -double.Epsilon)
                {
                    q = -.5 * Math.Sqrt(q);

                    yield return (float)(q / a);

                    if (q >= double.Epsilon)
                        yield return (float)(c / q);
                }
            }
            else if (Degree == 3)
            {
                // y = f(x)
                // y = ax³ + bx² + cx + d
                // y - d = ax³ + bx² + cx
                // y - d = (ax² + bx + c)x
                // y - d = ((ax + b)x + c)x

            }
            else if (Degree == 4)
            {
                // y = f(x)
                // y = ax⁴ + bx³ + cx² + dx + e
                // y - e = ax⁴ + bx³ + cx² + dx
                // y - e = (ax³ + bx² + cx + d)x
                // y - e = ((ax² + bx + c)x + d)x
                // y - e = (((ax + b)x + c)x + d)x
            }
            else
            {
                
            }
        }

        public override bool Equals(object obj) => obj is poly p && _coeff.ZipOuter(p._coeff, glm.@is).All(b => b);

        public override int GetHashCode() => this.Aggregate(-Degree, (h, f) => h ^ f.GetHashCode());

        public override string ToString() => string.Join(" + ", this.Select((c, i) =>
        {
            if (c.is_zero())
                return "";

            string cstr = c.ToString(null as IFormatProvider);

            if (i == 0)
                return cstr;

            if (Math.Abs(c).@is(1))
                cstr = cstr.Trim('1');
            
            if (i == 1)
                return cstr + "x";
            else
                return $"{cstr}x{glm.ToSuperScript(i)}";
        }).Where(s => s.Length > 0).Reverse());


        public static implicit operator poly(float f) => new poly(f);

        public static implicit operator poly(float[] c) => new poly(c);

        public static poly operator +(poly p) => p;

        public static poly operator -(poly p) => p.Select(c => -c).ToArray();

        public static poly operator +(poly p1, poly p2) => p1._coeff.ZipOuter(p2._coeff, (c1, c2) => c1 + c2).ToArray();

        public static poly operator -(poly p1, poly p2) => p1 + -p2;

        public static poly operator *(float f, poly p) => p * f;

        public static poly operator *(poly p, float f) => p.Select(c => c * f).ToArray();

        public static poly operator *(poly p1, poly p2) => p1.Select((c, i) => (p2 * c) >> i).Aggregate(Zero, (p, a) => p + a);

        public static poly operator ^(poly p, int e)
        {
            if (e < 0)
                throw new ArgumentOutOfRangeException(nameof(e));

            poly r = One;

            for (int i = 0; i < e; ++i)
                r *= p;

            return r;
        }

        public static poly operator /(poly p, float f) => p * (1 / f);

        // public static poly operator /(poly p1, poly p2) => ;

        /// <summary>
        /// <b>Increaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Increacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator >>(poly p, int a) => a < 0 ? p >> -a : a == 0 ? p :  Enumerable.Repeat(0f, a).Concat(p).ToArray();

        /// <summary>
        /// <b>Decreaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^-a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Decreacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator <<(poly p, int a) => a < 0 ? p << -a : a == 0 ? p : p.Skip(Math.Min(p.Degree, a)).ToArray();
    }
}
