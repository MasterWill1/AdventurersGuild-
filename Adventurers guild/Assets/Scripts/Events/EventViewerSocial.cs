using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventViewerSocial : MonoBehaviour
{
    public GameObject allCharStorageObject;
    EventViewer EventViewer;
    AllCharacterStorage AllCharacterStorage;

    private void Start()
    {
        EventViewer = gameObject.GetComponent<EventViewer>();

        AllCharacterStorage = allCharStorageObject.GetComponent<AllCharacterStorage>();
    }
    /// <summary>
    /// Creates the visual event card for good on contract events
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="goodSocialEvent"></param>
    public void createAGoodSocialEvent(int charId, int targetCharId, int contractId, EventTypes.goodSocialEvents goodSocialEvent) //Could add extra parameter for bonus effects of event, for example increased crit success
    {
        SingleEvent socialEvent = EventViewer.createBlankEvent();
        socialEvent.charId = charId;
        socialEvent.targetCharId = targetCharId;
        socialEvent.contractId = contractId;

        switch (goodSocialEvent)
        {
            case EventTypes.goodSocialEvents.hadChat:
                hadChatSocialEvent(charId, targetCharId, socialEvent);
                break;
            case EventTypes.goodSocialEvents.hadDeepChat:
                hadDeepChatSocialEvent(charId, targetCharId, socialEvent);
                break;
            case EventTypes.goodSocialEvents.goodTeamwork:
                goodTeamworkSocialEvent(charId, targetCharId, socialEvent);
                break;
            case EventTypes.goodSocialEvents.socialisedTogether:
                socialisedTogetherSocialEvent(charId, targetCharId, socialEvent);
                break;
        }
    }
    /// <summary>
    /// creates the visual event card for negative on contract events
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="damageTaken"></param>
    /// <param name="badSocialEvents"></param>
    public void createABadOnContractSocialEvent(int charId, int targetCharId, int contractId, int[,] damageTaken, EventTypes.badSocialEvents badSocialEvents)//Could add extra parameter for bonus effects of event, for example increased contract time
    {
        SingleEvent socialEvent = EventViewer.createBlankEvent();
        socialEvent.charId = charId;
        socialEvent.targetCharId = targetCharId;
        socialEvent.contractId = contractId;

        switch (badSocialEvents)
        {
            case EventTypes.badSocialEvents.hadDisagreement:
                hadDisagreementSocialEvent(charId, targetCharId, socialEvent, contractId);
                break;
            case EventTypes.badSocialEvents.hadArgument:
                hadArgumementSocialEvent(charId, targetCharId, socialEvent, contractId);
                break;
            case EventTypes.badSocialEvents.hadSmallFight:
                hadSmallFightSocialEvent(charId, targetCharId, socialEvent, damageTaken, contractId);
                break;
            case EventTypes.badSocialEvents.hadBigFight:
                hadBigFightSocialEvent(charId, targetCharId, socialEvent, damageTaken, contractId);
                break;
        }
    }
    void hadChatSocialEvent(int charId, int targetCharId, SingleEvent socialEvent)
    {
        string chatText = EventTypes.chatSetupsStrings[Random.Range(0, EventTypes.chatSetupsStrings.Length)];
        string chatTopic = EventTypes.chatTopicStrings[Random.Range(0, EventTypes.chatTopicStrings.Length)];

        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void hadDeepChatSocialEvent(int charId, int targetCharId, SingleEvent socialEvent)
    {
        string chatText = EventTypes.deepChatSetupsStrings[Random.Range(0, EventTypes.deepChatSetupsStrings.Length)];
        string chatTopic = EventTypes.deepChatTopicStrings[Random.Range(0, EventTypes.deepChatTopicStrings.Length)];

        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void goodTeamworkSocialEvent(int charId, int targetCharId, SingleEvent socialEvent)
    {
        string chatText = EventTypes.goodTeamworkSetupsStrings[Random.Range(0, EventTypes.goodTeamworkSetupsStrings.Length)];
        string chatTopic = EventTypes.goodTeamworkTopicStrings[Random.Range(0, EventTypes.goodTeamworkTopicStrings.Length)];

        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void socialisedTogetherSocialEvent(int charId, int targetCharId, SingleEvent socialEvent)
    {
        string chatText = EventTypes.goodDowntimeSetupStrings[Random.Range(0, EventTypes.goodTeamworkSetupsStrings.Length)];
        string chatTopic = EventTypes.goodDowntimeTopicStrings[Random.Range(0, EventTypes.goodTeamworkTopicStrings.Length)];

        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void hadDisagreementSocialEvent(int charId, int targetCharId, SingleEvent socialEvent, int contractId)
    {
        string chatText = EventTypes.disagreementSetupsStrings[Random.Range(0, EventTypes.disagreementSetupsStrings.Length)];
        string chatTopic;

        //if char is not on contract then get generic disagreement topics. Chance of having generic disagreement even on contract
        if (contractId == 0 || HelperFunctions.doWeightedCoinToss(40))
        {
            chatTopic = EventTypes.genericDisagreementTopicStrings[Random.Range(0, EventTypes.genericDisagreementTopicStrings.Length)];
        }
        else
        {
            chatTopic = EventTypes.contractDisagreementTopicStrings[Random.Range(0, EventTypes.contractDisagreementTopicStrings.Length)];
        }

        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void hadArgumementSocialEvent(int charId, int targetCharId, SingleEvent socialEvent, int contractId)
    {
        string chatText = EventTypes.argumentSetupsStrings[Random.Range(0, EventTypes.argumentSetupsStrings.Length)];
        string chatTopic;  // = EventTypes.argumentTopicStrings[Random.Range(0, EventTypes.argumentTopicStrings.Length)];

        //if char is not on contract then get generic disagreement topics. Chance of having generic disagreement even on contract
        if (contractId == 0 || HelperFunctions.doWeightedCoinToss(40))
        {
            chatTopic = EventTypes.genericDisagreementTopicStrings[Random.Range(0, EventTypes.genericDisagreementTopicStrings.Length)];
        }
        else
        {
            chatTopic = EventTypes.contractDisagreementTopicStrings[Random.Range(0, EventTypes.contractDisagreementTopicStrings.Length)];
        }
        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic);
    }
    void hadSmallFightSocialEvent(int charId, int targetCharId, SingleEvent socialEvent, int[,] damageTaken, int contractId)
    {
        string chatText = EventTypes.smallFightSetupStrings[Random.Range(0, EventTypes.smallFightSetupStrings.Length)];
        string chatTopic; // = EventTypes.argumentTopicStrings[Random.Range(0, EventTypes.argumentTopicStrings.Length)];

        if (contractId == 0 || HelperFunctions.doWeightedCoinToss(40))
        {
            chatTopic = EventTypes.genericDisagreementTopicStrings[Random.Range(0, EventTypes.genericDisagreementTopicStrings.Length)];
        }
        else
        {
            chatTopic = EventTypes.contractDisagreementTopicStrings[Random.Range(0, EventTypes.contractDisagreementTopicStrings.Length)];
        }
        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic, damageTaken);
    }
    void hadBigFightSocialEvent(int charId, int targetCharId, SingleEvent socialEvent, int[,] damageTaken, int contractId)
    {
        string chatText = EventTypes.bigFightSetupStrings[Random.Range(0, EventTypes.bigFightSetupStrings.Length)];
        string chatTopic; // = EventTypes.argumentTopicStrings[Random.Range(0, EventTypes.argumentTopicStrings.Length)];

        if (contractId == 0 || HelperFunctions.doWeightedCoinToss(40))
        {
            chatTopic = EventTypes.genericDisagreementTopicStrings[Random.Range(0, EventTypes.genericDisagreementTopicStrings.Length)];
        }
        else
        {
            chatTopic = EventTypes.contractDisagreementTopicStrings[Random.Range(0, EventTypes.contractDisagreementTopicStrings.Length)];
        }
        finaliseTextAndCreateSocialEventBar(charId, targetCharId, socialEvent, chatText, chatTopic, damageTaken);
    }
    public void finaliseTextAndCreateSocialEventBar(int charId, int targetCharId, SingleEvent socialEvent, string eventOpener, string eventTopic, int[,] damageTaken = null)
    {
        if(charId == 0 || targetCharId == 0)
        {
            Debug.LogError("Error while creating social event bar. A characters ID is 0. charId: " + charId + ", targetCharId: " + targetCharId);
        }

        string charName = AllCharacterStorage.findAliveCharacterFromID(charId).characterDetails.charName;
        string targetCharName = AllCharacterStorage.findAliveCharacterFromID(targetCharId).characterDetails.charName;

        string eventString = charName + " and " + targetCharName + eventOpener + eventTopic;

        if(damageTaken != null)
        {
            string damageString = ". ";
            for(int i = 0; i < damageTaken.GetLength(0); i++)
            {
                string damageTakerName = AllCharacterStorage.findAliveCharacterFromID(damageTaken[i,0]).characterDetails.charName;
                damageString = damageString + damageTakerName + " took " + damageTaken[i, 1] +" damage. ";

            }
            eventString = eventString + damageString;
        }

        socialEvent.eventDescription = eventString;

        EventViewer.createEventBar(socialEvent);
    }
}
