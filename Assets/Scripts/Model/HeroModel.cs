using System;
using System.Collections.Generic;
using Parameters;

namespace Model
{
    /// <summary>
    /// contains hero's current state parameters
    /// </summary>
    public class HeroModel: IDisposable, IHeroParamContainer
    {
        public readonly ReactiveParameter Speed;
        public readonly ReactiveParameter Height;

        public Dictionary<ParamName, ReactiveParameter> Parameters { get; }
        
        public HeroModel(GameplayConfig config)
        {
            Speed = new ReactiveParameter(ParamName.SPEED, config.DefaultSpeed, config.MaxSpeed, 0f);
            Height = new ReactiveParameter(ParamName.HEIGHT, 0, config.MaxHeight, 0);

            Parameters = new Dictionary<ParamName, ReactiveParameter>()
            {
                { ParamName.SPEED, Speed },
                { ParamName.HEIGHT, Height },
            };
        }

        public void Dispose()
        {
            foreach (var pair in Parameters)
            {
                pair.Value.Release();
            }
        }
    }
}