using System;
using System.Collections.Generic;

namespace LocalSearch
{
    class Program
    {
        static List<Node> DownloadData()
        {
            List<Node> nodeList = new List<Node>();
            string data;

            while ((data = Console.ReadLine()) != null)
            {
                Node newNode = new Node();
                string[] dataSplit = data.Split(' ');

                newNode.Id = Convert.ToInt32(dataSplit[0]);
                newNode.X = Convert.ToDouble(dataSplit[1]);
                newNode.Y = Convert.ToDouble(dataSplit[2]);

                nodeList.Add(newNode);
            }

            return nodeList;
        }

        static double NodeListLength(ref List<Node> nodeList)
        {
            double length = 0;
            for (int i = 0; i < nodeList.Count - 1; i++)
            {
                length += Math.Sqrt(Math.Pow(nodeList[i].X - nodeList[i + 1].X, 2) + Math.Pow(nodeList[i].Y - nodeList[i + 1].Y, 2));
            }

            return length;
        }

        static void Main(string[] args)
        {
            List<Node> nodeList = new List<Node>();
            DateTime endTime = DateTime.Now;
            endTime = endTime.AddSeconds(60);

            // --- IN --- //
            nodeList = DownloadData();

            // --- LOCAL SEARCH --- //
            int x1, x2;
            Random rand = new Random();

            double firstLen = NodeListLength(ref nodeList);

            while (true)
            {
                x1 = rand.Next(0, nodeList.Count);
                x2 = rand.Next(0, nodeList.Count);
                if (x1 == x2)
                    continue;
                Node buf = nodeList[x1];
                nodeList[x1] = nodeList[x2];
                nodeList[x2] = buf;

                double secondLen = NodeListLength(ref nodeList);

                if (secondLen > firstLen)
                {
                    nodeList[x2] = nodeList[x1];
                    nodeList[x1] = buf;
                }
                else
                    firstLen = secondLen;

                DateTime nowTime = DateTime.Now;
                if (DateTime.Compare(endTime, nowTime) < 0)
                    break;
            }
            
            // --- OUT --- //
            for (int i = 0; i < nodeList.Count; i++)
            {
                Console.WriteLine(nodeList[i].Id);
            }
            //Console.Read();
        }

        public class Node
        {
            public int Id { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
