namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    using UnityEngine;

    public class Room
    {
        public Room(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;

            TopLeft = new Vector2(x, y);
            BottomRight = new Vector2(x + w, y + h);
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Vector2 TopLeft { get; set; }

        public Vector2 BottomRight { get; set; }

        public bool DoesOverlap(Room other, int? padding = null)
        {
            if (!padding.HasValue)
            {
                padding = 0;
            }

            if (TopLeft.x >= other.BottomRight.x + padding ||
                other.TopLeft.x >= BottomRight.x + padding ||
                BottomRight.y < other.TopLeft.y - padding ||
                other.BottomRight.y < TopLeft.y - padding)
            {
                return false;
            }

            return true;
        }
    }

}
