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

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = Convert.ToString(textBox1.Text);
            string key = Convert.ToString(textBox2.Text);
            string cnt;
            string cnt1 = "";

            CorrectStringLength(ref text);
            CorrectStringLength(ref key);

            text = ConvertBinary(ConvertASCII(text));
            key = ConvertBinary(ConvertASCII(key));

            FirstPermutation(text, out string resultText);
            SecondPermutation(key, out string resultKey);

            Division(resultText, out string divisionLeftText, out string divisionRightText);
            Division(resultKey, out string divisionLeftKey, out string divisionRightKey);

            for (int i = 1; i <= 16; i++)
            {
                KeyBitShift(divisionLeftKey, i, out string resultLeft);
                KeyBitShift(divisionRightKey, i, out string resultRight);
                cnt = resultLeft + resultRight;
                for (int j = 0; j < thirdArray.Length; j++)
                {
                    cnt1 += cnt[thirdArray[i]];
                }

            }
            textBox3.Text = cnt1;



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

        private void FirstPermutation(string text, out string result) // делаем 1-ую перестановку текста по первому массиву
        {
            result = "";
            for (int i = 0; i < text.Length; i++)
            {
                result += text[firstArray[i]];
            }
        }

        private void SecondPermutation(string key, out string result) // делаем 1-ую перестановку ключа по второму массиву
        {
            result = "";
            for (int i = 0; i < secondArray.Length; i++)
            {
                result += key[secondArray[i]];
            }
        }

        private void Division(string text, out string divisionLeft, out string divisionRight) // делим последовательность на 2 равные части
        {
            divisionLeft = text.Substring(0, text.Length / 2);
            divisionRight = text.Substring(text.Length / 2, text.Length / 2);
        }

        private void KeyBitShift(string text, int number, out string result)
        {
            result = "";

            for (int i = 1; i <= number; i++)
            {
                if (i == 1 || i == 2 || i == 9 || i == 16)
                    result = text.Substring(1, text.Length - 1) + text.Substring(0, 1);
                else
                    result = text.Substring(2, text.Length - 2) + text.Substring(0, 2);
                text = result;
            }
        }

    }
}
