using System;
using System.Collections.Generic;
using Xunit;

namespace com.huayunfly.app.xunit.tests
{
    public class DBConnectionSamplesTest
    {
        [Fact]
        public void OpenConnectionTest()
        {
            Assert.True(DBConnectionSamples.OpenConnection());
        }

        [Fact]
        public void OpenConnectionUsingConfigTest()
        {
            Assert.True(DBConnectionSamples.OpenConnectionUsingConfig());
        }
    }
}
