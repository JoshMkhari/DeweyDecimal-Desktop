using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class DeweySystem
    {
        public DeweySystem()
        {
            //https://yetanotherchris.dev/csharp/6-ways-to-get-the-current-directory-in-csharp/
            char[] path = Directory.GetCurrentDirectory().ToCharArray();
            
            int eCount = 0;
            int location = 0;
            for (int i = path.Length-1; i > 0; i--)
            {
                if (path[i] == 'e')
                {
                    eCount++;
                }

                if (eCount == 2)
                {
                    location = i+2;
                    break;
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < location; i++)
            {
                stringBuilder.Append(path[i]);
            }

            //https://zetcode.com/csharp/path/
            string filePath = Path.Combine(stringBuilder.ToString(), @"Logic\FindCallNumbers\MySystem.txt");
            
            //Read a textfile line by line
            //https://www.c-sharpcorner.com/UploadFile/mahesh/how-to-read-a-text-file-in-C-Sharp/
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                Debug.WriteLine(line);
            }
        }
    }
}