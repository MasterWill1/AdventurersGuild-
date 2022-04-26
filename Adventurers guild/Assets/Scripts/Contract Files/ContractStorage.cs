using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Xml;

public class ContractStorage : MonoBehaviour
{
    int contractID = 0;
    [HideInInspector]
    public List<Contract> activeContracts, completedContracts;
    [HideInInspector]
    public Contract contractToBeDelegated;

    public GameObject traitHandlerObject, contractHandler, keyWorldDetailsHandlerObject, partyHandler, characterHandler, eventHandler, AllCharStorageObject;
    TraitHandler TraitHandler;
    ContractFacade ContractFacade;
    keyWorldDetailsHandler keyWorldDetailsHandler;
    PartyStorage PartyStorage;
    PartyFunctions PartyFunctions;
    CharStorage CharStorage;
    EventHandler EventHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractDetailsStorage ContractDetailsStorage;

    public Button NewContractButton;

    public IDictionary<int, Target> targetDictionary = new Dictionary<int, Target>();

    XmlDocument targetXmlDocument;

    // Start is called before the first frame update
    void Start()
    {
        TraitHandler = traitHandlerObject.GetComponent<TraitHandler>();
        ContractFacade = contractHandler.GetComponent<ContractFacade>();
        keyWorldDetailsHandler = keyWorldDetailsHandlerObject.GetComponent<keyWorldDetailsHandler>();
        PartyStorage = partyHandler.GetComponent<PartyStorage>();
        PartyFunctions = partyHandler.GetComponent<PartyFunctions>();
        CharStorage = characterHandler.GetComponent<CharStorage>();
        EventHandler = eventHandler.GetComponent<EventHandler>();
        AllCharacterStorage = AllCharStorageObject.GetComponent<AllCharacterStorage>();
        ContractDetailsStorage = gameObject.GetComponent<ContractDetailsStorage>();

        Debug.Log("---Started creating targets.---");

        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/Targets");

        targetXmlDocument = new XmlDocument();
        targetXmlDocument.LoadXml(xmlTextAsset.text);

        XmlNodeList targets = targetXmlDocument.SelectNodes("/Defs/TargetDef");

        foreach(XmlNode target in targets)
        {
            createTargetDatum(target);
        }

        Debug.Log("---Finished creating targets. Created " + targetDictionary.Count + " targets---");

        Button NewContractBtn = NewContractButton.GetComponent<Button>();
        NewContractBtn.onClick.AddListener(generateContract);

        startupChecks();
    }

    public Target getTargetFromDictionary(ContractTypes.contractTargetTag contractTargetTag)
    {
        Target target;
        targetDictionary.TryGetValue((int)contractTargetTag, out target);

        return target;
    }

    void createTargetDatum(XmlNode target)
    {
        Target newTarget = new Target(target);

        targetDictionary.Add((int)newTarget.contractTargetTag, newTarget);
        Debug.Log("Created target: " + newTarget.targetName);
    }

    void generateContract()
    {
        contractID++;
        int difficulty = Random.Range(5, 50);
        int reward = difficulty + Random.Range(-(int)(difficulty * 0.2), (int)(difficulty * 0.2));
        int timeLeft = difficulty + Random.Range(-(int)(difficulty * 0.3), -(int)(difficulty * 0.7));

        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = contractID; generatedContract.difficulty = difficulty; generatedContract.reward = reward; generatedContract.timeLeft = timeLeft; generatedContract.isOngoing = false;
        generatedContract.contractDetails = ContractDetailsStorage.genRandomContractDetails(contractID, difficulty);

        BuildContract(generatedContract);
    }

    public void BuildContract(Contract contract)
    {
        if (contract.timeLeft == 0)
        {
            completedContracts.Add(contract);
        }
        else if (contract.timeLeft > 0)
        {
            activeContracts.Add(contract);
        }
        else
        {
            Debug.LogError("contract to build has time left of less than 0. Time Left for contract ID: " + contract.ID + " is " + contract.timeLeft + " days");
        }
        ContractFacade.displayContract(contract);
    }

    public void setContractAsContractToBeDelegated(int conID)
    {
        contractToBeDelegated = findContractFromID(conID);
    }

    //finds a contract according to its ID
    public Contract findContractFromID(int ID)
    {
        foreach (Contract c in activeContracts)
        {
            if (c.ID == ID)
            {
                return c;
            }
        }
        foreach (Contract cc in completedContracts)
        {
            if (cc.ID == ID)
            {
                return cc;
            }
        }
        Debug.LogError("No Contract found with Id: " + ID);
        return null;
    }

    //gets called when a contract is completed
    public void completeContract(int partyID, int contractID)
    {
        keyWorldDetailsHandler.pauseTime();

        Contract completedContract = findContractFromID(contractID);

        Debug.Log("This Contracts Difficulty: " + completedContract.difficulty);

        Party thisParty = PartyStorage.findPartyFromID(partyID);

        int partyStrength = PartyFunctions.CalculatePartyStrength(partyID, contractID);

        float PartyEffectivenessForQuest = ContractFunctions.getPartyEffectivenessForQuest(partyStrength, completedContract.difficulty);

        bool didPartySucceed = ContractFunctions.CalculateSuccess(PartyEffectivenessForQuest);
        bool wasCriticalResult = ContractFunctions.wasCriticalResult();
        Debug.Log("Did party succeed: " + didPartySucceed +". Was it a critical result: " + wasCriticalResult);

        //determine how much xp to award party
        int XpToAward = completedContract.difficulty;

        if (didPartySucceed == true)
        {
            PartyStorage.giveEachPartyMemberMoodlet(partyID, MoodTypes.moodletTag.successfulQuest);
            PartyStorage.giveEachPartyMemberOpinionletOfEachOther(partyID, OpinionTypes.opinionletTag.successfulQuestTogether);
        }
        else 
        {
            PartyStorage.giveEachPartyMemberMoodlet(partyID, MoodTypes.moodletTag.unsuccessfulQuest);
            PartyStorage.giveEachPartyMemberOpinionletOfEachOther(partyID, OpinionTypes.opinionletTag.unsuccessfulQuestTogether);

            XpToAward = (int)(XpToAward * 0.5);
        }
        

        AllCharacterStorage.setEachPartyCharMoodFromContractGoodness(thisParty.members, completedContract.contractDetails.goodness, true);

        triggerEndOfContractPartyEvents(didPartySucceed, wasCriticalResult, partyID, contractID);

        //award each of the characters the set amount of xp
        CharStorage.awardXpToCharacters(thisParty.members, XpToAward);

        //display the contract result screen
        ContractFacade.displayContractResultsScreen(didPartySucceed, XpToAward, completedContract.reward, thisParty.members);

        keyWorldDetailsHandler.changeTotalGold(completedContract.reward);
        keyWorldDetailsHandler.changeRenown(completedContract.difficulty);
        keyWorldDetailsHandler.changeReputation(HelperFunctions.goodnessScoreToModifier
            (ContractDetailsStorage.getContractDetailsFromID(contractID).goodness));

        thisParty.onQuest = 0;

        Destroy(ContractFacade.findContractGameObjectFromId(completedContract.ID));
        ContractFacade.displayContract(completedContract);
    }

    //set a contract as active
    public void setContractAsOngoing(int partyID)
    {
        Party party = PartyStorage.findPartyFromID(partyID);
        party.onQuest = contractToBeDelegated.ID;

        Contract contract = findContractFromID(contractToBeDelegated.ID);
        contract.isOngoing = true;
        AllCharacterStorage.setEachPartyCharMoodFromContractGoodness(party.members, contract.contractDetails.goodness, false);

        //update all contracts so ongoing ones are no longer able to be assigned
        ContractFacade.updateAllContractVisuals();
    }

    void triggerEndOfContractPartyEvents(bool wasSuccess, bool wasCritical, int partyID, int contractID)
    {
        List<EventTypes.goodContractEvents> outGoodEndOfContractEvents = new List<EventTypes.goodContractEvents>();
        List<EventTypes.critGoodEndOfContractEvents> outCritGoodEndOfContractEvents = new List<EventTypes.critGoodEndOfContractEvents>();
        List<EventTypes.softDamageEvents> outSoftDamageEvents = new List<EventTypes.softDamageEvents>();
        List<EventTypes.hardDamageEvents> outHardDamageEvents = new List<EventTypes.hardDamageEvents>();
        //populate event list
        EventHandler.partyEventsFromContractFinish
            (wasSuccess, wasCritical, out outGoodEndOfContractEvents, out outCritGoodEndOfContractEvents, out outSoftDamageEvents, out outHardDamageEvents);

        //if a event does not include no one got hurt then add all chars damage low
        if (!outCritGoodEndOfContractEvents.Contains(EventTypes.critGoodEndOfContractEvents.noPartyMembersHurt))
        {
            //call event trigger without adding standard end of mission low damage
            outSoftDamageEvents.Add(EventTypes.softDamageEvents.allCharsDamageLow);
        }

        //trigger each event in list
        EventHandler.triggerListOfPartyEvents
            (partyID, contractID, outGoodEndOfContractEvents, outCritGoodEndOfContractEvents,
            outSoftDamageEvents, outHardDamageEvents, wasSuccess, wasCritical);
    }

    //for each contract that is ongoing tick their days remaining down by 1
    public void tickContractsDown()
    {
        List<Contract> finishedContractsTemp = new List<Contract>();
        foreach (Contract c in activeContracts)
        {
            if (c.isOngoing == true)
            {
                c.timeLeft--;

                ContractFacade.findContractGameObjectFromId(c.ID).GetComponent<aContract>().updateVisuals();

                if (c.timeLeft == 0)
                {
                    completeContract(PartyStorage.findPartyFromContractID(c.ID).partyID, c.ID);
                    finishedContractsTemp.Add(c);
                }
            }
        }
        //moves completed contract to finished contract list
        if (finishedContractsTemp.Count > 0)
        {
            foreach (Contract conToRemove in finishedContractsTemp)
            {
                activeContracts.Remove(conToRemove);
                completedContracts.Add(conToRemove);
            }
        }
        finishedContractsTemp.Clear();

        //update the 
        ContractFacade.updateContractsTimeLeft();
    }

    public void ClearContractData()
    {
        activeContracts.Clear();
        completedContracts.Clear();
        ContractFacade.ClearContractData();
    }

    public void LoadContract(int ID, int difficulty, int reward, int timeleft, int isOngoing)
    {
        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = ID;
        generatedContract.difficulty = difficulty;
        generatedContract.reward = reward;
        generatedContract.timeLeft = timeleft;
        generatedContract.isOngoing = HelperFunctions.IntToBool(isOngoing);

        bool foundTraits = false;
        foreach(ContractDetails contractDetails in ContractDetailsStorage.AllContractDetailsList)
        {
            if(contractDetails.contractID == ID)
            {
                generatedContract.contractDetails = contractDetails;
                foundTraits = true;
                break;
            }
        }
        if (!foundTraits)
        {
            Debug.LogError("Couldnt find Contract trait reference for contract Id: " + ID);
        }
        BuildContract(generatedContract);
    }

    //TO DO investigate if this is neccesary
    public void LoadCompletedContract(int ID, int difficulty, int reward, int timeleft, int isOngoing)
    {
        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = ID;
        generatedContract.difficulty = difficulty;
        generatedContract.reward = reward;
        generatedContract.timeLeft = timeleft;
        generatedContract.isOngoing = HelperFunctions.IntToBool(isOngoing);

        BuildContract(generatedContract);
    }

    //resets ID counter to 1 above the highest ID recorded for a contract
    public void setCounters()
    {
        contractID = 0;
        if (activeContracts.Count != 0)
        {
            contractID = activeContracts[0].ID;
            foreach (Contract c in activeContracts)
            {
                if (c.ID > contractID)
                {
                    contractID = c.ID;
                }
            }
        }
        if (completedContracts.Count != 0)
        {
            contractID = completedContracts[0].ID;
            foreach (Contract c in completedContracts)
            {
                if (c.ID > contractID)
                {
                    contractID = c.ID;
                }
            }
        }
        contractID++;
    }

    void startupChecks()
    {
        if(targetDictionary.Count != ContractTypes.getAllTargetTags().Count)
        {
            Debug.LogError("Mismatch between number of targets generated and number of target enums. Traits generated: " + targetDictionary.Count
                + ", trait enums: " + ContractTypes.getAllTargetTags().Count);
            foreach (ContractTypes.contractTargetTag targetTag in ContractTypes.getAllTargetTags())
            {
                targetDictionary.TryGetValue((int)targetTag, out Target thisTarget);
                if (thisTarget == null)
                {
                    Debug.LogError(targetTag);
                }
            }
        }
    }
}
