using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;


namespace GlmNet
{
    public static partial class glm
    {
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
