using System;
using System.Collections.Generic;

namespace TestOptilio
{
    class Program
    {
        static List<Node> Bruteforce(List<Node> nodeList, List<Node> res, List<Node> bestRes)
        {
            // Jesli nie ma już indexów
            if (nodeList.Count == 0)
            {
                foreach (Node item in res)
                    Console.Write(item.Id); // Tutaj mamy ostateczny wynik
                Console.Write('\n');

                double resMinPath = 0;
                double bestResMinPath = 0;
                int index = res.Count;

                for (int i = 0; i < index - 1; i++)
                {
                    // Wyliczenie długości ścieżki obecnej i tej z bestRes
                    resMinPath += Math.Sqrt(Math.Pow(res[i].X - res[i + 1].X, 2) + Math.Pow(res[i].Y - res[i + 1].Y, 2));
                    bestResMinPath += Math.Sqrt(Math.Pow(bestRes[i].X - bestRes[i + 1].X, 2) + Math.Pow(bestRes[i].Y - bestRes[i + 1].Y, 2));
                }

                // pierwszy i ostatni
                resMinPath += Math.Sqrt(Math.Pow(res[0].X - res[index - 1].X, 2) + Math.Pow(res[0].Y - res[index - 1].Y, 2));
                bestResMinPath += Math.Sqrt(Math.Pow(bestRes[0].X - bestRes[index - 1].X, 2) + Math.Pow(bestRes[0].Y - bestRes[index - 1].Y, 2));

                if (bestResMinPath > resMinPath)
                    return res;
                else
                    return bestRes;
            }
            else
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    List<Node> newPart = new List<Node>();
                    newPart = nodeList.ConvertAll(item => new Node
                    {
                        Id = item.Id,
                        X = item.X,
                        Y = item.Y
                    });

                    res.Add(nodeList[i]); // dodawanie do rozwiązania obiektu
                    newPart.RemoveAt(i); // zbiór obiektów, których nie ma w rozwiązaniu

                    bestRes = Bruteforce(newPart, res, bestRes).ConvertAll(item => new Node
                    {
                        Id = item.Id,
                        X = item.X,
                        Y = item.Y
                    });

                    res.RemoveAt(res.Count - 1); // usunięcie tej opcji kolejności, którą sprawdziliśy i lecimy próbować kolejnej
                }
            }

            return bestRes;
        }

        static void Main(string[] args)
        {
            List<Node> nodeList = new List<Node>();
            string data;
            int index = 0;

            // --- IN --- //

            while ((data = Console.ReadLine()) != null)
            {
                index++;
                Node newNode = new Node();
                string[] dataSplit = data.Split(' ');

                newNode.Id = Convert.ToInt32(dataSplit[0]);
                newNode.X = Convert.ToDouble(dataSplit[1]);
                newNode.Y = Convert.ToDouble(dataSplit[2]);

                nodeList.Add(newNode);
            }


            // --- BRUTEFORCE --- //
            List<Node> res = new List<Node>();
            List<Node> bestRes = new List<Node>();

            bestRes = nodeList.ConvertAll(item => new Node
            {
                Id = item.Id,
                X = item.X,
                Y = item.Y
            });

            bestRes = Bruteforce(nodeList, res, bestRes).ConvertAll(item => new Node
            {
                Id = item.Id,
                X = item.X,
                Y = item.Y
            }); ;

            // --- OUT --- //
            double bestResMinPath = 0;
            for (int i = 0; i < index - 1; i++)
            {
                bestResMinPath += Math.Sqrt(Math.Pow(bestRes[i].X - bestRes[i + 1].X, 2) + Math.Pow(bestRes[i].Y - bestRes[i + 1].Y, 2));
            }

            // pierwszy i ostatni
            bestResMinPath += Math.Sqrt(Math.Pow(bestRes[0].X - bestRes[index - 1].X, 2) + Math.Pow(bestRes[0].Y - bestRes[index - 1].Y, 2));
            Console.Write("Wynik:\n");
            Console.Write(bestResMinPath + "\n");
            for (int i = 0; i < index; i++)
            {
                Console.Write(bestRes[i].Id);
            }
            Console.Read();
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
