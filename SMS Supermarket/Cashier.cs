using System;

namespace SMS_Supermarket
{
    internal class Cashier : Generator
    {
        public readonly string Name;
        private Buyer _buyer;

        public int NumberOfServedBuyers { get; private set; }
        public double TimeInSystem { get; private set; }
        public bool IsBusy { get; set; }

        // Cach desk constructor
        // Create cash desk and elapse serve time
        public Cashier(int time, string name) : base(time)
        {
            Name = name;

            AddOnStopHandler((source, e) =>
            {
                Console.WriteLine(Name + " stoped serving");

                IsBusy = false;
                _buyer.SetServed();
                TimeInSystem += _buyer.GetLifeTime();
                NumberOfServedBuyers++;
            }
                );
        }

        public void Serve(Buyer buyer)
        {
            Console.WriteLine(Name + " started serving");

            _buyer = buyer;
            IsBusy = true;
            Start();
        }
    }
}