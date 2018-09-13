﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.huayunfly.app.xunit.tests
{
    public class DBCommandSamplesTest
    {
        [Fact]
        public void ExecuteNoQueryTest() => 
            Assert.Equal(1, DBCommandSamples.ExecuteNoQuery());

        [Fact]
        public void ExecuteReaderTest() =>
            Assert.Equal(13, DBCommandSamples.ExecuteReader("Professional"));
    }
}
