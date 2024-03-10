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
            Assert.IsTrue(log.ToString() == "Pepa 1 10/3/2024\nHello");
        }
        [TestMethod]
        public void LogCompareSame() 
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Today);
            Log log2 = new Log("Pepa", "F", 2, DateTime.Today);
            Assert.IsTrue(log.CompareTo(log2) == 0);
        }
        [TestMethod]
        public void LogCompareFirst()
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Now);
            Log log2 = new Log("Pepa", "F", 2, DateTime.Now);
            Assert.IsTrue(log.CompareTo(log2) == -1);
        }
        [TestMethod]
        public void LogCompareSecond()
        {
            Log log2 = new Log("Pepa", "Hello", 1, DateTime.Now);
            Log log = new Log("Pepa", "F", 2, DateTime.Now);
            Assert.IsTrue(log.CompareTo(log2) == 1);
        }
    }
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void LoggerExpand() 
        {
            Logger<Log> logger = new Logger<Log>(5);
            logger.Expand(2);
            Assert.IsTrue(logger.Length == 7);
        }
        [TestMethod]
        public void LoggerReduce()
        {
            Logger<Log> logger = new Logger<Log>(5);
            logger.Reduce(2);
            Assert.IsTrue(logger.Length == 3);
        }
        [TestMethod]
        public void LoggerExtract()
        {
            Logger<Log> logger = new Logger<Log>(5);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            Assert.IsTrue(logger.Extract() == "Pepa 1 10/3/2024\nHello\nPepa 2 10/3/2024\nF\n");
        }
    }
}