using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    public GameObject FactionHandler, HexGridHandler;
    FactionStorage FactionStorage;
    HexGrid HexGrid;
    public string currentDropdownChoice;

    public Dropdown factionChoiceDropdown;
    void Start()
    {
        FactionStorage = FactionHandler.GetComponent<FactionStorage>();
        HexGrid = HexGridHandler.GetComponent<HexGrid>();
        factionChoiceDropdown.ClearOptions();
        populateFactionDropdown();

        factionChoiceDropdown.onValueChanged.AddListener(delegate { dropdownChoiceChanged(factionChoiceDropdown); });
    }

    void populateFactionDropdown()
    {
        List<string> availableTags = new List<string>();
        availableTags.Add("none");
        foreach(KeyValuePair<int, FactionDef> keyValuePair in FactionStorage.factionDefDictionary)
        {
            //if a faction has a suitable locaiton (ie it does not inherit from its parent and can be independant)
            if(keyValuePair.Value.suitableLocations.Count != 0)
            {
                availableTags.Add(keyValuePair.Value.title);
            }
        }
        factionChoiceDropdown.AddOptions(availableTags);
    }

    void dropdownChoiceChanged(Dropdown dropdown)
    {
        currentDropdownChoice = factionChoiceDropdown.options[factionChoiceDropdown.value].text;
        HexGrid.factionSpawnerTag = currentDropdownChoice;
    }
}
