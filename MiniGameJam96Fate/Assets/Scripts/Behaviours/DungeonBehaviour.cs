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
       
        public Tilemap Floor;
        public Tilemap Walls;
        
        public TileBase[] FloorTiles;
        public TileBase[] WallTiles;

        public OrbBehaviour FlareOrbPrefab;
        public OrbBehaviour FreezeOrbPrefab;
        public CoinBehaviour CoinPrefab;
        public KeyBehaviour KeyPrefab;

        public int FlareOrbCount { get; set; }

        public int FreezeOrbCount { get; set; }

        public int CoinCount { get; set; }

        public Dungeon Dungeon { get; set; }

        protected HashSet<Vector3> UsedPositions { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
            UsedPositions = new HashSet<Vector3>();

            FlareOrbCount = GameState.CurrentStage.FlareOrbCount;
            FreezeOrbCount = GameState.CurrentStage.FreezeOrbCount;
            CoinCount = GameState.CurrentStage.CoinCount;

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

            var cellPosition = new Vector3Int(x, y, 0);

            var tile = Walls
                .GetTile(cellPosition);
            
            if (tile != null)
            {
                return false;
            }

            UsedPositions
                .Add(position);

            T item = Instantiate(prefab);
            item.transform.position = position;

            return true;
        }

        protected void MakeKey()
        {            
            bool keySpawned;
            do
            {
                int hw = Dungeon.Width / 2;
                int hh = Dungeon.Height / 2;

                int x = Random.Range(hw - 8, hw - 2);
                int y = Random.Range(hh - 8, hh - 2);

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
            int hw = Dungeon.Width / 2;
            int hh = Dungeon.Height / 2;

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
            int hw = Dungeon.Width / 2;
            int hh = Dungeon.Height / 2;

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

        public void UpdateWalls()
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
                for (int x = 0; x < Dungeon.Width; x++)
                {
                    int wall = Dungeon.Walls[y, x];

                    var cellPosition = new Vector3Int(x - Dungeon.Width / 2, y - Dungeon.Height / 2, 0);

                    if (wall == 2)
                    {
                        Walls
                            .SetTile(cellPosition, WallTiles[0]);
                    }
                    else
                    {
                        Walls
                            .SetTile(cellPosition, null);
                    }

                    if (wall == 1)
                    {
                        Floor
                            .SetTile(cellPosition, FloorTiles[1]);
                    }
                    else
                    {
                        Floor
                            .SetTile(cellPosition, FloorTiles[0]);
                    }
                }
            }
        }

        protected void BuildDungeon()
        {
            Dungeon = new Dungeon
            {
                Width = GameState.CurrentStage.Width,
                Height = GameState.CurrentStage.Height,
                RoomThreshold = GameState.CurrentStage.RoomThreshold,
                RoomOverlapAmount = GameState.CurrentStage.RoomOverlapAmount,
                MinRoomWidth = GameState.CurrentStage.MinRoomWidth,
                MaxRoomWidth = GameState.CurrentStage.MaxRoomWidth,
                MinRoomHeight = GameState.CurrentStage.MinRoomHeight,
                MaxRoomHeight = GameState.CurrentStage.MaxRoomHeight
            };

            Dungeon
                .Build();

            for (int y = 0; y < Dungeon.Height; y++)
            {
                for (int x = 0; x < Dungeon.Width; x++)
                {
                    var cellPosition = new Vector3Int(x - Dungeon.Width / 2, y - Dungeon.Height / 2, 0);

                    Floor
                        .SetTile(cellPosition, FloorTiles[0]);
                }
            }

            UpdateWalls();

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