using AnyRetry.Math;
using NUnit.Framework;

namespace AnyRetry.Tests.Math
{
    [TestFixture]
    public class EasingTests
    {
        [Test]
        public void Should_Interpolate_BackEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.BackEaseInOut);
            Assert.AreEqual(3881197, value);
        }

        [Test]
        public void Should_Interpolate_BackEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.BackEaseIn);
            Assert.AreEqual(1000000, value);
        }

        [Test]
        public void Should_Interpolate_BackEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.BackEaseOut);
            Assert.AreEqual(970300, value);
        }

        [Test]
        public void Should_Interpolate_BounceEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.BounceEaseIn);
            Assert.AreEqual(-74119.0625d, value);
        }

        [Test]
        public void Should_Interpolate_BounceEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.BounceEaseInOut);
            Assert.AreEqual(211809.52000000002d, value);
        }

        [Test]
        public void Should_Interpolate_BounceEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.BounceEaseOut);
            Assert.AreEqual(105958.72d, value);
        }

        [Test]
        public void Should_Interpolate_CircularEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.CircularEaseIn);
            Assert.IsTrue(double.IsNaN(value));
        }

        [Test]
        public void Should_Interpolate_CircularEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.CircularEaseInOut);
            Assert.IsTrue(double.IsNaN(value));
        }

        [Test]
        public void Should_Interpolate_CircularEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.CircularEaseOut);
            Assert.IsTrue(double.IsNaN(value));
        }

        [Test]
        public void Should_Interpolate_CubicEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.CubicEaseIn);
            Assert.AreEqual(1000000.0d, value);
        }

        [Test]
        public void Should_Interpolate_CubicEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.CubicEaseInOut);
            Assert.AreEqual(3881197.0d, value);
        }

        [Test]
        public void Should_Interpolate_CubicEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.CubicEaseOut);
            Assert.AreEqual(970300.0d, value);
        }

        [Test]
        public void Should_Interpolate_ElasticEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.ElasticEaseIn);
            Assert.AreEqual(4.3101248653016293E+284d, value);
        }

        [Test]
        public void Should_Interpolate_ElasticEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.ElasticEaseInOut);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void Should_Interpolate_ElasticEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.ElasticEaseOut);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void Should_Interpolate_ExponentialEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.ExponentialEaseIn);
            Assert.AreEqual(1.0463951242053392E+298d, value);
        }

        [Test]
        public void Should_Interpolate_ExponentialEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.ExponentialEaseInOut);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void Should_Interpolate_ExponentialEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.ExponentialEaseOut);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void Should_Interpolate_Linear()
        {
            var value = Easings.Interpolate(100, EasingFunction.Linear);
            Assert.AreEqual(100, value);
        }

        [Test]
        public void Should_Interpolate_QuadraticEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuadraticEaseIn);
            Assert.AreEqual(10000, value);
        }

        [Test]
        public void Should_Interpolate_QuadraticEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuadraticEaseInOut);
            Assert.AreEqual(-19601.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuadraticEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuadraticEaseOut);
            Assert.AreEqual(-9800.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuarticEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuarticEaseIn);
            Assert.AreEqual(100000000.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuarticEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuarticEaseInOut);
            Assert.AreEqual(-768476807.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuarticEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuarticEaseOut);
            Assert.AreEqual(-96059600.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuinticEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuinticEaseIn);
            Assert.AreEqual(10000000000.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuinticEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuinticEaseInOut);
            Assert.AreEqual(152158407985.0d, value);
        }

        [Test]
        public void Should_Interpolate_QuinticEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.QuinticEaseOut);
            Assert.AreEqual(9509900500.0d, value);
        }

        [Test]
        public void Should_Interpolate_SineEaseIn()
        {
            var value = Easings.Interpolate(100, EasingFunction.SineEaseIn);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void Should_Interpolate_SineEaseInOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.SineEaseInOut);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void Should_Interpolate_SineEaseOut()
        {
            var value = Easings.Interpolate(100, EasingFunction.SineEaseOut);
            Assert.AreEqual(9.8219336186423597E-16d, value);
        }
    }
}
