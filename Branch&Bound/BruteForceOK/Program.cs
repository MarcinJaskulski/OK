using System;
using System.Collections.Generic;

namespace TestOptilio
{
    class Program
    {
        static List<Node> Bruteforce(List<Node> nodeList, List<Node> res, List<Node> bestRes, double lowerBound, ref double upperBound, ref DateTime endTime)
        {
            // Jesli nie ma już indexów
            if (nodeList.Count == 0)
            {

                if (lowerBound < upperBound)
                {
                    upperBound = lowerBound;
                    return res;
                }    
                else
                    return bestRes;
            }
            else
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    int lowerIndex = res.Count;
                    // usuniecie ostatniego polaczenia nawracającego 
                    if (lowerIndex > 0)
                        lowerBound -= Math.Sqrt(Math.Pow(res[0].X - res[lowerIndex - 1].X, 2) + Math.Pow(res[0].Y - res[lowerIndex - 1].Y, 2));

                    List<Node> newPart = new List<Node>();
                    newPart = nodeList.ConvertAll(item => new Node
                    {
                        Id = item.Id,
                        X = item.X,
                        Y = item.Y
                    });

                    res.Add(nodeList[i]); // dodawanie do rozwiązania obiektu
                    newPart.RemoveAt(i); // zbiór obiektów, których nie ma w rozwiązaniu
                    lowerIndex++; // muszę dodać do kolejnych operacji, bo przecież res się zwiększył

                    // dodanie nowego polaczenia
                    if (lowerIndex > 1) // muszą być minimum 2 połączniea
                        lowerBound += Math.Sqrt(Math.Pow(res[lowerIndex - 2].X - res[lowerIndex - 1].X, 2) + Math.Pow(res[lowerIndex - 2].Y - res[lowerIndex - 1].Y, 2));
                    // dodanie nowego polaczenia nawracajacego
                    if (lowerIndex > 0)
                        lowerBound += Math.Sqrt(Math.Pow(res[0].X - res[lowerIndex - 1].X, 2) + Math.Pow(res[0].Y - res[lowerIndex - 1].Y, 2));

                    // --- odciecie ---//
                    if (lowerBound >= upperBound)
                    {
                        res.RemoveAt(res.Count - 1);
                        return bestRes;
                    }
                    // --- koniec odciecia ---/

                    bestRes = Bruteforce(newPart, res, bestRes, lowerBound, ref upperBound, ref endTime).ConvertAll(item => new Node
                    {
                        Id = item.Id,
                        X = item.X,
                        Y = item.Y
                    });

                    res.RemoveAt(res.Count - 1); // usunięcie tej opcji kolejności, którą sprawdziliśy i lecimy próbować kolejnej

                    // Odcięcie
                    DateTime nowTime = DateTime.Now;
                    if (DateTime.Compare(endTime, nowTime) < 0)
                        break;
                }
            }

            return bestRes;
        }

        static void Main(string[] args)
        {
            List<Node> nodeList = new List<Node>();
            string data;
            int index = 0;
            DateTime endTime = DateTime.Now;
            endTime = endTime.AddSeconds(60);

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
            double lowerBound = 0;
            double upperBound = 0;

            bestRes = nodeList.ConvertAll(item => new Node
            {
                Id = item.Id,
                X = item.X,
                Y = item.Y
            });

            // Wyliczenie długości ścieżki obecnej
            for (int i = 0; i < index - 1; i++)
            {
                upperBound += Math.Sqrt(Math.Pow(bestRes[i].X - bestRes[i + 1].X, 2) + Math.Pow(bestRes[i].Y - bestRes[i + 1].Y, 2));
            }
            upperBound += Math.Sqrt(Math.Pow(bestRes[0].X - bestRes[index - 1].X, 2) + Math.Pow(bestRes[0].Y - bestRes[index - 1].Y, 2));

              //  ---Rekurencja-- - //
              bestRes = Bruteforce(nodeList, res, bestRes, lowerBound, ref upperBound, ref endTime).ConvertAll(item => new Node
              {
                  Id = item.Id,
                  X = item.X,
                  Y = item.Y
              }); ;

            // --- OUT --- //
            //double bestResMinPath = 0;
            //for (int i = 0; i < index - 1; i++)
            //{
            //    bestResMinPath += Math.Sqrt(Math.Pow(bestRes[i].X - bestRes[i + 1].X, 2) + Math.Pow(bestRes[i].Y - bestRes[i + 1].Y, 2));
            //}

            // pierwszy i ostatni
            //bestResMinPath += Math.Sqrt(Math.Pow(bestRes[0].X - bestRes[index - 1].X, 2) + Math.Pow(bestRes[0].Y - bestRes[index - 1].Y, 2));
            //Console.Write("Wynik:\n");
            // Console.Write(bestResMinPath + "\n");

            for (int i = 0; i < index; i++)
            {
                Console.WriteLine(bestRes[i].Id);
            }
            //Console.Read();
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
