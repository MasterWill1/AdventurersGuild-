using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public int factionId;
    public FactionTypes.factionTag factionTag;
    public string factionName;
    public int parentFactionId;
    public List<Faction> boundChildFactions;
    public List<Faction> externalChildFactions;
    public List<GoodsNeed> goodsNeeds;
    public GoodsStorage GoodsStorage;
    public int locationId;
    public int hiearchyPos;
    public List<NamedNPC> namedNPCs;
    public List<UnamedNPCGroup> unamedNPCGroups;
    //Relation opinion to other factions including player

    public Faction(int _Id, FactionTypes.factionTag _factionTag, List<GoodsNeed> _goodsNeeds, int _locationId, int _hiearchyPos, 
        int _parentFactionId = 0)
    {
        boundChildFactions = new List<Faction>();
        externalChildFactions = new List<Faction>();
        GoodsStorage = new GoodsStorage();
        namedNPCs = new List<NamedNPC>();
        unamedNPCGroups = new List<UnamedNPCGroup>();

        factionId = _Id;
        factionTag = _factionTag;
        goodsNeeds = _goodsNeeds;
        locationId = _locationId;
        hiearchyPos = _hiearchyPos;
        parentFactionId = _parentFactionId;

        factionName = "f_"+factionTag + " " + factionId.ToString();
    }

    /// <summary>
    /// Creates a faction with all roles filled
    /// </summary>
    /// <param name="factionDef"></param>
    public void fullyPopulate(FactionDef factionDef)
    {
        foreach(FactionApplicableRole factionApplicableRole in factionDef.applicableRoles)
        {
            UnamedNPCGroup unamedNPCGroup = new UnamedNPCGroup(0, factionId, ContractTypes.contractTargetTag.human, 
                factionApplicableRole.maxNumberOfSelf, factionApplicableRole.hierarchyPosition, factionApplicableRole.roleTag);

            unamedNPCGroups.Add(unamedNPCGroup);
        }
    }

    /// <summary>
    /// Gets the total number of people/creatures (named or unnamed) in this faction
    /// </summary>
    /// <returns></returns>
    public int getThisFactionPopulation()
    {
        int count = 0;
        count = count + namedNPCs.Count;

        foreach(UnamedNPCGroup unamedNPCGroup in unamedNPCGroups)
        {
            count = count + unamedNPCGroup.npcGroupCount;
        }
        return count;
    }

    /// <summary>
    /// Gets the population of of this faction and its bound children
    /// </summary>
    /// <returns></returns>
    public int getFactionTotalPopulation()
    {
        int count = getThisFactionPopulation();

        foreach(Faction boundChildFaction in boundChildFactions)
        {
            count = count + boundChildFaction.getThisFactionPopulation();
        }
        return count;
    }

    /// <summary>
    /// Returns the total number of unamed people in faction of certain role type
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public int getTotalNumberOfUnamedRoleType(FactionTypes.roleTag roleTag)
    {
        int count = 0;

        foreach(UnamedNPCGroup unamedNPCGroup in unamedNPCGroups)
        {
            if(unamedNPCGroup.npcGroupRole == roleTag)
            {
                count = count + unamedNPCGroup.npcGroupCount;
            }
        }
        return count;
    }
}
