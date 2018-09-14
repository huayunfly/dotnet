using System;
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

        [Fact]
        public void StoredProcedureTest() =>
            Assert.Equal(21, DBCommandSamples.StoredProcedure("Wrox press"));

        [Fact]
        public void ExecuteScalarTest() =>
            Assert.Equal(24, DBCommandSamples.ExecuteScalar());
    }
}
