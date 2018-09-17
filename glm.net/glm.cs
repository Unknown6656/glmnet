using System.Runtime.CompilerServices;
using System;


namespace GlmNet
{
#if DOUBLE_PRECISION
    using scalar = Double;
#else
    using scalar = Single;
#endif


    public static partial class glm
    {
        public static bool is_zero(this scalar x) => Math.Abs(x) <=  2 * scalar.Epsilon;

        public static bool @is(this scalar x, scalar y) => is_zero(x - y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar acos(scalar x) => (scalar)Math.Acos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar acosh(scalar x) => x < 1 ? 0 : (scalar)Math.Log(x + Math.Sqrt(x * x - 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar asin(scalar x) => (scalar)Math.Asin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar asinh(scalar x) => (x < 0 ? -1 : x > 0 ? 1 : 0) * (scalar)Math.Log(Math.Abs(x) + Math.Sqrt(1 + x * x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar atan(scalar y, scalar x) => (scalar)Math.Atan2(y, x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar atan(scalar y_over_x) => (scalar)Math.Atan(y_over_x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar atanh(scalar x) => Math.Abs(x) >= 1 ? 0 : .5f * (scalar)Math.Log((1 + x) / (1 - x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar cos(scalar angle) => (scalar)Math.Cos(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar cosh(scalar angle) => (scalar)Math.Cosh(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar degrees(scalar radians) => radians * 57.295779513082320876798154814105f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar radians(scalar degrees) => degrees * 0.01745329251994329576923690768489f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar sin(scalar angle) => (scalar)Math.Sin(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar sinh(scalar angle) => (scalar)Math.Sinh(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar tan(scalar angle) => (scalar)Math.Tan(angle);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static scalar tanh(scalar angle) => (scalar)Math.Tanh(angle);

        [Obsolete("Use `vec2::Length` instead.")]
        public static scalar length(this vec2 v) => v.Length;

        [Obsolete("Use `vec2::Length` instead.")]
        public static scalar length(this vec3 v) => v.Length;

        [Obsolete("Use `vec2::Length` instead.")]
        public static scalar length(this vec4 v) => v.Length;
        
        public static vec3 cross(this vec3 x, vec3 y) => (
            x.Y * y.Z - y.Y * x.Z,
            x.Z * y.X - y.Z * x.X,
            x.X * y.Y - y.X * x.Y);

        [Obsolete("Use `vec2::Normalized` instead.")]
        public static vec2 normalize(this vec2 v) => v.Normalized;

        [Obsolete("Use `vec3::Normalized` instead.")]
        public static vec3 normalize(this vec3 v) => v.Normalized;

        [Obsolete("Use `vec4::Normalized` instead.")]
        public static vec4 normalize(this vec4 v) => v.Normalized;
    }
}
