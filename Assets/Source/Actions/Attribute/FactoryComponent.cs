using Entitas;
using System;

namespace Action.Attribute
{
    /// <summary>
    /// This make it easier to instatiate actions.
    /// Todo: Use callBacks instead of OOP.
    /// </summary>
    public struct FactoryComponent  : IComponent
    {
        public Action.ActionCreator ActionFactory;
    }
}
