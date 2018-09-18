using System;
using System.Collections.Generic;
using Xunit;

namespace com.huayunfly.app
{
    public class CarInfoEventArgs : EventArgs
    {
        public CarInfoEventArgs(string car) => Car = car;

        public string Car{ get; }
    }

    public class CarDealer
    {
        /* delegate EventHandler<TEventArgs>:
         * public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) 
         * where TEventArgs: EventArgs
         */
        public event EventHandler<CarInfoEventArgs> NewCarInfo;

        public void NewCar(string car)
        {
            /* Before C#6:
             * EventHandler<CarInfoEventArgs> newCarInfo = NewCarInfo;
             * if (newCarInfo != null)
             * {
             *      newCarInfo(this, new CarInfoEventArgs(car));
             * }
             * 
             * If no one subscribe, the delegate is null.
             */
            Console.WriteLine($"CarDealer, new car {car}");
            NewCarInfo?.Invoke(this, new CarInfoEventArgs(car));
        }
    }

    public class Consumer
    {
        List<string> firedEvents = new List<string>();
        private string _name;
        public Consumer(string name) => _name = name;

        public List<string> FiredEvents { get => firedEvents; }

        public void NewCarIsHere(object sender, CarInfoEventArgs args)
        {
            Console.WriteLine($"{_name}: car {args.Car} is new");
            firedEvents.Add(args.Car);
        }
    }

    public class CarDealerTest
    {
        [Fact]
        public void CarInfoEventTest()
        {
            var dealer = new CarDealer();
            var xiaoming = new Consumer("Xiaoming");
            dealer.NewCarInfo += xiaoming.NewCarIsHere;
            dealer.NewCar("Willimas");
        }
    }
}
