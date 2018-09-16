using System;

using GlmNet;


namespace RandomTests
{
    using static poly;


    public static class Program
    {
        public static int Main(string[] args)
        {
            mat2 m = (1, 2, 0, -1);

            poly p1 = new poly(0, 0, 3);
            poly p2 = new poly(0, 0, 0, 2);

            var r0 = p2.Derivative;
            var r1 = p1 << 1;
            var r2 = p1 >> 1;
            var r3 = p1 + p2;
            var r4 = p1 - p2;
            var r5 = p1 * p2;
            //var r6 = p1 / p2;

            

            mat3 mm = (
                1, 0, 0,
                0, 1, 0,
                0, 0, 1
            );

            var mm1 = mm.SwapRows(0, 1);
            var mm2 = mm.AddRows(0, 1, -4);


            //var lu = mm.LUDecompose();
            //var col = mm.CholeskyDecompose<mat3, vec3>();

            //Console.WriteLine(col);


            Console.WriteLine($"A:\n{mm}\n");
            Console.WriteLine($"1:\n{mm1}\n");
            Console.WriteLine($"2:\n{mm2}\n");
            //Console.WriteLine($"C:\n{col}\n");
            Console.ReadKey(true);

            return 0;
        }
    }
}
