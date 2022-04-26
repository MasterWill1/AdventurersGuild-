using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aContract : MonoBehaviour
{
    [HideInInspector]
    public int thisContractID;

    public Button contractDetailsButton;
    //public GameObject thisGameObject;

    GameObject  ContractHandler, contractDetailsScreenGO;
    GameObject[] partiesToAdd;

    //Generator genScriptCaller;
    //ContractHandler conScriptCaller;
    contractDetailsScreen contractDetailsScreen;
    ContractStorage ContractStorage;

    // Start is called before the first frame update
    void Awake()
    {
        Button contractDetailsBtn = contractDetailsButton.GetComponent<Button>();
        contractDetailsBtn.onClick.AddListener(showContractDetails);

        //genCaller = GameObject.FindWithTag("GenHandler");
        ContractHandler = GameObject.FindWithTag("ContractHandler");
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();

        contractDetailsScreenGO = GameObject.FindWithTag("contractDetails");
        contractDetailsScreen = contractDetailsScreenGO.GetComponent<contractDetailsScreen>();
    }

    //sets the text boxes to the correct values
    public void updateVisuals()
    {
        Contract thisContract = ContractStorage.findContractFromID(thisContractID);

        gameObject.transform.Find("Name").gameObject.GetComponent<Text>().text = thisContract.ID.ToString();
        gameObject.transform.Find("Difficulty").gameObject.GetComponent<Text>().text = thisContract.difficulty.ToString();
        gameObject.transform.Find("Reward").gameObject.GetComponent<Text>().text = thisContract.reward.ToString() + "gp";
        gameObject.transform.Find("Time").gameObject.GetComponent<Text>().text = thisContract.timeLeft.ToString() + " days";
    }

    void showContractDetails()
    {
        contractDetailsScreen.setVisuals(thisContractID);
    }
}
