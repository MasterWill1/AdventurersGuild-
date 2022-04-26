using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class FactionStorage : MonoBehaviour
{
    public GameObject LocationHandler, HexGridHandler;
    LocationStorage LocationStorage;
    HexGrid HexGrid;
    public IDictionary<int, FactionDef> factionDefDictionary = new Dictionary<int, FactionDef>();
    public IDictionary<string, FactionDef> factionDefStringDictionary = new Dictionary<string, FactionDef>();
    public IDictionary<int, GoodsNeed> goodsNeedDictionary = new Dictionary<int, GoodsNeed>();

    public IDictionary<int, Faction> factionDictionary = new Dictionary<int, Faction>();
    XmlDocument factionXmlDocument, goodsNeedsXmlDocument;
    int factionIdCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        loadFactionDefs();
        loadGoodsNeedsDefs();

        LocationStorage = LocationHandler.GetComponent<LocationStorage>();
        HexGrid = HexGridHandler.GetComponent<HexGrid>();

        startupChecks();
    }

    public void spawnIndependantFaction(string factionString, OffsetCoordinate offsetCoordinate)
    {
        factionIdCounter++;
        FactionDef factionDef = getFactionDefFromDictionary(factionString);
        int factionLocationId = 0;

        //Spawn the factions Location
        if (factionDef.suitableLocations.Count != 0)
        {
            Location factionLocation = LocationStorage.spawnLocation(factionDef.suitableLocations[0], offsetCoordinate, factionIdCounter);
            factionLocationId = factionLocation.locationId;
        }
        else
        {
            Debug.LogError("No Location available for spawning of independant faction: " + factionString);
        }

        List <GoodsNeed> needs = new List<GoodsNeed>();
        needs.Add(getGoodsNeedDefFromDictionary(FactionTypes.tradeableGood.rawFood));

        HexGrid.changeTileController(offsetCoordinate, factionIdCounter);

        Faction faction = new Faction(factionIdCounter, factionDef.factionTag, needs, factionLocationId, 0);
        fullyPopulateFaction(faction);

        factionDictionary.Add(faction.factionId, faction);
    }


    public void spawnBoundChildFaction(string factionString, Faction parentFaction)
    {
        factionIdCounter++;
        FactionDef factionDef = getFactionDefFromDictionary(factionString);
        FactionDef parentFactionDef = getFactionDefFromDictionary(parentFaction.factionTag);

        List<GoodsNeed> goodsNeeds = new List<GoodsNeed>();

        Faction faction = new Faction
            (factionIdCounter, factionDef.factionTag, goodsNeeds, parentFaction.locationId, 
            getHiearchyPositionOfChildFaction(parentFactionDef, factionDef.factionTag), parentFaction.factionId);
        LocationStorage.addFactionToLocation(parentFaction.locationId, faction.factionId);
        parentFaction.boundChildFactions.Add(faction);

        fullyPopulateFaction(faction);

        factionDictionary.Add(faction.factionId, faction);

    }

    void fullyPopulateFaction(Faction faction)
    {
        FactionDef factionDef = getFactionDefFromDictionary(faction.factionTag);

        faction.fullyPopulate(factionDef);
    }

    int getHiearchyPositionOfChildFaction(FactionDef parentFactionDef, FactionTypes.factionTag childfactionTag)
    {
        foreach(FactionChildFaction factionChildFaction in parentFactionDef.boundChildFactions)
        {
            if(factionChildFaction.factionTag == childfactionTag)
            {
                return factionChildFaction.hierarchyPosition;
            }
        }

        //no matching tag was found
        Debug.LogError("No bound child hiearchy for faction tag: " + childfactionTag + " in parent faction def " 
            + parentFactionDef.factionTag + " was found");
        return 0;
    }

    void loadFactionDefs()
    {
        Debug.Log("---Started loading faction defs.---");

        TextAsset factionXmlTextAsset = Resources.Load<TextAsset>("XML/Factions");

        factionXmlDocument = new XmlDocument();
        factionXmlDocument.LoadXml(factionXmlTextAsset.text);

        XmlNodeList factions = factionXmlDocument.SelectNodes("/Defs/Faction");

        foreach (XmlNode faction in factions)
        {
            createFactionDatum(faction);
        }

        Debug.Log("---Finished loading faction defs. Created " + factionDefDictionary.Count + " faction defs---");
    }

    void loadGoodsNeedsDefs()
    {
        Debug.Log("---Started loading goods needs defs.---");

        TextAsset goodsNeedsxmlTextAsset = Resources.Load<TextAsset>("XML/GoodsNeeds");

        goodsNeedsXmlDocument = new XmlDocument();
        goodsNeedsXmlDocument.LoadXml(goodsNeedsxmlTextAsset.text);

        XmlNodeList goodsNeeds = goodsNeedsXmlDocument.SelectNodes("/Defs/GoodsNeed");

        foreach (XmlNode goodNeed in goodsNeeds)
        {
            createGoodsNeedDatum(goodNeed);
        }

        Debug.Log("---Finished loading goods needs defs. Created " + goodsNeedDictionary.Count + " goods needs defs---");
    }

    public Faction getFactionFromDictionary(int factionId)
    {
        factionDictionary.TryGetValue(factionId, out Faction faction);
        if (faction == null)
        {
            Debug.LogError("Cannot find a faction using Id: " + factionId);
        }
        return faction;
    }

    public FactionDef getFactionDefFromDictionary(FactionTypes.factionTag factionTag)
    {
        factionDefDictionary.TryGetValue((int)factionTag, out FactionDef factionDef);
        if (factionDef == null)
        {
            Debug.LogError("Cannot find a faction def using tag Id: " + factionTag);
        }
        return factionDef;
    }

    public FactionDef getFactionDefFromDictionary(string factionString)
    {
        factionDefStringDictionary.TryGetValue(factionString, out FactionDef factionDef);
        if(factionDef == null)
        {
            Debug.LogError("Cannot find a faction def using string Id: " + factionString);
        }
        return factionDef;
    }
    public GoodsNeed getGoodsNeedDefFromDictionary(FactionTypes.tradeableGood tradeableGood)
    {
        goodsNeedDictionary.TryGetValue((int)tradeableGood, out GoodsNeed goodsNeed);
        if (goodsNeed == null)
        {
            Debug.LogError("Cannot find a goods needs def using tag: " + tradeableGood);
        }
        return goodsNeed;
    }
    void createFactionDatum(XmlNode faction)
    {
        FactionDef newFaction = new FactionDef(faction);

        factionDefDictionary.Add((int)newFaction.factionTag, newFaction);
        factionDefStringDictionary.Add(newFaction.title, newFaction);
        Debug.Log("Created faction def: " + newFaction.title);
    }

    void createGoodsNeedDatum(XmlNode goodsNeed)
    {
        GoodsNeed newGoodsNeed = new GoodsNeed(goodsNeed);

        goodsNeedDictionary.Add((int)newGoodsNeed.Good, newGoodsNeed);
        Debug.Log("Created goods def: " + newGoodsNeed.Good);
    }

    void startupChecks()
    {
        if (factionDefDictionary.Count != FactionTypes.getAllFactionTags().Count)
        {
            Debug.LogError("Mismatch between number of factions generated and number of faction enums. Factions generated: " + factionDefDictionary.Count
                + ", Faction enums: " + FactionTypes.getAllFactionTags().Count);
            foreach (FactionTypes.factionTag factionTag in FactionTypes.getAllFactionTags())
            {
                factionDefDictionary.TryGetValue((int)factionTag, out FactionDef thisFactionDef);
                if (thisFactionDef == null)
                {
                    Debug.LogError(factionTag);
                }
            }
        }
    }
}
