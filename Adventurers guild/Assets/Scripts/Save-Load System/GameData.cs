using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[][] characterData;
    public int[][] contractData;
    public int[][] completedContractData;
    public int[][] partyData;
    public int[][] nameData;
    public int[][] partyNameData;
    public int[] keyWorldDetails;
    public int[][] attributesData;
    public int[][] traitsData;
    public int[][] contractTraitData;
    public int[][] charHealthData;
    public int[][] moodletData;
    public int[][] opinionletData;
    public int[][] equipmentData;
    public int[][] hexTileBiomeData;

    public GameData(List<CharacterDetails> CharList, List<Party> PartyList, List<Contract> ContractList, List<NameReference> NamesList, List<NameReference> partyNamesList,
        int[] keyWorldDetailsArray, List<Contract> CompletedContractList, List<CharacterAttributes> AttributesList, List<TraitReference> TraitsList, 
        List<ContractDetails> contractDetailsList, List<CharacterHealth> characterHealthList, List<Moodlet> allMoodletsList, List<Opinionlet> allOpinionletsList,
        List<equipmentItem> allEquipmentItems, HexTile[,] HexTileArray)
    {
        //setting list of characters
        characterData = new int[CharList.Count][];
        int charCounter = 0;

        for(int x = 0; x < CharList.Count; x++)
        {
            characterData[x] = new int[7];
        }

        foreach (CharacterDetails character in CharList)
        {
            int[] currentChar = new int[7];

            currentChar[0] = character.ID;
            currentChar[1] = character.XP;
            currentChar[2] = (int)character.charClass;
            currentChar[3] = (int)character.race;
            currentChar[4] = character.inParty;
            currentChar[5] = HelperFunctions.BoolToInt(character.isRecruited);
            currentChar[6] = character.cost;

            characterData[charCounter] = currentChar;

            charCounter++;
        }


        //setting list of parties
        partyData = new int[PartyList.Count][];
        int partyCounter = 0;

        for (int y = 0; y < PartyList.Count; y++)
        {
            partyData[y] = new int[2];
        }

        foreach (Party party in PartyList)
        {
            int[] currentParty = new int[2];

            currentParty[0] = party.partyID;
            currentParty[1] = party.onQuest;

            partyData[partyCounter] = currentParty;
            partyCounter++;
        }


        //setting list of contracts
        contractData = new int[ContractList.Count][];
        int contractCounter = 0;

        for (int z = 0; z < ContractList.Count; z++)
        {
            contractData[z] = new int[5];
        }

        foreach (Contract contract in ContractList)
        {
            int[] currentContract = new int[5];

            currentContract[0] = contract.ID;
            currentContract[1] = contract.difficulty;
            currentContract[2] = contract.reward;
            currentContract[3] = contract.timeLeft;
            currentContract[4] = HelperFunctions.BoolToInt(contract.isOngoing);

            contractData[contractCounter] = currentContract;
            contractCounter++;
        }


        //setting list of names
        nameData = new int[NamesList.Count][];
        int nameCounter = 0;

        for (int nd = 0; nd < NamesList.Count; nd++)
        {
            nameData[nd] = new int[3];
        }

        foreach (NameReference nameReference in NamesList)
        {
            int[] currentName = new int[3];

            currentName[0] = nameReference.ID;
            currentName[1] = nameReference.FN;
            currentName[2] = nameReference.SN;

            nameData[nameCounter] = currentName;
            nameCounter++;
        }

        Debug.Log("Number of name references saved: " + nameCounter);

        //setting lists of party names
        partyNameData = new int[partyNamesList.Count][];
        int partyNameCounter = 0;

        foreach(NameReference partyNameReference in partyNamesList)
        {
            int[] currentPartyName = new int[3];

            currentPartyName[0] = partyNameReference.ID;
            currentPartyName[1] = partyNameReference.FN;
            currentPartyName[2] = partyNameReference.SN;

            partyNameData[partyNameCounter] = currentPartyName;
            partyNameCounter++;
        }

        //setting key world details
        keyWorldDetails = new int[keyWorldDetailsArray.Length];
        System.Array.Copy(keyWorldDetailsArray, keyWorldDetails, keyWorldDetailsArray.Length);

        //setting list of completed contracts
        completedContractData = new int[CompletedContractList.Count][];
        int completedContractCounter = 0;

        for (int cc = 0; cc < CompletedContractList.Count; cc++)
        {
            completedContractData[cc] = new int[5];
        }

        foreach (Contract contract in CompletedContractList)
        {
            int[] currentCompletedContract = new int[5];

            currentCompletedContract[0] = contract.ID;
            currentCompletedContract[1] = contract.difficulty;
            currentCompletedContract[2] = contract.reward;
            currentCompletedContract[3] = contract.timeLeft;
            currentCompletedContract[4] = HelperFunctions.BoolToInt(contract.isOngoing);

            completedContractData[completedContractCounter] = currentCompletedContract;
            completedContractCounter++;
        }

        //setting list of attributes
        attributesData = new int[AttributesList.Count][];
        int attributeCounter = 0;

        for(int a = 0; a < AttributesList.Count; a++)
        {
            attributesData[a] = new int[5];
        }

        foreach(CharacterAttributes attributeReference in AttributesList)
        {
            int[] currentAttributeList = new int[5];

            currentAttributeList[0] = attributeReference.charID;
            currentAttributeList[1] = attributeReference.gender;
            currentAttributeList[2] = attributeReference.lawfulness;
            currentAttributeList[3] = attributeReference.goodness;
            currentAttributeList[4] = (int)attributeReference.nativeBiome;

            attributesData[attributeCounter] = currentAttributeList;
            attributeCounter++;
        }


        //setting list of traits
        traitsData = new int[TraitsList.Count][];
        int traitCounter = 0;

        for (int a = 0; a < TraitsList.Count; a++)
        {
            traitsData[a] = new int[4];
        }

        foreach (TraitReference traitReference in TraitsList)
        {
            int[] currentTraitsList = new int[4];

            currentTraitsList[0] = traitReference.ID;

            currentTraitsList[1] = (int)traitReference.traitList[0];
            currentTraitsList[2] = (int)traitReference.traitList[1];
            currentTraitsList[3] = (int)traitReference.traitList[2];

            traitsData[traitCounter] = currentTraitsList;
            traitCounter++;
        }


        //setting list of contract traits
        contractTraitData = new int[contractDetailsList.Count][];
        int conTraitCounter = 0;

        for(int ct = 0; ct < contractDetailsList.Count; ct++)
        {
            contractTraitData[ct] = new int[3];
        }

        foreach(ContractDetails contractdetails in contractDetailsList)
        {
            int[] currentConTraitList = new int[3];

            currentConTraitList[0] = contractdetails.contractID;
            currentConTraitList[1] = contractdetails.goodness;
            currentConTraitList[2] = contractdetails.locationCoordinate;

            contractTraitData[conTraitCounter] = currentConTraitList;
            conTraitCounter++;
        }


        //setting list of Characters healths
        charHealthData = new int[characterHealthList.Count][];
        int charHealthCounter = 0;

        for(int ch = 0; ch < characterHealthList.Count; ch++)
        {
            charHealthData[ch] = new int[3];
        }

        foreach(CharacterHealth characterHealth in characterHealthList)
        {
            int[] currentCharHealth = new int[3];

            currentCharHealth[0] = characterHealth.ID;
            currentCharHealth[1] = characterHealth.maxHealth;
            currentCharHealth[2] = characterHealth.currentHealth;

            charHealthData[charHealthCounter] = currentCharHealth;
            charHealthCounter++;
        }


        //setting list of moodlets
        moodletData = new int[allMoodletsList.Count][];
        int moodletCounter = 0;

        for(int ml = 0; ml < allMoodletsList.Count; ml++)
        {
            moodletData[ml] = new int[5];
        }
        foreach(Moodlet moodlet in allMoodletsList)
        {
            int[] currentMoodlet = new int[5];

            currentMoodlet[0] = moodlet.charId;
            currentMoodlet[1] = (int)moodlet.moodEventEnum;
            currentMoodlet[2] = moodlet.moodBuff;
            currentMoodlet[3] = moodlet.timeLeft;
            currentMoodlet[4] = HelperFunctions.BoolToInt(moodlet.canRepeat);

            moodletData[moodletCounter] = currentMoodlet;
            moodletCounter++;
        }


        //setting list of opinionlets
        opinionletData = new int[allOpinionletsList.Count][];
        int opinionletCounter = 0;

        for (int ol = 0; ol < allOpinionletsList.Count; ol++)
        {
            opinionletData[ol] = new int[6];
        }
        foreach(Opinionlet opinionlet in allOpinionletsList)
        {
            int[] currentOpinionlet = new int[6];

            currentOpinionlet[0] = opinionlet.charId;
            currentOpinionlet[1] = opinionlet.targetCharId;
            currentOpinionlet[2] = (int)opinionlet.opinionletTag;
            currentOpinionlet[3] = opinionlet.opinionBuff;
            currentOpinionlet[4] = opinionlet.timeLeft;
            currentOpinionlet[5] = HelperFunctions.BoolToInt(opinionlet.canRepeat);

            opinionletData[opinionletCounter] = currentOpinionlet;
            opinionletCounter++;
        }


        //EQUIPMENT
        //setting list of equipment items
        equipmentData = new int[allEquipmentItems.Count][];
        int equipmentCounter = 0;

        for(int it = 0; it <allEquipmentItems.Count; it++)
        {
            equipmentData[it] = new int[5];
        }
        foreach(equipmentItem headEquipmentItem in allEquipmentItems)
        {
            int[] currentEquipment = new int[5];

            currentEquipment[0] = headEquipmentItem.itemId;
            currentEquipment[1] = headEquipmentItem.charId;
            currentEquipment[2] = (int)headEquipmentItem.equipmentName;
            currentEquipment[3] = (int)headEquipmentItem.equipmentQuality;
            currentEquipment[4] = (int)headEquipmentItem.magicEffect;

            equipmentData[equipmentCounter] = currentEquipment;
            equipmentCounter++;

        }


        //HEXTILE DATA
        hexTileBiomeData = new int[HexTileArray.Length][];
        int hexTileCounter = 0;

        for(int ht = 0; ht < HexTileArray.Length; ht++)
        {
            hexTileBiomeData[ht] = new int[3];
        }
        foreach(HexTile hexTile in HexTileArray)
        {
            int[] currentTile = new int[3];

            currentTile[0] = hexTile.OffsetCoordinates.X;
            currentTile[1] = hexTile.OffsetCoordinates.Y;
            currentTile[2] = (int)hexTile.biome;

            hexTileBiomeData[hexTileCounter] = currentTile;
            hexTileCounter++;
        }
    }
}
