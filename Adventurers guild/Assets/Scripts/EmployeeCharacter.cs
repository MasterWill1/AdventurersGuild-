using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeCharacter : MonoBehaviour
{
    public Button addToPartyButton, characterDetailsButton;
    public GameObject addToPartyButtonGameObject;
    GameObject CharHandler, PartyHandler, characterDetailsScreenGO;
    CharStorage CharStorage;
    PartyFacade PartyFacade;
    //Generator genScriptCaller;
    aCharacter thisCharCaller;
    characterDetailsScreen characterDetailsScreenScript;

    // Start is called before the first frame update
    void Awake()
    {
        Button addToPartyBtn = addToPartyButton.GetComponent<Button>();
        addToPartyBtn.onClick.AddListener(addCharToParty);

        Button characterDetailsBtn = characterDetailsButton.GetComponent<Button>();
        characterDetailsBtn.onClick.AddListener(showCharacterDetails);

        //genCaller = GameObject.FindWithTag("GenHandler");
        //genScriptCaller = genCaller.GetComponent<Generator>();
        CharHandler = GameObject.FindWithTag("CharHandler");
        PartyHandler = GameObject.FindWithTag("PartyHandler");
        CharStorage = CharHandler.GetComponent<CharStorage>();
        PartyFacade = PartyHandler.GetComponent<PartyFacade>();

        characterDetailsScreenGO = GameObject.FindWithTag("characterDetails");
        characterDetailsScreenScript = characterDetailsScreenGO.GetComponent<characterDetailsScreen>();

        thisCharCaller = gameObject.GetComponent<aCharacter>();

        foreach (CharacterDetails c in CharStorage.characterDetailsList)
        {
            if (c.ID == thisCharCaller.thisCharacterID)
            {
                //if character is in party
                if (c.inParty != 0)
                {
                    addToPartyButtonGameObject.SetActive(false);
                }
                break;
            }
        }
    }

    void addCharToParty()
    {
        PartyFacade.addEmployeeToNewParty(thisCharCaller.thisCharacterID);
        PartyFacade.NewPartyScreen.SetActive(true);

        deActivateAddToPartyButton();
    }

    public void deActivateAddToPartyButton()
    {
        addToPartyButtonGameObject.SetActive(false);
    }

    public void reActivateAddToPartyButton()
    {
        addToPartyButtonGameObject.SetActive(true);
    }

    public void decideOnReActivateAddToPartyButton()
    {
        foreach (CharacterDetails c in CharStorage.characterDetailsList)
        {
            if (c.ID == thisCharCaller.thisCharacterID)
            {
                //if character is not in party
                if (c.inParty == 0)
                {
                    addToPartyButtonGameObject.SetActive(true);
                }
                else
                {
                    addToPartyButtonGameObject.SetActive(false);
                }
            }
        }
    }

    void showCharacterDetails()
    {
        characterDetailsScreenScript.setVisuals(thisCharCaller.thisCharacterID);
    }
}
