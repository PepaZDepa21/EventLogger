using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EventLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger(5);

            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            Console.WriteLine(logger.Extract());
            
        }
    }
    public class Logger
    {
        private Log[] buffer;
        private int count;
        public int Count
        {
            get => count;
            set => count = value;
        }
        private int length;
        public int Length
        {
            get => length;
            set => length = value;
        }
        public delegate void NewLog(string logContent);
        public event NewLog OnNewLog;
        public Logger(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException("Capacity has to be bigger than 0 and integer");
            }
            buffer = new Log[capacity];
            Count = 0;
            Length = capacity;
            OnNewLog += WriteLog;
        }

        public void Expand(int size)
        {
            Log[] temp = new Log[buffer.Length];
            Array.Copy(buffer, temp, buffer.Length);
            buffer = new Log[temp.Length + size];
            Length = buffer.Length;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != null)
                {
                    buffer[i] = temp[i];
                }
            }

        }
        public void Reduce(int size)
        {
            Log[] temp = new Log[buffer.Length];
            Array.Copy(buffer, temp, buffer.Length);
            buffer = new Log[temp.Length - size];
            Length = buffer.Length;
            Count = 0;
            while(Count < Length)
            {
                buffer[Count] = temp[Count + size];
                Count++;
            }
        }
        public void Clear()
        {
            Array.Clear(buffer, 0, buffer.Length);
            Count = 0;
        }
        public void AddLog(Log log)
        {
            
            if (Count == buffer.Length)
            {
                buffer = buffer.Skip(0).ToArray();
                for (int i = 1; i < Length; i++)
                {
                    buffer[i - 1] = buffer[i];
                }
                buffer[Length-1] = log;
            }
            else if (Count < buffer.Length)
            {
                buffer[Count++] = log;
            }
            OnNewLog.Invoke(log.Message);
        }
        public string Extract()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in buffer.Order())
            {
                if (item != null)
                {
                    sb.Append(item.ToString() + "\n");
                }
            }
            return sb.ToString();
        }
        public static void WriteLog(string logContent)
        {
            Console.WriteLine(logContent);
        }
    }
    public class Log : IComparable
    {
        
        private int priority;
        public int Priority
        {
            get => priority; 
            set => priority = value;
        }
        private string message;
        public string Message
        {
            get => message;
            set => message = value;
        }
        private string user;
        public string User
        {
            get => user;
            set => user = value;
        }
        private DateTime timeCreated;
        public DateTime TimeCreated
        {
            get => timeCreated;
            set => timeCreated = value;
        }
        public Log(string userCreator, string massage, int ppriority, DateTime timeStamp) 
        {
            User = userCreator;
            Priority = ppriority;
            Message = massage;
            TimeCreated = timeStamp;
        }

        public int CompareTo(object? obj)
        {
            Log log = obj as Log;
            if(TimeCreated < log.TimeCreated) 
            {
                return 1;
            }
            else if(TimeCreated > log.TimeCreated)
            {
                return -1;
            }
            return 0;
        }
        public override string ToString() => $"{User} {Priority} {TimeCreated.ToString("d/M/yyyy")}\n{Message}";
    }
}