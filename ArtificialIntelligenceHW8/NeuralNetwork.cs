using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialIntelligenceHW8
{
	public class NeuralNetwork
	{
		private double biasValue = -1;
		public int LayersNum { get; private set; }

		public int HiddenLayersNeuronNum { get; private set; }

		private IList<Neuron[]> neuronLayers;

		public NeuralNetwork(int layersNum, int hiddenLayersNum)
		{
			this.LayersNum = layersNum;
			this.HiddenLayersNeuronNum = hiddenLayersNum;

			this.InitNeuronsAndConnections();
		}

		private void InitLayers()
		{
			this.neuronLayers = new List<Neuron[]>();
			this.neuronLayers.Add(new Neuron[2]); // input
			for (int i = 0; i < this.LayersNum-2; i++)
			{
				this.neuronLayers.Add(new Neuron[HiddenLayersNeuronNum]);
			}
			this.neuronLayers.Add(new Neuron[1]); // output
		}

		private void SetInputNeurons(double v1, double v2)
		{
			this.neuronLayers[0][0].UpdateOutput(v1);
			this.neuronLayers[0][1].UpdateOutput(v2);
		}
		private void InitNeuronsAndConnections()
		{
			this.InitLayers();

			this.neuronLayers[0][0] = new Neuron(null, false);
			this.neuronLayers[0][1] = new Neuron(null, false);

			for (int i = 1; i < this.LayersNum-1; i++)
			{
				for (int j = 0; j < this.HiddenLayersNeuronNum; j++)
				{
					this.neuronLayers[i][j] = this.CreateNeuron(i-1, true);
				}
			}

			//output neuron
			this.neuronLayers[this.LayersNum - 1][0] = this.CreateNeuron(this.LayersNum - 2, false);
		}

		private Neuron CreateNeuron(int layer, bool isHidden)
		{
			var connList = this.neuronLayers[layer].Select(n => {
				var con = new Connection(n.Output, this.GetRandomWeight(-0.5, 0.5));
				n.AddRightConnection(con);
				return con;
				}).ToList();
			//add bias
			connList.Add(new Connection(this.biasValue, this.GetRandomWeight(-0.5, 0.5)));
			return new Neuron(connList, isHidden);
		}

		public void TrainNetwork(Tuple<double, double, double>[] trainData)
		{
			double v1, v2, expected;

			for (int i = 0; i <= 50000; i++)
			{
				(v1, v2, expected) = trainData[i % 4];
				this.SetInputNeurons(v1, v2);
				foreach (var layer in neuronLayers)
				{
					foreach (var neuron in layer)
					{
						neuron.Activate();
					}
				}
				for (int j = this.LayersNum-1; 0 <= j; j--)
				{
					var layer = this.neuronLayers[j];
					foreach (var neuron in layer)
					{
						var err = neuron.CalculateError(expected);
					}
				}
			}
		}

		public double RunNetwork(int v1, int v2)
		{
			this.SetInputNeurons(v1, v2);
			double output = 0;

			foreach (var layer in neuronLayers)
			{
				foreach (var neuron in layer)
				{
					neuron.Activate();
					output = neuron.Output;
				}
			}

			return Math.Round(output, 5);
		}

		Random random = new Random();
		public double GetRandomWeight(double minimum, double maximum)
		{
			return random.NextDouble() * (maximum - minimum) + minimum;
		}

	}
}
