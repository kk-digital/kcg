using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Action
{
    // 
    public class ActionSchedulerSystem
    {
        public void Update(Contexts contexts, float deltaTime, ref Planet.PlanetState planet)
        {
            ActionEntity[] actions = contexts.action.GetEntities();

            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].hasActionExecution)
                {
                    switch (actions[i].actionExecution.State)
                    {
                        case Enums.ActionState.Entry:
                            actions[i].actionExecution.Logic.OnEnter(ref planet);
                            break;
                        case Enums.ActionState.Running:
                            actions[i].actionExecution.Logic.OnUpdate(deltaTime, ref planet);
                            break;
                        case Enums.ActionState.Success:
                            actions[i].actionExecution.Logic.OnExit(ref planet);
                            break;
                        case Enums.ActionState.Fail:
                            actions[i].actionExecution.Logic.OnExit(ref planet);
                            break;
                        default:
                            Debug.Log("Not valid Action state.");
                            break;
                    }
                }
            }
        }
    }
}
