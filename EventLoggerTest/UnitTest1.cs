using EventLogger;
using System.Diagnostics;

namespace EventLoggerTest
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void LogString()
        {
            Log log = new Log("Pepa", "Hello", 1, DateTime.Today);
            DateTime dt = DateTime.Now;
            Assert.IsTrue(log.ToString() == $"Pepa 1 {dt.ToString("d/M/yyyy")}\nHello");
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
            Assert.IsTrue(log.CompareTo(log2) == 1);
        }
        [TestMethod]
        public void LogCompareSecond()
        {
            Log log2 = new Log("Pepa", "Hello", 1, DateTime.Now);
            Log log = new Log("Pepa", "F", 2, DateTime.Now);
            Assert.IsTrue(log.CompareTo(log2) == -1);
        }
    }
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void LoggerExpand() 
        {
            Logger logger = new Logger(5);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            logger.Expand(2);
            Assert.IsTrue(logger.Length == 7 && logger.Count == 2);
        }
        [TestMethod]
        public void LoggerReduce()
        {
            Logger logger = new Logger(5);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            logger.AddLog(new Log("Pepa", "Hell", 4, DateTime.Now));
            logger.AddLog(new Log("Pepa", "FF", 2, DateTime.Now));
            logger.AddLog(new Log("Pepa", "Helo", 5, DateTime.Now));
            logger.Reduce(2);
            Assert.IsTrue(logger.Length == 3 && logger.Count == 3);
        }
        [TestMethod]
        public void LoggerExtract()
        {
            Logger logger = new Logger(5);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            DateTime dt = DateTime.Now;
            Assert.IsTrue(logger.Extract() == $"Pepa 2 {dt.ToString("d/M/yyyy")}\nF\nPepa 1 {dt.ToString("d/M/yyyy")}\nHello\n");
        }
        [TestMethod]
        public void LoggerOverflow()
        {
            Logger logger = new Logger(3);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 3, DateTime.Now));
            DateTime dt = DateTime.Now;
            Assert.IsTrue(logger.Extract() == $"Pepa 3 {dt.ToString("d/M/yyyy")}\nF\nPepa 1 {dt.ToString("d/M/yyyy")}\nHello\nPepa 2 {dt.ToString("d/M/yyyy")}\nF\n");
        }
        [TestMethod]
        public void LoggerClear() 
        {
            Logger logger = new Logger(3);
            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            logger.Clear();
            Assert.IsTrue(logger.Extract() == string.Empty);

        }
    }
}