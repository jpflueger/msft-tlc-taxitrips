using System;

namespace TLC.Taxi.Data
{
    public class Class1
    {
        private readonly IInterface1 _iface;

        public Class1(IInterface1 iface)
        {
            _iface = iface;
        }

        public void TestMethod()
        {
            _iface.Method1();
        }
    }
}
