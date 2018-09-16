using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a generic vector
    /// </summary>
    /// <typeparam name="V">Generic vector type</typeparam>
    public interface ivec<V>
        : IComparable<V>
        where V : struct, ivec<V>
    {
        /// <summary>
        /// Sets or gets the vector's coefficient at the given index
        /// </summary>
        /// <param name="index">Coefficient index (zero-based)</param>
        /// <value>New coefficient value</value>
        /// <returns>Coefficient value</returns>
        float this[int index] { set; get; }
        /// <summary>
        /// The vector's dimension
        /// </summary>
        int Size { get; }
        /// <summary>
        /// The vector's eucledian length
        /// </summary>
        float Length { get; }
        /// <summary>
        /// The eucledian normalized vector
        /// </summary>
        V Normalized { get; }

        /// <summary>
        /// Calculates the dot product (aka. scalar product) of the current vector and the given one
        /// </summary>
        /// <param name="other">Second vector</param>
        /// <returns>Dot product</returns>
        float Dot(V other);
        /// <summary>
        /// Returns whether the given vector is linear independent from the current one
        /// </summary>
        /// <param name="other">Second vector</param>
        bool IsLinearIndependent(V other);
        /// <summary>
        /// Returns the array representation of the vector
        /// </summary>
        /// <returns>Flat array representation</returns>
        float[] ToArray();
        /// <summary>
        /// Replaces the vector coefficients with the ones in the given array
        /// </summary>
        /// <param name="v">New vector coefficients</param>
        void FromArray(params float[] v);
    }

    /// <summary>
    /// Represents a generic square matrix
    /// </summary>
    /// <typeparam name="M">Generic matrix type</typeparam>
    /// <typeparam name="V">Generic underlying vector type</typeparam>
    public interface imat<M, V>
        where M : imat<M, V>
        where V : struct, ivec<V>
    {
        /// <summary>
        /// Sets or gets the matrix' column vector at the given index
        /// </summary>
        /// <param name="column">Column vector index (zero-based)</param>
        /// <value>New column vector</value>
        /// <returns>Column vector</returns>
        V this[int column] { set; get; }
        /// <summary>
        /// Sets or gets the vector's coefficient at the given index
        /// </summary>
        /// <param name="column">Coefficient column index (zero-based)</param>
        /// <param name="row">Coefficient row index (zero-based)</param>
        /// <value>New coefficient value</value>
        /// <returns>Coefficient value</returns>
        float this[int column, int row] { set; get; }
        /// <summary>
        /// The matrix' determinant
        /// </summary>
        float Determinant { get; }
        /// <summary>
        /// Indicates whether the matrix is invertible, meaning that a multiplicative inverse exists
        /// </summary>
        bool IsInvertible { get; }
        /// <summary>
        /// Indicates whether the matrix is symmetric
        /// </summary>
        bool IsSymmetric { get; }
        bool IsProjection { get; }
        bool IsInvolutory { get; }
        /// <summary>
        /// Indicates whether the matrix is orthogonal
        /// </summary>
        bool IsOrthogonal { get; }
        /// <summary>
        /// Indicates whether the matrix is skew symmetric
        /// </summary>
        bool IsSkewSymetric { get; }
        /// <summary>
        /// The matrix' characteristic polynomial
        /// </summary>
        poly CharacteristicPolynomial { get; }
        V[] Columns { get; }
        /// <summary>
        /// The matrix' orthonormal basis
        /// </summary>
        M OrthonormalBasis { get; }
        /// <summary>
        /// The transposed matrix
        /// </summary>
        M Transposed { get; }
        /// <summary>
        /// The multiplicative inverse matrix
        /// </summary>
        M Inverse { get; }
        /// <summary>
        /// The rank of the matrix
        /// </summary>
        int Rank { get; }
        /// <summary>
        /// The matrix dimension (= side length)
        /// </summary>
        int Size { get; }

        (M U, M D) IwasawaDecompose();

        /// <summary>
        /// Returns the matrix as a flat array of matrix elements in column major format.
        /// </summary>
        /// <returns>Column major representation of the matrix</returns>
        float[] ToArray();
        void FromArray(V[] v);
        void FromArray(float[] v);
        M SwapRows(int src_row, int dst_row);
        M AddRows(int src_row, int dst_row);
        M AddRows(int src_row, int dst_row, float factor);
        M SwapColumns(int src_row, int dst_row);
        M AddColumns(int src_col, int dst_col);
        M AddColumns(int src_col, int dst_col, float factor);
    }

    public static partial class glm
    {
        public static mat4 inverse(this in mat4 m)
		{
			float Coef00 = m[2, 2] * m[3, 3] - m[3, 2] * m[2, 3];
			float Coef02 = m[1, 2] * m[3, 3] - m[3, 2] * m[1, 3];
			float Coef03 = m[1, 2] * m[2, 3] - m[2, 2] * m[1, 3];

		    float Coef04 = m[2, 1] * m[3, 3] - m[3, 1] * m[2, 3];
			float Coef06 = m[1, 1] * m[3, 3] - m[3, 1] * m[1, 3];
			float Coef07 = m[1, 1] * m[2, 3] - m[2, 1] * m[1, 3];

			float Coef08 = m[2, 1] * m[3, 2] - m[3, 1] * m[2, 2];
			float Coef10 = m[1, 1] * m[3, 2] - m[3, 1] * m[1, 2];
			float Coef11 = m[1, 1] * m[2, 2] - m[2, 1] * m[1, 2];

			float Coef12 = m[2, 0] * m[3, 3] - m[3, 0] * m[2, 3];
			float Coef14 = m[1, 0] * m[3, 3] - m[3, 0] * m[1, 3];
			float Coef15 = m[1, 0] * m[2, 3] - m[2, 0] * m[1, 3];

			float Coef16 = m[2, 0] * m[3, 2] - m[3, 0] * m[2, 2];
			float Coef18 = m[1, 0] * m[3, 2] - m[3, 0] * m[1, 2];
			float Coef19 = m[1, 0] * m[2, 2] - m[2, 0] * m[1, 2];

			float Coef20 = m[2, 0] * m[3, 1] - m[3, 0] * m[2, 1];
			float Coef22 = m[1, 0] * m[3, 1] - m[3, 0] * m[1, 1];
			float Coef23 = m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1];


			vec4 Fac0 = new vec4(Coef00, Coef00, Coef02, Coef03);
			vec4 Fac1 = new vec4(Coef04, Coef04, Coef06, Coef07);
			vec4 Fac2 = new vec4(Coef08, Coef08, Coef10, Coef11);
			vec4 Fac3 = new vec4(Coef12, Coef12, Coef14, Coef15);
			vec4 Fac4 = new vec4(Coef16, Coef16, Coef18, Coef19);
			vec4 Fac5 = new vec4(Coef20, Coef20, Coef22, Coef23);

			vec4 Vec0 = new vec4(m[1, 0], m[0, 0], m[0, 0], m[0, 0]);
			vec4 Vec1 = new vec4(m[1, 1], m[0, 1], m[0, 1], m[0, 1]);
			vec4 Vec2 = new vec4(m[1, 2], m[0, 2], m[0, 2], m[0, 2]);
			vec4 Vec3 = new vec4(m[1, 3], m[0, 3], m[0, 3], m[0, 3]);

			vec4 Inv0 = new vec4(Vec1 * Fac0 - Vec2 * Fac1 + Vec3 * Fac2);
			vec4 Inv1 = new vec4(Vec0 * Fac0 - Vec2 * Fac3 + Vec3 * Fac4);
			vec4 Inv2 = new vec4(Vec0 * Fac1 - Vec1 * Fac3 + Vec3 * Fac5);
			vec4 Inv3 = new vec4(Vec0 * Fac2 - Vec1 * Fac4 + Vec2 * Fac5);

			vec4 SignA = new vec4(+1, -1, +1, -1);
			vec4 SignB = new vec4(-1, +1, -1, +1);
			mat4 Inverse = new mat4(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

			vec4 Row0 = new vec4(Inverse[0, 0], Inverse[1, 0], Inverse[2, 0], Inverse[3, 0]);

			vec4 Dot0 = new vec4(m[0] * Row0);
			float det = Dot0.X + Dot0.Y + (Dot0.Z + Dot0.W);
            
			return Inverse / det;
        }

        public static M Zero<M, V>()
            where M : struct, imat<M, V>
            where V : struct, ivec<V>
        {
            switch (new M())
            {
                case mat2 _: return (M)(dynamic)mat2.Zero;
                case mat3 _: return (M)(dynamic)mat3.Zero;
                case mat4 _: return (M)(dynamic)mat4.Zero;
            }

            throw new NotImplementedException();
        }

        public static M Identity<M, V>()
            where M : struct, imat<M, V>
            where V : struct, ivec<V>
        {
            switch (new M())
            {
                case mat2 _: return (M)(dynamic)mat2.Identity;
                case mat3 _: return (M)(dynamic)mat3.Identity;
                case mat4 _: return (M)(dynamic)mat4.Identity;
            }

            throw new NotImplementedException();
        }

        public static M CholeskyDecompose<M, V>(this M a)
            where M : struct, imat<M, V>
            where V : struct, ivec<V>
        {
            M res = Zero<M, V>();

            for (int i = 0; i < res.Size; ++i)
                for (int j = i; j >= 0; --j)
                    if (i == j)
                        res[i, i] = 1;
                    else
                        res[j, i] = 2;

            // res[i,i] = Math.Sqrt(a[i, i] + );

            return res;
        }
    }
}
