using System;
using System.Timers;

namespace SMS_Supermarket
{
    internal class Supermarket
    {
        private double _averageTimeOfStaying;   // average time of staying buyer in queue
        private double _timeInSystem;
        private readonly Cashier[] _cashiers;
        private readonly Generator _flow;       // input flow of buyers
        private bool _isWorking;
        private int _numberOfServedBuyers;      // number of serving buyers with purchases
        private int _numberOfUnservedBuyers;    // number of buyers without purchases
        private readonly Queue _queue;

        private readonly double _probabilityOfReject;
        private readonly double _absoluteTrafficCapacity;
        private readonly double _loadSystemFactor;

        public Supermarket(
            int queueLength,
            int cashDesksNumber,
            double inputIntensity,     // lyambda - avarage number of incoming buyers per unit of time
            int serveTime)          // t - average time of buyer serving
        {
            // Creating queue
            _queue = new Queue(queueLength);

            // Creating cashers with individual serving time
            _cashiers = new Cashier[cashDesksNumber];
            for (var i = 0; i < cashDesksNumber; i++)
            {
                _cashiers[i] = new Cashier(serveTime, "Cash desk #" + (i + 1));
                _cashiers[i].AddOnStopHandler(ServeBuyer);
            }

            // Creating of input buyer flow
            _flow = new Generator(inputIntensity);
            _flow.AddOnStopHandler(CreateBuyer);

            // Calculating of system characteristics
            _loadSystemFactor = inputIntensity / 50 * serveTime / 50;
            _probabilityOfReject = 1.0;
            int factorial = 1;
            for (var i = 1; i <= cashDesksNumber; i++)
            {
                factorial *= i;
                _probabilityOfReject += Math.Pow(_loadSystemFactor, i)/factorial;
            }

            _probabilityOfReject *= Math.Pow(_loadSystemFactor, cashDesksNumber) / factorial;

            var relativeTrafficCapacity = 1 - _probabilityOfReject;
            _absoluteTrafficCapacity = relativeTrafficCapacity*inputIntensity;

            _displayCharacteristics();
        }

        private void CreateBuyer(object source, ElapsedEventArgs e)
        {
            if (_isWorking)
            {
                if (_queue.Push(new Buyer()))
                {
                    ServeBuyer(source, e);
                }
                else
                {
                    _numberOfUnservedBuyers++;
                }
                _flow.Start();
            }
        }

        private void ServeBuyer(object source, ElapsedEventArgs e)
        {
            if (_isWorking)
            {
                if (!_queue.IsEmpty())
                {
                    foreach (Cashier cashDesk in _cashiers)
                    {
                        if (cashDesk.IsBusy) continue;
                        cashDesk.Serve(_queue.Pop());
                        break;
                    }
                }
            }
        }

        private void _displayCharacteristics()
        {
            Console.WriteLine("Coefficient of system load: " + _loadSystemFactor);
            Console.WriteLine("Probability of reject is: {0} %", _probabilityOfReject*100);
            Console.WriteLine("Absolute traffic capacity is: " + _absoluteTrafficCapacity);
        }

        public void DisplayResults()
        {
            Console.WriteLine("Number of served buyers: " + _numberOfServedBuyers);
            Console.WriteLine("Number of buyers without purchases : " + _numberOfUnservedBuyers);
            Console.WriteLine("Average time of staying in queue " + _averageTimeOfStaying);
        }

        public void Start()
        {
            _timeInSystem = 0;
            _numberOfServedBuyers = 0;
            _numberOfUnservedBuyers = 0;

            _isWorking = true;
            _flow.Start();
        }

        public void Stop()
        {
            _isWorking = false;

            foreach (Cashier cashDesk in _cashiers)
            {
                _timeInSystem += cashDesk.TimeInSystem;
                _numberOfServedBuyers += cashDesk.NumberOfServedBuyers;
            }

            _averageTimeOfStaying = _timeInSystem/_numberOfServedBuyers;
        }
    }
}