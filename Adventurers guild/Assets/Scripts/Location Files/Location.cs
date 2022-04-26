using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public int locationId;
    public string locationName;
    public LocationTypes.locationSpecificTag locationTag;
    public OffsetCoordinate OffsetCoordinate;
    public int ownerFactionId;
    public List<int> otherFactionsInLocation;

    public Location(int Id, LocationTypes.locationSpecificTag _locationTag, OffsetCoordinate _offsetCoordinate, int _ownerFactionId = 0)
    {
        otherFactionsInLocation = new List<int>();

        locationId = Id;
        locationName = "l_" + _locationTag + " " + Id;
        locationTag = _locationTag;
        OffsetCoordinate = _offsetCoordinate;
        ownerFactionId = _ownerFactionId;
    }
}
