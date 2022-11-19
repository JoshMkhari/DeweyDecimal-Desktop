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
                    _left.Insert(value);
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
                    _right.Insert(value);
                }
            }
        }
        
        //num can be 700
        //num can be 723

        public DeweyObject ReturnObject(int num)
        {
            //data number = 700
            if (num == _data._number || (num>_data._number && num<_data._number+25))
            {
                return _data;
            }
            if (num < _data._number)
            {
                return _left.ReturnObject(num);
            }
            return _right.ReturnObject(num);
        } 
    }
}
