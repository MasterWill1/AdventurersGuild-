using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionApplicableRole
{
    public FactionTypes.roleTag roleTag;
    public int maxNumberOfSelf;
    public int hierarchyPosition;

    public FactionApplicableRole(FactionTypes.roleTag _roleTag, int _maxNumberOfSelf, int _hierarchyPosition)
    {
        roleTag = _roleTag;
        maxNumberOfSelf = _maxNumberOfSelf;
        hierarchyPosition = _hierarchyPosition;
    }
}
