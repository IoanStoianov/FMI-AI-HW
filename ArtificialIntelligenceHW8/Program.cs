using System;

namespace ArtificialIntelligenceHW8
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Input layers num:");
			int leyarsNum = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Input hidden layers neuron count:");
			int hiddenLayersNeuronNum = Int32.Parse(Console.ReadLine());


			var network = new NeuralNetwork(leyarsNum, hiddenLayersNeuronNum);

			var or = new Tuple<double, double, double>[]{
				new Tuple<double,double,double>(1, 1, 1),
				new Tuple<double,double,double>(0, 1, 1),
				new Tuple<double,double,double>(1, 0, 1),
				new Tuple<double,double,double>(0, 0, 0)};

			network.TrainNetwork(or);

			Console.WriteLine("Input 1, 1 Expected: 1    Actual: {0}", network.RunNetwork(1, 1));
			Console.WriteLine("Input 0, 1 Expected: 1    Actual: {0}", network.RunNetwork(0, 1));
			Console.WriteLine("Input 1, 0 Expected: 1    Actual: {0}", network.RunNetwork(1, 0));
			Console.WriteLine("Input 0, 0 Expected: 0    Actual: {0}", network.RunNetwork(0, 0));
			Console.WriteLine("\n");


			network = new NeuralNetwork(leyarsNum, hiddenLayersNeuronNum);
			var and = new Tuple<double, double, double>[]{
				new Tuple<double,double,double>(1, 1, 1),
				new Tuple<double,double,double>(0, 1, 0),
				new Tuple<double,double,double>(1, 0, 0),
				new Tuple<double,double,double>(0, 0, 0)};

			network.TrainNetwork(and);

			Console.WriteLine("Input 1, 1 Expected: 1    Actual: {0}", network.RunNetwork(1, 1));
			Console.WriteLine("Input 0, 1 Expected: 0    Actual: {0}", network.RunNetwork(0, 1));
			Console.WriteLine("Input 1, 0 Expected: 0    Actual: {0}", network.RunNetwork(1, 0));
			Console.WriteLine("Input 0, 0 Expected: 0    Actual: {0}", network.RunNetwork(0, 0).ToString("F5"));
			Console.WriteLine("\n");



			network = new NeuralNetwork(leyarsNum, hiddenLayersNeuronNum);
			var xor = new Tuple<double, double, double>[]{
				new Tuple<double,double,double>(1, 1, 0),
				new Tuple<double,double,double>(0, 1, 1),
				new Tuple<double,double,double>(1, 0, 1),
				new Tuple<double,double,double>(0, 0, 0)};

			network.TrainNetwork(xor);

			Console.WriteLine("Input 1, 1 Expected: 0    Actual: {0}", network.RunNetwork(1, 1));
			Console.WriteLine("Input 0, 1 Expected: 1    Actual: {0}", network.RunNetwork(0, 1));
			Console.WriteLine("Input 1, 0 Expected: 1    Actual: {0}", network.RunNetwork(1, 0));
			Console.WriteLine("Input 0, 0 Expected: 0    Actual: {0}", network.RunNetwork(0, 0));
		}
	}
}
