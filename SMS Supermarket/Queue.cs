using System;
using System.Collections.Generic;

namespace SMS_Supermarket
{
    internal class Queue : Queue<Buyer>
    {
        private readonly int _capacity;

        public Queue(int capacity) { _capacity = capacity; }

        public bool IsEmpty() { return Count == 0; }
        public bool IsFull() { return Count >= _capacity; }

        // add buyer to queue or reject him
        public bool Push(Buyer buyer)
        {
            if (IsFull())
            {
                Console.WriteLine("Queue is full");
                return false;
            }

            Enqueue(buyer);
            Console.WriteLine("Buyer added, queue length is " + Count);
            return true;
        }

        // remove buyer from queue
        public Buyer Pop()
        {
            Console.WriteLine("Buyer removed, queue length is " + (Count - 1));
            return Dequeue();
        }
    }
}