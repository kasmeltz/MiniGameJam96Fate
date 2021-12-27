namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public OrbBehaviour DoorOrbPrefab;
        public CoinBehaviour CoinPrefab;
        public KeyBehaviour KeyPrefab;

        public int FlareOrbCount { get; set; }

        public int FreezeOrbCount { get; set; }

        public int CoinCount { get; set; }

        public int RoomCount { get; set; }

        protected HashSet<Vector3> UsedPositions { get; set; }

        protected List<RoomBehaviour> RoomTypes { get; set; }

        public List<RoomBehaviour> Rooms { get; set; }

        #endregion

        #region Public Methods

        public void Reset()
        {
            UsedPositions = new HashSet<Vector3>();
            RoomCount = GameState.CurrentStage.RoomCount;
            FlareOrbCount = GameState.CurrentStage.FlareOrbCount;
            FreezeOrbCount = GameState.CurrentStage.FreezeOrbCount;
            CoinCount = GameState.CurrentStage.CoinCount;

            BuildDungeon();
        }

        #endregion

        #region Protected Methods

        protected bool TrySpawnItem<T>(int x, int y, T prefab, Action<T> doAfterSpawn = null)
            where T : Component
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

            doAfterSpawn?
                .Invoke(item);

            return true;
        }

        protected void MakeKey()
        {
            /*
            bool keySpawned;
            do
            {
                int x = UnityEngine.Random.Range(-(hw - 8), hw - 8);
                int y = UnityEngine.Random.Range(-(hh - 8), hh - 8);

                int dx = UnityEngine.Random.Range(0, 2);
                if (dx == 1)
                {
                    x *= -1;
                }

                int dy = UnityEngine.Random.Range(0, 2);
                if (dy == 1)
                {
                    y *= -1;
                }

                keySpawned = TrySpawnItem(x, y, KeyPrefab, (o) => o.gameObject.SetActive(false));                
            } while (!keySpawned);
            */
        }

        protected void MakeOrbs()
        {
            /*
            int hw = Dungeon.Width / 2;
            int hh = Dungeon.Height / 2;

            int orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-16, 16);
                int y = UnityEngine.Random.Range(-(hh - 8), -(hh - 2));
                bool spawned = TrySpawnItem(x, y, DoorOrbPrefab, (o) => o.OrbIndex = 0);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < 1);

            orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-(hw - 8), -(hw - 2));
                int y = UnityEngine.Random.Range(-16, 16);
                bool spawned = TrySpawnItem(x, y, DoorOrbPrefab, (o) => o.OrbIndex = 1);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < 1);

            orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(hw - 8, hw - 2);
                int y = UnityEngine.Random.Range(-16, 16);
                bool spawned = TrySpawnItem(x, y, DoorOrbPrefab, (o) => o.OrbIndex = 2);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < 1);

            orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-16, 16);
                int y = UnityEngine.Random.Range(hh - 8, hh - 2);
                bool spawned = TrySpawnItem(x, y, DoorOrbPrefab, (o) => o.OrbIndex = 3);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < 1);

            orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-hw + 3, hw - 2);
                int y = UnityEngine.Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, FlareOrbPrefab);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < FlareOrbCount);

            orbsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-hw + 3, hw - 2);
                int y = UnityEngine.Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, FreezeOrbPrefab);
                if (spawned)
                {
                    orbsSpawned++;
                }
            } while (orbsSpawned < FreezeOrbCount);
            */
        }

        protected void MakeCoins()
        {
            /*
            int hw = Dungeon.Width / 2;
            int hh = Dungeon.Height / 2;

            int coinsSpawned = 0;
            do
            {
                int x = UnityEngine.Random.Range(-hw + 3, hw - 2);
                int y = UnityEngine.Random.Range(-hh + 3, hh - 2);
                bool spawned = TrySpawnItem(x, y, CoinPrefab);
                if (spawned)
                {                    
                    coinsSpawned++;
                }
            } while (coinsSpawned < CoinCount);
            */
        }

        public void UpdateWalls()
        {
            /*
            for (int y = 0; y < Dungeon.Height; y++)
            {
                for (int x = 0; x < Dungeon.Width; x++)
                {
                    int wall = Dungeon.Walls[y, x];

                    var cellPosition = new Vector3Int(x - Dungeon.Width / 2, y - Dungeon.Height / 2, 0);

                    Floor
                        .SetTile(cellPosition, FloorTiles[0]);

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
            */
        }

        protected RoomBehaviour MakeRoom(RoomBehaviour prefab, Vector3 position)
        {
            var room = Instantiate(prefab);
            room.transform.position = position;

            var cellPosition = Floor.WorldToCell(position);

            int sy = cellPosition.y - (room.TileHeight / 2) + 1;
            int ey = cellPosition.y + (room.TileHeight / 2) - 1;
            int sx = cellPosition.x - (room.TileWidth / 2) + 1;
            int ex = cellPosition.x + (room.TileWidth / 2) - 1;

            for (int y = sy; y <= ey; y++)
            {
                for (int x = sx; x <= ex; x++)
                {
                    cellPosition.x = x;
                    cellPosition.y = y;

                    Floor.SetTile(cellPosition, FloorTiles[0]);
                }
            }
            // TODO - MAKE DESTRUCTIBLE TILES

            return room;
        }

        protected void MakeRooms()
        {
            var cellSize = Walls.layoutGrid.cellSize;
            Rooms = new List<RoomBehaviour>();

            Vector3 roomPosition = new Vector3(0, 0, 0);
            List<RoomBehaviour> roomsInProgress = new List<RoomBehaviour>();
            List<RoomConnectionBehaviour> possibleConnections = new List<RoomConnectionBehaviour>();

            var initialRoom = RoomTypes
                .Where(o =>
                    o.Connections.Count >= 4)
                .FirstOrDefault();

            var createdRoom = MakeRoom(initialRoom, roomPosition);

            Rooms
                .Add(createdRoom);

            roomsInProgress
                .Add(createdRoom);

            do
            {
                var roomIndex = UnityEngine
                    .Random
                    .Range(0, roomsInProgress.Count);

                var chosenRoom = roomsInProgress[roomIndex];

                if (chosenRoom.IsFilled)
                {
                    roomsInProgress
                        .Remove(chosenRoom);

                    continue;
                }

                possibleConnections = chosenRoom
                    .Connections
                    .Where(o => !o.IsFilled)
                    .ToList();

                int connectionIndex = UnityEngine.Random.Range(0, possibleConnections.Count);
                var connection = possibleConnections[connectionIndex];
                roomPosition = chosenRoom.transform.position;

                switch (connection.Direction)
                {
                    case RoomConnectionDirection.Up:                        
                        roomPosition.y += initialRoom.TileHeight * cellSize.y;
                        break;
                    case RoomConnectionDirection.Down:
                        roomPosition.y -= initialRoom.TileHeight * cellSize.y;
                        break;
                    case RoomConnectionDirection.Right:
                        roomPosition.x += initialRoom.TileWidth * cellSize.x;
                        break;
                    case RoomConnectionDirection.Left:
                        roomPosition.x -= initialRoom.TileWidth * cellSize.x;
                        break;
                }

                createdRoom = MakeRoom(initialRoom, roomPosition);

                AddNeighbour(chosenRoom, createdRoom, connection);

                AddNeighbours(createdRoom);

                Rooms
                    .Add(createdRoom);

                roomsInProgress
                    .Add(createdRoom);

            } while (Rooms.Count < RoomCount);

            /*
            foreach(var room in Rooms)
            {
                foreach(var connection in room.Connections)
                {
                    connection
                        .gameObject
                        .SetActive(false);
                }
            }
            */
        }

        protected void AddNeighbour(RoomBehaviour room1, RoomBehaviour room2, RoomConnectionBehaviour connection)
        {
            connection.IsFilled = true;

            room1
                .Neighbours
                .Add(room2);

            var cellPosition = Floor
                .WorldToCell(connection.transform.position);

            int sy = cellPosition.y - 1;
            int ey = cellPosition.y + 1;
            int sx = cellPosition.x - 1;
            int ex = cellPosition.x + 1;

            for (int y = sy; y <= ey; y++)
            {
                for (int x = sx; x <= ex; x++)
                {
                    cellPosition.x = x;
                    cellPosition.y = y;

                    Floor.SetTile(cellPosition, FloorTiles[0]);
                }
            }
        }

        protected void AddNeighbours(RoomBehaviour room)
        {
            foreach (var connection in room.Connections)
            {
                if (connection.IsFilled)
                {
                    continue;
                }

                foreach (var otherRoom in Rooms)
                {                    
                    foreach (var otherConnection in otherRoom.Connections)
                    {
                        var distance = (connection.transform.position - otherConnection.transform.position).magnitude;

                        if (distance <= 0.16f)
                        {
                            AddNeighbour(room, otherRoom, connection);
                            break;
                        }                        
                    }

                    if (connection.IsFilled)
                    {
                        break;
                    }
                }
            }
        }

        protected void BuildDungeon()
        {
            /*
            Dungeon = new Dungeon
            {
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
            */

            MakeRooms();
            UpdateWalls();
            MakeKey();
            MakeOrbs();
            MakeCoins();                        
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            RoomTypes = Resources
                .LoadAll<RoomBehaviour>("Prefabs/Rooms")
                .ToList();            

            Reset();            
        }

        #endregion
    }
}