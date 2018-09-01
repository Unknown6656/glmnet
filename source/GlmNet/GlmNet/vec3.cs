using System;

namespace GlmNet
{
    /// <summary>
    /// Represents a three dimensional vector.
    /// </summary>
    public struct vec3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:  return x;
                    case 1:  return y;
                    case 2:  return z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else if (index == 2) z = value;
                else throw new IndexOutOfRangeException();
            }
        }


        public vec3(float s)
            : this(s, s, s)
        {
        }

        public vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public vec3(vec3 v)
            : this(v.x, v.y, v.z)
        {
        }

        public vec3(vec4 v)
            : this(v.x, v.y, v.z)
        {
        }

        public vec3(vec2 xy, float z)
            : this(xy.x, xy.y, z)
        {
        }

        public float[] to_array() => new[] { x, y, z };


        public static vec3 operator -(vec3 v) => v * -1;

        public static vec3 operator +(vec3 v1, vec3 v2) => new vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static vec3 operator +(vec3 v, float f) => v + new vec3(f);

        public static vec3 operator -(vec3 v1, vec3 v2) => v1 + -v2;

        public static vec3 operator -(vec3 v, float f) => v + -f;

        public static vec3 operator *(vec3 v, float f) => v * new vec3(f);

        public static vec3 operator *(float f, vec3 v) => v * f;

        public static vec3 operator *(vec3 v1, vec3 v2) => new vec3(v2.x * v1.x, v2.y * v1.y, v2.z * v1.z);

        public static vec3 operator /(vec3 v, float f) => v * (1 / f);


        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is vec3 vec && vec.x == x && vec.y == y && vec.z == z;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(vec3 v1, vec3 v2) => v1.Equals(v2);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="v1">The first Vector.</param>
        /// <param name="v2">The second Vector.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(vec3 v1, vec3 v2) => !(v1 == v2);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }
}