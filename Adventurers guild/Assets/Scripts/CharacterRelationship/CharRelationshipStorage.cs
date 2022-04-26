using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharRelationshipStorage : MonoBehaviour
{
    [HideInInspector]
    public List<CharacterRelationships> allCharacterRelationshipsList;

    public GameObject allCharStorageObject;
    AllCharacterStorage AllCharacterStorage;

    public IDictionary<int, OpinionletDef> opinionletDefDictionary = new Dictionary<int, OpinionletDef>();
    XmlDocument opinionletDefXmlDocument;

    private void Start()
    {
        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();

        Debug.Log("---Started creating opinionletDefs.---");

        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/Opinionlets");

        opinionletDefXmlDocument = new XmlDocument();
        opinionletDefXmlDocument.LoadXml(xmlTextAsset.text);

        XmlNodeList opinionlets = opinionletDefXmlDocument.SelectNodes("/Defs/Opinionlet");

        foreach (XmlNode opinionlet in opinionlets)
        {
            createOpinionletDatum(opinionlet);
        }

        Debug.Log("---Finished creating opinionletDefs. Created " + opinionletDefDictionary.Count + " opinionletDefs---");

        startupChecks();
    }

    void createOpinionletDatum(XmlNode opinionlet)
    {
        OpinionletDef opinionletDef = new OpinionletDef(opinionlet);

        opinionletDefDictionary.Add((int)opinionletDef.opinionletTag, opinionletDef);
        Debug.Log("Created opinionletDef: " + opinionletDef.title);
    }

    public CharacterRelationships generateBlankRelationships(int charId)
    {
        CharacterRelationships characterRelationships = gameObject.AddComponent<CharacterRelationships>();
        characterRelationships.Id = charId;
        characterRelationships.opinionList = new List<Opinionlet>();

        allCharacterRelationshipsList.Add(characterRelationships);
        return characterRelationships;
    }

    public CharacterRelationships findCharacterRelationshipsById(int charId)
    {
        foreach(CharacterRelationships characterRelationships in allCharacterRelationshipsList)
        {
            if(characterRelationships.Id == charId)
            {
                return characterRelationships;
            }
        }
        Debug.LogError("Couldn't find a CharacterRelationships with Id: " + charId);
        return null;
    }

    public void addOpinionletToChar(int charId, int targetCharId, OpinionTypes.opinionletTag opinionletEnum)
    {
        Opinionlet opinionlet = gameObject.AddComponent<Opinionlet>();
        opinionlet.charId = charId; opinionlet.targetCharId = targetCharId; opinionlet.opinionletTag = opinionletEnum;

        opinionletDefDictionary.TryGetValue((int)opinionletEnum, out OpinionletDef thisOpinionletDef);

        opinionlet.updateUsingOpinionletDef(thisOpinionletDef);

        CharacterRelationships characterRelationships = findCharacterRelationshipsById(charId);
        if (opinionlet.canRepeat == true)
        {
            characterRelationships.opinionList.Add(opinionlet);
        }
        else if (!doesCharRelationshipsContainOpinionletForChar(characterRelationships.opinionList, opinionletEnum, targetCharId))
        {
            characterRelationships.opinionList.Add(opinionlet);
        }
        else
        {
            //cant add opinionlet so destory it
            Destroy(opinionlet);
        }

    }

    public void removeOpinionletFromChar(int charId, int targetId, OpinionTypes.opinionletTag opinionletEnum)
    {
        List<Opinionlet> opinionletsList = findCharacterRelationshipsById(charId).opinionList;
        //if list is empty, return
        if(opinionletsList.Count == 0)
        {
            return;
        }
        //set placeholder opinionlet
        Opinionlet opinionletToRemove = opinionletsList[0];
        bool foundOpinionlet = false;

        foreach(Opinionlet opinionlet in opinionletsList)
        {
            if (opinionlet.opinionletTag == opinionletEnum && opinionlet.targetCharId == targetId)
            {
                opinionletToRemove = opinionlet;
                foundOpinionlet = true;
                break;
            }
        }
        if (foundOpinionlet)
        {
            opinionletsList.Remove(opinionletToRemove);
            Destroy(opinionletToRemove);
        }
    }

    public bool doesCharRelationshipsContainOpinionletForChar(List<Opinionlet> opinionletsList, OpinionTypes.opinionletTag opinionletEnum, int targetCharId)
    {
        foreach(Opinionlet opinionlet in opinionletsList)
        {
            if(opinionlet.opinionletTag == opinionletEnum && opinionlet.targetCharId == targetCharId)
            {
                return true;
            }
        }
        return false;
    }

    public void setGoodnessDifferenceOpinionlet(int charId, int targetId)
    {
        //remove existing opinionlets on opinions of each other
        removeOpinionletFromChar(charId, targetId, OpinionTypes.opinionletTag.sameGoodness);
        removeOpinionletFromChar(charId, targetId, OpinionTypes.opinionletTag.smallGoodnessDifference);
        removeOpinionletFromChar(charId, targetId, OpinionTypes.opinionletTag.largeGoodnessDifference);

        int charGoodness = AllCharacterStorage.findAliveCharacterFromID(charId).characterAttributes.goodness;
        int targetGoodness = AllCharacterStorage.findAliveCharacterFromID(targetId).characterAttributes.goodness;

        //get the goodness difference and normalise it, as dont want negative
        int goodnessDifference = HelperFunctions.calculateGoodnessDifference(charGoodness, targetGoodness);
        goodnessDifference = (int)Mathf.Sqrt(Mathf.Pow(goodnessDifference, 2));

        //calculate which opinionlet to give. For those that are different but not to different, no opinionlet occurs
        if(goodnessDifference >= 0 && goodnessDifference < 3)
        {
            addOpinionletToChar(charId, targetId, OpinionTypes.opinionletTag.sameGoodness);
        }
        else if(goodnessDifference >= 5 && goodnessDifference < 8)
        {
            addOpinionletToChar(charId, targetId, OpinionTypes.opinionletTag.smallGoodnessDifference);
        }
        else if (goodnessDifference >= 8 && goodnessDifference <= 10)
        {
            addOpinionletToChar(charId, targetId, OpinionTypes.opinionletTag.largeGoodnessDifference);
        }
        else if (goodnessDifference >= 11 && goodnessDifference < 0)//error catching for when difference is to high or low
        {
            Debug.LogError("Invalid goodness difference: " + goodnessDifference + "between chars: " + charId + " and " + targetId);
        }

    }

    //gets list of all Ids character has relationship with
    public List<int> getIdsOfCharsCharacterHasOpinionletFor(int charId)
    {
        CharacterRelationships characterRelationships = findCharacterRelationshipsById(charId);
        List<int> targetIdList = new List<int>();

        foreach(Opinionlet opinionlet in characterRelationships.opinionList)
        {
            if (!targetIdList.Contains(opinionlet.targetCharId))
            {
                targetIdList.Add(opinionlet.targetCharId);
            }
        }
        return targetIdList;
    }
    //gets total opinion a character has for another character
    public int getCharsCumulativeOpinionOfTargetChar(int charId, int targetId)
    {
        CharacterRelationships characterRelationships = findCharacterRelationshipsById(charId);
        int opinion = 0;

        foreach(Opinionlet opinionlet in characterRelationships.opinionList)
        {
            if(opinionlet.targetCharId == targetId)
            {
                opinion = opinion + opinionlet.opinionBuff;
            }
        }
        return opinion;
    }

    public void tickAllOpinionletsDown()
    {
        foreach(CharacterRelationships characterRelationships in allCharacterRelationshipsList)
        {
            if(characterRelationships.opinionList.Count > 0)
            {
                List<Opinionlet> finishedOpinionlets = new List<Opinionlet>();
                //loop through each and reduce time by 1, and if it becomes 0 or less remove it
                foreach(Opinionlet opinionlet in characterRelationships.opinionList)
                {
                    //if moodlet is not perminant, tick it down
                    if(opinionlet.timeLeft != 999)
                    {
                        opinionlet.timeLeft--;
                        if (opinionlet.timeLeft <= 0)
                        {
                            finishedOpinionlets.Add(opinionlet);
                        }
                    }
                }
                if (finishedOpinionlets.Count > 0)
                {
                    foreach(Opinionlet opinionlet in finishedOpinionlets)
                    {
                        characterRelationships.opinionList.Remove(opinionlet);
                    }
                    for(int i = finishedOpinionlets.Count - 1; i >= 0; i--)
                    {
                        Opinionlet opinionletToDelete = finishedOpinionlets[i];
                        finishedOpinionlets.Remove(opinionletToDelete);
                        Destroy(opinionletToDelete);
                    }
                }
            }
        }
    }

    //receives each opinionlet and inserts it into the correct character relationship.
    //if one does not exist, it will create it 
    public void loadOpinionletIntoCharacterRelationships(int charId, int targetId, int opinionletEnumInt, int opinionBuff, int timeleft, int canRepeatBool)
    {
        bool characterRelationshipExists = false;
        Opinionlet opinionlet = gameObject.AddComponent<Opinionlet>();
        opinionlet.charId = charId; opinionlet.targetCharId = targetId; opinionlet.opinionletTag = (OpinionTypes.opinionletTag)opinionletEnumInt;
        opinionlet.opinionBuff = opinionBuff; opinionlet.timeLeft = timeleft; opinionlet.canRepeat = HelperFunctions.IntToBool(canRepeatBool);
        foreach(CharacterRelationships characterRelationships in allCharacterRelationshipsList)
        {
            if(charId == characterRelationships.Id)
            {
                characterRelationshipExists = true;
                characterRelationships.opinionList.Add(opinionlet);
                break;
            }
        }
        if (!characterRelationshipExists)
        {
            generateBlankRelationships(charId).opinionList.Add(opinionlet);
        }
    }

    //as we dont store opinionlets in a list, this function gathers them all from the character relationship list
    public List<Opinionlet> getAllOpinionlets()
    {
        List<Opinionlet> opinionletList = new List<Opinionlet>();

        foreach (CharacterRelationships characterRelationships in allCharacterRelationshipsList)
        {
            foreach (Opinionlet opinionlet in characterRelationships.opinionList)
            {
                opinionletList.Add(opinionlet);
            }
        }
        return opinionletList;
    }

    void startupChecks()
    {
        if (opinionletDefDictionary.Count != OpinionTypes.getAllOpinionletTags().Count)
        {
            Debug.LogError("Mismatch between number of opinionletDefs generated and number of opinionlet enums. " +
                "Traits generated: " + opinionletDefDictionary.Count + ", trait enums: " + OpinionTypes.getAllOpinionletTags().Count);

            foreach (OpinionTypes.opinionletTag opinionTag in OpinionTypes.getAllOpinionletTags())
            {
                opinionletDefDictionary.TryGetValue((int)opinionTag, out OpinionletDef thisOpinionlet);
                if (thisOpinionlet == null)
                {
                    Debug.LogError(opinionTag);
                }
            }
        }
    }
}
