using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Components;
using Systems;

public class VehicleList
{
    public VehiclePhysicsSystem[] vehiclePhysicsList;
    public VehicleDrawSystem[] vehicleDrawList;
    public VehicleComponentCollider[] vehicleComponentColliderList;
    public VehicleComponentDraw[] vehicleComponentDrawList;
}
