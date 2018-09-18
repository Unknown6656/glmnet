using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;


namespace GlmNet
{
#if DOUBLE_PRECISION
    using scalar = Double;
#else
    using scalar = Single;
#endif


    /// <summary>
    /// Represents a polynomial
    /// </summary>
    public readonly partial struct poly
        : iarithmeticfield<poly>
        , IEnumerable<scalar>
        , IEquatable<poly>
        , ICloneable
    {
        private readonly scalar[] _coeff;
        

        /// <summary>
        /// The polynomial's degree
        /// </summary>
        public int Degree => _coeff.Length - 1;

        /// <summary>
        /// The polynomial's coefficient in ascending order
        /// </summary>
        public ReadOnlyCollection<scalar> Coefficients => new ReadOnlyCollection<scalar>(_coeff);

        /// <summary>
        /// The polynomial's leading coefficient
        /// </summary>
        public scalar LeadingCoefficient => _coeff.Length > 0 ? _coeff[_coeff.Length - 1] : 0;

        /// <summary>
        /// Evaluates the polynomial at the given X value
        /// </summary>
        /// <param name="x">X value</param>
        /// <returns>Polynomial value at X</returns>
        public scalar this[scalar x] => _coeff.Reverse().Aggregate((acc, c) => acc * x + c);

        /// <summary>
        /// Returns the roots of the polynomial (meaning all x-values, at which poly(x) is evaluated to zero)
        /// </summary>
        public IEnumerable<scalar> Roots => Solve(0);

        /// <summary>
        /// The polynomial's derivative
        /// </summary>
        public poly Derivative => Degree > 0 ? (this << 1).Zip(Enumerable.Range(1, _coeff.Length), (f, i) => f * i).ToArray() : new scalar[0];


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
        public poly(params scalar[] c)
        {
            _coeff = (c ?? new scalar[0])
                    .Reverse()
                    .SkipWhile(glm.is_zero)
                    .Reverse()
                    .ToArray();

            if (_coeff.Length == 0)
                _coeff = new scalar[] { 0 };
        }

        /// <summary>
        /// Creates a new polynomial using the given coefficients in ascending exponential order
        /// </summary>
        /// <param name="c">Polynomial coefficients</param>
        public poly(IEnumerable<scalar> c)
            : this(c?.ToArray())
        {
        }

        /// <inheritdoc />
        IEnumerator<scalar> IEnumerable<scalar>.GetEnumerator() => Coefficients.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<scalar>).GetEnumerator();

        /// <inheritdoc cref="this"/>
        public scalar Evaluate(scalar x) => this[x];

        /// <summary>
        /// Solves the current polynomial for any given y-value
        /// </summary>
        /// <param name="y">y-value</param>
        /// <returns>X-values</returns>
        public IEnumerable<scalar> Solve(scalar y)
        {
            scalar[] co = _coeff;
            poly llc = this << 1;
            int deg = Degree;

            IEnumerable<scalar> __solve()
            {
                if (deg == 0)
                {
                    if (co[0].@is(y))
                        foreach (scalar f in glm.AllNumbers())
                            yield return f;
                }
                else if (deg == 1)
                    yield return (y - co[0]) / co[1];
                else if (co[0].@is(y))
                {
                    yield return 0;

                    foreach (scalar f in llc.Solve(y))
                        yield return f;
                }
                else
                {
                    if (deg == 2)
                    {
                        scalar a = co[2];
                        scalar b = co[1];
                        scalar c = co[0] - y;
                        scalar q = b * b - 4 * a * c;

                        if (q > -scalar.Epsilon)
                        {
                            q = (scalar)(-.5 * Math.Sqrt(q));

                            yield return q / a;

                            if (!q.is_zero())
                                yield return c / q;
                        }
                    }
                    else if (deg == 3)
                        foreach ((double real, double imag) in glm.SolveCardano(co[3], co[2], co[1], co[0] - y))
                        {
                            if (imag.is_zero())
                                yield return (scalar)real;
                        }
                    else if (deg == 4)
                    {
                        // solve for 
                        // 0 = ax⁴ + bx³ + cx² + dx + e
                        //   = (((ax + b)x + c)x + d)x + e

                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            foreach (scalar x in __solve().Distinct())
                yield return x;
        }

        /// <summary>
        /// Compares the given polynomial with the current instance and returns whether both are equal.
        /// </summary>
        /// <param name="obj">Second polynomial</param>
        /// <returns>Comparison result</returns>
        public override bool Equals(object obj) => obj is poly p && Equals(p);

        /// <inheritdoc/>
        public override int GetHashCode() => this.Aggregate(-Degree, (h, f) => h ^ f.GetHashCode());

        /// <inheritdoc/>
        public object Clone() => new poly(_coeff as IEnumerable<scalar>);

        /// <summary>
        /// Compares the given polynomial with the current instance and returns whether both are equal.
        /// </summary>
        /// <param name="other">Second polynomial</param>
        /// <returns>Comparison result</returns>
        public bool Equals(poly other) => _coeff.ZipOuter(other._coeff, glm.@is).All(b => b);

        /// <summary>
        /// Returns the string representation of the current polynomial.
        /// </summary>
        /// <returns>String representation</returns>
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
        }).Where(s => s.Length > 0).Reverse()).Replace("+ -", "- ");

        /// <summary>
        /// Divides the two given polynomials using polynomial long division and returns the division result
        /// </summary>
        /// <param name="x">First polynomial</param>
        /// <param name="y">Second polynomial</param>
        /// <returns>Divison quotient and remainder</returns>
        public static (poly Quotient, poly Remainder) PolynomialDivision(poly x, poly y)
        {
            int nd = x.Degree;
            int dd = y.Degree;

            if (dd < 0)
                throw new ArgumentException("Divisor must have at least one one-zero coefficient");

            if (nd < dd)
                throw new ArgumentException("The degree of the divisor cannot exceed that of the numerator");

            poly r = (poly)x.Clone();
            scalar[] q = new scalar[nd * 2];

            while (nd >= dd)
            {
                poly d2 = y >> (nd - dd);

                q[nd - dd] = r[nd] / d2[nd];

                d2 *= q[nd - dd];
                r -= d2;

                nd = r.Degree;
            }

            return (q, r);
        }

        /// <inheritdoc/>
        public poly Add(poly second) => this + second;

        /// <inheritdoc/>
        public poly Negate(poly second) => -this;

        /// <inheritdoc/>
        public poly Subtract(poly second) => this - second;

        /// <inheritdoc/>
        public poly Multiply(poly second) => this * second;

        /// <inheritdoc/>
        public poly Multiply(scalar factor) => this * factor;


        /// <summary>
        /// Implicitly converts the given scalar to a polynomial of degree zero.
        /// </summary>
        /// <param name="f">Scalar</param>
        public static implicit operator poly(scalar f) => new poly(f);

        /// <summary>
        /// Implicitly converts the given array of scalar coefficients to their respective polynomial representation.
        /// </summary>
        /// <param name="c">Scalar coefficients</param>
        public static implicit operator poly(scalar[] c) => new poly(c);

        /// <summary>
        /// Compares the two given polynomials and returns whether both are equal.
        /// </summary>
        /// <param name="p1">Second polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Comparison result</returns>
        public static bool operator ==(poly p1, poly p2) => p1.Equals(p2);

        /// <summary>
        /// Compares the two given polynomials and returns whether both are not equal to each other.
        /// </summary>
        /// <param name="p1">Second polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Comparison result</returns>
        public static bool operator !=(poly p1, poly p2) => !(p1 == p2);

        /// <summary>
        /// Represents the identity-function
        /// </summary>
        /// <param name="p">Polynomial</param>
        /// <returns>Unchanged polynomial</returns>
        public static poly operator +(poly p) => p;
        
        /// <summary>
        /// Negates the given polynomial by negating each coefficient
        /// </summary>
        /// <param name="p">Polynomial</param>
        /// <returns>Negated polynomial</returns>
        public static poly operator -(poly p) => p.Select(c => -c).ToArray();

        /// <summary>
        /// Performs the addition of two polynomials by adding their respective coefficients.
        /// </summary>
        /// <param name="p1">First polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Addition result</returns>
        public static poly operator +(poly p1, poly p2) => p1._coeff.ZipOuter(p2._coeff, (c1, c2) => c1 + c2).ToArray();

        /// <summary>
        /// Performs the subtraction of two polynomials by subtracting their respective coefficients.
        /// </summary>
        /// <param name="p1">First polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Subtraction result</returns>
        public static poly operator -(poly p1, poly p2) => p1 + -p2;

        /// <summary>
        /// Performs the multiplication of a polynomial with a single scalar. All of the polynomial's coefficients will be multiplied by the scalar
        /// </summary>
        /// <param name="f">Scalar</param>
        /// <param name="p">Polynomial</param>
        /// <returns>Multiplication result</returns>
        public static poly operator *(scalar f, poly p) => p * f;

        /// <summary>
        /// Performs the multiplication of a polynomial with a single scalar. All of the polynomial's coefficients will be multiplied by the scalar
        /// </summary>
        /// <param name="f">Scalar</param>
        /// <param name="p">Polynomial</param>
        /// <returns>Multiplication result</returns>
        public static poly operator *(poly p, scalar f) => p.Select(c => c * f).ToArray();

        /// <summary>
        /// Performs the multiplication of two polynomials.
        /// </summary>
        /// <param name="p1">First polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Multiplication result</returns>
        public static poly operator *(poly p1, poly p2) => p1.Select((c, i) => (p2 * c) >> i).Aggregate(Zero, (p, a) => p + a);

        /// <summary>
        /// Raises the given polynomial to the given (non-negative) power
        /// </summary>
        /// <param name="p">Polynomial</param>
        /// <param name="e">Exponent</param>
        /// <returns>Result</returns>
        public static poly operator ^(poly p, int e)
        {
            if (e < 0)
                throw new ArgumentOutOfRangeException(nameof(e));

            poly r = One;

            for (int i = 0; i < e; ++i)
                r *= p;

            return r;
        }

        /// <summary>
        /// Performs the scalar division by dividing each of the given polynomial's coefficient by the given scalar value.
        /// </summary>
        /// <param name="p">Polynomial</param>
        /// <param name="f">Scalar divisor</param>
        /// <returns>Division result</returns>
        public static poly operator /(poly p, scalar f) => p * (1 / f);

        /// <summary>
        /// Performs the polynomial long division and returns the quotient
        /// </summary>
        /// <param name="p1">First polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Quotient</returns>
        public static poly operator /(poly p1, poly p2) => p2.Degree == 0 ? p1 / p2[0] : PolynomialDivision(p1, p2).Quotient;

        /// <summary>
        /// Performs the polynomial long division and returns the remainder
        /// </summary>
        /// <param name="p1">First polynomial</param>
        /// <param name="p2">Second polynomial</param>
        /// <returns>Remainder</returns>
        public static poly operator %(poly p1, poly p2) => p1 == p2 ? 0 : PolynomialDivision(p1, p2).Remainder;

        /// <summary>
        /// <b>Increaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Increacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator >>(poly p, int a) => a < 0 ? p >> -a : a == 0 ? p :  Enumerable.Repeat((scalar)0, a).Concat(p).ToArray();

        /// <summary>
        /// <b>Decreaces</b> the polynomial's degree by the given amount <paramref name="a"/> (equivalent to multiplying it with X^-a)
        /// </summary>
        /// <param name="p">Input polynomial</param>
        /// <param name="a">Decreacement 'amount'</param>
        /// <returns>Output polynomial</returns>
        public static poly operator <<(poly p, int a) => a < 0 ? p << -a : a == 0 ? p : p.Skip(Math.Min(p.Degree, a)).ToArray();
    }
}