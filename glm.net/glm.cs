using System.Runtime.CompilerServices;
using System;


namespace GlmNet
{
    public interface ivec<out M>
        where M : struct, ivec<M>
    {
        float this[int index] { set; get; }
        float Length { get; }
        M Normalized { get; }

        float[] to_array();
        void from_array(params float[] v);
    }

    public interface imat<out M, V>
        where M : struct, imat<M, V>
        where V : struct, ivec<V>
    {
        V this[int column] { set; get; }
        float this[int column, int row] { set; get; }
        float Determinant { get; }
        bool IsInvertible { get; }
        M Inverse { get; }

        float[] to_array();
    }

    public static partial class glm
    {
        internal static readonly int[] _2 = { 0, 1 };
        internal static readonly int[] _3 = { 0, 1, 2 };
        internal static readonly int[] _4 = { 0, 1, 2, 3 };


        public static bool is_zero(this float x) => Math.Abs(x) <= float.Epsilon;

        public static bool @is(this float x, float y) => is_zero(x - y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float acos(float x) => (float)Math.Acos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float acosh(float x) => x < 1 ? 0 : (float)Math.Log(x + Math.Sqrt(x * x - 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float asin(float x) => (float)Math.Asin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float asinh(float x) => (x < 0 ? -1 : x > 0 ? 1 : 0) * (float)Math.Log(Math.Abs(x) + Math.Sqrt(1 + x * x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float atan(float y, float x) => (float)Math.Atan2(y, x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float atan(float y_over_x) => (float)Math.Atan(y_over_x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float atanh(float x) => Math.Abs(x) >= 1 ? 0 : .5f * (float)Math.Log((1 + x) / (1 - x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float cos(float angle) => (float)Math.Cos(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float cosh(float angle) => (float)Math.Cosh(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float degrees(float radians) => radians * 57.295779513082320876798154814105f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float radians(float degrees) => degrees * 0.01745329251994329576923690768489f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sin(float angle) => (float)Math.Sin(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sinh(float angle) => (float)Math.Sinh(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float tan(float angle) => (float)Math.Tan(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float tanh(float angle) => (float)Math.Tanh(angle);

        [Obsolete("Use `vec2::Length` instead.")]
        public static float length(this vec2 v) => v.Length;

        [Obsolete("Use `vec2::Length` instead.")]
        public static float length(this vec3 v) => v.Length;

        [Obsolete("Use `vec2::Length` instead.")]
        public static float length(this vec4 v) => v.Length;

        [Obsolete("Use `mat2::Inverse` instead.")]
        public static mat2 inverse(this mat2 m) => m.Inverse;

        [Obsolete("Use `mat3::Inverse` instead.")]
        public static mat3 inverse(this mat3 m) => m.Inverse;

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
			float det = Dot0.x + Dot0.y + (Dot0.z + Dot0.w);
            
			return Inverse / det;
		}

        public static vec3 cross(this vec3 x, vec3 y) => (
            x.y * y.z - y.y * x.z,
            x.z * y.x - y.z * x.x,
            x.x * y.y - y.x * x.y);

        public static float dot(this vec2 x, vec2 y)
        {
            vec2 tmp = new vec2(x * y);

            return tmp.x + tmp.y;
        }

        public static float dot(this vec3 x, vec3 y)
        {
            vec3 tmp = new vec3(x * y);

            return tmp.x + tmp.y + tmp.z;
        }

        public static float dot(this vec4 x, vec4 y)
        {
            vec4 tmp = new vec4(x * y);

            return tmp.x + tmp.y + tmp.z + tmp.w;
        }

        [Obsolete("Use `vec2::Normalized` instead.")]
        public static vec2 normalize(this vec2 v) => v.Normalized;

        [Obsolete("Use `vec3::Normalized` instead.")]
        public static vec3 normalize(this vec3 v) => v.Normalized;

        [Obsolete("Use `vec4::Normalized` instead.")]
        public static vec4 normalize(this vec4 v) => v.Normalized;
    }
}
