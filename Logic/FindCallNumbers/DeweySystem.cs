using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class DeweySystem
    {
        private Node _root;
        private string[] lines;
        public List<DeweyObject> _offTen;
        public List<DeweyObject> _offHun;
        public DeweySystem()
        {
            //https://yetanotherchris.dev/csharp/6-ways-to-get-the-current-directory-in-csharp/

            _offTen = new List<DeweyObject>();
            _offHun = new List<DeweyObject>();
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
            
            InsertTopLevel();
        }

        private List<DeweyObject> InsertLeaves(int num)
        {
            List<DeweyObject> leaves = new List<DeweyObject>();
            
            for (int i = 1; i < 25; i++)
            {
                int current = Int32.Parse(lines[num + i].Substring(0, 3));
                DeweyObject deweyObject = new DeweyObject(current, lines[num+i].Substring(4));
                leaves.Add(deweyObject);

                if (current%10 == 0)
                {
                    _offTen.Add(deweyObject);
                }
            }

            return leaves;
        }
        private void InsertTopLevel()
        {
            int currentNum = 500;
            DeweyObject deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4));
            deweyObject._leaves = InsertLeaves(currentNum);
            _root = new Node(deweyObject);
            _offHun.Add(deweyObject);
            
            for (int i = 0; i < 4; i++)
            {
                currentNum += 100;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                _offHun.Add(deweyObject);
                
            }
            currentNum = 500;
            for (int i = 0; i < 5; i++)
            {
                currentNum -= 100;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                _offHun.Add(deweyObject);
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
                deweyObject._leaves = InsertLeaves(currentNum);
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
                deweyObject._leaves = InsertLeaves(currentNum);
                _offTen.Add(deweyObject);
                _root.Insert(deweyObject);
            }
            
            InsertBottomLevel();

        }

        private void InsertBottomLevel()
        {
            int currentNum = 500;
            DeweyObject deweyObject;

            for (int i = 0; i < 5; i++)
            {
                currentNum += 25;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                
                currentNum += 50;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                
                currentNum += 25;
            }
            currentNum = 500;
            for (int i = 0; i < 5; i++)
            {
                currentNum -= 25;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                
                currentNum -= 50;
                deweyObject = new DeweyObject(Int32.Parse(lines[currentNum].Substring(0, 3)), lines[currentNum].Substring(4)); 
                deweyObject._leaves = InsertLeaves(currentNum);
                _root.Insert(deweyObject);
                
                currentNum -= 25;
            }
        }
        
        //public List<DeweyObject> ReturnNodes(int find)
        public DeweyObject ReturnNodes(int find)
        {

            Debug.WriteLine("We are searching for " + find);
            Debug.WriteLine("");
            
            return _root.ReturnObjectLeaf(find);
        }

        public DeweyObject ReturnTop(int num)
        {
            for (int i = 0; i < _offHun.Count; i++)
            {
                if (_offHun.ElementAt(i)._number == num / 100 * 100)
                {
                    return _offHun.ElementAt(i);
                }
            }

            return new DeweyObject(0, "0");
        }
    }
}