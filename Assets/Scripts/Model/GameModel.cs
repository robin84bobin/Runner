namespace Model
{
    public class GameModel
    {
        public HeroModel HeroModel { get; }
        public AbilitiesModel AbilitiesModel { get; }

        public GameModel(HeroModel heroModel, AbilitiesModel abilitiesModel)
        {
            HeroModel = heroModel;
            AbilitiesModel = abilitiesModel;
        }
    }
}