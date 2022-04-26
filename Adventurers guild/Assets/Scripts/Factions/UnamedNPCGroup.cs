using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnamedNPCGroup
{
    public int npcGroupId;
    public int npcGroupFactionId;
    public ContractTypes.contractTargetTag npcGroupRace;
    public int npcGroupCount;
    public int npcGroupHiearchyPos;
    public FactionTypes.roleTag npcGroupRole;

    public UnamedNPCGroup(int _npcGroupId, int _npcGroupFactionId, ContractTypes.contractTargetTag _npcGroupRace, int _npcGroupCount,
        int _npcGroupHiearchyPos, FactionTypes.roleTag _npcGroupRole)
    {
        npcGroupId = _npcGroupId;
        npcGroupFactionId = _npcGroupFactionId;
        npcGroupRace = _npcGroupRace;
        npcGroupCount = _npcGroupCount;
        npcGroupHiearchyPos = _npcGroupHiearchyPos;
        npcGroupRole = _npcGroupRole;
    }
}
