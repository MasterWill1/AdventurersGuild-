using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialEvents : MonoBehaviour
{
    public GameObject AllCharacterStorageHandler, PartyHandler, TraitHandlerObject;
    AllCharacterStorage AllCharacterStorage;
    PartyStorage PartyStorage;
    EventViewer EventViewer;
    EventHandler EventHandler;
    TraitHandler TraitHandler;

    private void Start()
    {
        AllCharacterStorage = AllCharacterStorageHandler.GetComponent<AllCharacterStorage>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        EventViewer = gameObject.GetComponent<EventViewer>();
        EventHandler = gameObject.GetComponent<EventHandler>();
        TraitHandler = TraitHandlerObject.GetComponent<TraitHandler>();
    }

    public void doRandomSocialEventForChar(int charId)
    {
        Character character = AllCharacterStorage.findAliveCharacterFromID(charId);

        //if character is in party
        if(character.characterDetails.inParty != 0)
        {
            Party party = PartyStorage.findPartyFromID(character.characterDetails.inParty);
            
            //if party is on a quest
            if(party.onQuest != 0)
            {
                int targetId = PartyStorage.getRandomOtherCharacterFromParty(character.charId, party.partyID);
                //do social event for on quest to a random party member
                getAndDoRandomSocialEvent(charId, targetId, party.onQuest);
            }
            else//party is not on quest
            {                
                //75% chance of dowtime social event to another character in party, 50% chance to random other character
                if (HelperFunctions.doWeightedCoinToss(75))
                {
                    int targetId = PartyStorage.getRandomOtherCharacterFromParty(character.charId, party.partyID);
                    getAndDoRandomSocialEvent(charId, targetId, party.onQuest);
                }
                else //do social event with char not in party
                {
                    int targetId = AllCharacterStorage.GetRandomCharIdNotOnQuestOrInSameParty(charId, party.partyID);
                    getAndDoRandomSocialEvent(charId, targetId, party.onQuest);
                }
            }
        }
        else//character not in party
        {
            //do downtime event social event to random other char not on quest
            int targetId = AllCharacterStorage.GetRandomCharIdNotOnQuestOrInSameParty(charId, 0);
            getAndDoRandomSocialEvent(charId, targetId, 0);
        }
    }

    /// <summary>
    /// Gets a good or bad on-contract social event between two characters and executes it
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    void getAndDoRandomSocialEvent(int charId, int targetCharId, int contractId)
    {
        //if its a good event
        if(isGoodSocialEvent(charId, targetCharId))
        {
            EventTypes.goodSocialEvents eventToHappen = getRandomGoodSocialEvent(charId, targetCharId, contractId);
            triggerGoodSocialEvent(charId, targetCharId, contractId, eventToHappen);
        }
        else //its a bad event
        {
            EventTypes.badSocialEvents eventToHappen = getRandomBadSocialEvent(charId, targetCharId);
            triggerBadSocialEvent(charId, targetCharId, contractId, eventToHappen);
        }
    }


    /// <summary>
    /// Adds opinionlet and creates event bar accoring to social event. In some cases, events have extra effects
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="goodSocialEventToExecute"></param>
    public void triggerGoodSocialEvent(int charId, int targetCharId, int contractId, EventTypes.goodSocialEvents goodSocialEventToExecute)
    {
        switch (goodSocialEventToExecute)
        {
            case EventTypes.goodSocialEvents.hadChat:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadChat);
                EventViewer.createAGoodSocialEvent(charId, targetCharId, contractId, EventTypes.goodSocialEvents.hadChat);
                break;
            case EventTypes.goodSocialEvents.hadDeepChat:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadDeepChat);
                EventViewer.createAGoodSocialEvent(charId, targetCharId, contractId, EventTypes.goodSocialEvents.hadDeepChat);
                break;
            case EventTypes.goodSocialEvents.goodTeamwork: //TO DO - Could add bonus to contract crit success chance
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.goodTeamwork);
                EventViewer.createAGoodSocialEvent(charId, targetCharId, contractId, EventTypes.goodSocialEvents.goodTeamwork);
                break;
            case EventTypes.goodSocialEvents.socialisedTogether:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.socialisedTogether);
                EventViewer.createAGoodSocialEvent(charId, targetCharId, contractId, EventTypes.goodSocialEvents.socialisedTogether);
                break;
        }
    }

    /// <summary>
    /// Adds opinionlet and creates event bar accoring to social event. In some cases, events have extra effects (ie damage to chars)
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="contractId"></param>
    /// <param name="badSocialEventToExecute"></param>
    public void triggerBadSocialEvent(int charId, int targetCharId, int contractId, EventTypes.badSocialEvents badSocialEventToExecute)
    {
        switch (badSocialEventToExecute)
        {
            case EventTypes.badSocialEvents.hadDisagreement:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadDisagreement);
                EventViewer.createABadOnContractSocialEvent(charId, targetCharId, contractId, EventTypes.badSocialEvents.hadDisagreement);
                break;
            case EventTypes.badSocialEvents.hadArgument:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadArgument);
                EventViewer.createABadOnContractSocialEvent(charId, targetCharId, contractId, EventTypes.badSocialEvents.hadArgument);
                break;
            case EventTypes.badSocialEvents.hadSmallFight:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadSmallFight);
                int[,] lowFightDamage = triggerFightDamage(charId, targetCharId, EventTypes.damageType.low);
                EventViewer.createABadOnContractSocialEvent(charId, targetCharId, contractId, EventTypes.badSocialEvents.hadSmallFight, lowFightDamage);                
                break;
            case EventTypes.badSocialEvents.hadBigFight:
                AllCharacterStorage.addOpinionletToChar(charId, targetCharId, OpinionTypes.opinionletTag.hadBigFight);
                int[,] highFightDamage = triggerFightDamage(charId, targetCharId, EventTypes.damageType.high);
                EventViewer.createABadOnContractSocialEvent(charId, targetCharId, contractId, EventTypes.badSocialEvents.hadBigFight, highFightDamage);
                break;
        }
    }

    int[,] triggerFightDamage(int charId, int targetCharId, EventTypes.damageType damageType)
    {
        Character aggressor = AllCharacterStorage.findAliveCharacterFromID(charId);
        Character defender = AllCharacterStorage.findAliveCharacterFromID(targetCharId);

        List<Trait> aggressorTraits = TraitHandler.getTraitListFromTagList(aggressor.traitReference.traitList);
        List<Trait> defenderTraits = TraitHandler.getTraitListFromTagList(defender.traitReference.traitList);

        int aggressorFightDifficulty = HelperFunctions.calculateCharacterStrength(aggressor.characterDetails, aggressorTraits, aggressor.characterAttributes, null, aggressor.characterEquipment);
        int defenderFightDifficulty = HelperFunctions.calculateCharacterStrength(defender.characterDetails, defenderTraits, defender.characterAttributes, null, aggressor.characterEquipment);

        List<int> aggressorIdInList = new List<int> { charId };
        List<int> defenderIdInList = new List<int> { targetCharId };

        int[,] fightDamage = new int[2, 2];
        fightDamage[0, 0] = charId;
        fightDamage[0, 1] = EventHandler.damageXCharactersWithXDamageByExactDifficulty(aggressorFightDifficulty, defenderIdInList, damageType)[0, 1];
        fightDamage[1, 0] = targetCharId;
        fightDamage[1, 1] = EventHandler.damageXCharactersWithXDamageByExactDifficulty(defenderFightDifficulty, aggressorIdInList, damageType)[0, 1];

        return fightDamage;
    }

    /// <summary>
    /// gets a random good social event, with increased weighting/chance of a better event if they are good friends
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <returns></returns>
    public EventTypes.goodSocialEvents getRandomGoodSocialEvent(int charId, int targetCharId, int contractId) //TO DO - have weightings affected by character personality. ie kind chars are more likely to have deep chats
    {
        int opinionOfThem = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(charId, targetCharId);
        int chanceOfChat = 50;
        int chanceOfDeepChat = 30;
        int chanceOfTogetherEvent = 20; //This is either a teamwork event or them socialising event depending if they are on contract or not

        if (opinionOfThem >= 50 && opinionOfThem < 80)
        {
            chanceOfDeepChat = chanceOfDeepChat + 15;
            chanceOfTogetherEvent = chanceOfTogetherEvent + 10;
        }else if (opinionOfThem >= 80)
        {
            chanceOfDeepChat = chanceOfDeepChat + 25;
            chanceOfTogetherEvent = chanceOfTogetherEvent + 20;
        }

        int totalChances = chanceOfChat + chanceOfDeepChat + chanceOfTogetherEvent;
        int chanceScore = Random.Range(0, totalChances);

        if(chanceScore <= chanceOfChat)
        {
            return EventTypes.goodSocialEvents.hadChat;
        }else if(chanceScore >chanceOfChat && chanceScore <= chanceOfDeepChat)
        {
            return EventTypes.goodSocialEvents.hadDeepChat;
        }else if(chanceScore > chanceOfTogetherEvent)
        {
            //If char is on contract
            if (contractId != 0)
            {
                return EventTypes.goodSocialEvents.goodTeamwork; //they did goodteamwork
            }
            else //char is not on contract
            {
                return EventTypes.goodSocialEvents.socialisedTogether; //they socialised together
            }
            
        }
        else
        {
            Debug.LogError("Invalid chance score: " + chanceScore + " out of total available: " + totalChances);
            return EventTypes.goodSocialEvents.nullEvent;
        }

    }

    /// <summary>
    /// gets a random bad social event, with increased weighting/chance of a worse event if they are hate each other friends
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <returns></returns>
    public EventTypes.badSocialEvents getRandomBadSocialEvent(int charId, int targetCharId) //TO DO - have weightings affected by character personality. ie hotheaded chars are more likely to start fights
    {
        int opinionOfThem = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(charId, targetCharId);

        int hadDisagreementChance = 50;
        int hadArgumentChance = 30;
        int hadSmallFightChance = 15;
        int hadBigFightChance = 5;

        if (opinionOfThem <= -40 && opinionOfThem > -60)
        {
            hadArgumentChance = hadArgumentChance + 15;
            hadSmallFightChance = hadSmallFightChance + 10;
            hadBigFightChance = hadBigFightChance + 5;
        }
        else if (opinionOfThem <= -60 && opinionOfThem > -80)
        {
            hadArgumentChance = hadArgumentChance + 20;
            hadSmallFightChance = hadSmallFightChance + 15;
            hadBigFightChance = hadBigFightChance + 10;
        }else if(opinionOfThem <= -80)
        {
            hadArgumentChance = hadArgumentChance + 25;
            hadSmallFightChance = hadSmallFightChance + 20;
            hadBigFightChance = hadBigFightChance + 15;
        }

        int totalChances = hadDisagreementChance + hadArgumentChance + hadSmallFightChance + hadBigFightChance;
        int chanceScore = Random.Range(0, totalChances);

        if (chanceScore <= hadDisagreementChance)
        {
            return EventTypes.badSocialEvents.hadDisagreement;
        }
        else if (chanceScore > hadDisagreementChance && chanceScore <= hadArgumentChance)
        {
            return EventTypes.badSocialEvents.hadArgument;
        }
        else if (chanceScore > hadArgumentChance && chanceScore <= hadSmallFightChance)
        {
            return EventTypes.badSocialEvents.hadSmallFight;
        }
        else if (chanceScore > hadSmallFightChance)
        {
            return EventTypes.badSocialEvents.hadBigFight;
        }
        else
        {
            Debug.LogError("Invalid chance score: " + chanceScore + " out of total available: " + totalChances);
            return EventTypes.badSocialEvents.nullEvent;
        }
    }

    /// <summary>
    /// Gets the opinion one character has of another and determines if they have a positive or negative social event
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <returns></returns>
    public bool isGoodSocialEvent(int charId, int targetCharId)
    {
        int opinionOfThem = AllCharacterStorage.getCharsCumulativeOpinionOfTargetChar(charId, targetCharId);
        int chanceOfGoodEvent = 0;

        if(opinionOfThem > 60)
        {
            chanceOfGoodEvent = 80;

        }
        else if(opinionOfThem <= 60 && opinionOfThem > 20)
        {
            chanceOfGoodEvent = 60;

        }
        else if(opinionOfThem <= 20 && opinionOfThem > -20)
        {
            chanceOfGoodEvent = 50;

        }
        else if(opinionOfThem <= -20 && opinionOfThem > -60)
        {
            chanceOfGoodEvent = 40;

        }
        else if(opinionOfThem <= -60)
        {
            chanceOfGoodEvent = 20;

        }
        else
        {
            Debug.LogError("Char Id: " + charId + "Invalid opinion of target Id: " + targetCharId + ", opinion: " + opinionOfThem);
        }

        int goodEventScore = Random.Range(0, 100);
        if (goodEventScore > chanceOfGoodEvent)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
