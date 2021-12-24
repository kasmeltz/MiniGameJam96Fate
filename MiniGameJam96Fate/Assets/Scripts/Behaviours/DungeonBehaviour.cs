namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using System.Collections.Generic;
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
        public int CoinCount = 50;

        public Tilemap Floor;
        public Tilemap Walls;
        
        public TileBase[] FloorTiles;
        public TileBase[] WallTiles;

        public OrbBehaviour FlareOrbPrefab;
        public OrbBehaviour FreezeOrbPrefab;
        public CoinBehaviour CoinPrefab;
        public KeyBehaviour KeyPrefab;

        public Dungeon Dungeon { get; set; }

        protected HashSet<Vector3> UsedPositions { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
            UsedPositions = new HashSet<Vector3>();
            BuildDungeon();
        }

        #endregion

        #region Protected Methods

        protected bool TrySpawnItem<T>(int x, int y, T prefab) 
            where T: Component
        {
            var position = new Vector3(x * Walls.layoutGrid.cellSize.x, y * Walls.layoutGrid.cellSize.y, 0);

            if (UsedPositions.Contains(position))
            {
                return false;
            }

            UsedPositions
                .Add(position);

            T item = Instantiate(prefab);
            item.transform.position = position;

            var cellCoords = Walls
                .WorldToCell(position);

            Walls
                .SetTile(cellCoords, null);

            return true;
        }

        protected void MakeKey()
        {            
            bool keySpawned;
            do
            {
                int x = Random.Range(Width / 4, Width / 2);
                int y = Random.Range(Height / 4, Height / 2);

                int dx = Random.Range(0, 2);
                if (dx == 1)
                {
                    x *= -1;
                }

                int dy = Random.Range(0, 2);
                if (dy == 1)
                {
                    y *= -1;
                }

                keySpawned = TrySpawnItem(x, y, KeyPrefab);
            } while (!keySpawned);
        }

        protected void MakeOrbs()
        {            
            int hw = Width / 2;
            int hh = Height / 2;

            int orbsSpawned = 0;
            do
            {
                int x = Random.Range(-hw + 3, hw - 2);
                int y = Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, FlareOrbPrefab);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < FlareOrbCount);

            orbsSpawned = 0;
            do
            {
                int x = Random.Range(-hw + 3, hw - 2);
                int y = Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, FreezeOrbPrefab);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < FreezeOrbCount);
        }

        protected void MakeCoins()
        {
            int hw = Width / 2;
            int hh = Height / 2;

            int coinsSpawned = 0;
            do
            {
                int x = Random.Range(-hw + 3, hw - 2);
                int y = Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, CoinPrefab);
                if (spawned)
                {
                    coinsSpawned++;
                }
            } while (coinsSpawned < CoinCount);
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

                    var cellPosition = new Vector3Int(x - Dungeon.Width / 2, y - Dungeon.Height / 2, 0);

                    Floor
                        .SetTile(cellPosition, FloorTiles[0]);

                    if (wall == 1)
                    {
                        Walls
                            .SetTile(cellPosition, WallTiles[0]);
                    } 
                }
            }

            var pos = new Vector3(0, 0, 0);
            var cellCoords = Walls
                .WorldToCell(pos);

            Walls
                .SetTile(cellCoords, null);

            MakeKey();
            MakeOrbs();
            MakeCoins();                        
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            Reset();
        }

        #endregion
    }
}