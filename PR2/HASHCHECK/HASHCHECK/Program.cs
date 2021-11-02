using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Collections.Generic;

namespace HASHCHECK
{

class Hashcheck
    {
        public const int passLength = 5;
        public const int maxBackgroundThreads = 6;
        public const string hash1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
        public const string hash2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
        public const string hash3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";
        public const string symbols = "abcdefghijklmnopqrstuvwxyz";
        static List<string> passwords;
        public class passRange
        {
            public int Num
            { get; set; }

            public int Begin
            { get; set; }

            public int End
            { get; set; }
        };

        static List<string> dictGen(int size, string symbols)
        {
            int position = 1;
            int last = 0;
            List<string> passList = new List<string> {""};
            int delta = symbols.Length;
            for (int j = 0; j < symbols.Length; j++)
            {
                passList.Add("" + symbols[j]);
            }
            if (size == 1) return passList;
            while (++position <= passLength)
            {
                delta = (int)Math.Pow(symbols.Length, position-1);
                last = passList.Count - delta;
                int i = 0;
                for (; i < delta; i++)
                {
                    for (int j = 0; j < symbols.Length; j++)
                    {
                        passList.Add(passList[last+i] + symbols[j]);

                    }
                }
                
            }
            
            return passList;

        }
        static void Main(string[] args)
        {
            passwords = dictGen(passLength, symbols);


            int window = passwords.Count / maxBackgroundThreads;
            for (int i = 0; i < maxBackgroundThreads; i++)
            {
                var range = new passRange { Num = i + 1,  Begin = i* window, End = (i+1)*window  < passwords.Count ? (i + 1) * window : passwords.Count };
                Thread myThread = new Thread(checkPass);
                myThread.Start(range); // запускаем поток
            }


        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static void checkPass(Object obj)
        {
            passRange r;
            try
            {
                r = (passRange)obj;
            }
            catch (InvalidCastException)
            {
                r = new passRange { Begin = 0, End = 26 };
            }

            for (int i = r.Begin; i < r.End; i++)
            {
                string hash = ComputeSha256Hash(passwords[i]);
                //Console.WriteLine(passwords[i] + " " + hash);
                if (hash == hash1 || hash == hash2 || hash == hash3)
                    Console.WriteLine("Thread" + r.Num.ToString() + ":" + passwords[i]);
                
                //Thread.Sleep(400);
            }
        }
    }
}
