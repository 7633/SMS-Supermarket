using System;

namespace SMS_Supermarket
{
    internal class ExponentialDistribution
    {
        private readonly double _lambda;
        private readonly Random _random;

        public ExponentialDistribution(double lambda)
        {
            _random = new Random();
            _lambda = lambda;
        }

        public double NextValue()
        {
            return Math.Log(1 - _random.NextDouble())/-_lambda;
        }
    }
}