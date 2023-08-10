using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGramasCSharp
{
    public class Transition
    {
		private string key;
		private int concurrency;
		private double probability;

		public double Probability
        {
			get { return probability; }
			set { probability = value; }
		}

		public int Concurrency
        {
			get { return concurrency; }
			set { concurrency = value; }
		}

		public string Key
        {
			get { return key; }
			set { key = value; }
		}

        public Transition(int concurrency, string key)
        {
            Concurrency = concurrency;
            Key = key;
        }
    }
}
