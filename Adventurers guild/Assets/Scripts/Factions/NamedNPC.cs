using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedNPC
{
    public int npcId;
    public int npcFactionId;
    public int npcHiearchyPos;
    public string npcName;
    public FactionTypes.roleTag npcRole;
    //npc strength
    //npc wealth
    //npc owned items
    //npc opinions/relationships
    public NamedNPC(int _npcId, int _npcFactionId, int _npcHiearchyPos, string _npcName, FactionTypes.roleTag _npcRole)
    {
        npcId = _npcId;
        npcFactionId = _npcFactionId;
        npcHiearchyPos = _npcHiearchyPos;
        npcName = _npcName;
        npcRole = _npcRole;
    }

}
