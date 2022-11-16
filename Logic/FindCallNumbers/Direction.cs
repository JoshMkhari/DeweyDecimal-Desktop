namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public class Direction
    {
        public static readonly Direction Left = new Direction(0, -1);
        public static readonly Direction Right = new Direction(0, 1);
        public static readonly Direction Up = new Direction(-1, 0);
        public static readonly Direction Down = new Direction(1, 0);
        public int RowOffset { get; }
        public int ColOffset { get; }

        private Direction(int rowOffset, int colOffset)
        {
            ColOffset = colOffset;
            RowOffset = rowOffset;
        }

        public Direction Opposite()
        {
            return new Direction(-RowOffset, -ColOffset);
        }

        protected bool Equals(Direction other)
        {
            return RowOffset == other.RowOffset && ColOffset == other.ColOffset;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Direction)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RowOffset * 397) ^ ColOffset;
            }
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !Equals(left, right);
        }
    }
}