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

        public Dungeon Dungeon { get; set; }

        #endregion

        #region Unity

        public void Awake()
        {
            Dungeon = new Dungeon
            {
                Width = 41,
                Height = 23,
                RoomThreshold = 0.6,
                RoomOverlapAmount =0,
                MinRoomWidth = 2,
                MaxRoomWidth = 10,
                MinRoomHeight = 2,
                MaxRoomHeight = 10
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
        }

        #endregion
    }
}
