using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionChildFaction
{
    public FactionTypes.factionTag factionTag;
    public int hierarchyPosition;

    public FactionChildFaction(FactionTypes.factionTag _factionTag, int _hierarchyPosition)
    {
        factionTag = _factionTag;
        hierarchyPosition = _hierarchyPosition;
    }
}
