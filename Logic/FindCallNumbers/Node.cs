using System.Diagnostics;

namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class Node
    {
        private Node _left, _right;
        private DeweyObject _data;

        public Node(DeweyObject data)
        {
            _data = data;
        }

        public void Insert(DeweyObject value)
        {
            if (value._number <= _data._number)
            {
                if (_left == null)
                {
                    _left = new Node(value);
                }
                else
                {
                    if (_left._data._number != value._number)
                    {
                        _left.Insert(value);
                    }
                }
            }
            else
            {
                if (_right == null)
                {
                    _right = new Node(value);
                }
                else
                {
                    if (_right._data._number != value._number)
                    {
                        _right.Insert(value);
                    }
                }
            }
        }
        
        public DeweyObject ReturnObject(int num)
        {
            Debug.WriteLine("This is current num " + _data._number);
            if (num == _data._number || (num>_data._number && num<_data._number+25))
            {
                return _data;
            }
            if (num < _data._number)
            {
                Debug.WriteLine("we going left ");
                return _left.ReturnObject(num);
            }
            Debug.WriteLine("we going right ");
            return _right.ReturnObject(num);
        }

        public void PrintInOrder()
        {
            if (_left != null)
            {
                _left.PrintInOrder();
            }
            Debug.WriteLine(_data._number);
            if (_right != null)
            {
                _right.PrintInOrder();
            }
        }

        public void PrintPreOrder()
        {
            Debug.WriteLine(_data._number);
            if (_left != null)
            {
                _left.PrintPreOrder();
            }
            if (_right != null)
            {
                _right.PrintPreOrder();
            }
        }
    }
}
