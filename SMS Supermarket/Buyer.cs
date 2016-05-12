using System;
using System.Diagnostics;

namespace SMS_Supermarket
{
    internal class Buyer
    {
        private readonly long _arrivalTime;
        private readonly Stopwatch _timer = Stopwatch.StartNew();
        private double _lifeTime;

        public Buyer()
        {
            _arrivalTime = _timer.ElapsedMilliseconds;

            Console.WriteLine("Buyer added to the queue");
        }

        public double GetLifeTime()
        {
            return _lifeTime;
        }

        public void SetServed()
        {
            _lifeTime = (_timer.ElapsedMilliseconds - _arrivalTime)/1000.0;
        }
    }
}