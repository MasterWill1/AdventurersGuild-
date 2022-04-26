using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPartyChar : MonoBehaviour
{
    public Button removeCharFromPartyButton, characterDetailsButton;
    GameObject PartyHandler, characterDetailsScreenGO;
    GameObject[] CharacterFinder;

    //Generator genScriptCaller;
    aCharacter thisCharCaller;
    PartyStorage PartyStorage;
    characterDetailsScreen characterDetailsScreenScript;

    void Awake()
    {
        Button removeCharBtn = removeCharFromPartyButton.GetComponent<Button>();
        removeCharBtn.onClick.AddListener(removeCharFromNewParty);

        Button characterDetailsBtn = characterDetailsButton.GetComponent<Button>();
        characterDetailsBtn.onClick.AddListener(showCharacterDetails);

        PartyHandler = GameObject.FindWithTag("PartyHandler");
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();

        characterDetailsScreenGO = GameObject.FindWithTag("characterDetails");
        characterDetailsScreenScript = characterDetailsScreenGO.GetComponent<characterDetailsScreen>();

        thisCharCaller = gameObject.GetComponent<aCharacter>();

    }

    void removeCharFromNewParty()
    {
        PartyStorage.removeCharFromNewParty(thisCharCaller.thisCharacterID);

        //reactivate add to new party button
        CharacterFinder = GameObject.FindGameObjectsWithTag("YourAdventurer");
        foreach(GameObject n in CharacterFinder)
        {
            if(n.GetComponent<aCharacter>().thisCharacterID == thisCharCaller.thisCharacterID)
            {
                n.GetComponent<EmployeeCharacter>().reActivateAddToPartyButton();
                break;
            }
        }

        Destroy(gameObject);
    }

    void showCharacterDetails()
    {
        characterDetailsScreenScript.setVisuals(thisCharCaller.thisCharacterID);
    }
}
