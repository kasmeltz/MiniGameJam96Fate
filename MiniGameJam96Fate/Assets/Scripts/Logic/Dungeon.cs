/*namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    using System;
    using System.Collections.Generic;

    public class Dungeon
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public double RoomThreshold { get; set; }

        public int RoomOverlapAmount { get; set; }

        4public int MinRoomWidth { get; set; }

        public int MaxRoomWidth { get; set; }

        public int MinRoomHeight { get; set; }

        public int MaxRoomHeight { get; set; }

        public int[,] Walls { get; set; }

        public List<Room> Rooms { get; set; }

        public void Build(int? seed = null)
        {
            Rooms = new List<Room>();

            Random rnd;
            if (seed.HasValue)
            {
                rnd = new Random(seed.Value);
            }
            else
            {
                rnd = new Random();
            }

            Walls = new int[Height, Width];
            for (int y = 0; y < Walls.GetLength(0); y++)
            {
                for (int x = 0; x < Walls.GetLength(1); x++)
                {
                    Walls[y, x] = 2;
                }
            }

            int failedAttempts = 0;
            int roomSquares;
            int roomsRequired = (int)((Width * Height) * RoomThreshold);
            do
            {
                int x = rnd.Next(1, Width - 2);
                int y = rnd.Next(1, Height - 2);
                int w = rnd.Next(MinRoomWidth, MaxRoomWidth);
                int h = rnd.Next(MinRoomHeight, MaxRoomHeight);

                var room = new Room(x, y, w, h);

                bool overlap = false;
                foreach (var existing in Rooms)
                {
                    if (existing
                        .DoesOverlap(room, RoomOverlapAmount))
                    {
                        //Console.WriteLine("OVERLAP");
                        overlap = true;
                        break;
                    }
                }

                if (overlap)
                {
                    failedAttempts++;
                    if (failedAttempts >= 100)
                    {
                        break;
                    }
                }
                else
                {
                    failedAttempts = 0;

                    Rooms
                        .Add(room);

                    for (int ry = room.Y; ry < room.Y + room.Height; ry++)
                    {
                        if (ry >= Walls.GetLength(0) - 1)
                        {
                            break;
                        }

                        for (int rx = room.X; rx < room.X + room.Width; rx++)
                        {
                            if (rx >= Walls.GetLength(1) - 1)
                            {
                                break;
                            }

                            Walls[ry, rx] = 1;
                        }
                    }
                }

                roomSquares = 0;
                foreach (int value in Walls)
                {
                    if (value == 0)
                    {
                        roomSquares++;
                    }
                }

            } while (roomSquares < roomsRequired);
        }
    }
}

*/