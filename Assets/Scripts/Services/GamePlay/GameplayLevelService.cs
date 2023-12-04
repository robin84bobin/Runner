namespace Services.GamePlay
{
    public class GameplayLevelService
    {
        private int _level;
        public string GetHeroPrefabName => $"Hero{_level}";

        public void SetupLevel(int level)
        {
            _level = level;
        }
    }
}