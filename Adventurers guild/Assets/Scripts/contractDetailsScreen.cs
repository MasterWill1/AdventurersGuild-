using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class contractDetailsScreen : MonoBehaviour
{
    public GameObject CharHandler, ContractHandler, TraitHandlerAccessor, selfGO, GOchoosePartyForConButton;
    ContractStorage ContractStorage;
    ContractFacade ContractFacade;
    ContractDetailsStorage ContractDetailsStorage;

    //Generator GeneratorScriptAccessor;
    TraitHandler TraitHandlerScriptAccessor;
    //ContractHandler ContractHandlerScriptAccessor;
    public Button closeScreenButton;

    int thisConID;

    [Header("Text boxes")]
    public GameObject TitleTextbox;
    public GameObject difficultyTextbox, timeTextbox, rewardTextbox, goodnessTextbox, locationTextbox;
    // Start is called before the first frame update
    void Start()
    {
        //GeneratorScriptAccessor = GeneratorHandlerAccessor.GetComponent<Generator>();
        TraitHandlerScriptAccessor = TraitHandlerAccessor.GetComponent<TraitHandler>();
        //ContractHandlerScriptAccessor = ContractHandlerAccessor.GetComponent<ContractHandler>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        ContractFacade = ContractHandler.GetComponent<ContractFacade>();
        ContractDetailsStorage = ContractHandler.GetComponent<ContractDetailsStorage>();

        Button closeScreenBtn = closeScreenButton.GetComponent<Button>();
        closeScreenBtn.onClick.AddListener(closeScreen);

        Button choosePartyBtn = GOchoosePartyForConButton.GetComponent<Button>();
        choosePartyBtn.onClick.AddListener(beginPartySelectionForContract);
    }

    public void setVisuals(int conID)
    {
        selfGO.SetActive(true);
        thisConID = conID;

        Contract thisContract = ContractStorage.findContractFromID(conID);
        ContractDetails thisConDetails = thisContract.contractDetails;

        TitleTextbox.GetComponent<Text>().text = "Contract: "+ thisConDetails.contractGoalTag + " the " + thisConDetails.contractTargetTag;
        difficultyTextbox.GetComponent<Text>().text = "Difficulty: " + thisContract.difficulty.ToString();
        rewardTextbox.GetComponent<Text>().text = "Reward: " + thisContract.reward.ToString() + "gp";
        goodnessTextbox.GetComponent<Text>().text = HelperFunctions.goodnessIntToString(thisConDetails.goodness);
        locationTextbox.GetComponent<Text>().text = "Located in the " + thisConDetails.locationSpecificTag
            + " in the "+ thisConDetails.locationBiomeTag;
        updateTimeLeft();

        if (thisContract.isOngoing == true)
        {
            GOchoosePartyForConButton.SetActive(false);
        }
        else if (thisContract.isOngoing == false)
        {
            GOchoosePartyForConButton.SetActive(true);
        }
    }

    public void updateTimeLeft()
    {
        timeTextbox.GetComponent<Text>().text = "Time on Contract: " + ContractStorage.findContractFromID(thisConID).timeLeft.ToString() + " days";
    }

    void closeScreen()
    {
        TitleTextbox.GetComponent<Text>().text = "unset name";
        difficultyTextbox.GetComponent<Text>().text = "unset level";
        timeTextbox.GetComponent<Text>().text = "unset race";
        rewardTextbox.GetComponent<Text>().text = "unset class";
        goodnessTextbox.GetComponent<Text>().text = "unset xp";
        locationTextbox.GetComponent<Text>().text = "unset gender";

        ContractFacade.clearAndCloseChoosePartyForConScreen(true);

        thisConID = -1;
    }

    void beginPartySelectionForContract()
    {
        ContractStorage.setContractAsContractToBeDelegated(thisConID);
        //wipe the choose party screen so we dont duplicate
        ContractFacade.clearAndCloseChoosePartyForConScreen(false);
        //reload it
        ContractFacade.displayAssignPartyToContractCanvas();
    }
}
