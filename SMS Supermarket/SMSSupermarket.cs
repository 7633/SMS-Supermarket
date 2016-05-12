using System;
using System.Timers;

namespace SMS_Supermarket
{
    internal class SmsSupermarket
    {
        private static void Main(string[] args)
        {
            // Create supermarket serve system with
            var smarket = new Supermarket(
                5,  // queue length
                2,  // cash desks
                60, // avarage number of incoming buyers per unit of time
                2   // average time of buyer serving
            );

            // start serving model for 50 seconds
            smarket.Start();

            var timer = new Timer(50000);
            timer.Elapsed += (source, e) =>
            {
                smarket.Stop();
                smarket.DisplayResults();
            };
            timer.AutoReset = false;
            timer.Enabled = true;

            Console.ReadLine();
        }
    }
}