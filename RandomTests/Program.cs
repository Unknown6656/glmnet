using System;
using System.Collections.Generic;
using System.Linq;

using GlmNet;


namespace RandomTests
{
    using static poly;


    public static class Program
    {
        public static int Main(string[] args)
        {
            mat2 m = (1, 2, 0, -1);

            poly p1 = (0, 0, 0, 2);
            poly p2 = (0, 0, 3);

            var r0 = p2.Derivative;
            var r1 = p1 << 1;
            var r2 = p1 >> 1;
            var r3 = p1 + p2;
            var r4 = p1 - p2;
            var r5 = p1 * p2;
            var r6 = p1 / p2;
            var r7 = p1 % p2;


            mat3 mat = (
                1, 1, 1,
                1, 0, 1,
                1, 0, 1
            );
            vec3 b = (0, 4, 2);
            var x = mat.Solve(b);

            poly p3 = (-1, 1);
            poly p4 = (-1, 0, 1);
            poly p5 = (-1, 0, 0, 1);
            poly p6 = (0, 0, 0, 0, 0, 0, 1);
            poly p7 = (2, 1, 8, 3);

            return 0;
        }
    }
}
