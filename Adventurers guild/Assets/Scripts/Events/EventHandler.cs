using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//top level for event handler.
//handles all incoming calls to the event area and will call the correct function
public class EventHandler : MonoBehaviour
{
    public GameObject AllCharacterStorageHandler, ContractHandler, PartyHandler, KeyWorldDetailsHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractStorage ContractStorage;
    PartyStorage PartyStorage;
    keyWorldDetailsHandler keyWorldDetailsHandler;
    GenericEvents GenericEvents;
    EndOfContractEvents EndOfContractEvents;
    OnContractEvents OnContractEvents;
    SocialEvents SocialEvents;

    private void Start()
    {
        AllCharacterStorage = AllCharacterStorageHandler.GetComponent<AllCharacterStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        keyWorldDetailsHandler = KeyWorldDetailsHandler.GetComponent<keyWorldDetailsHandler>();
        GenericEvents = gameObject.GetComponent<GenericEvents>();
        EndOfContractEvents = gameObject.GetComponent<EndOfContractEvents>();
        OnContractEvents = gameObject.GetComponent<OnContractEvents>();
        SocialEvents = gameObject.GetComponent<SocialEvents>();
    }

    public int[,] damageXCharactersWithXDamageByContractDifficulty(int contractId, List<int> charIds, EventTypes.damageType damageType)
    {
        return GenericEvents.damageXCharactersWithXDamageByContractDifficulty(contractId, charIds, damageType);
    }

    public int[,] damageXCharactersWithXDamageByExactDifficulty(int ExactDifficulty, List<int> charIds, EventTypes.damageType damageType)
    {
        return GenericEvents.damageXCharactersWithXDamageByExactDifficulty(ExactDifficulty, charIds, damageType);
    }

    public void getSmallBonusGoldByContractDifficulty(int conId)
    {
        GenericEvents.getSmallBonusGoldByContractDifficulty(conId);
    }

    public void getLargeBonusGoldByContractDifficulty(int conId)
    {
        GenericEvents.getLargeBonusGoldByContractDifficulty(conId);
    }

    public void partyEventsFromContractFinish(bool wasSuccess, bool wasCritical, out List<EventTypes.goodContractEvents> outGoodEndOfContractEvents,
    out List<EventTypes.critGoodEndOfContractEvents> outCritGoodEndOfContractEvents, out List<EventTypes.softDamageEvents> outSoftDamageEvents, out List<EventTypes.hardDamageEvents> outHardDamageEvents)
    {
        EndOfContractEvents.partyEventsFromContractFinish(wasSuccess, wasCritical, out outGoodEndOfContractEvents,
             out outCritGoodEndOfContractEvents, out outSoftDamageEvents, out outHardDamageEvents);
    }

    public void triggerListOfPartyEvents(int partyID, int contractID, List<EventTypes.goodContractEvents> GoodEndOfContractEventsToExecute,
        List<EventTypes.critGoodEndOfContractEvents> CritGoodEndOfContractEventsToExecute, List<EventTypes.softDamageEvents> SoftDamageEvents,
        List<EventTypes.hardDamageEvents> HardDamageEvents, bool wasSuccess, bool wasCritical)
    {
        EndOfContractEvents.triggerListOfPartyEvents(partyID, contractID, GoodEndOfContractEventsToExecute, CritGoodEndOfContractEventsToExecute,
            SoftDamageEvents, HardDamageEvents, wasSuccess, wasCritical);
    }

    public void doRandomSocialEventForChar(int charId)
    {
        SocialEvents.doRandomSocialEventForChar(charId);
    }

}
