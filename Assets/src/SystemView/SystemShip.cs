using System;

namespace SystemView
{
    public class SystemShip
    {
        public OrbitingObjectDescriptor Descriptor;
        public OrbitingObjectDescriptor Start, Destination;
        public bool PathPlanned = false;
        public bool Reached = false;

        public SystemShip()
        {
            Descriptor = new OrbitingObjectDescriptor();
        }

        private const int segments = 64;

        // this math is a mess
        // todo: clean it up
        public bool PlanPath(OrbitingObjectDescriptor Start, OrbitingObjectDescriptor Destination)
        {
            Descriptor.CenterX = Start.CenterX;
            Descriptor.CenterY = Start.CenterY;

            float StartAltitude = Start.GetDistanceFromCenter();
            float DestinationAltitude = Destination.GetDistanceFromCenter();

            float[] StartPos = Start.GetPosition();

            Descriptor.Rotation = Start.Rotation + Start.RotationalPosition;

            float DestinationPosX = (float)Math.Cos(Descriptor.Rotation);
            float DestinationPosY = (float)Math.Sin(Descriptor.Rotation);

            float[] IntersectionAt = Destination.GetIntersectionWith(DestinationPosY / DestinationPosX);

            DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            float Periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            float Apoapsis  = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            Descriptor.SemiMajorAxis = (Periapsis + Apoapsis) / 2.0f;

            float EccentricDistance = Descriptor.SemiMajorAxis - Periapsis;

            Descriptor.SemiMinorAxis = (float)Math.Sqrt(2 * Descriptor.SemiMajorAxis * Periapsis - Periapsis * Periapsis);

            Descriptor.RotationalPosition = Descriptor.GetRotationalPositionAt(StartPos[0], StartPos[1]);

            float TimeToApoapsis = 0.0f;
            float TargetRotationalMovement = 0.0f;

            for(int i = 0; i < segments; i++)
            {
                float segmentLength = 3.1415926f / segments;
                float altitude = Descriptor.GetDistanceFromCenterAt(segmentLength * i + Descriptor.RotationalPosition);
                float targetAltitude = Destination.GetDistanceFromCenterAt(segmentLength + TargetRotationalMovement + Destination.RotationalPosition);

                float segmentDuration = segmentLength * altitude * altitude;

                TimeToApoapsis += segmentDuration;
                TargetRotationalMovement += segmentDuration / targetAltitude / targetAltitude;
            }

            float[] ApoapsisPos = Descriptor.GetPositionAt(Descriptor.RotationalPosition + 3.1415926f);
            float[] TargetPos = Destination.GetPositionAt(Destination.RotationalPosition + TargetRotationalMovement);

            if(Math.Abs(ApoapsisPos[0] - TargetPos[0]) < 0.25 && Math.Abs(ApoapsisPos[1] - TargetPos[1]) < 0.25)
            {
                return PathPlanned = true;
            }

            Descriptor.SemiMajorAxis = Start.SemiMajorAxis;
            Descriptor.SemiMinorAxis = Start.SemiMinorAxis;
            Descriptor.Rotation = Start.Rotation;
            Descriptor.RotationalPosition = Start.RotationalPosition;

            return PathPlanned = false;
        }

        public void UpdatePosition(float dt)
        {
            Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();
        }
    }
}
