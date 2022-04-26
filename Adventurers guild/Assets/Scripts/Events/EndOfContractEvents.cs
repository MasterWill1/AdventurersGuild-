using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndOfContractEvents : MonoBehaviour
{
    public GameObject AllCharacterStorageHandler, ContractHandler, PartyHandler, KeyWorldDetailsHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractStorage ContractStorage;
    PartyStorage PartyStorage;
    keyWorldDetailsHandler keyWorldDetailsHandler;
    EventHandler EventHandler;
    EventViewer EventViewer;
    private void Start()
    {
        AllCharacterStorage = AllCharacterStorageHandler.GetComponent<AllCharacterStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        keyWorldDetailsHandler = KeyWorldDetailsHandler.GetComponent<keyWorldDetailsHandler>();
        EventHandler = gameObject.GetComponent<EventHandler>();
        EventViewer = gameObject.GetComponent<EventViewer>();
    }

    public void triggerListOfPartyEvents(int partyID, int contractID, List<EventTypes.goodContractEvents> GoodEndOfContractEventsToExecute,
    List<EventTypes.critGoodEndOfContractEvents> CritGoodEndOfContractEventsToExecute, List<EventTypes.softDamageEvents> softDamageEndOfContractEventsToExecute,
    List<EventTypes.hardDamageEvents> hardDamageEndOfContractEventsToExecute, bool wasSuccess, bool wasCritical)
    {
        List<int[,]> damageEvents = new List<int[,]>();


        Debug.Log("Begin Party End of contract events Events");
        string eventsToOccur = "Party Events That just Occured: ";
        foreach (EventTypes.goodContractEvents goodEvent in GoodEndOfContractEventsToExecute)
        {
            triggerGoodEndOfContractEvent(goodEvent, partyID, contractID);
            eventsToOccur += goodEvent.ToString() + ", ";
        }
        foreach (EventTypes.critGoodEndOfContractEvents critGoodEvent in CritGoodEndOfContractEventsToExecute)
        {
            triggerCritGoodEndOfContractEvent(critGoodEvent, partyID, contractID);
            eventsToOccur += critGoodEvent.ToString() + ", ";
        }
        foreach (EventTypes.softDamageEvents damageEvent in softDamageEndOfContractEventsToExecute)
        {
            damageEvents.Add(triggerContractSoftDamageEvent(damageEvent, partyID, contractID));
            eventsToOccur += damageEvent.ToString() + ", ";
        }
        foreach (EventTypes.hardDamageEvents damageEvent in hardDamageEndOfContractEventsToExecute)
        {
            damageEvents.Add(triggerContractHardDamageEvent(damageEvent, partyID, contractID));
            eventsToOccur += damageEvent.ToString() + ", ";
        }
        EventViewer.createAEndOfContractEvent(contractID, wasSuccess, wasCritical, damageEvents);
        Debug.Log(eventsToOccur);
    }

    void triggerGoodEndOfContractEvent(EventTypes.goodContractEvents goodEvent, int partyID, int contractID)
    {
        switch (goodEvent)
        {
            case EventTypes.goodContractEvents.smallGoldBonus:
                EventHandler.getSmallBonusGoldByContractDifficulty(contractID);
                break;
        }
    }
    void triggerCritGoodEndOfContractEvent(EventTypes.critGoodEndOfContractEvents critGoodEvent, int partyID, int contractID)
    {
        switch (critGoodEvent)
        {
            case EventTypes.critGoodEndOfContractEvents.noPartyMembersHurt:
                break;
            case EventTypes.critGoodEndOfContractEvents.largeGoldBonus:
                EventHandler.getLargeBonusGoldByContractDifficulty(contractID);
                break;
        }
    }
    int[,] triggerContractSoftDamageEvent(EventTypes.softDamageEvents badEvent, int partyID, int contractID)
    {
        switch (badEvent)
        {
            case EventTypes.softDamageEvents.allCharsDamageLow: //all characters in party receive low damage. not included in list below as this will always happen so it cant happen twice
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getAllCharIdsInParty(partyID), EventTypes.damageType.low);
            case EventTypes.softDamageEvents.oneCharDamageLow: //random party member takes a low amount of damage (according to contract difficulty)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, 1), EventTypes.damageType.low);
            case EventTypes.softDamageEvents.oneCharDamageMedium: //random party member takes a medium amount of damage (according to contract difficulty)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, 1), EventTypes.damageType.medium);
            case EventTypes.softDamageEvents.severalCharsDamageLow://several random party members takes a low amount of damage (according to contract difficulty)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, PartyStorage.getRandomCountOfChars(partyID)), EventTypes.damageType.low);
            case EventTypes.softDamageEvents.severalCharsDamageMedium: //several random party members takes a medium amount of damage (according to contract difficulty)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, PartyStorage.getRandomCountOfChars(partyID)), EventTypes.damageType.medium);
            default:
                Debug.LogError("Invalid softDamageEvent Enum: " + badEvent);
                return null;
        }
    }
    int[,] triggerContractHardDamageEvent(EventTypes.hardDamageEvents critBadEvent, int partyID, int contractID)
    {
        switch (critBadEvent)
        {
            case EventTypes.hardDamageEvents.allCharsDamageMedium://all party members are damaged (medium) 
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getAllCharIdsInParty(partyID), EventTypes.damageType.medium);
            case EventTypes.hardDamageEvents.oneCharDies: //random party member dies
                int charIdToDie = PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, 1)[0];
                AllCharacterStorage.damageCharacter(charIdToDie, 999);
                int[,] damageArray = new int[1,2] { { charIdToDie, 999 } };
                return damageArray;
            case EventTypes.hardDamageEvents.oneCharDamageHigh: //random party member takes a high amount of damage (according to contract difficulty)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, 1), EventTypes.damageType.high);
            case EventTypes.hardDamageEvents.severalCharsDamageHigh: //random number party members are damaged (high)
                return EventHandler.damageXCharactersWithXDamageByContractDifficulty
                    (contractID, PartyStorage.getNumberOfRandomCharactersIdFromParty(partyID, PartyStorage.getRandomCountOfChars(partyID)), EventTypes.damageType.high);
            default:
                Debug.LogError("Invalid hardDamageEvent Enum: " + critBadEvent);
                return null;
        }
    }

    public void partyEventsFromContractFinish(bool wasSuccess, bool wasCritical, out List<EventTypes.goodContractEvents> outGoodEndOfContractEvents,
     out List<EventTypes.critGoodEndOfContractEvents> outCritGoodEndOfContractEvents, out List<EventTypes.softDamageEvents> outSoftDamageEndOfContractEvents, out List<EventTypes.hardDamageEvents> outHardDamageEndOfContractEvents)
    {
        outGoodEndOfContractEvents = new List<EventTypes.goodContractEvents>();
        outCritGoodEndOfContractEvents = new List<EventTypes.critGoodEndOfContractEvents>();
        outSoftDamageEndOfContractEvents = new List<EventTypes.softDamageEvents>();
        outHardDamageEndOfContractEvents = new List<EventTypes.hardDamageEvents>();

        if (wasSuccess)
        {
            if (wasCritical)
            {
                //critical success
                //pick a random number of good events
                //one exceptional good event
                int numberOfNormalEvents = UnityEngine.Random.Range(1, 3);
                List<int> goodEventsThatHappenedList = EventFunctions.getCollectionOfEventsFromEventsList(numberOfNormalEvents, Enum.GetValues(typeof(EventTypes.goodContractEvents)).Length);
                foreach (int i in goodEventsThatHappenedList)
                {
                    outGoodEndOfContractEvents.Add((EventTypes.goodContractEvents)i);
                }

                int critEvent = UnityEngine.Random.Range(0, Enum.GetValues(typeof(EventTypes.critGoodEndOfContractEvents)).Length);
                outCritGoodEndOfContractEvents.Add((EventTypes.critGoodEndOfContractEvents)critEvent);
            }
            else
            {
                //normal success
                //pick a smaller random number of good events and 1 bad
                int numberOfNormalEvents = UnityEngine.Random.Range(1, 3);
                List<int> goodEventsThatHappenedList = EventFunctions.getCollectionOfEventsFromEventsList(numberOfNormalEvents, Enum.GetValues(typeof(EventTypes.goodContractEvents)).Length);
                foreach (int i in goodEventsThatHappenedList)
                {
                    outGoodEndOfContractEvents.Add((EventTypes.goodContractEvents)i);
                }
            }
        }
        else
        {
            if (wasCritical)
            {
                //critical failure
                //pick a random number of bad events
                //one exceptional bad event
                int numberOfNormalEvents = UnityEngine.Random.Range(1, 3);
                List<int> softDamageEventsThatHappenedList = EventFunctions.getCollectionOfEventsFromEventsList(numberOfNormalEvents, Enum.GetValues(typeof(EventTypes.softDamageEvents)).Length);
                foreach (int i in softDamageEventsThatHappenedList)
                {
                    outSoftDamageEndOfContractEvents.Add((EventTypes.softDamageEvents)i);
                }

                int critEvent = UnityEngine.Random.Range(0, Enum.GetValues(typeof(EventTypes.hardDamageEvents)).Length);
                outHardDamageEndOfContractEvents.Add((EventTypes.hardDamageEvents)critEvent);
            }
            else
            {
                //normal failure
                //pick a smaller random number of good bad and 1 good
                int numberOfNormalEvents = UnityEngine.Random.Range(1, 3);
                List<int> softDamageEventsThatHappenedList = EventFunctions.getCollectionOfEventsFromEventsList(numberOfNormalEvents, Enum.GetValues(typeof(EventTypes.softDamageEvents)).Length);
                foreach (int i in softDamageEventsThatHappenedList)
                {
                    outSoftDamageEndOfContractEvents.Add((EventTypes.softDamageEvents)i);
                }
            }
        }
    }
}
