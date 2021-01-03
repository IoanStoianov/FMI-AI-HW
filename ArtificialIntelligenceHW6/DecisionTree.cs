using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialIntelligenceHW6
{
	class DecisionTree
	{

		private int K = 6;
		private int classIndex = 9;
		private HashSet<int> takenIndexes;

		public DecisionTree()
		{
			this.takenIndexes = new HashSet<int>() { classIndex };
		}

		private IDictionary<string,int> CalculateTable(int index, string[][] set)
		{
			var table = set.Select(element => element[index])
				.GroupBy(item => item)
				.ToDictionary(item => item.Key, item => item.Count());
			return table;
		}

		private IDictionary<string, Dictionary<string, int>> CalculateTable2D(int index, int classIndex, string[][] set)
		{
			var table = new Dictionary<string, Dictionary<string,int>>();
			foreach (var item in set)
			{
				if (!table.ContainsKey(item[index]))
				{
					table.Add(item[index], new Dictionary<string, int>());
				}
				if (!table[item[index]].ContainsKey(item[classIndex]))
				{
					table[item[index]].Add(item[classIndex], 0);
				}
				table[item[index]][item[classIndex]]++;
			}

			return table;
		}

		private double Entropy(IDictionary<string, int> occurances, int setLength)
		{
			double sum = 0;
			foreach (double item in occurances.Values)
			{
				double probability = item / setLength;
				sum -= probability * Math.Log(probability, 2);
			}
			return sum;
		}

		private double OneAttribteEntropy(int index, string[][] set)
		{
			IDictionary<string, int> occurances = this.CalculateTable(index, set);
			return this.Entropy(occurances, set.Length);
		}

		private double TwoAttribteEntropy(int index, string[][] set)
		{
			double sum = 0;

			IDictionary<string, int> attributeOccurances = this.CalculateTable(index, set);
			if (attributeOccurances.Count == set.Length)
				return 2;
			IDictionary<string, Dictionary<string, int>> occurances = this.CalculateTable2D(index, classIndex, set);
			foreach (var item in occurances)
			{
				double probability = (double)attributeOccurances[item.Key] / set.Length;
				double entropy = this.Entropy(item.Value, attributeOccurances[item.Key]);
				sum += probability * entropy;
			}
			return sum;
		}

		private int InformationGain(string[][] set)
		{
			var classEntropy = this.OneAttribteEntropy(classIndex, set);
			if(classEntropy == 0)
			{
				return -1;
			}
			double maxGain = 0;
			var bestIndex = -1;
			for (int i = 0; i < set[0].Length; i++)
			{
				if (takenIndexes.Contains(i)) continue;

				var attribteEntropy = this.TwoAttribteEntropy(i, set);
				if((classEntropy - attribteEntropy) > maxGain)
				{
					maxGain = classEntropy - attribteEntropy;
					bestIndex = i;
				}
			}

			return bestIndex;
		}

		private Node BuildTree(string[][] set, int depth = 0)
		{
			if(set.Length <= K)
			{
				string best = set.Select(element => element[classIndex])
					.GroupBy(item => item)
					.Select(item => (item.Key, item.Count()))
					.OrderBy(item => item.Item2).First().Key;
				return new Node(-1, null, true, best);
			}

			var index = this.InformationGain(set);
			if(index == -1)
			{
				return new Node(-1, null, true, set[0][classIndex]);
			}
			this.takenIndexes.Add(index);

			var childs = new Dictionary<string, Node>();
			var sets = set.GroupBy(item => item[index]);

			foreach(var group in sets)
			{
				var nextNodeSet = group.ToArray();
				var nextNode = BuildTree(nextNodeSet, depth+1);
				childs.Add(group.Key, nextNode);
			}

			this.takenIndexes.Remove(index);
			var node = new Node(index, childs);

			return node;
		}

		public class Node
		{
			public bool isLeaf;
			public string value;
			public int index;
			public IDictionary<string, Node> childs;

			public Node(int index, IDictionary<string, Node> childs, bool leaf = false, string val = default)
			{
				this.value = val;
				this.isLeaf = leaf;
				this.index = index;
				this.childs = childs;
			}
		}

		private double TestModel(Node model, string[][] set)
		{
			double accuracy = 0;
			foreach(var item in set)
			{
				Node node = model;

				while (!node.isLeaf)
				{
					if (node.childs.ContainsKey(item[node.index]))
					{
						node = node.childs[item[node.index]];
					}
					else
					{
						node = node.childs.First().Value;
					}
				}

				if (node.value == item[classIndex]) accuracy++;
			}

			Console.WriteLine("Accurancy: {0}", (accuracy / 28).ToString("#0.##%"));
			return (int)accuracy;
		}

		public void TrainAndTest(string[][] set)
		{
			double accuracySum = 0;
			set = this.Shuffle(set);
			for (int i = 0; i < 10; i++)
			{
				var trainSet = set.Take(i * 28).Union(set.Skip(i * 28 + 28)).ToArray();
				var model = this.BuildTree(trainSet);

				var testSet = set.Skip(i * 28).Take(28).ToArray();
				accuracySum += this.TestModel(model, testSet);
			}

			Console.WriteLine("Average accurancy: {0}", (accuracySum / 252).ToString("#0.##%"));
		}

		private string[][] Shuffle(string[][] set)
		{
			Random rnd = new Random();
			return set.OrderBy(x => rnd.Next()).ToArray();
		}
	}
}
