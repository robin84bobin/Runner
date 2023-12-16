using System.Collections.Generic;
using Parameters;

namespace Model
{
    public interface IActorParamContainer
    {
        Dictionary<ParamName, ReactiveParameter> Parameters { get; }
    }
}