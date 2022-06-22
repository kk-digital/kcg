
using KMath;
using System.Web.UI.WebControls;
using UnityEngine;

namespace Action
{
    public class FireWeaponAction : ActionBase
    {
        public FireWeaponAction(int actionID, int agentID) : base(actionID, agentID)
        {

        }

        public override void OnEnter()
        {
            
        }
    }

    // Factory Method
    public class FireWeaponActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new MiningLaserToolAction(actionID, agentID);
        }
    }
}

