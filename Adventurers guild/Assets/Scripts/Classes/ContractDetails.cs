using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractDetails : MonoBehaviour
{
    public int contractID;
    public int goodness;
    public int locationCoordinate;
    public LocationTypes.locationBiomeTag locationBiomeTag;
    public LocationTypes.locationSpecificTag locationSpecificTag;
    public ContractTypes.contractTargetTag contractTargetTag;
    public ContractTypes.contractGoalTag contractGoalTag;

    public ContractDetails(int _contractID, int _goodness, int _locationCoordinate,
        LocationTypes.locationBiomeTag _locationBiomeTag, LocationTypes.locationSpecificTag _locationSpecificTag, 
        ContractTypes.contractTargetTag _contractTargetTag, ContractTypes.contractGoalTag _contractGoalTag)
    {
        contractID = _contractID;
        goodness = _goodness; //0=evil 10=good
        locationCoordinate = _locationCoordinate;
        locationBiomeTag = _locationBiomeTag;
        locationSpecificTag = _locationSpecificTag;
        contractTargetTag = _contractTargetTag;
        contractGoalTag = _contractGoalTag;
    }
}
