using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligenceHW8
{
	class Connection
	{
		public double LeftNeuronValue { get; set; }

		public double Weight { get; set; }

		public double RightNeuronError { get; set; }

		public Connection(double value, double weight)
		{
			this.LeftNeuronValue = value;
			this.Weight = weight;
		}
	}
}
