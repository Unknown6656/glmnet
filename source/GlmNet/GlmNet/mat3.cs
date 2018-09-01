using System.Collections.Generic;
using System.Linq;
using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a 3x3 matrix.
    /// </summary>
    public readonly struct mat3
        : imat<mat3, vec3>
    {
        /// <summary>
        /// The columms of the matrix.
        /// </summary>
        private readonly vec3[] cols;


        /// <summary>
        /// Gets or sets the <see cref="vec3"/> column at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="vec3"/> column.
        /// </value>
        /// <param name="column">The column index.</param>
        /// <returns>The column at index <paramref name="column"/>.</returns>
        public vec3 this[int column]
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

        public float Determinant =>
            this[0, 0] * (this[1, 1] * this[2, 2] - this[2, 1] * this[1, 2])
          - this[1, 0] * (this[0, 1] * this[2, 2] - this[2, 1] * this[0, 2])
          + this[2, 0] * (this[0, 1] * this[1, 2] - this[1, 1] * this[0, 2]);

        public bool IsInvertible => Math.Abs(Determinant) >= float.Epsilon;

        public mat3 Inverse => this.inverse();


        /// <summary>
        /// Initializes a new instance of the <see cref="mat3"/> struct.
        /// This matrix is the identity matrix scaled by <paramref name="scale"/>.
        /// </summary>
        /// <param name="scale">The scale.</param>
        public mat3(float scale)
            : this(new vec3(scale, 0.0f, 0.0f),
                   new vec3(0.0f, scale, 0.0f),
                   new vec3(0.0f, 0.0f, scale))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="mat3"/> struct.
        /// The matrix is initialised with the <paramref name="cols"/>.
        /// </summary>
        /// <param name="cols">The colums of the matrix.</param>
        public mat3(IEnumerable<vec3> cols) => this.cols = cols.Take(3).ToArray();

        public mat3(vec3 a, vec3 b, vec3 c)
            : this(new[]{ a, b, c })
        {
        }

        /// <summary>
        /// Returns the matrix as a flat array of elements, column major.
        /// </summary>
        /// <returns></returns>
        public float[] to_array() => cols.SelectMany(v => v.to_array()).ToArray();

        /// <summary>
        /// Returns the <see cref="mat3"/> portion of this matrix.
        /// </summary>
        /// <returns>The <see cref="mat3"/> portion of this matrix.</returns>
        public mat2 to_mat2() => new mat2(
            new vec2(cols[0][0], cols[0][1]),
            new vec2(cols[1][0], cols[1][1]));

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is mat3 mat &&
                                                   mat[0] == this[0] &&
                                                   mat[1] == this[1] &&
                                                   mat[2] == this[2];

        /// <inheritdoc/>
        public override int GetHashCode() => this[0].GetHashCode() ^ this[1].GetHashCode() ^ this[2].GetHashCode();

        public mat2 minor(int column, int row)
        {
            vec3[] c = cols; // local copy because w/ever

            return new mat2(glm._3.Except(new[] { column }).Select(j => new vec2(glm._3.Except(new[] { row }).Select(i => c[j][i]))));
        }


        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <returns>A new identity matrix.</returns>
        public static mat3 identity() => new mat3(1);

        /// <summary>
        /// Creates an zero matrix.
        /// </summary>
        /// <returns>A new zero matrix.</returns>
        public static mat3 zero() => new mat3(0);


        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(mat3 m1, mat3 m2) => m1.Equals(m2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(mat3 m1, mat3 m2) => !(m1 == m2);

        public static mat3 operator +(mat3 m) => m;

        public static mat3 operator -(mat3 m) => m * -1f;

        public static mat3 operator -(float f, mat3 m) => new mat3(f) - m;

        public static mat3 operator -(mat3 m, float f) => m + -f;

        public static mat3 operator +(float f, mat3 m) => m + f;

        public static mat3 operator +(mat3 m, float f) => m + new mat3(f);

        public static mat3 operator -(mat3 m1, mat3 m2) => m1 + -m2;

        /// <summary>
        /// Multiplies the <paramref name="m"/> matrix by the <paramref name="v"/> vector.
        /// </summary>
        /// <param name="m">The LHS matrix.</param>
        /// <param name="v">The RHS vector.</param>
        /// <returns>The product of <paramref name="m"/> and <paramref name="v"/>.</returns>
        public static vec3 operator *(mat3 m, vec3 v) => new vec3(
            m[0, 0] * v[0] + m[1, 0] * v[1] + m[2, 0] * v[2],
            m[0, 1] * v[0] + m[1, 1] * v[1] + m[2, 1] * v[2],
            m[0, 2] * v[0] + m[1, 2] * v[1] + m[2, 2] * v[2]
        );

        /// <summary>
        /// Multiplies the <paramref name="m1"/> matrix by the <paramref name="m2"/> matrix.
        /// </summary>
        /// <param name="m1">The LHS matrix.</param>
        /// <param name="m2">The RHS matrix.</param>
        /// <returns>The product of <paramref name="m1"/> and <paramref name="m2"/>.</returns>
        public static mat3 operator *(mat3 m1, mat3 m2) => new mat3(glm._3.Select(j => new vec3(glm._3.Select(i => glm._3.Sum(k => m1[k, j] * m2[i, k])))));

        public static mat3 operator *(mat3 m, float f) => new mat3(m[0] * f, m[1] * f, m[2] * f);

        public static mat3 operator /(mat3 m, float f) => m * (1 / f);

        public static implicit operator (vec3 a, vec3 b, vec3 c) (mat3 v) => (v[0], v[1], v[2]);

        public static implicit operator mat3((vec3 a, vec3 b, vec3 c) t) => new mat3(t.a, t.b, t.c);
    }
}