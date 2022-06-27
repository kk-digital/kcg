using Entitas;
using Planet;

namespace Action
{
    public class ReloadAction : ActionBase
    {
        private ItemPropertiesEntity ItemPropretiesEntity;

        public ReloadAction(int actionID) : base(actionID)
        {
            var ItemEntity = Contexts.sharedInstance.game.GetEntityWithItemIDID(ActionEntity.actionItem.ItemID);
            ItemPropretiesEntity = Contexts.sharedInstance.itemProperties.GetEntityWithItemProperty(ItemEntity.itemID.ItemType);
        }

        public override void OnEnter(ref PlanetState planet)
        {
            // Todo: start playing some animation
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
        }

        public override void OnUpdate(float deltaTime, ref PlanetState planet)
        {
           
        }

        public override void OnExit(ref PlanetState planet)
        {
            base.OnExit(ref planet);
        }
    }


    public class ReloadActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID)
        {
            return new ReloadAction(actionID);
        }
    }
}
