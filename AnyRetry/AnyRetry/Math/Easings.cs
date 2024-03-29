﻿namespace AnyRetry.Math
{
    /// <summary>
    /// Easings library - create numbers based on curved easing algorithms
    /// </summary>
    public static class Easings
    {
        /// <summary>
        /// Constant Pi.
        /// </summary>
        private const double Pi = System.Math.PI;

        /// <summary>
        /// Constant Pi / 2.
        /// </summary>
        private const double HalfPi = System.Math.PI / 2.0;

        /// <summary>
        /// Interpolate using the specified function
        /// </summary>
        /// <param name="p">The interpolated step value</param>
        /// <param name="function">The easing function to use</param>
        public static double Interpolate(double p, EasingFunction function)
        {
            switch (function)
            {
                default:
                case EasingFunction.Linear:
                    return Linear(p);
                case EasingFunction.QuadraticEaseOut:
                    return QuadraticEaseOut(p);
                case EasingFunction.QuadraticEaseIn:
                    return QuadraticEaseIn(p);
                case EasingFunction.QuadraticEaseInOut:
                    return QuadraticEaseInOut(p);
                case EasingFunction.CubicEaseIn:
                    return CubicEaseIn(p);
                case EasingFunction.CubicEaseOut:
                    return CubicEaseOut(p);
                case EasingFunction.CubicEaseInOut:
                    return CubicEaseInOut(p);
                case EasingFunction.QuarticEaseIn:
                    return QuarticEaseIn(p);
                case EasingFunction.QuarticEaseOut:
                    return QuarticEaseOut(p);
                case EasingFunction.QuarticEaseInOut:
                    return QuarticEaseInOut(p);
                case EasingFunction.QuinticEaseIn:
                    return QuinticEaseIn(p);
                case EasingFunction.QuinticEaseOut:
                    return QuinticEaseOut(p);
                case EasingFunction.QuinticEaseInOut:
                    return QuinticEaseInOut(p);
                case EasingFunction.SineEaseIn:
                    return SineEaseIn(p);
                case EasingFunction.SineEaseOut:
                    return SineEaseOut(p);
                case EasingFunction.SineEaseInOut:
                    return SineEaseInOut(p);
                case EasingFunction.CircularEaseIn:
                    return CircularEaseIn(p);
                case EasingFunction.CircularEaseOut:
                    return CircularEaseOut(p);
                case EasingFunction.CircularEaseInOut:
                    return CircularEaseInOut(p);
                case EasingFunction.ExponentialEaseIn:
                    return ExponentialEaseIn(p);
                case EasingFunction.ExponentialEaseOut:
                    return ExponentialEaseOut(p);
                case EasingFunction.ExponentialEaseInOut:
                    return ExponentialEaseInOut(p);
                case EasingFunction.ElasticEaseIn:
                    return ElasticEaseIn(p);
                case EasingFunction.ElasticEaseOut:
                    return ElasticEaseOut(p);
                case EasingFunction.ElasticEaseInOut:
                    return ElasticEaseInOut(p);
                case EasingFunction.BackEaseIn:
                    return BackEaseIn(p);
                case EasingFunction.BackEaseOut:
                    return BackEaseOut(p);
                case EasingFunction.BackEaseInOut:
                    return BackEaseInOut(p);
                case EasingFunction.BounceEaseIn:
                    return BounceEaseIn(p);
                case EasingFunction.BounceEaseOut:
                    return BounceEaseOut(p);
                case EasingFunction.BounceEaseInOut:
                    return BounceEaseInOut(p);
            }
        }

        /// <summary>
        /// Modeled after the line y = x
        /// </summary>
        /// <param name="p">The step value</param>
        public static double Linear(double p)
        {
            return p;
        }

        /// <summary>
        /// Modeled after the parabola y = x^2
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuadraticEaseIn(double p)
        {
            return p * p;
        }

        /// <summary>
        /// Modeled after the parabola y = -x^2 + 2x
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuadraticEaseOut(double p)
        {
            return -(p * (p - 2));
        }

        /// <summary>
        /// Modeled after the piecewise quadratic
        /// y = (1/2)((2x)^2)             ; [0, 0.5)
        /// y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuadraticEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 2 * p * p;
            }
            else
            {
                return (-2 * p * p) + (4 * p) - 1;
            }
        }

        /// <summary>
        /// Modeled after the cubic y = x^3
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CubicEaseIn(double p)
        {
            return p * p * p;
        }

        /// <summary>
        /// Modeled after the cubic y = (x - 1)^3 + 1
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CubicEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f + 1;
        }

        /// <summary>	
        /// Modeled after the piecewise cubic
        /// y = (1/2)((2x)^3)       ; [0, 0.5)
        /// y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CubicEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 4 * p * p * p;
            }
            else
            {
                double f = ((2 * p) - 2);
                return 0.5d * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after the quartic x^4
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuarticEaseIn(double p)
        {
            return p * p * p * p;
        }

        /// <summary>
        /// Modeled after the quartic y = 1 - (x - 1)^4
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuarticEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f * (1 - p) + 1;
        }

        /// <summary>
        /// Modeled after the piecewise quartic
        /// y = (1/2)((2x)^4)        ; [0, 0.5)
        /// y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuarticEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 8 * p * p * p * p;
            }
            else
            {
                double f = (p - 1);
                return -8 * f * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after the quintic y = x^5
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuinticEaseIn(double p)
        {
            return p * p * p * p * p;
        }

        /// <summary>
        /// Modeled after the quintic y = (x - 1)^5 + 1
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuinticEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f * f * f + 1;
        }

        /// <summary>
        /// Modeled after the piecewise quintic
        /// y = (1/2)((2x)^5)       ; [0, 0.5)
        /// y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double QuinticEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 16 * p * p * p * p * p;
            }
            else
            {
                double f = ((2 * p) - 2);
                return 0.5d * f * f * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after quarter-cycle of sine wave
        /// </summary>
        /// <param name="p">The step value</param>
        public static double SineEaseIn(double p)
        {
            return System.Math.Sin((p - 1) * HalfPi) + 1;
        }

        /// <summary>
        /// Modeled after quarter-cycle of sine wave (different phase)
        /// </summary>
        /// <param name="p">The step value</param>
        public static double SineEaseOut(double p)
        {
            return System.Math.Sin(p * HalfPi);
        }

        /// <summary>
        /// Modeled after half sine wave
        /// </summary>
        /// <param name="p">The step value</param>
        public static double SineEaseInOut(double p)
        {
            return 0.5d * (1 - System.Math.Cos(p * Pi));
        }

        /// <summary>
        /// Modeled after shifted quadrant IV of unit circle
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CircularEaseIn(double p)
        {
            return 1 - System.Math.Sqrt(1 - (p * p));
        }

        /// <summary>
        /// Modeled after shifted quadrant II of unit circle
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CircularEaseOut(double p)
        {
            return System.Math.Sqrt((2 - p) * p);
        }

        /// <summary>	
        /// Modeled after the piecewise circular function
        /// y = (1/2)(1 - Math.Sqrt(1 - 4x^2))           ; [0, 0.5)
        /// y = (1/2)(Math.Sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double CircularEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 0.5d * (1 - System.Math.Sqrt(1 - 4 * (p * p)));
            }
            else
            {
                return 0.5d * (System.Math.Sqrt(-((2 * p) - 3) * ((2 * p) - 1)) + 1);
            }
        }

        /// <summary>
        /// Modeled after the exponential function y = 2^(10(x - 1))
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ExponentialEaseIn(double p)
        {
            return (p == 0d) ? p : System.Math.Pow(2d, 10d * (p - 1d));
        }

        /// <summary>
        /// Modeled after the exponential function y = -2^(-10x) + 1
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ExponentialEaseOut(double p)
        {
            return (p == 1d) ? p : 1d - System.Math.Pow(2d, -10d * p);
        }

        /// <summary>
        /// Modeled after the piecewise exponential
        /// y = (1/2)2^(10(2x - 1))         ; [0,0.5)
        /// y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ExponentialEaseInOut(double p)
        {
            if (p == 0d || p == 1d)
                return p;

            if (p < 0.5d)
            {
                return 0.5d * System.Math.Pow(2d, (20d * p) - 10d);
            }
            else
            {
                return -0.5d * System.Math.Pow(2d, (-20d * p) + 10d) + 1d;
            }
        }

        /// <summary>
        /// Modeled after the damped sine wave y = sin(13pi/2*x)*Math.Pow(2, 10 * (x - 1))
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ElasticEaseIn(double p)
        {
            return System.Math.Sin(13 * HalfPi * p) * System.Math.Pow(2, 10 * (p - 1));
        }

        /// <summary>
        /// Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*Math.Pow(2, -10x) + 1
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ElasticEaseOut(double p)
        {
            return System.Math.Sin(-13 * HalfPi * (p + 1)) * System.Math.Pow(2, -10 * p) + 1;
        }

        /// <summary>
        /// Modeled after the piecewise exponentially-damped sine wave:
        /// y = (1/2)*sin(13pi/2*(2*x))*Math.Pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
        /// y = (1/2)*(sin(-13pi/2*((2x-1)+1))*Math.Pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double ElasticEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 0.5d * System.Math.Sin(13 * HalfPi * (2 * p)) * System.Math.Pow(2, 10 * ((2 * p) - 1));
            }
            else
            {
                return 0.5d * (System.Math.Sin(-13 * HalfPi * ((2 * p - 1) + 1)) * System.Math.Pow(2, -10 * (2 * p - 1)) + 2);
            }
        }

        /// <summary>
        /// Modeled after the overshooting cubic y = x^3-x*sin(x*pi)
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BackEaseIn(double p)
        {
            return p * p * p - p * System.Math.Sin(p * Pi);
        }

        /// <summary>
        /// Modeled after overshooting cubic y = 1-((1-x)^3-(1-x)*sin((1-x)*pi))
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BackEaseOut(double p)
        {
            double f = (1 - p);
            return 1 - (f * f * f - f * System.Math.Sin(f * Pi));
        }

        /// <summary>
        /// Modeled after the piecewise overshooting cubic function:
        /// y = (1/2)*((2x)^3-(2x)*sin(2*x*pi))           ; [0, 0.5)
        /// y = (1/2)*(1-((1-x)^3-(1-x)*sin((1-x)*pi))+1) ; [0.5, 1]
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BackEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                double f = 2 * p;
                return 0.5d * (f * f * f - f * System.Math.Sin(f * Pi));
            }
            else
            {
                double f = (1 - (2 * p - 1));
                return 0.5d * (1 - (f * f * f - f * System.Math.Sin(f * Pi))) + 0.5d;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BounceEaseIn(double p)
        {
            return 1 - BounceEaseOut(1 - p);
        }

        /// <summary>
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BounceEaseOut(double p)
        {
            if (p < 4 / 11.0d)
            {
                return (121 * p * p) / 16.0d;
            }
            else if (p < 8 / 11.0d)
            {
                return (363 / 40.0d * p * p) - (99 / 10.0d * p) + 17 / 5.0d;
            }
            else if (p < 9 / 10.0d)
            {
                return (4356 / 361.0d * p * p) - (35442 / 1805.0d * p) + 16061 / 1805.0d;
            }
            else
            {
                return (54 / 5.0d * p * p) - (513 / 25.0d * p) + 268 / 25.0d;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="p">The step value</param>
        public static double BounceEaseInOut(double p)
        {
            if (p < 0.5d)
            {
                return 0.5d * BounceEaseIn(p * 2);
            }
            else
            {
                return 0.5d * BounceEaseOut(p * 2 - 1) + 0.5d;
            }
        }
    }
}
