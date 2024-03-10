using EventLogger;

namespace EventLoggerTest
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void LogString()
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Today);
            Assert.IsTrue(log.ToString() == "Pepa 1 3/9/2024\nHello");
        }
        [TestMethod]
        public void LogCompareSame() 
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Today);
            Log log2 = new Log("Pepa", "Hello", 1, DateTime.Today);
            Assert.IsTrue(log.CompareTo(log2) == 0);
        }
        [TestMethod]
        public void LogCompareFirst()
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Now);
            Log log2 = new Log("Pepa", "Hello", 1, DateTime.Now);
            Assert.IsTrue(log.CompareTo(log2) == 1);
        }
        [TestMethod]
        public void LogCompareSecond()
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Now);
            Log log2 = new Log("Pepa", "Hello", 1, DateTime.Today);
            Assert.IsTrue(log.CompareTo(log2) == -1);
        }
    }
}