using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligenceHW8
{
	class Neuron
	{
		public double Output { get; private set; }

		private IList<Connection> rightConnections;

		private IList<Connection> connections;


		private readonly bool isHidden;

		public double Error { get; private set; }

		public Neuron(IList<Connection> cons, bool hidden, double output = 0)
		{
			this.connections = cons;
			this.isHidden = hidden;
			this.Output = output;

			this.rightConnections = new List<Connection>();
		}

		public void UpdateOutput(double input)
		{
			if(this.connections == null)
			{
				this.Output = input;
			}
		}

		public void AddRightConnection(Connection con)
		{
			this.rightConnections.Add(con);
		}

		private double CalculateInputSum()
		{
			if (this.connections == null)
			{
				return this.Output;
			}

			double sum = 0;
			foreach(var con in connections)
			{
				sum += con.LeftNeuronValue * con.Weight;
			}

			return sum;
		}

		private double ActivationFunction(double inputSum)
		{
			if (this.connections == null)
			{
				return this.Output;
			}
			this.Output = Sigmoid(inputSum);
			return this.Output;
		}

		public void Activate()
		{
			var input = CalculateInputSum();
			this.ActivationFunction(input);
			foreach (var con in rightConnections)
			{
				con.LeftNeuronValue = this.Output;
			}
		}

		public static double Sigmoid(double value)
		{
			return 1.0f / (1.0f + (double)Math.Exp(-value));
		}

		private double Derivative()
		{
			return 1 - this.Output;
		}

		private double RightNeighboursSum()
		{
			double sum = 0;
			foreach(var con in rightConnections)
			{
				sum += con.Weight * con.RightNeuronError;
			}
			return sum;
		}

		private void BackPropagation()
		{
			if (this.connections == null) return;

			foreach (var con in this.connections)
			{
				con.RightNeuronError = this.Error;
				con.Weight = con.Weight + 0.5 * con.RightNeuronError * con.LeftNeuronValue;
			}
		}

		public double CalculateError(double expected)
		{
			if (isHidden)
			{
				this.Error = this.Output * Derivative() * RightNeighboursSum();
			}
			else
			{
				this.Error = this.Output * Derivative() * (expected - this.Output);
			}

			this.BackPropagation();

			return this.Error;
		}
	}
}
