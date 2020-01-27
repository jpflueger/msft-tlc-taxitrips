using System;
using Xunit;
using Moq;

namespace TLC.Taxi.Data.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // This is a temporary test to make sure compilation and running the tests work

            // arrange
            var mock = new Mock<IInterface1>();
            mock.Setup(i => i.Method1()).Throws(new Exception());
            var sut = new Class1(mock.Object);

            // act
            // assert
            Assert.Throws<Exception>(() => sut.TestMethod());
        }
    }
}
