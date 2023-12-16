namespace Model
{
    public class GameModel
    {
        public ActorModel ActorModel { get; }
        public AbilitiesModel AbilitiesModel { get; }

        public GameModel(ActorModel actorModel, AbilitiesModel abilitiesModel)
        {
            ActorModel = actorModel;
            AbilitiesModel = abilitiesModel;
        }
    }
}