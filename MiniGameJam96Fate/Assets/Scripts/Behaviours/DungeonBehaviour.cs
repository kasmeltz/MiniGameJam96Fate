namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("HairyNerd/MGJ96/Dungeon")]
    public class DungeonBehaviour : BehaviourBase
    {
        #region Members

        public int Width = 200;
        public int Height = 100;
        public float RoomThreshold = 0.5f;
        public int RoomOverlapAmount = -2;
        public int MinRoomWidth = 2;
        public int MaxRoomWidth = 5;
        public int MinRoomHeight = 2;
        public int MaxRoomHeight = 5;
        public int FlareOrbCount = 10;
        public int FreezeOrbCount = 10;

        public Tilemap Walls;

        public TileBase[] WallTiles;

        public OrbBehaviour FlareOrbPrefab;
        public OrbBehaviour FreezeOrbPrefab;

        public KeyBehaviour KeyPrefab;

        public Dungeon Dungeon { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
        }

        #endregion

        #region Protected Methods

        protected void MakeKey()
        {
            System.Random rnd = new System.Random();
            int x = rnd.Next(Width / 4, Width / 2);
            int y = rnd.Next(Height / 4, Height / 2);

            int dx = rnd.Next(0, 2);
            if (dx == 1)
            {
                x *= -1;
            }

            int dy = rnd.Next(0, 2);
            if (dy == 1)
            {
                y *= -1;
            }

            var key = Instantiate(KeyPrefab);            
            var pos = new Vector3(x * Walls.layoutGrid.cellSize.x, y * Walls.layoutGrid.cellSize.y, 0);
            key.transform.position = pos;

            var cellCoords = Walls
                    .WorldToCell(pos);

            Walls
                .SetTile(cellCoords, null);
        }

        protected void MakeOrbs()
        {
            System.Random rnd = new System.Random();

            int hw = Width / 2;
            int hh = Height / 2;
            for (int i = 0; i < FlareOrbCount; i++)
            {
                int x = rnd.Next(-hw + 3, hw - 3);
                int y = rnd.Next(-hh + 3, hh - 3);
                var orb = Instantiate(FlareOrbPrefab);

                var pos = new Vector3(x * Walls.layoutGrid.cellSize.x, y * Walls.layoutGrid.cellSize.y, 0);
                orb.transform.position = pos;
                var cellCoords = Walls
                    .WorldToCell(pos);

                Walls
                    .SetTile(cellCoords, null);                
            }

            for (int i = 0; i < FreezeOrbCount; i++)
            {
                int x = rnd.Next(-hw + 3, hw - 3);
                int y = rnd.Next(-hh + 3, hh - 3);
                var orb = Instantiate(FreezeOrbPrefab);

                var pos = new Vector3(x * Walls.layoutGrid.cellSize.x, y * Walls.layoutGrid.cellSize.y, 0);
                orb.transform.position = pos;
                var cellCoords = Walls
                    .WorldToCell(pos);

                Walls
                    .SetTile(cellCoords, null);
            }
        }

        protected void BuildDungeon()
        {
            Dungeon = new Dungeon
            {
                Width = Width,
                Height = Height,
                RoomThreshold = RoomThreshold,
                RoomOverlapAmount = RoomOverlapAmount,
                MinRoomWidth = MinRoomWidth,
                MaxRoomWidth = MaxRoomWidth,
                MinRoomHeight = MinRoomHeight,
                MaxRoomHeight = MaxRoomHeight
            };

            Dungeon
                .Build();

            for (int y = 0; y < Dungeon.Height; y++)
            {
                for (int x = 0; x < Dungeon.Width; x++)
                {
                    int wall = Dungeon.Walls[y, x];

                    if (wall == 1)
                    {
                        Walls
                            .SetTile(new Vector3Int(x - Dungeon.Width / 2, y - Dungeon.Height / 2, 0), WallTiles[0]);
                    }
                }
            }

            var pos = new Vector3(0, 0, 0);
            var cellCoords = Walls
                .WorldToCell(pos);

            Walls
                .SetTile(cellCoords, null);

            MakeOrbs();
            MakeKey();
        }

        #endregion


        #region Unity

        protected override void Awake()
        {
            BuildDungeon();
        }

        #endregion
    }
}