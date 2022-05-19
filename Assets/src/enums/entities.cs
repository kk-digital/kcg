using System;

namespace Enums
{
    public static class EnumExt
    {
        // Get string name from any Enum
        // Use it like EntityTypeIds.PlanetMap.GetName();
        public static string GetName(this Enum enumObject)
        {
            return Enum.GetName(enumObject.GetType(), enumObject);
        }
    }
    
    public enum EntityTypeIds
    {
        Error = 0,
        PlanetMap,
        SystemMap,
        SectorMap
    }

    public enum PlanetObjects
    {
        PlanetAgent,
        PlanetItem,
        PlanetFurniture, //containers/machines/plants
        PlanetParticle,
        PlanetVehicle,
    }

    public enum SectorObjects
    {
        SectorShip,
        SectorItem,
        SectorStar,
        SectorPlanet,
        SectorMoon,
        SectorAstroidBelt,
        SectorAstroidField,
        SectorAstroid,
        SectorStation,
        SectorGate,
        SectorHighway,
    }

    public enum Misc
    {
        GlobalFaction, //defines faction information
        GlobalFactionStatistics, //
        GlobalStatistics,
    }
}
