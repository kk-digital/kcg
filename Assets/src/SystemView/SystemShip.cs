using System;
using UnityEngine;  

namespace SystemView
{
    public class SystemShip
    {
        public OrbitingObjectDescriptor Descriptor;

        public SystemShip()
        {
            Descriptor = new OrbitingObjectDescriptor();
        }

        // this math is a mess
        public bool PlanPath(OrbitingObjectDescriptor Start, OrbitingObjectDescriptor Destination)
        {
            // todo: this function should check whether an encounter is even possible right now
            //       for now this always tries to create an encounter. Sometimes it won't work as the objects
            //       are not lined up in a way where an encounter is possible (which results in the ship missing
            //       the destination).

            // todo: this is also kinda janky at times. fix that

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

            return true;
            // return false if encounter not possible
        }

        public void UpdatePosition(float dt)
        {
            Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();
        }
    }
}
