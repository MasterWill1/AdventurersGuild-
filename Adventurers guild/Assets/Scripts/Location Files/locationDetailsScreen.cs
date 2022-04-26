using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class locationDetailsScreen : MonoBehaviour
{
    public Button ControllingFactionButton;
    public GameObject LocationTextbox, descriptionTextbox, factionBarPrefab, factionListScrollArea, locationHandler, selfScreen, FactionHandler, 
        factionDetailsScreenGO;
    Location thisLocation;
    Faction controllingFaction;

    LocationStorage LocationStorage;
    FactionStorage FactionStorage;
    factionDetailsScreen factionDetailsScreen;
    List<GameObject> allFactionBarsList = new List<GameObject>();
    private void Start()
    {
        FactionStorage = FactionHandler.GetComponent<FactionStorage>();
        LocationStorage = locationHandler.GetComponent<LocationStorage>();
        factionDetailsScreen = factionDetailsScreenGO.GetComponent<factionDetailsScreen>();

        ControllingFactionButton.onClick.AddListener(viewControllingFaction);
    }

    public void setVisuals(Location location)
    {
        //reset the view
        closeMenu();

        thisLocation = location;
        LocationDef thisLocationDef = LocationStorage.getLocationDef(thisLocation.locationTag);
        controllingFaction = FactionStorage.getFactionFromDictionary(thisLocation.ownerFactionId);

        LocationTextbox.GetComponent<Text>().text = thisLocation.locationName;
        descriptionTextbox.GetComponent<Text>().text = thisLocationDef.description;

        ControllingFactionButton.GetComponentInChildren<Text>().text = controllingFaction.factionName;

        foreach(int factionId in location.otherFactionsInLocation)
        {
            Faction thisFaction = FactionStorage.getFactionFromDictionary(factionId);
            instantiateNewFactionBar(thisFaction);
        }

        selfScreen.SetActive(true);
    }

    void instantiateNewFactionBar(Faction faction)
    {
        GameObject newBar = Instantiate(factionBarPrefab);
        newBar.transform.SetParent(factionListScrollArea.transform, false);
        allFactionBarsList.Add(newBar);

        aFactionBar aFactionBar = newBar.GetComponent<aFactionBar>();
        aFactionBar.setSelf(faction);
    }

    void viewControllingFaction()
    {
        factionDetailsScreen.setVisuals(controllingFaction);
    }

    void closeMenu()
    {
        LocationTextbox.GetComponent<Text>().text = "unset location";
        descriptionTextbox.GetComponent<Text>().text = "unset description";

        foreach(GameObject g in allFactionBarsList)
        {
            Destroy(g);
        }
    }
}
