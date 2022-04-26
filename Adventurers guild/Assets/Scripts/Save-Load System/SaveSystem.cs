using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public Button ButtonSaveGame;
    public Button ButtonLoadGame;
    //public GameObject GeneratorHandlerAccessor, ContractHandlerAccessor;
    public GameObject CharHandler, PartyHandler, ContractHandler;
    public GameObject NameHandlerAccessor, keyWorldDetailAccessor, traitHandlerAccessor, HealthHandlerAccessor, AllCharacterStorageAccessor, 
        CharMoodAccessor, CharRelationshipsAccessor, CharEquipmentStorageAccessor, HexGridAccessor;
    //Generator GeneratorAccessor;
    //ContractHandler ContractAccessor;
    CharStorage CharStorage;
    PartyStorage PartyStorage;
    ContractStorage ContractStorage;
    NameHandler NameAccessor;
    keyWorldDetailsHandler keyWorldDetailsHandlerScript;
    TraitHandler TraitHandlerScript;
    HealthHandler HealthHandlerScript;
    AllCharacterStorage AllCharacterStorageScript;
    CharMoodStorage CharMoodStorage;
    CharRelationshipStorage CharRelationshipStorage;
    CharacterEquipmentStorage CharacterEquipmentStorage;
    ContractDetailsStorage ContractDetailsStorage;
    HexGrid HexGrid;

    void Start()
    {
        Button saveGameBtn = ButtonSaveGame.GetComponent<Button>();
        saveGameBtn.onClick.AddListener(getDataToSave);

        Button loadGameBtn = ButtonLoadGame.GetComponent<Button>();
        loadGameBtn.onClick.AddListener(LoadGame);

        //GeneratorAccessor = GeneratorHandlerAccessor.GetComponent<Generator>();
        //ContractAccessor = ContractHandlerAccessor.GetComponent<ContractHandler>();
        CharStorage = CharHandler.GetComponent<CharStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        NameAccessor = NameHandlerAccessor.GetComponent<NameHandler>();
        keyWorldDetailsHandlerScript = keyWorldDetailAccessor.GetComponent<keyWorldDetailsHandler>();
        TraitHandlerScript = traitHandlerAccessor.GetComponent<TraitHandler>();
        HealthHandlerScript = HealthHandlerAccessor.GetComponent<HealthHandler>();
        AllCharacterStorageScript = AllCharacterStorageAccessor.GetComponent<AllCharacterStorage>();
        CharMoodStorage = CharMoodAccessor.GetComponent<CharMoodStorage>();
        CharRelationshipStorage = CharRelationshipsAccessor.GetComponent<CharRelationshipStorage>();
        CharacterEquipmentStorage = CharEquipmentStorageAccessor.GetComponent<CharacterEquipmentStorage>();
        ContractDetailsStorage = ContractHandler.GetComponent<ContractDetailsStorage>();
        HexGrid = HexGridAccessor.GetComponent<HexGrid>();

    }

    void getDataToSave()
    {
        Debug.Log("------Begun saving game-------");
        List<CharacterDetails> CharacterList = new List<CharacterDetails>(CharStorage.characterDetailsList);
        List<Party> PartyList = new List<Party>(PartyStorage.allPartiesList);
        List<Contract> ContractList = new List<Contract>(ContractStorage.activeContracts);
        List<NameReference> NamesList = new List<NameReference>(NameAccessor.nameList);
        List<NameReference> PartyNamesList = new List<NameReference>(NameAccessor.partyNameList);
        int[] keyWorldDetailsToSave = new int[keyWorldDetailsHandlerScript.keyWorldDetailsArray.Length];
        System.Array.Copy(keyWorldDetailsHandlerScript.keyWorldDetailsArray, keyWorldDetailsToSave, keyWorldDetailsHandlerScript.keyWorldDetailsArray.Length);
        List<Contract> CompletedContractList = new List<Contract>(ContractStorage.completedContracts);
        List<CharacterAttributes> AttributesList = new List<CharacterAttributes>(TraitHandlerScript.charAttributesList);
        List<TraitReference> TraitsList = new List<TraitReference>(TraitHandlerScript.charTraitList);
        List<ContractDetails> conDetailsList = new List<ContractDetails>(ContractDetailsStorage.AllContractDetailsList);
        List<CharacterHealth> characterHealthList = new List<CharacterHealth>(HealthHandlerScript.AllCharactersHealths);
        List<Moodlet> moodletList = new List<Moodlet>(CharMoodStorage.getAllMoodlets());
        List<Opinionlet> opinionletList = new List<Opinionlet>(CharRelationshipStorage.getAllOpinionlets());
        List<equipmentItem> allRealEquipmentItemList = CharacterEquipmentStorage.allRealEquipmentItems;
        HexTile[,] allHexTiles = HexGrid.HexTileGridArray;

        foreach(ContractDetails c in conDetailsList)
        {
            Debug.Log("Con trait ref saved id: " + c.contractID);
        }

        SaveGame(CharacterList, PartyList, ContractList, NamesList, PartyNamesList, keyWorldDetailsToSave, CompletedContractList,
            AttributesList, TraitsList, conDetailsList, characterHealthList, moodletList, opinionletList, allRealEquipmentItemList,
            allHexTiles);
    }

    public static void SaveGame
    (List<CharacterDetails> CharList, List<Party> PartyList, List<Contract> ContractList, List<NameReference> NamesList,
    List<NameReference> PartyNamesList, int[] keyWorldDetails, List<Contract> CompletedContractList, List<CharacterAttributes> AttributesList, List<TraitReference> TraitsList,
    List<ContractDetails> conDetailsList, List<CharacterHealth> charHealthList, List<Moodlet> moodletsList, List<Opinionlet> opinionletsList, 
    List<equipmentItem> allRealEquipmentItemList, HexTile[,] allHexTiles)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamedata.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(CharList, PartyList, ContractList, NamesList, PartyNamesList, keyWorldDetails,
            CompletedContractList, AttributesList, TraitsList, conDetailsList, charHealthList, moodletsList, opinionletsList, 
            allRealEquipmentItemList, allHexTiles);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/gamedata.fun";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    void LoadGame()
    {
        Debug.Log("-------Begun Loading Game-------");
        GameData data = LoadGameData();

        ResetCurrentGame();

        //rebuild parties
        for (int p = 0; p < data.partyData.Length; p++)
        {
            PartyStorage.loadParty(data.partyData[p][0], data.partyData[p][1]);
        }

        //rebuild character details
        Debug.Log("Loading character details. Number of character details: " + data.characterData.Length);
        for (int i = 0; i < data.characterData.Length; i++)
        {
            CharStorage.loadCharacterDetails(data.characterData[i][0], data.characterData[i][1], data.characterData[i][2],
                data.characterData[i][3], data.characterData[i][4], data.characterData[i][5], data.characterData[i][6]);
        }

        for (int ct = 0; ct < data.contractTraitData.Length; ct++)
        {
            ContractDetailsStorage.loadContractDetails(data.contractTraitData[ct][0], data.contractTraitData[ct][1],
                data.contractTraitData[ct][2], (LocationTypes.locationBiomeTag)data.contractTraitData[ct][3],
                (LocationTypes.locationSpecificTag)data.contractTraitData[ct][4], 
                (ContractTypes.contractTargetTag)data.contractTraitData[ct][5], (ContractTypes.contractGoalTag)data.contractTraitData[ct][6]);
        }

        for (int c = 0; c < data.contractData.Length; c++)
        {
            ContractStorage.LoadContract(data.contractData[c][0], data.contractData[c][1], data.contractData[c][2], data.contractData[c][3], data.contractData[c][4]);
        }

        Debug.Log("Loading names. Number of name references: " + data.nameData.Length);
        for(int n = 0; n < data.nameData.Length; n++)
        {
            NameAccessor.LoadHumanName(data.nameData[n][0], data.nameData[n][1], data.nameData[n][2]);
        }

        for(int pn = 0; pn < data.partyNameData.Length; pn++)
        {
            NameAccessor.LoadPartyName(data.partyNameData[pn][0], data.partyNameData[pn][1], data.partyNameData[pn][2]);
        }

        keyWorldDetailsHandlerScript.loadKeyWorldData(data.keyWorldDetails);

        for (int c = 0; c < data.completedContractData.Length; c++)
        {
            ContractStorage.LoadContract(data.completedContractData[c][0], data.completedContractData[c][1], data.completedContractData[c][2], data.completedContractData[c][3], data.completedContractData[c][4]);
        }

        for (int a = 0; a < data.attributesData.Length; a++)
        {
            TraitHandlerScript.loadAttribute(data.attributesData[a][0], data.attributesData[a][1], data.attributesData[a][2], 
                data.attributesData[a][3], (LocationTypes.locationBiomeTag)data.attributesData[a][4]);
        }

        for(int t = 0; t < data.traitsData.Length; t++)
        {
            List<int> traitsListToLoad = new List<int>
            {
                data.traitsData[t][1],
                data.traitsData[t][2],
                data.traitsData[t][3],
            };

            TraitHandlerScript.loadTraits(data.traitsData[t][0], traitsListToLoad);
        }

        for(int ch = 0; ch < data.charHealthData.Length; ch++)
        {
            HealthHandlerScript.buildCharHealth(data.charHealthData[ch][0], data.charHealthData[ch][1], data.charHealthData[ch][2]);
        }

        for(int ml = 0; ml < data.moodletData.Length; ml++)
        {
            CharMoodStorage.loadMoodletIntoCharacterMood(data.moodletData[ml][0], data.moodletData[ml][1], data.moodletData[ml][2], data.moodletData[ml][3], data.moodletData[ml][4]);
        }

        for(int ol = 0; ol < data.opinionletData.Length; ol++)
        {
            CharRelationshipStorage.loadOpinionletIntoCharacterRelationships(data.opinionletData[ol][0], data.opinionletData[ol][1], data.opinionletData[ol][2], data.opinionletData[ol][3], data.opinionletData[ol][4], data.opinionletData[ol][5]);
        }


        //Equipment stuffs
        for(int ed = 0; ed < data.equipmentData.Length; ed++)
        {
            CharacterEquipmentStorage.buildEquipmentItem(data.equipmentData[ed][0], data.equipmentData[ed][1], (CharacterEquipmentTypes.equipmentName)data.equipmentData[ed][2], (CharacterEquipmentTypes.equipmentQuality)data.equipmentData[ed][3], (CharacterEquipmentTypes.magicEffect)data.equipmentData[ed][4]);
        }

        //Hex Map
        for(int ht = 0; ht<data.hexTileBiomeData.Length; ht++)
        {
            HexGrid.loadHexTile(data.hexTileBiomeData[ht][0], data.hexTileBiomeData[ht][1], (LocationTypes.locationBiomeTag)data.hexTileBiomeData[ht][2]);
        }
        HexGrid.rebuildTiles();

        CharacterEquipmentStorage.rebuildCharacterEquipment();

        AllCharacterStorageScript.setCounters();
        PartyStorage.setCounters();
        ContractStorage.setCounters();
        CharacterEquipmentStorage.setCounters();

        AllCharacterStorageScript.rebuildAllCharacters();
    }

    void ResetCurrentGame()
    {
        CharStorage.ClearAllData();
        PartyStorage.ClearAllData();
        ContractStorage.ClearContractData();
        TraitHandlerScript.clearTraitData();
    }
}
