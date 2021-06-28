using System.Collections.Generic;

namespace AlgebraCalculatorApp
{
    class database
    {
        public static Dictionary<string, string> dict;

        public static void createDict()
        {
            dict = new Dictionary<string, string>()
            {
                {"Area of a rectangle", "l*w" },
                {"Area of a triangle", "1/2(b*h)" },
                {"Area of a circle", "1/2(πr²)" },
                {"Area of a regular polygon", "A = a(p/2)" },
                {"Volume of a rect. prism", "l*w*h" },
                {"Volume of a prism", "a*h"},
                {"Volume of a cylinder", "a*h" },
                {"Volume of a cone", "1/3(a*h)" },
                {"Volume of a pyramid", "1/3(a*h)" },
                {"Volume of a sphere", "4/3(πr³)" },
                {"Surface area of a rectange", "2(lw+wh+lh)" },
                {"Surface area of a sphere", "4(πr²)" },
                {"Surface area of a cylinder", "2(πr²) + 2(πrh)" },
                {"Surface area of a cone", "π(r²) + πrs" },
                {"Surface area of a pyramid", "l² + 2(ls)" },
                {"Pythagorean theorem", "a² + b² = c²" },
                {"Standard form (linear)", "ax+by = c" },
                {"Slope- Intercept form (linear)", "y = mx+b" },
                {"Point-slope form (linear)", "y-y₁ = m(x-x₁)" },
                {"Slope (linear)", "(y₂-y₁)/(x₂-x₁)" },
                {"Quadratic form (quadratic)", "ax^2+bx+c = 0" },
                {"Vertex form (quadratic)", "y = a(x-h)+k" },
                {"Quadratic formula", "x = (−b±√b²-4ac)/2a" },
                {"Distance formula", "d = √(x₂-x₁)²+(y₂-y₁)²" },
                {"Midpoint formula", "(x₁+x₂)/2, (y₁+y₂)/2" },
                {"Direct variation", "y = kx" },
                {"Inverse variation", "y = k/x" },
                {"Distance", "d = rt" },
                {"Speed", "r = d/t" },
                {"Acceleration", "a = f/m" },
                {"Time elapsed", "t = d/r" },
                {"Force", "f = ma" },
                {"Mass", "m = f/a" },
                {"Density", "d = m/v" },
                {"Kinetic energy", "Ke = 1/2(mv²)" },
                {"Potential energy (gravitational)", "Pe = mhg (g = 10m/s²)" },
                {"Pressure", "p = f/a" },
                {"Compounding interest", "A = P(1+r/n)ⁿᵗ" },
                {"Arithmetic sequence", "aₙ = a₁ + (n - 1)d" }
            };
        }
    }
}
