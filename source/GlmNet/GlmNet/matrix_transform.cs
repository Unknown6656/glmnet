﻿using System;


namespace GlmNet
{
    public static partial class glm
    {
        /// <summary>
        /// Creates a frustrum projection matrix.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        /// <param name="nearVal">The near val.</param>
        /// <param name="farVal">The far val.</param>
        /// <returns></returns>
        public static mat4 frustum(float left, float right, float bottom, float top, float nearVal, float farVal) => new mat4(1)
        {
            [0, 0] = 2.0f * nearVal / (right - left),
            [1, 1] = 2.0f * nearVal / (top - bottom),
            [2, 0] = (right + left) / (right - left),
            [2, 1] = (top + bottom) / (top - bottom),
            [2, 2] = -(farVal + nearVal) / (farVal - nearVal),
            [2, 3] = -1.0f,
            [3, 2] = -(2.0f * farVal * nearVal) / (farVal - nearVal),
            [3, 3] = 0.0f
        };

        /// <summary>
        /// Creates a matrix for a symmetric perspective-view frustum with far plane at infinite.
        /// </summary>
        /// <param name="fovy">The fovy.</param>
        /// <param name="aspect">The aspect.</param>
        /// <param name="zNear">The z near.</param>
        /// <returns></returns>
        public static mat4 infinitePerspective(float fovy, float aspect, float zNear)
        {
            float range = tan(fovy / 2f) * zNear;
            float left = -range * aspect;
            float right = range * aspect;

            return new mat4
            {
                [0, 0] = 2f * zNear / (right - left),
                [1, 1] = 2f * zNear / (2 * range),
                [2, 2] = -1f,
                [2, 3] = -1f,
                [3, 2] = -2f * zNear
            };
        }

        /// <summary>
        /// Build a look at view matrix.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <param name="center">The center.</param>
        /// <param name="up">Up.</param>
        /// <returns></returns>
        public static mat4 lookAt(vec3 eye, vec3 center, vec3 up)
        {
            vec3 f = new vec3(normalize(center - eye));
            vec3 s = new vec3(normalize(cross(f, up)));
            vec3 u = new vec3(cross(s, f));

            mat4 Result = new mat4(1)
            {
                [0, 0] = s.x,
                [1, 0] = s.y,
                [2, 0] = s.z,
                [0, 1] = u.x,
                [1, 1] = u.y,
                [2, 1] = u.z,
                [0, 2] = -f.x,
                [1, 2] = -f.y,
                [2, 2] = -f.z,
                [3, 0] = -dot(s, eye),
                [3, 1] = -dot(u, eye),
                [3, 2] = dot(f, eye)
            };

            return Result;
        }

        /// <summary>
        /// Creates a matrix for an orthographic parallel viewing volume.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        /// <param name="zNear">The z near.</param>
        /// <param name="zFar">The z far.</param>
        /// <returns></returns>
        public static mat4 ortho(float left, float right, float bottom, float top, float zNear, float zFar) => new mat4(1)
        {
            [0, 0] = 2 / (right - left),
            [1, 1] = 2 / (top - bottom),
            [2, 2] = -2 / (zFar - zNear),
            [3, 0] = -(right + left) / (right - left),
            [3, 1] = -(top + bottom) / (top - bottom),
            [3, 2] = -(zFar + zNear) / (zFar - zNear)
        };

        /// <summary>
        /// Creates a matrix for projecting two-dimensional coordinates onto the screen.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public static mat4 ortho(float left, float right, float bottom, float top) => new mat4(1)
        {
            [0, 0] = 2 / (right - left),
            [1, 1] = 2 / (top - bottom),
            [2, 2] = -1,
            [3, 0] = -(right + left) / (right - left),
            [3, 1] = -(top + bottom) / (top - bottom)
        };

        /// <summary>
        /// Creates a perspective transformation matrix.
        /// </summary>
        /// <param name="fovy">The field of view angle, in radians.</param>
        /// <param name="aspect">The aspect ratio.</param>
        /// <param name="zNear">The near depth clipping plane.</param>
        /// <param name="zFar">The far depth clipping plane.</param>
        /// <returns>A <see cref="mat4"/> that contains the projection matrix for the perspective transformation.</returns>
        public static mat4 perspective(float fovy, float aspect, float zNear, float zFar)
        {
            float tanHalfFovy = (float)Math.Tan(fovy / 2);

            return new mat4(1)
            {
                [0, 0] = 1 / (aspect * tanHalfFovy),
                [1, 1] = 1 / tanHalfFovy,
                [2, 2] = -(zFar + zNear) / (zFar - zNear),
                [2, 3] = -1,
                [3, 2] = -(2 * zFar * zNear) / (zFar - zNear),
                [3, 3] = 0
            };
        }

        /// <summary>
        /// Builds a perspective projection matrix based on a field of view.
        /// </summary>
        /// <param name="fov">The fov (in radians).</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="zNear">The z near.</param>
        /// <param name="zFar">The z far.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static mat4 perspectiveFov(float fov, float width, float height, float zNear, float zFar)
        {
            if (width <= 0 || height <= 0 || fov <= 0)
                throw new ArgumentOutOfRangeException();
            
            float h = cos(fov / 2) / sin(fov / 2);
            float w = h * height / width;

            return new mat4(0)
            {
                [0, 0] = w,
                [1, 1] = h,
                [2, 2] = -(zFar + zNear) / (zFar - zNear),
                [2, 3] = -1,
                [3, 2] = -(2 * zFar * zNear) / (zFar - zNear)
            };
        }

        /// <summary>
        /// Define a picking region.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="viewport">The viewport.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static mat4 pickMatrix(vec2 center, vec2 delta, vec4 viewport)
        {
            if (delta.x <= 0 || delta.y <= 0)
                throw new ArgumentOutOfRangeException();

            mat4 res = mat4.identity();

            if (!(delta.x > 0 && delta.y > 0))
                return res; // Error

            vec3 tmp = new vec3(
                (viewport[2] - 2 * (center.x - viewport[0])) / delta.x,
                (viewport[3] - 2 * (center.y - viewport[1])) / delta.y,
                0
            );
            
            res = translate(res, tmp);

            return scale(res, new vec3(viewport[2]/delta.x, viewport[3]/delta.y, 1));
        }

        /// <summary>
        /// Map the specified object coordinates (obj.x, obj.y, obj.z) into window coordinates.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="model">The model.</param>
        /// <param name="proj">The proj.</param>
        /// <param name="viewport">The viewport.</param>
        /// <returns></returns>
        public static vec3 project(vec3 obj, mat4 model, mat4 proj, vec4 viewport)
        {
            vec4 tmp = proj * model * new vec4(obj, 1);

            tmp /= tmp.w;
            tmp = tmp * .5f + .5f;
            tmp[0] = tmp[0] * viewport[2] + viewport[0];
            tmp[1] = tmp[1] * viewport[3] + viewport[1];

            return new vec3(tmp.x, tmp.y, tmp.z);
        }

        /// <summary>
        /// Builds a rotation 4 * 4 matrix created from an axis vector and an angle.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static mat4 rotate(mat4 m, float angle, vec3 v)
        {
            float c = cos(angle);
            float s = sin(angle);
            vec3 axis = normalize(v);
            vec3 tmp = (1 - c) * axis;

            mat4 rot = new mat4(1)
            {
                [0, 0] = c + tmp[0] * axis[0],
                [0, 1] = 0 + tmp[0] * axis[1] + s * axis[2],
                [0, 2] = 0 + tmp[0] * axis[2] - s * axis[1],
                [1, 0] = 0 + tmp[1] * axis[0] - s * axis[2],
                [1, 1] = c + tmp[1] * axis[1],
                [1, 2] = 0 + tmp[1] * axis[2] + s * axis[0],
                [2, 0] = 0 + tmp[2] * axis[0] + s * axis[1],
                [2, 1] = 0 + tmp[2] * axis[1] - s * axis[0],
                [2, 2] = c + tmp[2] * axis[2]
            };

            return new mat4(1)
            {
                [0] = m[0] * rot[0][0] + m[1] * rot[0][1] + m[2] * rot[0][2],
                [1] = m[0] * rot[1][0] + m[1] * rot[1][1] + m[2] * rot[1][2],
                [2] = m[0] * rot[2][0] + m[1] * rot[2][1] + m[2] * rot[2][2],
                [3] = m[3]
            };
        }

        //  TODO: this is actually defined as an extension, put in the right file.
        public static mat4 rotate(float angle, vec3 v) => rotate(mat4.identity(), angle, v);

        /// <summary>
        /// Applies a scale transformation to matrix <paramref name="m"/> by vector <paramref name="v"/>.
        /// </summary>
        /// <param name="m">The matrix to transform.</param>
        /// <param name="v">The vector to scale by.</param>
        /// <returns><paramref name="m"/> scaled by <paramref name="v"/>.</returns>
        public static mat4 scale(this mat4 m, vec3 v)
        {
            mat4 result = m;

            result[0] = m[0] * v[0];
            result[1] = m[1] * v[1];
            result[2] = m[2] * v[2];
            result[3] = m[3];

            return result;
        }

        /// <summary>
        /// Applies a translation transformation to matrix <paramref name="m"/> by vector <paramref name="v"/>.
        /// </summary>
        /// <param name="m">The matrix to transform.</param>
        /// <param name="v">The vector to translate by.</param>
        /// <returns><paramref name="m"/> translated by <paramref name="v"/>.</returns>
        public static mat4 translate(this mat4 m, vec3 v)
        {
            mat4 result = m;

            result[3] = m[0] * v[0] + m[1] * v[1] + m[2] * v[2] + m[3];

            return result;
        }

        /// <summary>
        /// Creates a matrix for a symmetric perspective-view frustum with far plane 
        /// at infinite for graphics hardware that doesn't support depth clamping.
        /// </summary>
        /// <param name="fovy">The fovy.</param>
        /// <param name="aspect">The aspect.</param>
        /// <param name="zNear">The z near.</param>
        /// <returns></returns>
        public static mat4 tweakedInfinitePerspective(float fovy, float aspect, float zNear)
        {
            float range = tan(fovy/2)*zNear;
            float left = -range*aspect;
            float right = range*aspect;
            float bottom = -range;
            float top = range;

            return new mat4(0f)
            {
                [0, 0] = 2 * zNear / (right - left),
                [1, 1] = 2 * zNear / (top - bottom),
                [2, 2] = 0.0001f - 1f,
                [2, 3] = -1,
                [3, 2] = -(0.0001f - 2) * zNear
            };
        }

        /// <summary>
        /// Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
        /// </summary>
        /// <param name="win">The win.</param>
        /// <param name="model">The model.</param>
        /// <param name="proj">The proj.</param>
        /// <param name="viewport">The viewport.</param>
        /// <returns></returns>
        public static vec3 unProject(vec3 win, mat4 model, mat4 proj, vec4 viewport )
        {
            mat4 inv = (proj * model).inverse();
            vec4 tmp = new vec4(win, 1f);

            tmp.x = (tmp.x - viewport[0]) / viewport[2];
            tmp.y = (tmp.y - viewport[1]) / viewport[3];
            tmp = tmp * 2 - 1;

            vec4 obj = inv * tmp;
            
            return new vec3(obj /= obj.w);
        }
    }
}
