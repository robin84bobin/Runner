namespace Model
{
    public class GameModel
    {
        public ActorModel HeroModel { get; }
        public AbilityService AbilityService { get; }

        public GameModel(ActorModel heroModel, AbilityService abilityService)
        {
            HeroModel = heroModel;
            AbilityService = abilityService;
        }
    }
}