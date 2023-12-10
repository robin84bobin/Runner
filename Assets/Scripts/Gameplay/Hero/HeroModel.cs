using System.Collections.Generic;
using Parameters;

namespace Gameplay.Hero
{
    public class HeroModel
    {
        public readonly Parameter Speed;
        public readonly Parameter Height;

        public Dictionary<ParamName, Parameter> Parameters { get; private set; }
        public HeroModel(ProjectConfig config)
        {
            Speed = new Parameter(ParamName.SPEED, config.DefaultSpeed, config.MaxSpeed, 0f);
            Height = new Parameter(ParamName.HEIGHT, 0, config.MaxHeight, 0);

            Parameters = new Dictionary<ParamName, Parameter>()
            {
                { ParamName.SPEED, Speed },
                { ParamName.HEIGHT, Height },
            };
        }
    }
}