using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtificialIntelligenceHW7
{
	public class KMeans
	{
		private Point[] data;

		private int clustersNum;

		private Point[] centroids;

		private IList<Point>[] clusters;

		private double randMultiplayer;

		//Within-Cluster-Sum-of-Squares
		private double WCSS; 

		public KMeans(Point[] data, int K, double multiplayer)
		{
			this.data = data;
			this.clustersNum = K;
			this.randMultiplayer = multiplayer;
			this.centroids = new Point[K];
			this.clusters = new List<Point>[K];
			InitClusters();
		}

		private void InitClusters()
		{
			for (int i = 0; i < clustersNum; i++)
			{
				this.clusters[i] = new List<Point>();
			}
		}

		private void InitCentroids()
		{

			var rand = new Random();
			for (int i = 0; i < clustersNum; i++)
			{
				if (this.clusters[i].Count == 0)
				{
					this.centroids[i] = new Point(rand.NextDouble() * randMultiplayer, rand.NextDouble() * randMultiplayer);
				}
			}
		}

		private void Iteration()
		{
			foreach(var point in data)
			{
				double minDistance = double.MaxValue;
				double currentDistance;
				int minCentroidIndex = 0;
				int i = 0;
				foreach (var center in centroids)
				{
					currentDistance = CalculateDistance(point, center);
					if (currentDistance < minDistance)
					{
						minCentroidIndex = i;
						minDistance = currentDistance;
					}
					i++;
				}
				this.clusters[minCentroidIndex].Add(point);

			}
		}

		private double CalculateDistance(Point point1, Point point2)
		{
			var dist = (point1.x- point2.x) * (point1.x- point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
			return Math.Sqrt(dist);
		}

		private void CalculateNewCentroids()
		{
			int i = 0;
			foreach(var cluster in this.clusters)
			{
				double x = 0, y = 0;
				foreach (var item in cluster)
				{
					x += item.x;
					y += item.y;
				}
				if (cluster.Count > 0)
				{
					x /= cluster.Count;
					y /= cluster.Count;
				}
				else
				{
					x = this.centroids[i].x;
					y = this.centroids[i].y;
				}
				centroids[i] = new Point ( Math.Round(x,3), Math.Round(y, 3));

				i++;
			}
		}

		public void Execute()
		{
			double lastWCSS = 0;
			for (int i = 0; i < 100; i++)
			{

				this.InitCentroids();
				this.InitClusters();

				this.Iteration();
				this.CalculateWCSS();
				this.CalculateNewCentroids();
				if (lastWCSS == this.WCSS)
				{
					break;
				}
				lastWCSS = this.WCSS;
				

			}

		}

		public void RandonRestart()
		{
			double bestWCSS = double.MaxValue;
			IList<Point>[] resultClusters = new List<Point>[this.clustersNum];
			Point[] resultCentroids = new Point[this.clustersNum];
			for (int i = 0; i < 50; i++)
			{
				this.InitClusters();
				this.Execute();
				if(this.WCSS < bestWCSS)
				{
					bestWCSS = this.WCSS;
					this.Clone(resultClusters, resultCentroids);
				}
			}

			IList<Point>[] clustersWithCentroids = new List<Point>[2 * this.clustersNum];
			for (int i = 0, j = 0; i < this.clustersNum; i++)
			{
				clustersWithCentroids[j] = new List<Point>() { resultCentroids[i] };
				clustersWithCentroids[j + 1] = resultClusters[i];
				j += 2;
			}

			Console.WriteLine(bestWCSS);
			this.Write(clustersWithCentroids);
		}

		private void CalculateWCSS()
		{
			int i = 0;
			double sum = 0;
			foreach(var cluster in clusters)
			{
				foreach(var point in cluster)
				{
					var dist = CalculateDistance(point, centroids[i]);
					sum += dist * dist;
				}

				i++;
			}
			this.WCSS = sum;
		}

		private void Clone(IList<Point>[] resultClusters, Point[] resultCentroids)
		{
			int i = 0;
			
			foreach (var cluster in clusters)
			{
				resultClusters[i] = new List<Point>();
				foreach (var point in cluster)
				{
					resultClusters[i].Add(new Point(point.x, point.y));
				}

				i++;
			}

			
			i = 0;
			foreach (var point in this.centroids)
			{
				resultCentroids[i] = new Point(point.x, point.y);
				i++;
			}
		}

		private void Write(IList<Point>[] clustersWithCentroids)
		{
			var jsonString = JsonSerializer.Serialize(clustersWithCentroids);
			File.WriteAllText(@"C:\Users\ioan6\OneDrive\Desktop\II\kmeans-js\data.json", jsonString);
		}
	}
}
