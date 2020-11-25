using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DES
{
    public partial class Form1 : Form
    {

        private const int sizeOfBlock = 64; // размер принимаемого текста в битах
        private const int sizeOfChar = 8; // размер одного символа

        private readonly int[] firstArray = new int[64]
        {
            57, 49, 41, 33, 25, 17, 9 , 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
            56, 48, 40, 32, 24, 16, 8 , 0, 58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6

            //58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
            //62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
            //57, 49, 41, 33, 25, 17, 9 , 1, 59, 51, 43, 35, 27, 19, 11, 3,
            //61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7

        }; // массив начальной перестановки битов сообщения
        private readonly int[] secondArray = new int[56]
        {
            56, 48, 40, 32, 24, 16, 8 , 0 , 57, 49, 41, 33, 25, 17,
            9 , 1 , 58, 50, 42, 34, 26, 18, 10, 2 , 59, 51, 43, 35,
            62, 54, 46, 38, 30, 22, 14, 6 , 61, 53, 45, 37, 29, 21,
            13, 5 , 60, 52, 44, 36, 28, 20, 12, 4 , 27, 19, 11, 3 ,

            //57, 49, 41, 33, 25, 17, 9 , 1 , 58, 50, 42, 34, 26, 18,
            //10, 2 , 59, 51, 43, 35, 27, 19, 11, 3 , 60, 52, 44, 36,
            //63, 55, 47, 39, 31, 23, 15, 7 , 62, 54, 46, 38, 30, 22,
            //14, 6 , 61, 53, 45, 37, 29, 21, 13, 5 , 28, 20, 12, 4 ,

        }; // массив начальной перестановки битов ключа
        private readonly int[] thirdArray = new int[48]
        {
            13, 16, 10, 23, 0 , 4 , 2 , 27, 14, 5 , 20, 9 ,
            22, 18, 11, 3 , 25, 7 , 15, 6 , 26, 19, 12, 1 ,
            40, 51, 30, 36, 46, 54, 29, 39, 50, 44, 32, 47,
            43, 48, 38, 55, 33, 52, 45, 41, 49, 35, 28, 31,

            //14, 17, 11, 24, 1 , 5 , 3 , 28, 15, 6 , 21, 10,
            //23, 19, 12, 4 , 26, 8 , 16, 7 , 27, 20, 13, 2 ,
            //41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
            //44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32,
        };
        private readonly int[] fourthArray = new int[48]
        {
            31, 0 , 1 , 2 , 3 , 4 , 3 , 4 , 5 , 6 , 7 , 8 ,
            7 , 8 , 9 , 10, 11, 12, 11, 12, 13, 14, 15, 16,
            15, 16, 17, 18, 19, 20, 19, 20, 21, 22, 23, 24,
            23, 24, 25, 26, 27, 28, 27, 28, 29, 30, 31, 0 ,

            //32, 1 , 2 , 3 , 4 , 5 , 4 , 5 , 6 , 7 , 8 , 9 ,
            //8 , 9 , 10, 11, 12, 13, 12, 13, 14, 15, 16, 17,
            //16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25,
            //24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 ,

        };

        private readonly int[,] S_1 = new int[,]
        {
            {14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7 },
            { 0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8 },
            { 4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0 },
            {15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13 }
        };
        private readonly int[,] S_2 = new int[,]
{
            {15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
            { 3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5 },
            { 0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15 },
            {13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9 }
};
        private readonly int[,] S_3 = new int[,]
{
            {10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
            {13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1 },
            {13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7 },
            { 1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12 }
};
        private readonly int[,] S_4 = new int[,]
{
            { 7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
            {13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9 },
            {10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4 },
            { 3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14}
};
        private readonly int[,] S_5 = new int[,]
{
            { 2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
            {14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6 },
            { 4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14 },
            {11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5,  3 }
};
        private readonly int[,] S_6 = new int[,]
{
            {12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
            {10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8 },
            { 9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6 },
            { 4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13 }
};
        private readonly int[,] S_7 = new int[,]
{
            { 4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
            {13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6 },
            { 1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2 },
            { 6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12 }
};
        private readonly int[,] S_8 = new int[,]
{
            {13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 },
            { 1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2 },
            { 7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8 },
            { 2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11 }
};

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = Convert.ToString(textBox1.Text);
            string key = Convert.ToString(textBox2.Text);
            string result = "";

            CorrectStringLength(ref text);
            CorrectStringLength(ref key);

            text = ConvertBinary(ConvertASCII(text));
            key = ConvertBinary(ConvertASCII(key));

            PermutationArray(text, firstArray, out string resultText);
            PermutationArray(key, secondArray, out string resultKey);

            Division(resultText, out string divisionLeftText, out string divisionRightText);;
            Division(resultKey, out string divisionLeftKey, out string divisionRightKey);

            for (int i = 1; i < 2; i++)
            {
                int n = 0;
                string cnt_K = KeyBitShift(divisionLeftKey, i) + KeyBitShift(divisionRightKey, i);

                PermutationArray(cnt_K, thirdArray, out string result_K);
                PermutationArray(divisionRightText, fourthArray, out string result_R);

                text = XOR(result_K, result_R);
                for (int j = 1; j <= 8; j++)
                {
                    string line = Convert.ToString(text[n] + "" + text[n + 5]);
                    long l = Convert.ToInt64(line, 2);

                    string row = text.Substring(n + 1, 4);
                    long r = Convert.ToInt64(row, 2);
                    n += 6;

                    result += Switch(j, l, r);
                }
                textBox4.Text = result;

            }

        }

        private void CorrectStringLength(ref string text) // доводим текст до требуемой длины
        {
            while ((text.Length * sizeOfChar) % sizeOfBlock != 0)
                text += "*";
        }
        
        private int[] ConvertASCII(string text) // переводим символ в ASCII
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int[] array = new int[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                string symbol = Convert.ToString(text[i]);
                byte[] bytes = Encoding.GetEncoding(1251).GetBytes(symbol);
                array[i] = bytes[0];

            }
            return array;
        }
        private string ConvertBinary(int[] arrayASCII) // конвертируем строку в 2-ичный код
        {
            string result = "";

            for (int i = 0; i < arrayASCII.Length; i++)
            {
                string symbol_binary = Convert.ToString(arrayASCII[i], 2);

                while (symbol_binary.Length < sizeOfChar)
                    symbol_binary = "0" + symbol_binary;

                result += symbol_binary;
            }
            return result;
        }

        private void PermutationArray(string text, int[] array, out string result) // делаем 1-ую перестановку ключа по второму массиву
        {
            result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += text[array[i]];
            }
        }

        private void Division(string text, out string divisionLeft, out string divisionRight) // делим последовательность на 2 равные части
        {
            divisionLeft = text.Substring(0, text.Length / 2);
            divisionRight = text.Substring(text.Length / 2, text.Length / 2);
        }

        private string KeyBitShift(string text, int number)
        {
            string result = "";

            for (int i = 1; i <= number; i++)
            {
                if (i == 1 || i == 2 || i == 9 || i == 16)
                    result = text.Substring(1, text.Length - 1) + text.Substring(0, 1);
                else
                    result = text.Substring(2, text.Length - 2) + text.Substring(0, 2);
                text = result;
            }
            return result;
        } // сдвиг битов

        private string XOR(string one, string two)
        {
            string result = "";

            for (int i = 0; i < one.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(one[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(two[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }

        private string Switch(int number, long l, long r)
        {
            string result = "";
            switch(number)
            {
                case 1:
                    result = Convert.ToString(S_1[l, r] + " ");
                    break;
                case 2:
                    result = Convert.ToString(S_2[l, r] + " ");
                    break;
                case 3:
                    result = Convert.ToString(S_3[l, r] + " ");
                    break;
                case 4:
                    result = Convert.ToString(S_4[l, r] + " ");
                    break;
                case 5:
                    result = Convert.ToString(S_5[l, r] + " ");
                    break;
                case 6:
                    result = Convert.ToString(S_6[l, r] + " ");
                    break;
                case 7:
                    result = Convert.ToString(S_7[l, r] + " ");
                    break;
                case 8:
                    result = Convert.ToString(S_8[l, r] + " ");
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
