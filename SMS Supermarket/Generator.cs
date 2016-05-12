using System.Collections.Generic;
using System.Timers;

namespace SMS_Supermarket
{
    internal class Generator
    {
        private readonly ExponentialDistribution _distribution;
        protected List<ElapsedEventHandler> Handlers = new List<ElapsedEventHandler>();

        public Generator(int frequency)
        {
            var lambda = 1.0/frequency;
            _distribution = new ExponentialDistribution(lambda);
        }

        public Generator(double intensity)
        {
            _distribution = new ExponentialDistribution(intensity);
        }

        public void Start()
        {
            var time = (int) (_distribution.NextValue()*1000);

            var timer = new Timer(time);

            foreach (var handler in Handlers)
            {
                timer.Elapsed += handler;
            }

            timer.AutoReset = false;
            timer.Enabled = true;
        }

        public void AddOnStopHandler(ElapsedEventHandler handler)
        {
            Handlers.Add(handler);
        }
    }
}