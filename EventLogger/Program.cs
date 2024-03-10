using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EventLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger<Log> logger = new Logger<Log>(5);

            logger.AddLog(new Log("Pepa", "Hello", 1, DateTime.Now));
            logger.AddLog(new Log("Pepa", "F", 2, DateTime.Now));
            Console.WriteLine(logger.Extract());
            
        }
    }
    public class Logger<T>
    {
        private T[] buffer;
        private int count;
        private int length;
        public int Length
        {
            get => length;
            set => length = value;
        }
        public delegate void NewLog(T log);
        public event NewLog OnNewLog;
        public Logger(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException("Capacity has to be bigger than 0 and integer");
            }
            buffer = new T[capacity];
            count = 0;
            Length = capacity;
            OnNewLog += WriteLog;
        }

        public void Expand(int size)
        {
            T[] temp = new T[buffer.Length];
            Array.Copy(buffer, temp, buffer.Length);
            buffer = new T[temp.Length + size];
            Length = buffer.Length;
            foreach (var item in temp)
            {
                buffer.Append(item);
            }

        }
        public void Reduce(int size)
        {
            T[] temp = new T[buffer.Length];
            Array.Copy(buffer, temp, buffer.Length);
            buffer = new T[temp.Length - size];
            Length = buffer.Length;
            count = 0;
            foreach (var item in temp.Reverse())
            {
                buffer.Append(item);
                count++;
            }
            buffer.Reverse();
        }
        public void Clear()
        {
            Array.Clear(buffer, 0, buffer.Length);
            count = 0;
        }
        public void AddLog(T log)
        {
            if (count == buffer.Length)
            {
                buffer = buffer.Skip(0).ToArray();
            }
            else if (count < buffer.Length)
            {
                count++;
            }
            buffer.Append(log);
            OnNewLog.Invoke(log);
        }
        public string Extract()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in buffer.OrderDescending())
            {
                if (item != null)
                {
                    sb.AppendLine(item.ToString());
                }
            }
            return sb.ToString();
        }
        public static void WriteLog(T log)
        {
            Console.WriteLine(log);
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