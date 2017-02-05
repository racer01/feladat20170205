using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feladat20170205
{
    class Draw
    {
        private byte[] numbers;

        public byte[] Numbers
        {
            get { return numbers; }
            set { numbers = value; }
        }

        public Draw(byte[] numbers)
        {
            this.numbers = numbers;
        }

        public Draw(string numbers) : this(Array.ConvertAll(numbers.Split(' '), s => byte.Parse(s)))
        { }

        public override string ToString()
        {
            return String.Join(" ", numbers);
        }

        public void SortNumbers()
        {
            Array.Sort(numbers);
        }

        public static Draw readNumbers(byte LOTTOTYPE)
        {
            byte[] drawNumbers = new byte[LOTTOTYPE];
            bool isValid;
            do
            {
                isValid = true;
                Console.Write(string.Format("Kérem írjon be {0} számot (szünettel elválasztva): ", LOTTOTYPE));
                string inputStr = Console.ReadLine();
                string[] inputArr = inputStr.Split(' ');
                if (inputArr.Length == LOTTOTYPE)
                {
                    for (int i = 0; i < LOTTOTYPE; i++)
                    {
                        bool isParsable = byte.TryParse(inputArr[i], out drawNumbers[i]);
                        isValid = isParsable && isValid;
                    }
                }
                else
                    isValid = false;
            } while (!isValid);
            return new Draw(drawNumbers);
        }

    }
}
