using System.Collections.Generic;
using Parameters;

namespace Model
{
    public interface IHeroParamContainer
    {
        Dictionary<ParamName, ReactiveParameter> Parameters { get; }
    }
}