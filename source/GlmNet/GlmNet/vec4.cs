using System;

namespace GlmNet
{
    /// <summary>
    /// Represents a four dimensional vector.
    /// </summary>
    public struct vec4
    {
        public float x { set; get; }
        public float y { set; get; }
        public float z { set; get; }
        public float w { set; get; }


        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return x;
                    case 1:  return y;
                    case 2:  return z;
                    case 3:  return w;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else if (index == 2) z = value;
                else if (index == 3) w = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public vec4(float s)
            : this(s, s, s, s)
        {
        }

        public vec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public vec4(vec4 v)
            : this(v.x, v.y, v.z, v.w)
        {
        }

        public vec4(vec3 xyz, float w)
            : this(xyz.x, xyz.y, xyz.z, w)
        {
        }

        public float[] to_array() => new[] { x, y, z, w };

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is vec4 vec && vec.x == x && vec.y == y && vec.z == z && vec.w == w;

        /// <inheritdoc/>
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();


        public static vec4 operator -(vec4 v) => v * -1;

        public static vec4 operator +(vec4 v1, vec4 v2) => new vec4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);

        public static vec4 operator +(vec4 v, float f) => v + new vec4(f);

        public static vec4 operator -(vec4 v1, vec4 v2) => v1 + -v2;

        public static vec4 operator -(vec4 v, float f) => v + -f;

        public static vec4 operator *(vec4 v, float f) => v * new vec4(f);

        public static vec4 operator *(float f, vec4 v) => v * f;

        public static vec4 operator *(vec4 v1, vec4 v2) => new vec4(v2.x * v1.x, v2.y * v1.y, v2.z * v1.z, v2.w * v1.w);

        public static vec4 operator /(vec4 v, float f) => v * (1 / f);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(vec4 v1, vec4 v2) => v1.Equals(v2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(vec4 v1, vec4 v2) => !(v1 == v2);
    }
}