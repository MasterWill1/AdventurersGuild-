using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aCharacter : MonoBehaviour
{
    //[HideInInspector]
    //public Character thisCharacter;
    //[HideInInspector]
    public int thisCharacterID;
    string cName = "name";
    string cRace = "Race";
    string cClass = "Class";
    string cLevel = "level";

    //GameObject genCaller;
    //Generator genScriptCaller;    
    GameObject CharHandler;

    CharStorage CharStorage;

    private void Awake()
    {
        //genCaller = GameObject.FindWithTag("GenHandler");
        //genScriptCaller = genCaller.GetComponent<Generator>();
        CharHandler = GameObject.FindWithTag("CharHandler");
        CharStorage = CharHandler.GetComponent<CharStorage>();
    }


    public void updateVisuals()
    {
        CharacterDetails thisCharacter = CharStorage.findCharacterFromID(thisCharacterID);

        cName = thisCharacter.charName;
        cRace = HelperFunctions.CharRaceEnumToString(thisCharacter.race);
        cClass = HelperFunctions.CharClassEnumToString(thisCharacter.charClass);
        cLevel = HelperFunctions.getCharactersLevel(thisCharacter.XP).ToString();

        gameObject.transform.Find("Name").gameObject.GetComponent<Text>().text = cName;
        gameObject.transform.Find("Race").gameObject.GetComponent<Text>().text = cRace;
        gameObject.transform.Find("Class").gameObject.GetComponent<Text>().text = cClass;
        gameObject.transform.Find("Level").gameObject.GetComponent<Text>().text = cLevel;
    }

    public void increaseXp(int xp)
    {
        CharacterDetails thisCharacter = CharStorage.findCharacterFromID(thisCharacterID);

        int oldLevel = HelperFunctions.getCharactersLevel(thisCharacter.XP);
        thisCharacter.XP = thisCharacter.XP + xp;
        int newLevel = HelperFunctions.getCharactersLevel(thisCharacter.XP);

        if(oldLevel != newLevel)
        {
            thisCharacter.cost = thisCharacter.cost + 10;
            Debug.Log(cName + "Leveled up from lvl: " + oldLevel + " to lvl: " + newLevel);
        }

    }
}
