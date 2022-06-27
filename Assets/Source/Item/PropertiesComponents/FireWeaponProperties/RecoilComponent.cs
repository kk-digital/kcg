using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property.FireWeapon
{
    /// <summary>
    /// Define Accuracy of the firegun.
    /// MaxRecoilAngle -> Max Cone angle which shooted bullets can go to.
    /// MinRecoilAngle -> Cone angle of the first bullet.
    /// RateOfChange -> How much cone angle is increased after every shoot
    /// RecoverTime -> How long it takes for recoil to go back to min from MaxRecoilAngle in seconds.
    /// RecoverDelay -> How long it takes to recoil startRecovering in seconds.
    /// </summary>
    [ItemProperties]
    public struct RecoilComponent : IComponent
    {
        public float MaxRecoilAngle;
        public float MinRecoilAngle;
        public float RateOfChange; 
        public float RecoverTime;
        public float RecoverDelay;
    }
}
