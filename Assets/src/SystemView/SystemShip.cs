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

        const int segments = 256;

        public bool PlanPath(OrbitingObjectDescriptor Start, OrbitingObjectDescriptor Destination, float AcceptableDeviation)
        {
            // Copy center coordinates from start location
            Descriptor.CenterX = Start.CenterX;
            Descriptor.CenterY = Start.CenterY;

            // Set rotation to match current position of start object
            Descriptor.Rotation = Start.Rotation + Start.RotationalPosition;

            // Calculate intersection point between line with slope perpendicular to our orbit and the destination target's orbit
            float[] IntersectionAt = Destination.GetIntersectionWith((float)Math.Sin(Descriptor.Rotation) / (float)Math.Cos(Descriptor.Rotation));

            // Start altitude = current altitude of start object
            float StartAltitude = Start.GetDistanceFromCenter();

            // Destination altitude = altitude at intersection point
            float DestinationAltitude = Destination.GetDistanceFromCenterAt(Destination.GetRotationalPositionAt(IntersectionAt[0], IntersectionAt[1]));

            // Choose periapsis and apoapsis from start/destination altitude (lower value = periapsis, higher value = apoapsis)
            float Periapsis = StartAltitude < DestinationAltitude ? StartAltitude : DestinationAltitude;
            float Apoapsis  = StartAltitude > DestinationAltitude ? StartAltitude : DestinationAltitude;

            // Semi major axis = the longer "radius" of the ellipse
            // Can be calculated by adding periapsis and apoapsis together as they are the 2 farthest points on the orbit, and then dividing it in half
            Descriptor.SemiMajorAxis = (Periapsis + Apoapsis) / 2.0f;

            // This value represents how far off center the periapsis and apoapsis are, in other words it's the distance between the periapsis/apoapsis and the nearest focal point
            float EccentricDistance = Descriptor.SemiMajorAxis - Periapsis;

            // Semi minor axis can be calculated from periapsis and semi major axis
            // This formula is derived from the two following formulas:
            // 
            // (1) EccentricDistance = sqrt(SemiMajorAxis^2 - SemiMinorAxis^2)
            // (2) EccentricDistance = SemiMajorAxis - Periapsis
            // 
            // Adding the two into one equation gives
            // 
            // SemiMajorAxis - Periapsis = sqrt(SemiMajorAxis^2 - SemiMinorAxis^2)
            // 
            // Which can be farther solved to
            // 
            // (SemiMajorAxis - Periapsis)^2 = SemiMajorAxis^2 - SemiMinorAxis^2
            // (SemiMajorAxis - Periapsis)^2 - SemiMajorAxis^2 = -SemiMinorAxis^2
            // 
            // And finally taking the negative of the square root you get
            // 
            // Sqrt(SemiMajorAxis^2 - (SemiMajorAxis - Periapsis)^2) = SemiMinorAxis
            // 
            // The term (SemiMajorAxis - Periapsis)^2 can then be expanded and you end up with the following formula

            Descriptor.SemiMinorAxis = (float)Math.Sqrt(2 * Descriptor.SemiMajorAxis * Periapsis - Periapsis * Periapsis);

            float[] StartPos = Start.GetPosition();
            Descriptor.RotationalPosition = 0.0f;

            // Estimating time to apoapsis and how far the target will have moved in that time to try and see
            // whether the orbits are lined up in a way that an encounter is possible.
            
            // This is called apoapsis, but might be the periapsis if we are travelling from high altitude to low altitude.
            // Nevertheless, this makes no difference for the calculation.

            float TimeToApoapsis = 0.0f;
            float TargetRotationalMovement = 0.0f;
            
            // This could be an integral. However, after messing around with it I'm not sure it would be any faster
            // than this estimate, and this is definitely a lot easier and simpler to read.
            for (int i = 0; i < segments; i++)
            {
                // Total distance from periapsis to apoapsis is 180 degrees (pi) - so each segment is (pi / amount of segments) long
                float segmentLength = 3.1415926f / segments;

                // Use the segment length to calculate the altitude the ship and destination object will reach after this segment
                float altitude = Descriptor.GetDistanceFromCenterAt(segmentLength * i + Descriptor.RotationalPosition);
                float targetAltitude = Destination.GetDistanceFromCenterAt(segmentLength + TargetRotationalMovement + Destination.RotationalPosition);

                // We then use the altitude to calculate how much time passed, as d = t / altitude^2
                float segmentDuration = segmentLength * altitude * altitude;

                // Add values to our counters
                TimeToApoapsis += segmentDuration;
                TargetRotationalMovement += segmentDuration / targetAltitude / targetAltitude;
            }

            // Turn rotational positions into position vectors
            float[] ApoapsisPos = Descriptor.GetPositionAt(3.1415926f);
            float[] TargetPos = Destination.GetPositionAt(Destination.RotationalPosition + TargetRotationalMovement);

            // Check whether apoapsis is close enough to where the target will be to ensure an encounter
            if (Math.Sqrt((ApoapsisPos[0] - TargetPos[0]) * (ApoapsisPos[0] - TargetPos[0]) + (ApoapsisPos[1] - TargetPos[1]) * (ApoapsisPos[1] - TargetPos[1])) < AcceptableDeviation)
            {
                return PathPlanned = true;
            }

            // We were not able to establish an encounter - Reset our descriptor values to be the same as those of the start object
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
