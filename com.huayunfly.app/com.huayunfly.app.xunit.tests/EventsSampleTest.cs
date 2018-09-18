using System;
using Xunit;

namespace com.huayunfly.app.xunit.tests
{
    public class EventsSampleTest
    {

        [Fact]
        public void CarInfoEventTest()
        {
            var dealer = new CarDealer();
            var xiaoming = new Consumer("Xiaoming");
            dealer.NewCarInfo += xiaoming.NewCarIsHere;
            dealer.NewCar("Willimas");
            Assert.Equal("Willimas", xiaoming.FiredEvents[0]);

            var liudan = new Consumer("Liudan");
            dealer.NewCarInfo += liudan.NewCarIsHere;
            dealer.NewCar("Mercedes");
            Assert.Equal("Mercedes", xiaoming.FiredEvents[1]);
            Assert.Equal("Mercedes", liudan.FiredEvents[0]);

            dealer.NewCarInfo -= xiaoming.NewCarIsHere;
            dealer.NewCar("Ferrrari");
            Assert.Equal(2, liudan.FiredEvents.Count);
            Assert.Equal(2, xiaoming.FiredEvents.Count);
        }
    }
}
