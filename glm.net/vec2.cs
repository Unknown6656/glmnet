using System.Collections.Generic;
using System.Linq;
using System;


namespace GlmNet
{
    /// <summary>
    /// Represents a two dimensional vector.
    /// </summary>
    public struct vec2
        : ivec<vec2>
    {
        public float x { get; set; }
        public float y { get; set; }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return x;
                    case 1:  return y;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public static int Dimension => 2;

        public float Length => (float)Math.Sqrt(x * x + y * y);

        public vec2 Normalized => this / Length;


        public static vec2 Zero { get; } = new vec2(0);

        public static vec2 UnitX { get; } = (1, 0);

        public static vec2 UnitY { get; } = (0, 1);


        public vec2(float s)
            : this(s, s)
        {
        }

        public vec2(float x, float y)
            : this()
        {
            this.x = x;
            this.y = y;
        }

        public vec2(vec2 v)
            : this(v.x, v.y)
        {
        }

        public vec2(vec3 v)
            : this(v.x, v.y)
        {
        }

        public vec2(vec4 v)
            : this(v.x, v.y)
        {
        }

        public vec2(IEnumerable<float> v)
            : this() => from_array(v?.ToArray() ?? new float[0]);

        /// <inheritdoc/>
        public override string ToString() => $"({x}, {y})";

        public void from_array(params float[] v)
        {
            for (int i = 0; i < Dimension; ++i)
                this[i] = v[i];
        }

        public float[] to_array() => new[] { x, y };

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is vec2 vec && vec.x.@is(x) && vec.y.@is(y);

        /// <inheritdoc/>
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();


        public static vec2 operator ~(vec2 v) => v.Normalized;

        public static vec2 operator -(vec2 v) => v * -1;

        public static vec2 operator +(vec2 v1, vec2 v2) => (v1.x + v2.x, v1.y + v2.y);

        public static vec2 operator +(vec2 v, float f) => v + new vec2(f);

        public static vec2 operator -(vec2 v1, vec2 v2) => v1 + -v2;

        public static vec2 operator -(vec2 v, float f) => v + -f;

        public static vec2 operator *(vec2 v, float f) => v * new vec2(f);

        public static vec2 operator *(float f, vec2 v) => v * f;

        public static vec2 operator -(float f, vec2 v) => -v + f;

        public static vec2 operator *(vec2 v1, vec2 v2) => (v2.x * v1.x, v2.y * v1.y);

        public static vec2 operator /(vec2 v, float f) => v * (1 / f);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(vec2 v1, vec2 v2) => v1.Equals(v2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(vec2 v1, vec2 v2) => !(v1 == v2);

        public static implicit operator (float x, float y) (vec2 v) => (v.x, v.y);

        public static implicit operator vec2((float x, float y) t) => new vec2(t.x, t.y);
    }
}
