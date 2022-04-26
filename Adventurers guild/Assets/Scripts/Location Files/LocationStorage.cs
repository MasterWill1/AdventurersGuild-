using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LocationStorage : MonoBehaviour
{
    public IDictionary<int, BiomeDef> biomeDefDictionary = new Dictionary<int, BiomeDef>();
    public IDictionary<int, LocationDef> locationDefDictionary = new Dictionary<int, LocationDef>();

    public IDictionary<int, Location> locationDictionary = new Dictionary<int, Location>();
    XmlDocument biomeXmlDocument;
    XmlDocument locationXmlDocument;

    public GameObject HexGridHandler;
    HexGrid HexGrid;
    int locationIdCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("---Started loading biome defs.---");
        TextAsset biomesXmlTextAsset = Resources.Load<TextAsset>("XML/Biomes");
        biomeXmlDocument = new XmlDocument();
        biomeXmlDocument.LoadXml(biomesXmlTextAsset.text);
        XmlNodeList biomes = biomeXmlDocument.SelectNodes("/Defs/Biome");

        foreach (XmlNode biome in biomes)
        {
            createBiomeDatum(biome);
        }
        Debug.Log("---Finished loading biome defs. Created " + biomeDefDictionary.Count + " biome defs---");

        Debug.Log("---Started loading location defs.---");
        TextAsset locationsXmlTextAsset = Resources.Load<TextAsset>("XML/Locations");
        locationXmlDocument = new XmlDocument();
        locationXmlDocument.LoadXml(locationsXmlTextAsset.text);
        XmlNodeList locations = locationXmlDocument.SelectNodes("/Defs/Location");

        foreach (XmlNode location in locations)
        {
            createLocationDatum(location);
        }
        Debug.Log("---Finished loading location defs. Created " + locationDefDictionary.Count + " location defs---");

        startupChecks();

        HexGrid = HexGridHandler.GetComponent<HexGrid>();
    }

    public Location spawnLocation(LocationTypes.locationSpecificTag locationTag, OffsetCoordinate offsetCoordinate, int ownerFactionId = 0)
    {
        locationIdCounter++;
        Location newLocation = new Location(locationIdCounter, locationTag, offsetCoordinate, ownerFactionId);

        HexGrid.addLocationToHexTile(newLocation);
        locationDictionary.Add(newLocation.locationId, newLocation);
        return (newLocation);
    }

    public void addFactionToLocation(int locationId, int factionId)
    {
        Location location = getLocationFromDictionary(locationId);

        if(location.ownerFactionId == factionId || location.otherFactionsInLocation.Contains(factionId))
        {
            Debug.LogError("location already contains faction id: " + factionId);
        }
        else
        {
            location.otherFactionsInLocation.Add(factionId);
        }
    }

    public BiomeDef getBiomeDef(LocationTypes.locationBiomeTag locationBiomeTag)
    {
        biomeDefDictionary.TryGetValue((int)locationBiomeTag, out BiomeDef biomeDef);
        if(biomeDef == null)
        {
            Debug.LogError("Biome Def not found for biome tag: " + locationBiomeTag);
        }
        return biomeDef;
    }
    public LocationDef getLocationDef(LocationTypes.locationSpecificTag locationSpecificTag)
    {
        locationDefDictionary.TryGetValue((int)locationSpecificTag, out LocationDef locationDef);
        if (locationDef == null)
        {
            Debug.LogError("Location Def not found for location tag: " + locationSpecificTag);
        }
        return locationDef;
    }
    public Location getLocationFromDictionary(int locationId)
    {
        locationDictionary.TryGetValue(locationId, out Location location);
        if (location == null)
        {
            Debug.LogError("Location not found from Id: " + locationId);
        }
        return location;
    }

    void createBiomeDatum(XmlNode biome)
    {
        BiomeDef newBiome = new BiomeDef(biome);

        biomeDefDictionary.Add((int)newBiome.locationBiomeTag, newBiome);
        Debug.Log("Created Biome: " + newBiome.title);
    }

    void createLocationDatum(XmlNode location)
    {
        LocationDef newLocation = new LocationDef(location);

        locationDefDictionary.Add((int)newLocation.locationTag, newLocation);
        Debug.Log("Created Biome: " + newLocation.title);
    }
    void startupChecks()
    {
        if (biomeDefDictionary.Count != LocationTypes.getAllBiomeTags().Count)
        {
            Debug.LogError("Mismatch between number of biomes generated and number of biome enums. Biomes generated: " + biomeDefDictionary.Count
                + ", biome enums: " + LocationTypes.getAllBiomeTags().Count);
            foreach (LocationTypes.locationBiomeTag biomeTag in LocationTypes.getAllBiomeTags())
            {
                biomeDefDictionary.TryGetValue((int)biomeTag, out BiomeDef thisbiomeDef);
                if (thisbiomeDef == null)
                {
                    Debug.LogError(biomeTag);
                }
            }
        }
    }
}
