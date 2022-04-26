using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitableCharacter : MonoBehaviour
{
    public Button recruitButton, characterDetailsButton;

    //GameObject genCaller;
    GameObject CharHandler;

    //Generator genScriptCaller;
    CharStorage CharStorage;
    CharFacade CharFacade;
    aCharacter thisCharCaller;
    keyWorldDetailsHandler KWDaccessor;
    characterDetailsScreen characterDetailsScreenScript;

    public GameObject recruitButtonText;
    GameObject KWDGameObject, characterDetailsScreenGO;

    // Start is called before the first frame update
    void Awake()
    {
        Button recruitBtn = recruitButton.GetComponent<Button>();
        recruitBtn.onClick.AddListener(recruitCharacter);

        Button characterDetailsBtn = characterDetailsButton.GetComponent<Button>();
        characterDetailsBtn.onClick.AddListener(showCharacterDetails);

        CharHandler = GameObject.FindWithTag("CharHandler");
        CharStorage = CharHandler.GetComponent<CharStorage>();
        CharFacade = CharHandler.GetComponent<CharFacade>();

        //genCaller = GameObject.FindWithTag("GenHandler");
        //genScriptCaller = genCaller.GetComponent<Generator>();

        thisCharCaller = gameObject.GetComponent<aCharacter>();

        KWDGameObject = GameObject.FindWithTag("KeyWorldDetails");
        KWDaccessor = KWDGameObject.GetComponent<keyWorldDetailsHandler>();

        characterDetailsScreenGO = GameObject.FindWithTag("characterDetails");
        characterDetailsScreenScript = characterDetailsScreenGO.GetComponent<characterDetailsScreen>();
    }

    void recruitCharacter()
    {
        CharStorage.recruitCharacter(thisCharCaller.thisCharacterID);

        Destroy(gameObject);
    }
    
    public void setCharCostVisual()
    {
        recruitButtonText.GetComponent<Text>().text = "Recruit: " + getCharCost()
            .ToString() + "gp";
    }

    public int getCharCost()
    {
        return CharStorage.findCharacterFromID(thisCharCaller.thisCharacterID).cost;
    }

    void showCharacterDetails()
    {
        characterDetailsScreenScript.setVisuals(thisCharCaller.thisCharacterID);
    }
}
