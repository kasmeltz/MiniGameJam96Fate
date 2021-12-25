namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.Logic
{
    public static class GameState
    {
        private static Hero _hero;
        public static Hero Hero
        {
            get
            {
                if (_hero == null)
                {
                    _hero = new Hero();
                }

                return _hero;
            }
        }
    }
}
