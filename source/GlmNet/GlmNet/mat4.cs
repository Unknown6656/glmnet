using System.Collections.Generic;
using System.Linq;
using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a 4x4 matrix.
    /// </summary>
    public readonly struct mat4
        : imat<mat4, vec4>
    {
        /// <summary>
        /// The columms of the matrix.
        /// </summary>
        private readonly vec4[] cols;


        /// <summary>
        /// Gets or sets the <see cref="vec4"/> column at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="vec4"/> column.
        /// </value>
        /// <param name="column">The column index.</param>
        /// <returns>The column at index <paramref name="column"/>.</returns>
        public vec4 this[int column]
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

        public bool IsInvertible => Math.Abs(Determinant) >= float.Epsilon;


        /// <summary>
        /// Initializes a new instance of the <see cref="mat4"/> struct.
        /// This matrix is the identity matrix scaled by <paramref name="scale"/>.
        /// </summary>
        /// <param name="scale">The scale.</param>
        public mat4(float scale) => cols = new[]
        {
            new vec4(scale, 0.0f, 0.0f, 0.0f),
            new vec4(0.0f, scale, 0.0f, 0.0f),
            new vec4(0.0f, 0.0f, scale, 0.0f),
            new vec4(0.0f, 0.0f, 0.0f, scale),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="mat4"/> struct.
        /// The matrix is initialised with the <paramref name="cols"/>.
        /// </summary>
        /// <param name="cols">The colums of the matrix.</param>
        public mat4(IEnumerable<vec4> cols) => this.cols = cols.Take(4).ToArray();

        public mat4(vec4 a, vec4 b, vec4 c, vec4 d)
            : this(new[] { a, b, c, d })
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
        public mat3 to_mat3() => new mat3(
            new vec3(cols[0][0], cols[0][1], cols[0][2]),
            new vec3(cols[1][0], cols[1][1], cols[1][2]),
            new vec3(cols[2][0], cols[2][1], cols[2][2]));
        
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is mat4 mat &&
                                                   mat[0] == this[0] &&
                                                   mat[1] == this[1] &&
                                                   mat[2] == this[2] &&
                                                   mat[3] == this[3];

        /// <inheritdoc/>
        public override int GetHashCode() => this[0].GetHashCode() ^
                                             this[1].GetHashCode() ^
                                             this[2].GetHashCode() ^
                                             this[3].GetHashCode();


        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <returns>A new identity matrix.</returns>
        public static mat4 identity() => new mat4(1);

        /// <summary>
        /// Creates an zero matrix.
        /// </summary>
        /// <returns>A new zero matrix.</returns>
        public static mat4 zero() => new mat4(0);

        
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(mat4 m1, mat4 m2) => m1.Equals(m2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="m1">The first Matrix.</param>
        /// <param name="m2">The second Matrix.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(mat4 m1, mat4 m2) => !(m1 == m2);

        public static mat4 operator +(mat4 m) => m;

        public static mat4 operator -(mat4 m) => m * -1f;

        public static mat4 operator -(float f, mat4 m) => new mat4(f) - m;

        public static mat4 operator -(mat4 m, float f) => m + -f;

        public static mat4 operator +(float f, mat4 m) => m + f;

        public static mat4 operator +(mat4 m, float f) => m + new mat4(f);

        public static mat4 operator -(mat4 m1, mat4 m2) => m1 + -m2;

        /// <summary>
        /// Multiplies the <paramref name="m"/> matrix by the <paramref name="v"/> vector.
        /// </summary>
        /// <param name="m">The LHS matrix.</param>
        /// <param name="v">The RHS vector.</param>
        /// <returns>The product of <paramref name="m"/> and <paramref name="v"/>.</returns>
        public static vec4 operator *(mat4 m, vec4 v) => new vec4(
                                                                      m[0, 0] * v[0] + m[1, 0] * v[1] + m[2, 0] * v[2] + m[3, 0] * v[3],
                                                                      m[0, 1] * v[0] + m[1, 1] * v[1] + m[2, 1] * v[2] + m[3, 1] * v[3],
                                                                      m[0, 2] * v[0] + m[1, 2] * v[1] + m[2, 2] * v[2] + m[3, 2] * v[3],
                                                                      m[0, 3] * v[0] + m[1, 3] * v[1] + m[2, 3] * v[2] + m[3, 3] * v[3]
                                                                     );

        /// <summary>
        /// Multiplies the <paramref name="m1"/> matrix by the <paramref name="m2"/> matrix.
        /// </summary>
        /// <param name="m1">The LHS matrix.</param>
        /// <param name="m2">The RHS matrix.</param>
        /// <returns>The product of <paramref name="m1"/> and <paramref name="m2"/>.</returns>
        public static mat4 operator *(mat4 m1, mat4 m2) => new mat4(glm._4.Select(j => new vec4(glm._4.Select(i => glm._4.Sum(k => m1[k, j] * m2[i, k])))));

        public static mat4 operator *(mat4 m, float f) => new mat4(new[]
        {
            m[0] * f,
            m[1] * f,
            m[2] * f,
            m[3] * f
        });

        public static mat4 operator /(mat4 m, float f) => m * (1 / f);

        public static implicit operator (vec4 a, vec4 b, vec4 c, vec4 d) (mat4 v) => (v[0], v[1], v[2], v[3]);

        public static implicit operator mat4((vec4 a, vec4 b, vec4 c, vec4 d) t) => new mat4(t.a, t.b, t.c, t.d);
    }
}