using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feladat20170205
{
    class Program
    {
        static void Main(string[] args)
        {
            const string INPUTFILENAME = "lottosz.dat";
            const string OUTPUTFILENAME = "lotto52.ki";
            const byte LOTTOTYPE = 5;
            const byte DATALENGTH = 51;

            Draw[] year2003 = readFile(INPUTFILENAME, DATALENGTH);

            // 1st task - input 52th draw
            Draw inputDraw = Draw.readNumbers(LOTTOTYPE);

            // 2nd task - sort input 52th draw & print
            inputDraw.SortNumbers();
            Console.WriteLine(inputDraw);

            // 3rd task - input int 1..51
            byte weekNumber = 0;
            bool valid;
            do
            {
                Console.Write(string.Format("Kérem adjon meg egy számot 1 és {0} között: ", DATALENGTH));
                string input = Console.ReadLine();
                valid = byte.TryParse(input, out weekNumber);
            } while (!(valid && 1 <= weekNumber && weekNumber <= DATALENGTH));

            // 4th task - print nth week's number
            Console.WriteLine(string.Format("A {0}. hét számai növekvő sorrendben: {1}", weekNumber, year2003[weekNumber - 1]));

            // 5th task - which number haven't been drawn
            Console.Write("Van-e olyan szám, amit nem húztak ki 2003-ban: ");
            List<byte> notDrawn = notDrawnNumbers(year2003, LOTTOTYPE);
            if (notDrawn.Count > 0)
                Console.WriteLine(string.Format("Van: {0}" , string.Join(", ", notDrawn)));
            else
                Console.WriteLine("Nincs");

            // 6th task - count odd numbers & print
            Console.WriteLine(string.Format("2003-ban {0}db páratlan számot húztak ki.", countOdd(year2003)));

            // 7th task - add 52th week's numbers and write everything into a file
            Draw[] year2003full = new Draw[DATALENGTH + 1];
            Array.Copy(year2003, year2003full, DATALENGTH);
            year2003full[year2003full.Length - 1] = inputDraw;
            writeFile(OUTPUTFILENAME, year2003full);

            // 8th task - print in fancy format (9x10) which number happened how many times
            byte[] timesDrawn = countEachNumber(year2003full);
            printFancyFrequency(timesDrawn, 10, 9);

            // 9th task - print not drawn prime numbers
            byte[] primes = new byte[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89 };
            var common = notDrawn.Intersect(primes);
            Console.WriteLine(string.Format("Ki nem húzott prímszámok: {0}", string.Join(", ", common)));


            Console.ReadKey();
        }

        static Draw[] readFile(string FILENAME, byte length)
        {
            Draw[] allDraw = new Draw[length];
            using (StreamReader sr = new StreamReader(FILENAME))
            {
                for (int i = 0; i < allDraw.Length; i++)
                {
                    string s = sr.ReadLine();
                    allDraw[i] = new Draw(s);
                }
            }
            return allDraw;
        }

        static void writeFile(string FILENAME, Draw[] allDraw)
        {
            using (StreamWriter sw = new StreamWriter(FILENAME))
            {
                foreach (Draw draw in allDraw)
                    sw.WriteLine(draw);
            }
        }

        static List<byte> notDrawnNumbers(Draw[] allDraw, byte LOTTOTYPE)
        {
            List<byte> numberList = new List<byte>();
            for (byte i = 0; i < allDraw.Length; i++)
                numberList.Add(i);
            for (int week = 0; week < allDraw.Length; week++)
            {
                for (int number = 0; number < LOTTOTYPE; number++)
                {
                    numberList.Remove(allDraw[week].Numbers[number]);
                }
            }
            return numberList;
        }

        static int countOdd(Draw[] allDraw)
        {
            int count = 0;
            foreach (Draw draw in allDraw)
            {
                foreach (byte number in draw.Numbers)
                {
                    count += number % 2 == 0 ? 0 : 1;
                }
            }
            return count;
        }

        static byte[] countEachNumber(Draw[] allDrawn)
        {
            byte[] timesDrawn = new byte[90];
            for (int i = 0; i < timesDrawn.Length; i++)
                timesDrawn[i] = 0;
            foreach (Draw draw in allDrawn)
            {
                foreach (byte n in draw.Numbers)
                {
                    timesDrawn[n - 1]++;
                }
            }
            return timesDrawn;
        }

        static void printFancyFrequency(byte[] data, byte col, byte row)
        {
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    int n = (r * col) + c;
                    Console.Write(data[n] + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
