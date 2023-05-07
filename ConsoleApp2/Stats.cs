using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Stats : IComparable<Stats>
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int steps;
        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }
        private DateTime finishTime;
        public DateTime FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public void Add(string name, int steps, DateTime start, DateTime finish)
        {
            Name = name;
            Steps = steps;
            StartTime = start;
            FinishTime = finish;
        }
        public int CompareTo(Stats? other)
        {
            return this.Steps.CompareTo(other?.Steps);
        }
        public override string ToString()
        {
            return $"Ім'я {Name}, кількість ходів {Steps}, часу витраченно {FinishTime.Subtract(StartTime).TotalSeconds}";
        }
    }

}
