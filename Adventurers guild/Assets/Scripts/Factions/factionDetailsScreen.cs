using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class factionDetailsScreen : MonoBehaviour
{
    public GameObject titleTextbox, descriptionTextbox, factionBarPrefab, boundChildFactionListScrollArea, externalChildFactionListScrollArea,
        totalPopTextbox ,factionHandler, selfScreen;
    List<GameObject> allFactionBarsList;

    Faction thisFaction;
    FactionDef thisFactionDef;
    FactionStorage FactionStorage;

    public Button addChildFactionButton;
    public Dropdown boundChildFactionChoiceDropdown;

    private void Start()
    {
        allFactionBarsList = new List<GameObject>();
        FactionStorage = factionHandler.GetComponent<FactionStorage>();

        addChildFactionButton.onClick.AddListener(spawnChildFaction);
    }

    public void setVisuals(Faction faction)
    {
        closeMenu();

        selfScreen.SetActive(true);

        thisFaction = faction;
        thisFactionDef = FactionStorage.getFactionDefFromDictionary(faction.factionTag);

        titleTextbox.GetComponent<Text>().text = thisFaction.factionName;
        descriptionTextbox.GetComponent<Text>().text = thisFactionDef.description;
        totalPopTextbox.GetComponent<Text>().text = thisFaction.getFactionTotalPopulation().ToString();


        foreach (Faction f in thisFaction.boundChildFactions)
        {
            instantiateNewBoundChildFactionBar(f);
        }
        foreach (Faction f in thisFaction.externalChildFactions)
        {
            instantiateNewExternalChildFactionBar(f);
        }

        populateChildFactonsChoiceDropdown();
    }

    void closeMenu()
    {
        //remove all the faction bars
        foreach(GameObject g in allFactionBarsList)
        {
            Destroy(g);
        }
        boundChildFactionChoiceDropdown.ClearOptions();

        titleTextbox.GetComponent<Text>().text = "unset faction title";
        descriptionTextbox.GetComponent<Text>().text = "unset faction description";
    }

    void instantiateNewBoundChildFactionBar(Faction faction)
    {
        GameObject newBar = Instantiate(factionBarPrefab);
        newBar.transform.SetParent(boundChildFactionListScrollArea.transform, false);
        allFactionBarsList.Add(newBar);

        aFactionBar aFactionBar = newBar.GetComponent<aFactionBar>();
        aFactionBar.setSelf(faction);
    }

    /// <summary>
    /// Gets the currently viewed factions potential bound child factions and adds it to dropdown
    /// </summary>
    void populateChildFactonsChoiceDropdown()
    {
        List<string> availableTags = new List<string>();
        foreach(FactionChildFaction faction in thisFactionDef.boundChildFactions)
        {
            FactionDef factionDef = FactionStorage.getFactionDefFromDictionary(faction.factionTag);
            availableTags.Add(factionDef.title);
        }
        boundChildFactionChoiceDropdown.AddOptions(availableTags);
    }

    void spawnChildFaction()
    {
        FactionStorage.spawnBoundChildFaction
            (boundChildFactionChoiceDropdown.options[boundChildFactionChoiceDropdown.value].text, thisFaction);

        setVisuals(thisFaction);
    }

    void instantiateNewExternalChildFactionBar(Faction faction)
    {
        GameObject newBar = Instantiate(factionBarPrefab);
        newBar.transform.SetParent(externalChildFactionListScrollArea.transform, false);
        allFactionBarsList.Add(newBar);

        aFactionBar aFactionBar = newBar.GetComponent<aFactionBar>();
        aFactionBar.setSelf(faction);
    }
}
