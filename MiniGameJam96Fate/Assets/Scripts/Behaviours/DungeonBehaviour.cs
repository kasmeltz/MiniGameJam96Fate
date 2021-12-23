namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("HairyNerd/MGJ96/Dungeon")]
    public class DungeonBehaviour : BehaviourBase
    {
        #region Members

        public Tilemap Walls;

        public TileBase[] WallTiles;

        #endregion

        #region Unity

        public void Awake()
        {
            var dungeon = new Dungeon
            {
                Width = 40,
                Height = 23,
                RoomThreshold = 0.5,
                RoomOverlapAmount = -1,
                MinRoomWidth = 1,
                MaxRoomWidth = 10,
                MinRoomHeight = 1,
                MaxRoomHeight = 10
            };

            dungeon
                .Build();

            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    int wall = dungeon.Walls[y, x];

                    if (wall == 1)
                    {
                        Walls
                            .SetTile(new Vector3Int(x - dungeon.Width / 2, y - dungeon.Height / 2, 0), WallTiles[0]);
                    }
                }
            }
        }

        #endregion
    }
}
