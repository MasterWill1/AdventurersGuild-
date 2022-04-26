using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contractResultPredictionScreen : MonoBehaviour
{

    public GameObject resultScreenScrollView, resultScreenCharacterPrefab, ContractHandler; //conCaller, genCaller;
    public Button closeMenuButton, confirmButton;

    [Header("Text Boxes")]
    public GameObject PartyNameText;
    public GameObject goldGainedText, successChanceText, contractNameTitleText, contractDifficultyText, timeTakenForContractText;

    [HideInInspector]
    public List<GameObject> CharacterRowList;

    //Generator generatorScriptCaller;
    //ContractHandler conScriptCaller;
    ContractStorage ContractStorage;
    ContractFacade ContractFacade;

    int partyForContractID = 0;
    int contractID = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Button closeMenuBtn = closeMenuButton.GetComponent<Button>();
        closeMenuBtn.onClick.AddListener(closeMenu);

        Button confirmBtn = confirmButton.GetComponent<Button>();
        confirmBtn.onClick.AddListener(confirmPartyForContract);

        //conScriptCaller = conCaller.GetComponent<ContractHandler>();
        //generatorScriptCaller = genCaller.GetComponent<Generator>();

        ContractFacade = ContractHandler.GetComponent<ContractFacade>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
    }

    public void setVisuals(int partyID, int contractToDelegateID, float resultChance, List<Character> members, string partyName)
    {
        partyForContractID = partyID;
        contractID = contractToDelegateID;

        Contract contractToBeStarted = ContractStorage.findContractFromID(contractToDelegateID);

        contractNameTitleText.gameObject.GetComponent<Text>().text = "Contract  to be delegated: " + contractToBeStarted.ID;
        PartyNameText.gameObject.GetComponent<Text>().text = "Party Chosen: " + partyName;
        goldGainedText.gameObject.GetComponent<Text>().text = "Gold reward available: " + contractToBeStarted.reward;
        successChanceText.gameObject.GetComponent<Text>().text = "Success Chance: " + resultChance;
        contractDifficultyText.gameObject.GetComponent<Text>().text = "Difficulty: " + contractToBeStarted.difficulty;
        timeTakenForContractText.gameObject.GetComponent<Text>().text = "Time for contract: " + contractToBeStarted.timeLeft;

        foreach (Character c in members)
        {
            //generate object
            GameObject scrollItemObj = Instantiate(resultScreenCharacterPrefab);

            //generate visuals
            scrollItemObj.transform.SetParent(resultScreenScrollView.transform, false);

            //assign character to object
            aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
            objAccess.thisCharacterID = c.charId;

            //set visuals
            objAccess.updateVisuals();
            CharacterRowList.Add(scrollItemObj);
        }

        gameObject.SetActive(true);
    }

    void confirmPartyForContract()
    {
        if(partyForContractID == 0)
        {
            Debug.LogError("Party id trying to assign is 0");
        }

        ContractStorage.setContractAsOngoing(partyForContractID);

        gameObject.SetActive(false);
        closeMenu();

        //close the choose party for contract screen
        ContractFacade.clearAndCloseChoosePartyForConScreen(true);
    }

    void closeMenu()
    {
        contractNameTitleText.gameObject.GetComponent<Text>().text = "Contract result unset";

        PartyNameText.gameObject.GetComponent<Text>().text = "XP gained: unset";
        goldGainedText.gameObject.GetComponent<Text>().text = "Gold gained: unset";
        successChanceText.gameObject.GetComponent<Text>().text = "XP gained: unset";
        contractDifficultyText.gameObject.GetComponent<Text>().text = "Gold gained: unset";

        foreach (GameObject g in CharacterRowList)
        {
            Destroy(g);
        }

        partyForContractID = 0;
        contractID = 0;
    }
}
