using Entitas;
using System;

namespace Action.Property
{
    /// <summary>
    /// This make it easier to instatiate actions.
    /// Todo: Use callBacks instead of OOP.
    /// </summary>
    [ActionProperties]
    public class FactoryComponent  : IComponent
    {
        public Action.ActionCreator ActionFactory;
    }
}
