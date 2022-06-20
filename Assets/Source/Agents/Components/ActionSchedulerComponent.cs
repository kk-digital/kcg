using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using System;

namespace Agent
{
    public struct ActionSchedulerComponent : IComponent
    {
        public List<int> ActiveActionIDs;
        public List<int> CoolDownActionIDs;
    }
}
