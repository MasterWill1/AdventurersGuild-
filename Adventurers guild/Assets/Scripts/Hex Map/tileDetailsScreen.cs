using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tileDetailsScreen : MonoBehaviour
{
    public GameObject biomeTextbox, biomeDescription, resourcesAvailableScrollAreaContainer, factionListScrollAreaContainer,
        locationListScrollAreaContainer, factionBarPrefab, locationBarPrefab, hexGridHandler, factionHandler, LocationHandler, selfScreen;

    List<GameObject> allLocationBarsList, allFactionBarsList;

    HexGrid HexGrid;
    LocationStorage LocationStorage;
    FactionStorage FactionStorage;
    private void Start()
    {
        HexGrid = hexGridHandler.GetComponent<HexGrid>();
        LocationStorage = LocationHandler.GetComponent<LocationStorage>();
        FactionStorage = factionHandler.GetComponent<FactionStorage>();

        allLocationBarsList = new List<GameObject>();
        allFactionBarsList = new List<GameObject>();
    }

    public void setVisuals(OffsetCoordinate offsetCoordinate)
    {
        //reset the menu
        closeMenu();

        HexTile thisHexTile = HexGrid.getHexTile(offsetCoordinate);
        BiomeDef thisTileBiomeDef = LocationStorage.getBiomeDef(thisHexTile.biome);

        biomeTextbox.GetComponent<Text>().text = thisTileBiomeDef.title;
        biomeDescription.GetComponent<Text>().text = thisTileBiomeDef.description;

        foreach (Location location in thisHexTile.locations)
        {
            instantiateNewLocationBar(location);

            if(location.ownerFactionId > 0)
            {
                instantiateNewFactionBar(FactionStorage.getFactionFromDictionary(location.ownerFactionId));
            }

            foreach(int factionId in location.otherFactionsInLocation)
            {
                instantiateNewFactionBar(FactionStorage.getFactionFromDictionary(factionId));
            }
        }

        selfScreen.SetActive(true);
    }

    
    void instantiateNewLocationBar(Location location)
    {
        GameObject newLocationBar = Instantiate(locationBarPrefab);
        newLocationBar.transform.SetParent(locationListScrollAreaContainer.transform, false);
        allLocationBarsList.Add(newLocationBar);

        aLocationBar aLocationBar = newLocationBar.GetComponent<aLocationBar>();
        aLocationBar.setSelf(location);
    }

    void instantiateNewFactionBar(Faction faction)
    {
        GameObject newBar = Instantiate(factionBarPrefab);
        newBar.transform.SetParent(factionListScrollAreaContainer.transform, false);
        allFactionBarsList.Add(newBar);

        aFactionBar aFactionBar = newBar.GetComponent<aFactionBar>();
        aFactionBar.setSelf(faction);
    }
    
    public void closeMenu()
    {
        biomeTextbox.GetComponent<Text>().text = "unset biome";
        biomeDescription.GetComponent<Text>().text = "unset biome description";

        foreach (GameObject g in allLocationBarsList)
        {
            Destroy(g);
        }
        foreach (GameObject g in allFactionBarsList)
        {
            Destroy(g);
        }
    }
}