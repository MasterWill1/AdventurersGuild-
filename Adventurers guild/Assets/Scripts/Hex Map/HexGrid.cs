using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
public class HexGrid : MonoBehaviour
{
    public int gridWidth = 6;
    public int gridHeight = 6;
    //float width;
    //float height;
    public HexTile[,] HexTileGridArray;
    public GameObject tileDefaultPrefab, tilePlainsPrefab, tileHillsPrefab, tileMountainsPrefab, tileDesertPrefab, tileJunglePrefab, tileForestPrefab, 
        FactionHandler, tileDetailsScreenGO;
    public List<GameObject> allHexTiles;
    public LocationTypes.locationBiomeTag tileEditorTag = LocationTypes.locationBiomeTag.none;
    public string factionSpawnerTag = "none";
    FactionStorage FactionStorage;
    tileDetailsScreen tileDetailsScreen;
    public enum debugOption { debugOffsetCoordinates, debugHexCoordinates }
    public debugOption DebugOption;

    // Start is called before the first frame update
    void Start()
    {
        FactionStorage = FactionHandler.GetComponent<FactionStorage>();

        HexTileGridArray = new HexTile[gridWidth, gridHeight];
        allHexTiles = new List<GameObject>();
        tileDetailsScreen = tileDetailsScreenGO.GetComponent<tileDetailsScreen>();
        CreateHexGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            HexCoordinate hexCoordinate = HexCoordinate.FromPosition(worldPosition);
            OffsetCoordinate offsetCoordinate = GridConversions.OffsetfromHexCoordinate(hexCoordinate);
            if(offsetCoordinate.X < gridWidth && offsetCoordinate.X >= 0 && offsetCoordinate.Y < gridHeight && offsetCoordinate.Y >= 0)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    HexTile hexTile = HexTileGridArray[offsetCoordinate.X, offsetCoordinate.Y];

                    //if the tile editor has a correct tag then change the biome of selected hex
                    if (tileEditorTag != LocationTypes.locationBiomeTag.none)
                    {                        
                        hexTile.biome = tileEditorTag;
                        rebuildTiles();
                    }
                    //if the faction spawner has a valid tag and the tile isnt controlled then spawn the faction at the location
                    else if (factionSpawnerTag != "none" && hexTile.controllingFactionId == 0)
                    {
                        FactionStorage.spawnIndependantFaction(factionSpawnerTag, offsetCoordinate);
                    }
                    //if we dont want to change anything, then view the tile
                    else
                    {
                        tileDetailsScreenGO.SetActive(true);
                        tileDetailsScreen.setVisuals(offsetCoordinate);
                    }
                }

            }
        }
    }

    public void CreateHexGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPoint = GridConversions.worldPointFromOffsetCoordinates(x, y); 

                HexTileGridArray[x,y] = new HexTile(x, y, worldPoint);
            }
        }
        buildTiles();
    }

    public void addLocationToHexTile(Location location)
    {
        HexTile hexTile = getHexTile(location.OffsetCoordinate);
        hexTile.locations.Add(location);
    }

    public void changeTileController(OffsetCoordinate offsetCoordinate, int factionId)
    {
        getHexTile(offsetCoordinate).controllingFactionId = factionId;
    }

    public HexTile getHexTile(OffsetCoordinate offsetCoordinate)
    {
        return HexTileGridArray[offsetCoordinate.X, offsetCoordinate.Y];
    }

    public void loadHexTile(int x, int y, LocationTypes.locationBiomeTag locationBiomeTag)
    {
        Vector3 worldPoint = GridConversions.worldPointFromOffsetCoordinates(x, y);

        HexTileGridArray[x, y] = new HexTile(x, y, worldPoint, locationBiomeTag);
    }

    void buildTiles()
    {
        foreach(HexTile hexTile in HexTileGridArray)
        {
            GameObject tilePrefabToUse;
            switch (hexTile.biome)
            {
                case LocationTypes.locationBiomeTag.plains:
                    tilePrefabToUse = tilePlainsPrefab;
                    break;
                case LocationTypes.locationBiomeTag.hills:
                    tilePrefabToUse = tileHillsPrefab;
                    break;
                case LocationTypes.locationBiomeTag.mountains:
                    tilePrefabToUse = tileMountainsPrefab;
                    break;
                case LocationTypes.locationBiomeTag.desert:
                    tilePrefabToUse = tileDesertPrefab;
                    break;
                case LocationTypes.locationBiomeTag.jungle:
                    tilePrefabToUse = tileJunglePrefab;
                    break;
                case LocationTypes.locationBiomeTag.forest:
                    tilePrefabToUse = tileForestPrefab;
                    break;
                default:
                    tilePrefabToUse = tileDefaultPrefab;
                    break;
            }
            GameObject gameTile = Instantiate(tilePrefabToUse, hexTile.worldPosition, Quaternion.identity);
            gameTile.transform.SetParent(this.gameObject.transform);
            allHexTiles.Add(gameTile);
        }
    }

    public void rebuildTiles()
    {
        for(int i = 0; i < allHexTiles.Count; i++)
        {
            Destroy(allHexTiles[i]);
        }
        allHexTiles.Clear();
        buildTiles();
    }

    private void OnDrawGizmos()
    {
        if(HexTileGridArray != null)
        {
            foreach (HexTile hexTile in HexTileGridArray)
            {
                Vector3 worldPos = hexTile.worldPosition;
                worldPos.z = worldPos.z - 1;
                if (DebugOption == debugOption.debugOffsetCoordinates)
                {
                    string s = hexTile.OffsetCoordinates.ToString();
                    Handles.Label(worldPos, hexTile.OffsetCoordinates.ToStringOnSeparateLines());
                }
                else if (DebugOption == debugOption.debugHexCoordinates)
                {
                    Handles.Label(worldPos, hexTile.HexCoordinate.ToString());
                }
            }
        }
    }    
    
    public void isPlains()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.plains;
    }
    public void isHills()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.hills;
    }
    public void isMountains()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.mountains;
    }
    public void isDesert()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.desert;
    }
    public void isJungle()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.jungle;
    }
    public void isForest()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.forest;
    }
    public void disableBiome()
    {
        tileEditorTag = LocationTypes.locationBiomeTag.none;
    }
}
