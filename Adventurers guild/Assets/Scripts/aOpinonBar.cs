using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aOpinonBar : MonoBehaviour
{
    [HideInInspector]
    public int thisCharId, thisTargetCharId;

    GameObject allCharStorageObject;
    AllCharacterStorage AllCharacterStorage;
    string targetCharName = "";

    public GameObject opinionText, nameText;
    private void Awake()
    {
        allCharStorageObject = GameObject.FindGameObjectWithTag("allCharStorage");
        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();
    }

    public void updateVisuals()
    {
        nameText.GetComponent<Text>().text = targetCharName;

        int opinionOfThem = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(thisCharId, thisTargetCharId);
        int opinionOfUs = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(thisTargetCharId, thisCharId);
        opinionText.GetComponent<Text>().text = "Opinion of Char: " + opinionOfThem + ", Them of us: " + opinionOfUs;
    }

    public void setVariables(int charId, int targetCharId, string targetName)
    {
        targetCharName = targetName;
        thisCharId = charId;
        thisTargetCharId = targetCharId;
    }

}
