using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    public OffsetCoordinate OffsetCoordinates;
    public HexCoordinate HexCoordinate;
    public Vector3 worldPosition;
    public LocationTypes.locationBiomeTag biome;
    public List<Location> locations;
    public int controllingFactionId;
    //availableResources
    //tileusage

    public HexTile(int _gridX, int _gridY, Vector3 _worldPosition, LocationTypes.locationBiomeTag _locationBiomeTag = LocationTypes.locationBiomeTag.none)
    {
        locations = new List<Location>();

        worldPosition = _worldPosition;
        OffsetCoordinates = new OffsetCoordinate(_gridX, _gridY);
        HexCoordinate = HexCoordinate.FromOffsetCoordinates(_gridX, _gridY);
        biome = _locationBiomeTag;
    }
}
