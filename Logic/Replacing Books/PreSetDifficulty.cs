using System.Collections.Generic;

namespace JoshMkhariPROG7312Game.Logic.Replacing_Books
{
    public class PreSetDifficulty
    {
        private readonly IDictionary<int, bool>
            _activeAscDesc = new Dictionary<int, bool>(); //Stores Difficulty

        public IDictionary<int, bool> ChangeDifficulty(int current)
        {
            switch (current)
            {
                case 1:
                    return Normal();
                case 2:
                    return Difficult();
                case 3:
                    return Hard();
                case 4:
                    return Extreme();
                case 5:
                    return Insane();
                default:
                    return Easy();
            }
        }

        private IDictionary<int, bool> Easy()
        {
            _activeAscDesc.Clear();
            for (var i = 0; i < 8; i++) _activeAscDesc.Add(i, true);

            return _activeAscDesc;
        }

        private IDictionary<int, bool> Normal()
        {
            _activeAscDesc.Clear();

            //Top Up Arrows
            _activeAscDesc.Add(0, true);
            _activeAscDesc.Add(1, true);

            _activeAscDesc.Add(2, true);
            _activeAscDesc.Add(3, true);

            _activeAscDesc.Add(4, false);
            _activeAscDesc.Add(5, false);
            //Bottom 


            //Left

            _activeAscDesc.Add(6, true);

            //Right

            _activeAscDesc.Add(7, true);

            return _activeAscDesc;
        }

        private IDictionary<int, bool> Difficult()
        {
            _activeAscDesc.Clear();

            //Top
            _activeAscDesc.Add(0, true);
            _activeAscDesc.Add(1, true);
            _activeAscDesc.Add(2, true);
            _activeAscDesc.Add(3, true);
            _activeAscDesc.Add(4, false);

            //Bottom

            _activeAscDesc.Add(5, false);

            //Left

            _activeAscDesc.Add(6, false);

            //Right

            _activeAscDesc.Add(7, true);
            return _activeAscDesc;
        }

        private IDictionary<int, bool> Hard()
        {
            _activeAscDesc.Clear();

            //Top Ascending
            _activeAscDesc.Add(0, true);
            _activeAscDesc.Add(1, true);
            _activeAscDesc.Add(2, false);
            _activeAscDesc.Add(3, false);

            _activeAscDesc.Add(4, false);

            //Bottom Ascending

            _activeAscDesc.Add(5, false);

            //Left Descending

            _activeAscDesc.Add(6, true);

            //Right Descending

            _activeAscDesc.Add(7, true);
            return _activeAscDesc;
        }

        private IDictionary<int, bool> Extreme()
        {
            _activeAscDesc.Clear();

            //Top Ascending
            _activeAscDesc.Add(0, true);
            _activeAscDesc.Add(1, true);
            _activeAscDesc.Add(2, true);

            _activeAscDesc.Add(3, false);
            _activeAscDesc.Add(4, false);
            //Bottom Ascending

            _activeAscDesc.Add(5, false);

            //Left Ascending

            _activeAscDesc.Add(6, false);

            //Right Descending

            _activeAscDesc.Add(7, true);
            return _activeAscDesc;
        }

        private IDictionary<int, bool> Insane()
        {
            _activeAscDesc.Clear();

            //Top Ascending
            _activeAscDesc.Add(0, true);
            _activeAscDesc.Add(1, true);
            _activeAscDesc.Add(2, true);
            _activeAscDesc.Add(3, true);
            _activeAscDesc.Add(4, false);

            //Bottom Ascending

            _activeAscDesc.Add(5, false);

            //Left Ascending

            _activeAscDesc.Add(6, false);

            //Right Ascending

            _activeAscDesc.Add(7, false);
            return _activeAscDesc;
        }
    }
}