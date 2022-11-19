using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class DeweySystem
    {
        private Node _root;
        private string[] lines;
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
            lines = File.ReadAllLines(filePath);

            
            //Debug.WriteLine("What line is this " + lines[startnum].Substring(0,3));
            //Debug.WriteLine("What line is this " + lines[startnum].Substring(4));
            //500


            InsertTopLevel();
        }

        private void InsertTopLevel()
        {
            int currentNum = 500;
            DeweyObject deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
            _root = new Node(deweyObject);

            for (int i = 0; i < 4; i++)
            {
                currentNum += 100;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            currentNum = 500;
            for (int i = 0; i < 5; i++)
            {
                currentNum -= 100;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            InsertMidLevel();
        }
        
        private void InsertMidLevel()
        {
            int currentNum = 500;
            DeweyObject deweyObject;

            for (int i = 0; i < 5; i++)
            {
                currentNum += 50;
                if (currentNum % 100 == 0)
                {
                    currentNum += 50;
                }
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            currentNum = 500;
            for (int i = 0; i < 5; i++)
            {
                currentNum -= 50;
                if (currentNum % 100 == 0)
                {
                    currentNum -= 50;
                }
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            
            //InsertBottomLevel();
            _root.PrintInOrder(); 
        }

        private void InsertBottomLevel()
        {
            int currentNum = 500;
            DeweyObject deweyObject;

            for (int i = 0; i < 4; i++)
            {
                currentNum += 25;
                if (currentNum % 100 == 0)
                {
                    currentNum += 25;
                }
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            currentNum = 500;
            for (int i = 0; i < 5; i++)
            {
                currentNum -= 25;
                if (currentNum % 100 == 0)
                {
                    currentNum -= 25;
                }
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                _root.Insert(deweyObject);
            }
            
            _root.PrintInOrder(); 
        }
    }
}