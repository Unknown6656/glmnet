using System.Collections.Generic;
using System.Linq;
using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a 2x2 matrix.
    /// </summary>
    public readonly struct mat2
        : imat<mat2, vec2>
    {
        /// <summary>
        /// The columms of the matrix.
        /// </summary>
        private readonly vec2[] cols;


        /// <summary>
        /// Gets or sets the <see cref="vec2"/> column at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="vec2"/> column.
        /// </value>
        /// <param name="column">The column index.</param>
        /// <returns>The column at index <paramref name="column"/>.</returns>
        public vec2 this[int column]
        {
            get => cols[column];
            set => cols[column] = value;
        }

        /// <summary>
        /// Gets or sets the element at <paramref name="column"/> and <paramref name="row"/>.
        /// </summary>
        /// <value>
        /// The element at <paramref name="column"/> and <paramref name="row"/>.
        /// </value>
        /// <param name="column">The column index.</param>
        /// <param name="row">The row index.</param>
        /// <returns>
        /// The element at <paramref name="column"/> and <paramref name="row"/>.
        /// </returns>
        public float this[int column, int row]
        {
            get => cols[column][row];
            set => cols[column][row] = value;
        }

        public float Determinant => this[0, 0] * this[1, 1] - this[1, 0] * this[0, 1];

        public bool IsInvertible => Math.Abs(Determinant) >= float.Epsilon;


        /// <summary>
        /// Initializes a new instance of the <see cref="mat2"/> struct.
        /// This matrix is the identity matrix scaled by <paramref name="scale"/>.
        /// </summary>
        /// <param name="scale">The scale.</param>
        public mat2(float scale)
            : this(new vec2(scale, 0.0f), new vec2(0.0f, scale))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="mat2"/> struct.
        /// The matrix is initialised with the <paramref name="cols"/>.
        /// </summary>
        /// <param name="cols">The colums of the matrix.</param>
        public mat2(IReadOnlyList<vec2> cols) => this.cols = new[]
        {
            cols[0],
            cols[1]
        };

        public mat2(vec2 a, vec2 b)
            : this(new[] { a, b })
        {
        }

        public mat2(float a, float b, float c, float d)
            : this(new vec2(a, b), new vec2(c, d))
        {
        }


        /// <summary>
        /// Returns the matrix as a flat array of elements, column major.
        /// </summary>
        /// <returns></returns>
        public float[] to_array() => cols.SelectMany(v => v.to_array()).ToArray();

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is mat2 mat && mat[0] == this[0] && mat[1] == this[1];

        /// <inheritdoc/>
        public override int GetHashCode() => this[0].GetHashCode() ^ this[1].GetHashCode();


        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <returns>A new identity matrix.</returns>
        public static mat2 identity() => new mat2(1);

        /// <summary>
        /// Creates an zero matrix.
        /// </summary>
        /// <returns>A new zero matrix.</returns>
        public static mat2 zero() => new mat2(0);


        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(mat2 m1, mat2 m2) => m1.Equals(m2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(mat2 m1, mat2 m2) => !(m1 == m2);

        public static mat2 operator +(mat2 m) => m;

        public static mat2 operator -(mat2 m) => m * -1f;

        public static mat2 operator -(float f, mat2 m) => new mat2(f) - m;

        public static mat2 operator -(mat2 m, float f) => m + -f;

        public static mat2 operator +(float f, mat2 m) => m + f;

        public static mat2 operator +(mat2 m, float f) => m + new mat2(f);

        public static mat2 operator -(mat2 m1, mat2 m2) => m1 + -m2;

        /// <summary>
        /// Multiplies the <paramref name="m"/> matrix by the <paramref name="v"/> vector.
        /// </summary>
        /// <param name="m">The LHS matrix.</param>
        /// <param name="v">The RHS vector.</param>
        /// <returns>The product of <paramref name="m"/> and <paramref name="v"/>.</returns>
        public static vec2 operator *(mat2 m, vec2 v) => new vec2(
            m[0, 0] * v[0] + m[1, 0] * v[1],
            m[0, 1] * v[0] + m[1, 1] * v[1]
        );

        /// <summary>
        /// Multiplies the <paramref name="m1"/> matrix by the <paramref name="m2"/> matrix.
        /// </summary>
        /// <param name="m1">The LHS matrix.</param>
        /// <param name="m2">The RHS matrix.</param>
        /// <returns>The product of <paramref name="m1"/> and <paramref name="m2"/>.</returns>
        public static mat2 operator *(mat2 m1, mat2 m2) => new mat2(glm._2.Select(j => new vec2(glm._2.Select(i => glm._2.Sum(k => m1[k, j] * m2[i, k])))));

        public static mat2 operator *(mat2 m, float f) => new mat2(m[0] * f, m[1] * f);

        public static mat2 operator /(mat2 m, float f) => m * (1 / f);

        public static implicit operator (vec2 a, vec2 b) (mat2 v) => (v[0], v[1]);

        public static implicit operator mat2((vec2 a, vec2 b) t) => new mat2(t.a, t.b);
    }
}