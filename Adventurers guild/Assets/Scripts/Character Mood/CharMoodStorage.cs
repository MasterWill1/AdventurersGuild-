using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CharMoodStorage : MonoBehaviour
{
    [HideInInspector]
    public List<CharacterMood> allCharacterMoodsList;

    public GameObject allCharStorageObject, ContractHandler, PartyHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractStorage ContractStorage;
    PartyStorage PartyStorage;

    IDictionary<int, MoodletDef> moodletDefDictionary = new Dictionary<int, MoodletDef>();
    XmlDocument moodletDefXmlDocument;

    private void Start()
    {
        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();

        Debug.Log("---Started creating moodletDefs.---");

        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/Moodlets");

        moodletDefXmlDocument = new XmlDocument();
        moodletDefXmlDocument.LoadXml(xmlTextAsset.text);

        XmlNodeList moodlets = moodletDefXmlDocument.SelectNodes("/Defs/Moodlet");

        foreach (XmlNode moodlet in moodlets)
        {
            createMoodletDatum(moodlet);
        }

        Debug.Log("---Finished creating moodletDefs. Created " + moodletDefDictionary.Count + " moodletDefs---");

        startupChecks();
    }

    void createMoodletDatum(XmlNode moodlet)
    {
        MoodletDef moodletDef = new MoodletDef(moodlet);

        moodletDefDictionary.Add((int)moodletDef.moodletTag, moodletDef);
        Debug.Log("Created opinionletDef: " + moodletDef.title);
    }

    public CharacterMood generateBlankMood(int charId)
    {
        CharacterMood characterMood = gameObject.AddComponent<CharacterMood>();
        characterMood.Id = charId;
        characterMood.moodletList = new List<Moodlet>();

        allCharacterMoodsList.Add(characterMood);
        return characterMood;
    }

    public CharacterMood findCharacterMoodById(int Id)
    {
        foreach (CharacterMood characterMood in allCharacterMoodsList)
        {
            if(characterMood.Id == Id)
            {
                return characterMood;
            }
        }
        Debug.LogError("Couldn't find a CharacterMood with Id: " + Id);
        return null;
    }

    public void addMoodletToChar(int charId, MoodTypes.moodletTag moodletsEnum)
    {
        Moodlet moodlet = gameObject.AddComponent<Moodlet>();
        moodlet.moodEventEnum = moodletsEnum; moodlet.charId = charId;

        moodletDefDictionary.TryGetValue((int)moodletsEnum, out MoodletDef thisMoodletDef);

        moodlet.updateUsingMoodletDef(thisMoodletDef);

        CharacterMood characterMood = findCharacterMoodById(charId);
        if(moodlet.canRepeat == true)
        {
            characterMood.moodletList.Add(moodlet);

        }
        else if (!doesCharMoodContainMoodlet(characterMood.moodletList, moodlet.moodEventEnum))
        {
            characterMood.moodletList.Add(moodlet);
        }
        else
        {
            //cant add moodlet so destroy it
            Destroy(moodlet);
        }

        //remove conflicting moodlets (ie cant have mood debuff from minor and major injured)
        //if the moodlet is a injury debuff, remove the opposite version of it (cant be minor and major injured at same time)
        if(moodlet.moodEventEnum == MoodTypes.moodletTag.injuredMinor)
        {
            removeMoodletFromChar(charId, MoodTypes.moodletTag.injuredMajor);
        }else if(moodlet.moodEventEnum == MoodTypes.moodletTag.injuredMajor)
        {
            removeMoodletFromChar(charId, MoodTypes.moodletTag.injuredMinor);
        }
    }

    //Will remove a single instance of a moodlet from character. Used for non-repeating perminant moodbuffs
    public void removeMoodletFromChar(int charId, MoodTypes.moodletTag moodletsEnum)
    {
        List<Moodlet> MoodletList = findCharacterMoodById(charId).moodletList;
        //if moodlet list is empty, return
        if (MoodletList.Count == 0)
        {
            return;
        }
        //set placeholder moodlet
        Moodlet moodletToRemove = MoodletList[0];
        bool foundMoodlet = false;

        foreach (Moodlet moodlet in MoodletList)
        {
            if (moodlet.moodEventEnum == moodletsEnum)
            {
                moodletToRemove = moodlet;
                foundMoodlet = true;
                break;
            }
        }
        if (foundMoodlet)
        {
            MoodletList.Remove(moodletToRemove);
            Destroy(moodletToRemove);
        }
    }

    //remvoe all instance of moodlet from char by enum
    void removeAllInstancesOfMoodletFromChar(int charId, MoodTypes.moodletTag moodletsEnum)
    {
        List<Moodlet> MoodletList = findCharacterMoodById(charId).moodletList;
        //if moodlet list is empty, return
        if (MoodletList.Count == 0)
        {
            return;
        }
        List<Moodlet> moodletsToRemove = new List<Moodlet>();
        bool foundMoodlet = false;

        foreach (Moodlet moodlet in MoodletList)
        {
            if (moodlet.moodEventEnum == moodletsEnum)
            {
                moodletsToRemove.Add(moodlet);
                foundMoodlet = true;
            }
        }
        if (foundMoodlet)
        {
            destroyListOfMoodletsFromCharMood(moodletsToRemove, MoodletList);
        }
    }

    //char mood from being on quest according to its goodness score compared to chars goodness score
    public void setGoodnessDifferenceOpinionlet(int charId, int contractGoodness, bool isCompletedContract)
    {
        //remove existing opinionlets on opinions of each other
        removeMoodletFromChar(charId, MoodTypes.moodletTag.onAlignedContract);
        removeMoodletFromChar(charId, MoodTypes.moodletTag.onMisalignedContract);

        int charGoodness = AllCharacterStorage.findAliveCharacterFromID(charId).characterAttributes.goodness;

        //get the goodness difference and normalise it, as dont want negative
        int goodnessDifference = HelperFunctions.calculateGoodnessDifference(charGoodness, contractGoodness);
        goodnessDifference = (int)Mathf.Sqrt(Mathf.Pow(goodnessDifference, 2));

        if (!isCompletedContract)
        {
            //calculate which opinionlet to give. For those that are different but not to different, no opinionlet occurs
            if (goodnessDifference >= 0 && goodnessDifference < 3)
            {
                addMoodletToChar(charId, MoodTypes.moodletTag.onAlignedContract);
            }
            else if (goodnessDifference >= 6 && goodnessDifference < 11)
            {
                addMoodletToChar(charId, MoodTypes.moodletTag.onMisalignedContract);
            }
            else if(goodnessDifference >= 11 && goodnessDifference < 0)
            {
                Debug.LogError("Invalid goodness difference: " + goodnessDifference + "between chars: " + charId + " and new contract");
            }
        }
        else
        {
            //calculate which opinionlet to give. For those that are different but not to different, no opinionlet occurs
            if (goodnessDifference >= 0 && goodnessDifference < 3)
            {
                addMoodletToChar(charId, MoodTypes.moodletTag.completedAlignedQuest);
            }
            else if (goodnessDifference >= 6 && goodnessDifference < 11)
            {
                addMoodletToChar(charId, MoodTypes.moodletTag.completedMisalignedQuest);
            }
            else if (goodnessDifference > 10)
            {
                Debug.LogError("Invalid goodness difference: " + goodnessDifference + "between chars: " + charId + " and completed contract");
            }
        }
    }

    public void updateAllCharMoodFromPartyMembers()
    {
        foreach(Character character in AllCharacterStorage.allAliveCharactersList)
        {
            //first remove any working with... moodlets
            removeAllInstancesOfMoodletFromChar(character.charId, MoodTypes.moodletTag.dislikeColleague);
            removeAllInstancesOfMoodletFromChar(character.charId, MoodTypes.moodletTag.likeColleague);

            //if they are in a party
            if(character.characterDetails.inParty != 0)
            {
                List<int> otherPartyMembers = new List<int>(PartyStorage.getAllCharIdsInParty(character.characterDetails.inParty));
                otherPartyMembers.Remove(character.charId);

                foreach(int collegeId in otherPartyMembers)
                {
                    int opinion = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(character.charId, collegeId);

                    if(opinion >= 50)
                    {
                        addMoodletToChar(character.charId, MoodTypes.moodletTag.likeColleague);
                    }
                    else if(opinion <= -50)
                    {
                        addMoodletToChar(character.charId, MoodTypes.moodletTag.dislikeColleague);
                    }
                }
            }
        }
    }

    public bool doesCharMoodContainMoodlet(List<Moodlet> moodletsList, MoodTypes.moodletTag moodletsEnum)
    {
        foreach(Moodlet moodlet in moodletsList)
        {
            if (moodlet.moodEventEnum == moodletsEnum)
            {
                return true;
            }
        }
        return false;
    }

    //goes through each characters moodlet and ticks the time on the left down by 1
    public void tickAllMoodletsDown()
    {
        foreach(CharacterMood characterMood in allCharacterMoodsList)
        {
            //if the character has moodlets
            if(characterMood.moodletList.Count > 0)
            {
                List<Moodlet> finishedMoodlets = new List<Moodlet>();
                //loop through each and reduce time ticket by 1, and if it becomes 0 or less remove it
                foreach (Moodlet moodlet in characterMood.moodletList)
                {
                    //if moodlet is not perminant, tick it down
                    if(moodlet.timeLeft != 999)
                    {
                        moodlet.timeLeft--;
                        if (moodlet.timeLeft <= 0)
                        {
                            finishedMoodlets.Add(moodlet);
                        }
                    }                    
                }
                if (finishedMoodlets.Count > 0)
                {
                    //remove finished moodlets from moodlist of character
                    destroyListOfMoodletsFromCharMood(finishedMoodlets, characterMood.moodletList);
                }
            }
        }
    }

    void destroyListOfMoodletsFromCharMood(List<Moodlet> listOfMoodletsToRemove, List<Moodlet> charMoodList)
    {
        foreach (Moodlet moodlet in listOfMoodletsToRemove)
        {
            charMoodList.Remove(moodlet);
        }
        for (int i = listOfMoodletsToRemove.Count - 1; i >= 0; i--)
        {
            Moodlet moodletToDelete = listOfMoodletsToRemove[i];
            listOfMoodletsToRemove.Remove(moodletToDelete);
            Destroy(moodletToDelete);
        }
    }

    public int getCharsTotalMood(int charId)
    {
        CharacterMood characterMood = findCharacterMoodById(charId);
        int cumulativeMood = 0;

        foreach(Moodlet moodlet in characterMood.moodletList)
        {
            cumulativeMood = cumulativeMood + moodlet.moodBuff;
        }

        return cumulativeMood;
    }

    //receives each moodlet and inserts it into the correct character mood.
    //if one does not exist, it will create it 
    public void loadMoodletIntoCharacterMood(int charId, int moodletEnumInt, int moodbuff, int timeleft, int canRepeatBool)
    {
        bool characterMoodExists = false;
        Moodlet moodlet = gameObject.AddComponent<Moodlet>();
        moodlet.charId = charId;
        moodlet.moodEventEnum = (MoodTypes.moodletTag)moodletEnumInt;
        moodlet.moodBuff = moodbuff;
        moodlet.timeLeft = timeleft;
        moodlet.canRepeat = HelperFunctions.IntToBool(canRepeatBool);

        foreach (CharacterMood characterMood in allCharacterMoodsList)
        {
            if(charId == characterMood.Id)
            {
                characterMoodExists = true;
                characterMood.moodletList.Add(moodlet);
                break;
            }
        }
        if (!characterMoodExists)
        {
            generateBlankMood(charId).moodletList.Add(moodlet);
        }
    }

    //as we dont store moodlets in a list, this function gathers them all from the character mood list
    public List<Moodlet> getAllMoodlets()
    {
        List<Moodlet> moodletList = new List<Moodlet>();

        foreach(CharacterMood characterMood in allCharacterMoodsList)
        {
            foreach(Moodlet moodlet in characterMood.moodletList)
            {
                moodletList.Add(moodlet);
            }
        }
        return moodletList;
    }

    public MoodletDef getMoodletDefFromTag(MoodTypes.moodletTag moodletTag)
    {
        moodletDefDictionary.TryGetValue((int)moodletTag, out MoodletDef moodletDef);
        if(moodletDef == null)
        {
            Debug.LogError("Error getting moodletDef from tag: " + moodletTag +". a Null moodletDef was found");
        }
        return moodletDef;
    }

    void startupChecks()
    {
        if (moodletDefDictionary.Count != MoodTypes.getAllMoodletTags().Count)
        {
            Debug.LogError("Mismatch between number of moodletDefs generated and number of moodlet enums. " +
                "Moodlets generated: " + moodletDefDictionary.Count + ", moodlet enums: " + MoodTypes.getAllMoodletTags().Count);

            foreach (MoodTypes.moodletTag moodletTag in MoodTypes.getAllMoodletTags())
            {
                moodletDefDictionary.TryGetValue((int)moodletTag, out MoodletDef thisMoodlet);
                if (thisMoodlet == null)
                {
                    Debug.LogError(moodletTag);
                }
            }
        }
    }
}
