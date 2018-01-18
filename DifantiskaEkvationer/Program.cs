using System;
using System.Collections.Generic;
using System.Linq;

namespace DifantiskaEkvationer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out int a) || !int.TryParse(args[1], out int b))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            int[] koefficienter = GetFactors(a, b);
            Console.WriteLine($"Lösningen är {a} * {koefficienter[0]} + {b} * {koefficienter[1]} = 1");
            Console.ReadLine();
        }

        static int[] GetFactors(int a, int b)
        {
            //Sparar värden från ekvationen för att lösa ekvationen senare och stoppar då rest = 0
            //ex: givet ekvationen 8x + 5y = 1 är eukledes algoritm:
            //8 = 5 * 1 + 3 alternativt 8 ≡ 3 (mod 5) sparar 8 och 3
            //5 = 3 * 1 + 2 alternativt 5 ≡ 3 (mod 3) sparar 5 och 3
            //3 = 2 * 1 + 1 alternativt 3 ≡ 1 (mod 2) sparar 3 och 1
            //2 = 1 * 2 + 0 alternativt 2 ≡ 0 (mod 1) sparar inget då det inte är någon rest
            Dictionary<int, int> ekvationsdata = new Dictionary<int, int>();
            if (a < b)
            {
                throw new ArgumentException("a måste vara större än b");
            }
            //behöver också spara sista värdet för b (1 givet 8x + 5y = 1)
            int sistaBtal;
            while (true)
            {
                int mod = a % b;
                ekvationsdata.Add(a, a / b);
                if (b % mod == 0)
                {
                    sistaBtal = b;
                    break;
                }
                a = b;
                b = mod;
            }
            //aM och bM står för a/b som multipeltal
            MultTal aM = new MultTal(ekvationsdata.ElementAt(ekvationsdata.Count - 1).Key, 1);
            MultTal bM = new MultTal(sistaBtal, -ekvationsdata[aM.X]);
            for (int i = ekvationsdata.Count - 2; i >= 0; i--)
            {
                int amTemp = aM.M;
                int bmTemp = bM.M;
                aM.M = bM.M;
                bM.M = (bmTemp * (-1 * ekvationsdata[aM.X])) + amTemp;
                bM.X = aM.X;
                aM.X = ekvationsdata.ElementAt(i).Key;
            }
            return new int[2] { aM.M, bM.M };
        }
    }

    /// <summary>
    /// Ett tal beskrivet som a * b eller x * multipel
    /// </summary>
    struct MultTal
    {
        int x;
        int multipel;

        public MultTal(int x, int multipel)
        {
            this.x = x;
            this.multipel = multipel;
        }

        public int X { get => this.x; set => this.x = value; }
        public int M { get => this.multipel; set => this.multipel = value; }
    }
}