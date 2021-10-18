using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace HASHCHECK
{

class Hashcheck
    {
        public const int passLength = 5;
        public const int maxBackgroundThreads = 6;
        public const string hash1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
        public const string hash2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
        public const string hash3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";
        static string[] passwords;
        public class passRange
        {
            public int Num
            { get; set; }

            public int Begin
            { get; set; }

            public int End
            { get; set; }
        };
    static void Main(string[] args)
        {
            passwords = new string[(int)Math.Pow(26,passLength)];
            int counter = 0;
            for (char first = 'a'; first <= 'z'; first++)
                for (char sec = 'a'; sec <= 'z'; sec++)
                    for (char third = 'a'; third <= 'z'; third++)
                        for (char fourth = 'a'; fourth <= 'z'; fourth++)
                            for (char fith = 'a'; fith <= 'z'; fith++, counter++)
                            {
                                char[] password = { first, sec, third, fourth, fith };
                                passwords[counter] =  new string(password);
                            }



            int window = passwords.Length / maxBackgroundThreads;
            for (int i = 0; i < maxBackgroundThreads; i++)
            {
                var range = new passRange { Num = i + 1,  Begin = i* window, End = (i+1)*window  < passwords.Length ? (i + 1) * window : passwords.Length };
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
