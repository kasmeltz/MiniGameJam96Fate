namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("HairyNerd/MGJ96/Hero")]
    public class HeroBehaviour : ActorBehaviour
    {
        #region Members

        public float MovementStep = 0.32f;

        protected DungeonBehaviour DungeonBehaviour { get; set; }

        #endregion

        #region Protected Methods

        protected void TryMove(int x, int y)
        {
            bool canMove = false;

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

            cellPosition += new Vector3Int(x, y, 0);

            tile = DungeonBehaviour
                .Walls
                .GetTile(cellPosition);

            if (tile == null)
            {
                canMove = true;
            }

            if (canMove)
            {
                transform
                    .Translate(new Vector3(x * MovementStep, y * MovementStep, 0));
            }
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
