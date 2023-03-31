using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace KASUMI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[,] fullAscii = new char[,] {
            { 'α', 'β', 'γ', 'δ', 'ε', 'ϝ', 'ϛ', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ' }, // 0
            { 'ο', 'π', 'ϟ', 'ρ', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω', 'ϡ', 'Δ', 'Θ', 'Ξ', 'Ξ' }, // 1
            { 'Ψ', '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/' }, // 2
            { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '&' }, // 3
            { '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' }, // 4
            { 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_' }, // 5
            { '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o' }, // 6
            { 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', 'Ω' }, // 7
            { 'Ђ', 'Ѓ', '‚', 'ѓ', '„', '…', '†', '‡', '€', '‰', 'Љ', '‹' , 'Њ', 'Ќ', 'Ћ', 'Џ' }, // 8
            { 'ђ', '‘', '’', '“', '”', '•', '–', '—', ' ', '™', 'љ', '›', 'њ', 'ќ', 'ћ', 'џ' }, // 9
            { ' ', 'Ў', 'ў', 'Ј', '¤', 'Ґ', '¦', '§', 'Ё', '©', 'Є', '«', '¬', ' ', '®', 'Ї' }, // A
            { '°', '±', 'І', 'і', 'ґ', 'µ', '¶', '·', 'ё', '№', 'є', '»', 'ј', 'Ѕ', 'ѕ', 'ї' }, // B
            { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П' }, // C
            { 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' }, // D
            { 'а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п' }, // E
            { 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' }}; // F

        string[] 
            key = new string[16], keyHex = new string[16], 
            KL1 = new string[8], KL2 = new string[8], 
            KO1 = new string[8], KO2 = new string[8], KO3 = new string[8], 
            KI1 = new string[8], KI2 = new string[8], KI3 = new string[8];
        bool encryptionMode = true, decryptionMode = false;
        string inputDataFormat = "ascii", outputDataFormat = "ascii";

        public MainWindow()
        {
            InitializeComponent();
            modeCheck(encryption, decryption, encryptionMode, decryptionMode);
        }
        public void openFile(object sender, RoutedEventArgs e)
        {
            RichTextBox richTextBox = new RichTextBox();
            if (encryptionMode == true && decryptionMode == false)
                richTextBox = inputPlainData;
            else if (encryptionMode == false && decryptionMode == true)
                richTextBox = inputCipherData;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                richTextBox.Document.Blocks.Clear();
                string content = File.ReadAllText(openFileDialog.FileName);
                richTextBox.AppendText(content);
            }
        }
        public void exportFile(object sender, RoutedEventArgs e)
        {
            RichTextBox richTextBox = new RichTextBox();
            if (encryptionMode == true && decryptionMode == false)
                richTextBox = outputCipherData;
            else if (encryptionMode == false && decryptionMode == true)
                richTextBox = outputPlainData;
            TextRange tr = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string
                resultContent = tr.Text,
                output = "";
            if (resultContent.Length != 2)
            {
                for (int i = 0; i < resultContent.Length - 2; i++)
                    output += Convert.ToString(resultContent[i]);

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "result";
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filename = saveFileDialog.FileName;
                    using (StreamWriter writetext = new StreamWriter(filename))
                        writetext.WriteLine(output);
                }
            }
            else
                message("Ошибка: отсутствуют данные для экспорта.");
        }
        public void decrypt_Click(object sender, RoutedEventArgs e)
        {
            calculations("decryption");
        }
            
        public void encrypt_Click(object sender, RoutedEventArgs e)
        {
           calculations("encryption");
        }
        public void calculations(string mode)
        {
            RichTextBox
                inputRichTextBox = new RichTextBox(),
                outputRichTextBox = new RichTextBox();
            if (mode == "encryption")
            {
                inputRichTextBox = inputPlainData;
                outputRichTextBox = outputCipherData;
            }
            else if (mode == "decryption")
            {
                inputRichTextBox = inputCipherData;
                outputRichTextBox = outputPlainData;
            }
            outputRichTextBox.Document.Blocks.Clear();
            if (keyChecker(keyBox) && inputChecker(inputRichTextBox)) //проверка ввода ключа и текста
            {
                int extraChars = 0;
                keysGenerator();
                TextRange tr = new TextRange(inputRichTextBox.Document.ContentStart, inputRichTextBox.Document.ContentEnd);
                string
                    content = tr.Text,
                    textContent = "",
                    hexContent = "",
                    result = "",
                    L,
                    R;
                if ((mode == "decryption" && inputCipherDataFormatAscii.IsChecked == true) || (mode == "encryption" && inputPlainDataFormatAscii.IsChecked == true))
                {
                    for (int i = 0; i < content.Length - 2; i++)
                        textContent += Convert.ToString(content[i]);
                    string[] contentTextToHex = textToHex(textContent);
                    foreach (string element in contentTextToHex)
                        hexContent += element;
                }
                else if ((mode == "decryption" && inputCipherDataFormatHex.IsChecked == true) || (mode == "encryption" && inputPlainDataFormatHex.IsChecked == true))
                {
                    for (int i = 0; i < content.Length - 2; i++)
                        hexContent += Convert.ToString(content[i]);
                }
                while (hexContent.Length % 16 != 0)
                {
                    hexContent += "a0";
                    extraChars += 2;
                }
                string[] inputHex = new string[hexContent.Length / 2];
                int ct = 0;
                for (int i = 0; i < hexContent.Length; i += 2)
                {
                    inputHex[ct] = Convert.ToString(hexContent[i]) + Convert.ToString(hexContent[i + 1]);
                    ct++;
                }
                for (int blockIndex = 0; blockIndex < hexContent.Length / 16; blockIndex++)
                {
                    string[] dataBlock = new string[8];
                    int startIndex = 8 * blockIndex, endIndex = 8 * (blockIndex + 1), blockElementIndex = 0;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        dataBlock[blockElementIndex] = inputHex[i];
                        blockElementIndex++;
                    }
                    L = "";
                    R = "";
                    for (int i = 0; i < 4; i++)
                    {
                        string textL = dataBlock[i], textR = dataBlock[i + 4];
                        if (textL.Length < 2)
                            textL = "0" + textL;
                        if (textR.Length < 2)
                            textR = "0" + textR;
                        L += Convert.ToString(textL[0]) + Convert.ToString(textL[1]);
                        R += Convert.ToString(textR[0]) + Convert.ToString(textR[1]);
                    }
                    if (mode == "encryption")
                        for (int p = 0; p <= 6; p += 2)
                        {
                            R = oddRound(L, R, KL1[p], KL2[p], KO1[p], KO2[p], KO3[p], KI1[p], KI2[p], KI3[p]);
                            L = evenRound(L, R, KL1[p + 1], KL2[p + 1], KO1[p + 1], KO2[p + 1], KO3[p + 1], KI1[p + 1], KI2[p + 1], KI3[p + 1]);
                        }
                    else if (mode == "decryption")
                        for (int p = 6; p >= 0; p -= 2)
                        {
                            L = evenRound(L, R, KL1[p + 1], KL2[p + 1], KO1[p + 1], KO2[p + 1], KO3[p + 1], KI1[p + 1], KI2[p + 1], KI3[p + 1]);
                            R = oddRound(L, R, KL1[p], KL2[p], KO1[p], KO2[p], KO3[p], KI1[p], KI2[p], KI3[p]);
                        }
                    result += L + R;
                }
                if ((mode == "encryption" && outputCipherDataFormatAscii.IsChecked == true) || (mode == "decryption" && outputPlainDataFormatAscii.IsChecked == true))
                    outputRichTextBox.AppendText(hexToText(result));
                else if ((mode == "encryption" && outputCipherDataFormatHex.IsChecked == true) || (mode == "decryption" && outputPlainDataFormatHex.IsChecked == true))
                    outputRichTextBox.AppendText(result);
            }
        }
        // генерация основного ключа
        public void generate_Click(object sender, RoutedEventArgs e)
        {
            keyBox.Clear();
            Random random = new Random();
            for(int p = 0; p < 16; p++)
                keyBox.Text += fullAscii[random.Next(0, 16), random.Next(0, 16)];
        }
        // генерация раундовых ключей
        public void keysGenerator()
        {
            string[] inputKey = textToHex(keyBox.Text);
            for (int i = 0; i < 8; i++)
                keyHex[i] = inputKey[2 * i] + inputKey[2 * i + 1];
            string[] 
                c = new string[] { "0123", "4567", "89AB", "CDEF", "FEDC", "BA98", "7654", "3210" },
                K = new string[8],
                KShtrih = new string[8];
            for (int u = 0; u < 8; u++) 
            {
                K[u] = keyHex[u];
                KShtrih[u] = intToHex(hexToInt(K[u]) ^ hexToInt(c[u]), 4);
            }
            KL2 = rolArr(KShtrih, 2);
            KO1 = rolArr(K, 1);
            KO2 = rolArr(K, 5);
            KO3 = rolArr(K, 6);
            for (int i = 0; i < 8; i++)
            {
                KL1[i] = rol(K[i], 1);
                KO1[i] = rol(KO1[i], 5);
                KO2[i] = rol(KO2[i], 8);
                KO3[i] = rol(KO3[i], 13);
            }
            KI1 = rolArr(KShtrih, 4);
            KI2 = rolArr(KShtrih, 3);
            KI3 = rolArr(KShtrih, 7);
        }
        // метод нечетного раунда
        public string oddRound(string L, string R, string KL1, string KL2, string KO1, string KO2, string KO3, string KI1, string KI2, string KI3)
        {
            string L2 = FL(L, KL1, KL2);
            L2 = FO(L2, KO1, KO2, KO3, KI1, KI2, KI3);
            R = intToHex(hexToInt(R) ^ hexToInt(L2), 8);
            return R;
        }
        // метод четного раунда
        public string evenRound(string L, string R, string KL1, string KL2, string KO1, string KO2, string KO3, string KI1, string KI2, string KI3)
        {
            string R2 = FO(R, KO1, KO2, KO3, KI1, KI2, KI3);
            R2 = FL(R2, KL1, KL2);
            L = intToHex(hexToInt(L) ^ hexToInt(R2), 8);
            return L;
        }
        // операция FL
        public string FL(string input, string KL1, string KL2)
        {
            // вход: 32 бита текста (массив из 4 элементов); ключ: 16 бит в шестнадцатеричной (2 строки)
            string 
                L = Convert.ToString(input[0]) + Convert.ToString(input[1]) + Convert.ToString(input[2]) + Convert.ToString(input[3]),
                R = Convert.ToString(input[4]) + Convert.ToString(input[5]) + Convert.ToString(input[6]) + Convert.ToString(input[7]);
            string L2 = intToHex(hexToInt(L) & hexToInt(KL1), 4);
            L2 = rol(L2, 1); 
            R = intToHex(hexToInt(R) ^ hexToInt(L2), 4);
            string R2 = intToHex(hexToInt(R) | hexToInt(KL2), 4);
            L = intToHex(hexToInt(L) ^ hexToInt(R2), 4);
            return L + R;
        }
        // операция FI
        public string FI(string input, string KI)
        {
            // вход: 16 бит текста (2 символа); ключ: 16 бит в шестнадцатеричной (2 символа)
            string inputBinary = hexToBinary(input), keyBinary = hexToBinary(KI); 
            if(inputBinary.Length < 16)
                while(inputBinary.Length != 16)
                    inputBinary = "0" + inputBinary;

            string[] inputBinaryBits = new string[16], keyBinaryBits = new string[16];
            for (int i = 0; i < 16; i++)
            {
                inputBinaryBits[i] = Convert.ToString(inputBinary[i]);
                keyBinaryBits[i] = Convert.ToString(keyBinary[i]);
            }
            // разделение данных и ключа на 9-битные и 7-битные части
            string leftBinary = "", rightBinary = "", leftKey = "", rightKey = "";
            int y = 9;
            for (int i = 0; i < 9; i++)
            {
                leftBinary += inputBinaryBits[i];
                leftKey += keyBinaryBits[i];
                if (i > 1)
                {
                    rightBinary += inputBinaryBits[y];
                    rightKey += keyBinaryBits[y];
                    y++;
                }
            }
            int L = binaryToInt(leftBinary), R = binaryToInt(rightBinary);
            int K1 = binaryToInt(leftKey), K2 = binaryToInt(rightKey);
            L = S9(ZE(L));
            L = ZE(L) ^ TR(R); 
            R = S7(TR(R)); 
            R = TR(R) ^ ZE(L); 
            L = ZE(L) ^ K1; 
            R = TR(R) ^ K2; 
            L = S9(ZE(L));
            L = ZE(L) ^ TR(R); 
            R = S7(TR(R));  
            R = TR(R) ^ ZE(L); 
            string outR = intToBinary(TR(R)), outL = intToBinary(ZE(L));
            if (outL.Length < 9)
                while(outL.Length != 9)
                    outL = "0" + outL;
            string resultString = outR + outL;
            outR = ""; outL = "";
            for(int i = 1; i < 9; i++) 
            {
                outR += Convert.ToString(resultString[i]);
                if (i > 1)
                    outL += Convert.ToString(resultString[i + 7]);
            }
            outL += Convert.ToString(resultString[16]);
            outR = binaryToHex(outR);
            outL = binaryToHex(outL);
            while (outR.Length != 2)
                outR = "0" + outR;
            while (outL.Length != 2)
                outL = "0" + outL;
            return outR + outL;
        }
        // операция FO
        public string FO(string input, string KO1, string KO2, string KO3, string KI1, string KI2, string KI3)
        {
            // вход: 32 бит текста (4 символа); ключ: 3 по 16 бит в шестнадцатеричной (по 2 символа)
            string 
                L = Convert.ToString(input[0]) + Convert.ToString(input[1]) + Convert.ToString(input[2]) + Convert.ToString(input[3]),
                R = Convert.ToString(input[4]) + Convert.ToString(input[5]) + Convert.ToString(input[6]) + Convert.ToString(input[7]);
            L = intToHex(hexToInt(L) ^ hexToInt(KO1), 4);
            L = FI(L, KI1);
            L = intToHex(hexToInt(L) ^ hexToInt(R), 4);
            R = intToHex(hexToInt(R) ^ hexToInt(KO2), 4);
            R = FI(R, KI2);
            R = intToHex(hexToInt(R) ^ hexToInt(L), 4);
            L = intToHex(hexToInt(L) ^ hexToInt(KO3), 4);
            L = FI(L, KI3);
            L = intToHex(hexToInt(L) ^ hexToInt(R), 4);
            while (L.Length != 4)
                L = "0" + L;
            while (R.Length != 4)
                R = "0" + R;
            return R + L;
        }
        public int S7(int value)
        {
            int[] S7 = {
                 54, 50, 62, 56, 22, 34, 94, 96, 38,  6, 63, 93,  2, 18,123, 33,
                 55,113, 39,114, 21, 67, 65, 12, 47, 73, 46, 27, 25,111,124, 81,
                 53,  9,121, 79, 52, 60, 58, 48,101,127, 40,120,104, 70, 71, 43,
                 20,122, 72, 61, 23,109, 13,100, 77,  1, 16,  7, 82, 10,105, 98,
                117,116, 76, 11, 89,106,  0,125,118, 99, 86, 69, 30, 57,126, 87,
                112, 51, 17,  5, 95, 14, 90, 84, 91,  8, 35,103, 32, 97, 28, 66,
                102, 31, 26, 45, 75,  4, 85, 92, 37, 74, 80, 49, 68, 29,115, 44,
                 64,107,108, 24,110, 83, 36, 78, 42, 19, 15, 41, 88,119, 59,  3
            };
            return S7[value];
        }
        public int S9(int value)
        {
            int[] S9 = {
                167,239,161,379,391,334,  9,338, 38,226, 48,358,452,385, 90,397,
                183,253,147,331,415,340, 51,362,306,500,262, 82,216,159,356,177,
                175,241,489, 37,206, 17,  0,333, 44,254,378, 58,143,220, 81,400,
                 95,  3,315,245, 54,235,218,405,472,264,172,494,371,290,399, 76,
                165,197,395,121,257,480,423,212,240, 28,462,176,406,507,288,223,
                501,407,249,265, 89,186,221,428,164, 74,440,196,458,421,350,163,
                232,158,134,354, 13,250,491,142,191, 69,193,425,152,227,366,135,
                344,300,276,242,437,320,113,278, 11,243, 87,317, 36, 93,496, 27,
                487,446,482, 41, 68,156,457,131,326,403,339, 20, 39,115,442,124,
                475,384,508, 53,112,170,479,151,126,169, 73,268,279,321,168,364,
                363,292, 46,499,393,327,324, 24,456,267,157,460,488,426,309,229,
                439,506,208,271,349,401,434,236, 16,209,359, 52, 56,120,199,277,
                465,416,252,287,246,  6, 83,305,420,345,153,502, 65, 61,244,282,
                173,222,418, 67,386,368,261,101,476,291,195,430, 49, 79,166,330,
                280,383,373,128,382,408,155,495,367,388,274,107,459,417, 62,454,
                132,225,203,316,234, 14,301, 91,503,286,424,211,347,307,140,374,
                 35,103,125,427, 19,214,453,146,498,314,444,230,256,329,198,285,
                 50,116, 78,410, 10,205,510,171,231, 45,139,467, 29, 86,505, 32,
                 72, 26,342,150,313,490,431,238,411,325,149,473, 40,119,174,355,
                185,233,389, 71,448,273,372, 55,110,178,322, 12,469,392,369,190,
                  1,109,375,137,181, 88, 75,308,260,484, 98,272,370,275,412,111,
                336,318,  4,504,492,259,304, 77,337,435, 21,357,303,332,483, 18,
                 47, 85, 25,497,474,289,100,269,296,478,270,106, 31,104,433, 84,
                414,486,394, 96, 99,154,511,148,413,361,409,255,162,215,302,201,
                266,351,343,144,441,365,108,298,251, 34,182,509,138,210,335,133,
                311,352,328,141,396,346,123,319,450,281,429,228,443,481, 92,404,
                485,422,248,297, 23,213,130,466, 22,217,283, 70,294,360,419,127,
                312,377,  7,468,194,  2,117,295,463,258,224,447,247,187, 80,398,
                284,353,105,390,299,471,470,184, 57,200,348, 63,204,188, 33,451,
                 97, 30,310,219, 94,160,129,493, 64,179,263,102,189,207,114,402,
                438,477,387,122,192, 42,381,  5,145,118,180,449,293,323,136,380,
                 43, 66, 60,455,341,445,202,432,  8,237, 15,376,436,464, 59,461
            };
            return S9[value];
        }
        // 9 бит в 7 бит
        public int TR(int value)
        {
            string binaryValue = intToBinary(value);
            int k = binaryValue.Length - 1;
            string result = "";
            while(result.Length != 7)
            { 
                result = Convert.ToString(binaryValue[k]) + result;
                k--;
            }
            return binaryToInt(result);
        }
        // 7 бит в 9 бит
        public int ZE(int value)
        {
            string binaryValue = intToBinary(value);
            string[] binaryFragments = new string[8];
            for (int i = 0; i < 8; i++)
                binaryFragments[i] = Convert.ToString(binaryValue[i]);
            if (binaryFragments.Length == 9)
                return value;
            else
            {
                string result = binaryValue;
                while(result.Length != 9)
                    result = "0" + result;
                return binaryToInt(result);
            }
        }

        //метод перевода из десятичной в шестнадцатеричную
        public string intToHex(int value, int length)
        {
            string result = Convert.ToString(value, 16);
            while (result.Length != length)
                result = "0" + result;
            return result;
        }
        //метод перевода из шестнадцатеричную в двоичную
        public string hexToBinary(string value)
        {
            int newValue = hexToInt(value);
            string result = Convert.ToString(newValue, 2);
            while (result.Length != 8 || result.Length != 16) 
            {
                if (result.Length == 8 || result.Length == 16)
                    break;
                result = "0" + result;
            }
            return result;
        }
        //метод перевода из десятичной в двоичную
        public string intToBinary(int value)
        {
            string result = Convert.ToString(value, 2);
            if (result.Length < 8)
                while (result.Length != 8) 
                    result = "0" + result;
            return result;
        }
        //метод перевода из двоичной в десятичную
        public int binaryToInt(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
                result += Convert.ToInt32(Convert.ToDouble(Convert.ToString(value[i])) * Math.Pow(2, value.Length - 1 - i));
            return result;
        }
        //метод перевода из двоичной в десятичную
        public string binaryToHex(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
                result += Convert.ToInt32(Convert.ToDouble(Convert.ToString(value[i])) * Math.Pow(2, value.Length - 1 - i));
            return Convert.ToString(result, 16);
        }
        //метод перевода из текста в шестандцатеричное представление символов
        public string[] textToHex(string text)
        {
            bool continueFlag = true;
            string[] hex = new string[text.Length];
            for (int charNum = 0; charNum < text.Length; charNum++)
            {
                continueFlag = true;
                for (int i = 0; i < 16; i++)
                    if (continueFlag)
                    {
                        for (int j = 0; j < 16; j++)
                            if (text[charNum] == fullAscii[i, j])
                            {
                                hex[charNum] = Convert.ToString(i, 16) + Convert.ToString(j, 16);
                                while (hex[charNum].Length % 2 != 0)
                                    hex[charNum] = "0" + hex[charNum];
                                continueFlag = false;
                                break;
                            }
                    }
                    else
                        break;
            }
            return hex;
        }
        //метод перевода из текста в шестандцатеричное представление символов
        public string hexToText(string hex)
        {
            bool continueFlag = true;
            string text = "";
            for (int charNum = 0; charNum < hex.Length; charNum += 2)
            {
                continueFlag = true;
                for (int i = 0; i < 16; i++)
                    if (continueFlag)
                    {
                        for (int j = 0; j < 16; j++)
                            if (hexToInt(Convert.ToString(hex[charNum])) == i && hexToInt(Convert.ToString(hex[charNum + 1])) == j)
                            {
                                text += fullAscii[i, j];
                                break;
                            }
                    }
                    else
                        break;
            }
            return text;
        }
        //метод перевода из шестнадцатеричной в десятичную систему счисления
        public int hexToInt(string hexValue)
        {
            if (hexValue == null)
                return 0;
            else
            {
                int result = 0;
                while (hexValue.Length % 2 != 0)
                    hexValue = "0" + hexValue;
                int n = hexValue.Length;
                string[] stringPieces = new string[n]; 
                for (int i = 0; i < n; i++)
                    stringPieces[i] = Convert.ToString(hexValue[i]);
                int[] intPieces = new int[n];
                for (int i = 0; i < n; i++) 
                {
                    switch (stringPieces[i])
                    {
                        case "0":
                            intPieces[i] = 0;
                            continue;
                        case "1":
                            intPieces[i] = 1;
                            continue;
                        case "2":
                            intPieces[i] = 2;
                            continue;
                        case "3":
                            intPieces[i] = 3;
                            continue;
                        case "4":
                            intPieces[i] = 4;
                            continue;
                        case "5":
                            intPieces[i] = 5;
                            continue;
                        case "6":
                            intPieces[i] = 6;
                            continue;
                        case "7":
                            intPieces[i] = 7;
                            continue;
                        case "8":
                            intPieces[i] = 8;
                            continue;
                        case "9":
                            intPieces[i] = 9;
                            continue;
                        case "a":
                        case "A":
                            intPieces[i] = 10;
                            continue;
                        case "b":
                        case "B":
                            intPieces[i] = 11;
                            continue;
                        case "c":
                        case "C":
                            intPieces[i] = 12;
                            continue;
                        case "d":
                        case "D":
                            intPieces[i] = 13;
                            continue;
                        case "e":
                        case "E":
                            intPieces[i] = 14;
                            continue;
                        case "f":
                        case "F":
                            intPieces[i] = 15;
                            continue;
                    }
                }
                Array.Reverse(intPieces); 
                for (int i = 0; i < n; i++) 
                {
                    int arg2 = 1;
                    for (int j = 0; j < i; j++)
                        arg2 *= 16;
                    result += intPieces[i] * arg2;
                }
                return result;
            }
        }
        // циклический сдвиг на n бит влево
        public string rol(string value, int n)
        {
            int newValue = hexToInt(value);
            string binaryValue = Convert.ToString(newValue, 2);

            while (binaryValue.Length != 16) 
                binaryValue = "0" + binaryValue;

            string[] elements = new string[16];
            for (int i = 0; i < 16; i++)
                elements[i] = Convert.ToString(binaryValue[i]);

            elements = rolArr(elements, n);
            string toOut = "";
            foreach (string el in elements)
                toOut += el;

            return intToHex(binaryToInt(toOut), 4);
        }
        // циклический сдвиг массива 
        public string[] rolArr(string[] array, int shiftSize)
        {
            int arrLength = array.Length;
            string[] finalArr = new string[arrLength];
            for (int i = 0; i < arrLength; i++)
                finalArr[i] = array[i];

            string[] additionalArr = new string[shiftSize]; 
            for (int r = 0; r < shiftSize; r++) 
                additionalArr[r] = finalArr[r];

            for (int i = 0; i < arrLength - shiftSize; i++) 
                finalArr[i] = finalArr[i + shiftSize];

            int j = 0;
            for (int i = arrLength - shiftSize; i < arrLength; i++) 
            {
                finalArr[i] = additionalArr[j];
                j++;
            }
            return finalArr;
        }

        // проверка введения ключа
        public bool keyChecker(TextBox tb)
        {
            switch (tb.Text.Length)
            {
                case int n when n < 16:
                    message("  Введен слишком короткий ключ.\n\nНеобходимая длина - 16 символов.");
                    return false;
                case int n when n > 16:
                    message("  Введен слишком длинный ключ.\n\nНеобходимая длина - 16 символов.");
                    return false;
                case 16:
                    return true;
                default:
                    return false;
            }
        }
        public bool inputChecker(RichTextBox rtb)
        {
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            if (tr.Text.Length == 2)
            {
                message("Данные для шифрования не введены.");
                return false;
            }
            return true;
        }
        public void modeCheck(Button encryptionButton, Button decryptionButton, bool encryptionMode, bool decryptionMode)
        {
            if (encryptionMode == true && decryptionMode == false)
            {
                encryptionButton.Background = new SolidColorBrush(Color.FromRgb(229, 229, 229));
                encryptionButton.Foreground = new SolidColorBrush(Color.FromRgb(1, 1, 1));
                decryptionButton.Background = new SolidColorBrush(Color.FromRgb(64, 96, 233));
                decryptionButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                encryptionStackPanel.Visibility = Visibility.Visible;
                encryptButton.Visibility = Visibility.Visible;
                decryptionStackPanel.Visibility = Visibility.Hidden;
                decryptButton.Visibility = Visibility.Hidden;
            }
            else if (encryptionMode == false && decryptionMode == true)
            {
                encryptionButton.Background = new SolidColorBrush(Color.FromRgb(64, 96, 233));
                encryptionButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                decryptionButton.Background = new SolidColorBrush(Color.FromRgb(229, 229, 229));
                decryptionButton.Foreground = new SolidColorBrush(Color.FromRgb(1, 1, 1));
                encryptionStackPanel.Visibility = Visibility.Hidden;
                encryptButton.Visibility = Visibility.Hidden;
                decryptionStackPanel.Visibility = Visibility.Visible;
                decryptButton.Visibility = Visibility.Visible;
            }
        }
        private void inputPlainDataFormatHex_Checked(object sender, RoutedEventArgs e)
        {
            inputDataFormat = RichTextBoxHex(inputPlainData);
        }

        private void inputPlainDataFormatAscii_Checked(object sender, RoutedEventArgs e)
        {
            inputDataFormat = RichTextBoxAscii(inputPlainData);
        }

        private void outputCipherDataFormatHex_Checked(object sender, RoutedEventArgs e)
        {
            outputDataFormat = RichTextBoxHex(outputCipherData);
        }

        private void outputCipherDataFormatAscii_Checked(object sender, RoutedEventArgs e)
        {
            outputDataFormat = RichTextBoxAscii(outputCipherData);
        }
        private void inputCipherDataFormatHex_Checked(object sender, RoutedEventArgs e)
        {
            inputDataFormat = RichTextBoxHex(inputCipherData);
        }
        private void inputCipherDataFormatAscii_Checked(object sender, RoutedEventArgs e)
        {
            inputDataFormat = RichTextBoxAscii(inputCipherData);
        }
        private void outputPlainDataFormatHex_Checked(object sender, RoutedEventArgs e)
        {
            outputDataFormat = RichTextBoxHex(outputPlainData);
        }

        private void outputPlainDataFormatAscii_Checked(object sender, RoutedEventArgs e)
        {
            outputDataFormat = RichTextBoxAscii(outputPlainData);
        }
        public string RichTextBoxHex(RichTextBox rtb)
        {
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string content = tr.Text;
            if (content.Length != 2)
            {
                string textInput = "";
                for (int i = 0; i < content.Length - 2; i++)
                    textInput += Convert.ToString(content[i]);
                string[] hexInput = textToHex(textInput);
                rtb.Document.Blocks.Clear();
                foreach (string element in hexInput)
                    rtb.AppendText(element);
            }
            return "hex";
        }
        public string RichTextBoxAscii(RichTextBox rtb)
        {
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string content = tr.Text;
            if (content.Length != 2)
            {
                string hexInput = "";
                for (int i = 0; i < content.Length - 2; i++)
                    hexInput += Convert.ToString(content[i]);
                string textInput = hexToText(hexInput);
                rtb.Document.Blocks.Clear();
                rtb.AppendText(textInput);
            }
            return "ascii";
        }
       
        public void aboutProgram_Click(object sender, RoutedEventArgs e)
        {
            aboutProgram ap = new aboutProgram();
            ap.Show();
        }
        public void algorithm_Click(object sender, RoutedEventArgs e)
        {
            algorithm al = new algorithm();
            al.Show();
        }
        public void encryption_MouseEnter(object sender, MouseEventArgs e)
        {
            encryption.Background = new SolidColorBrush(Color.FromRgb(214, 252, 81));
            encryption.Foreground = new SolidColorBrush(Color.FromRgb(1, 1, 1));
        }
        public void encryption_MouseLeave(object sender, MouseEventArgs e)
        {
            modeCheck(encryption, decryption, encryptionMode, decryptionMode);
        }
        public void decryption_MouseEnter(object sender, MouseEventArgs e)
        {
            decryption.Background = new SolidColorBrush(Color.FromRgb(214, 252, 81));
            decryption.Foreground = new SolidColorBrush(Color.FromRgb(1, 1, 1));
        }
        public void decryption_MouseLeave(object sender, MouseEventArgs e)
        {
            modeCheck(encryption, decryption, encryptionMode, decryptionMode);
        }
        public void encryptionButton_Click(object sender, RoutedEventArgs e)
        {
            encryptionMode = true;
            decryptionMode = false;
            modeCheck(encryption, decryption, encryptionMode, decryptionMode);
        }
        public void decryptionButton_Click(object sender, RoutedEventArgs e)
        {
            encryptionMode = false;
            decryptionMode = true;
            modeCheck(encryption, decryption, encryptionMode, decryptionMode);
        }
        public void message(string text)
        {
            message msg = new message();
            msg.showMessage(text);
            msg.ShowDialog();
        }
    }
}
