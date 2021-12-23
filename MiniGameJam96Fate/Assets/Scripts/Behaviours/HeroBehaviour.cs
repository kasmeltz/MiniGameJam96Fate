namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        #region Members

        public float MovementStep = 0.32f;

        protected DungeonBehaviour DungeonBehaviour { get; set; }

        #endregion

        #region Protected Methods

        protected void TryBreakWall()
        {
            Vector3Int cellPosition = Vector3Int.zero;
            TileBase tile = null;

            int dx = 0;
            int dy = 0;

            if (Direction.x < 0)
            {
                dx = -1;
            }
            else if (Direction.x > 0)
            {
                dx = 1;
            }

            if (Direction.y < 0)
            {
                dy = -1;
            }
            else if (Direction.y > 0)
            {
                dy = 1;
            }

            var newPos = transform.position + new Vector3(dx * MovementStep, dy * MovementStep, 0);

            cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(newPos);

            tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile != null)
            {
                DungeonBehaviour
                    .Walls
                    .SetTile(cellPosition, null);
            }
        }

        protected void TryMove(int x, int y)
        {
            bool canMove = false;
            Vector3Int cellPosition = Vector3Int.zero;
            TileBase tile = null;

            /*
            var cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(transform.position);

            var tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile != null)
            {
                canMove = true;
            }
            */

            var newPos = transform.position + new Vector3(x * MovementStep, y * MovementStep, 0);

            cellPosition = DungeonBehaviour
                .Walls
                .WorldToCell(newPos);

            tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile == null)
            {
                canMove = true;
            }

            if (canMove)
            {
                transform.position = newPos;
            }

            var cameraPos = Camera.main.transform.position;
            cameraPos = new Vector3(transform.position.x, transform.position.y, cameraPos.z);
            Camera.main.transform.position = cameraPos;
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SetDirection(0, -1);
                TryMove(0, -1);                
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SetDirection(0, 1);
                TryMove(0, 1);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetDirection(-1 ,0);
                TryMove(-1, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetDirection(1, 0);
                TryMove(1, 0);
            }
            
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                TryBreakWall();
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            DungeonBehaviour = FindObjectOfType<DungeonBehaviour>();
        }

        #endregion
    }
}
