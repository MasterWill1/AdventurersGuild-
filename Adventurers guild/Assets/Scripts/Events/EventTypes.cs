using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTypes
{
    public enum damageType
    {
        low, medium, high
    }
    public enum goodContractEvents
    {
        smallGoldBonus//decent loot item
    }
    public enum critGoodEndOfContractEvents
    {
        noPartyMembersHurt, largeGoldBonus//rare artifact/weapon etc
    }
    public enum softDamageEvents
    {
        allCharsDamageLow, oneCharDamageLow, oneCharDamageMedium, severalCharsDamageLow, severalCharsDamageMedium
    }
    public enum hardDamageEvents
    {
        allCharsDamageMedium, oneCharDies, oneCharDamageHigh, severalCharsDamageHigh
    }
    public enum goodSocialEvents
    {
        nullEvent, hadChat, hadDeepChat, goodTeamwork, socialisedTogether
    }
    public enum badSocialEvents
    {
        nullEvent, hadDisagreement, hadArgument, hadSmallFight, hadBigFight
    }
    public enum goodDowntimeEvents
    {
        nullEvent, hadChat, hadDeepChat, socialisedTogether
    }
    public static string[] chatTopicStrings = { " their favourite foods", " their hometown", " their parents", " the weather" };
    public static string[] chatSetupsStrings = { " had a nice chat about", " discussed", " had a chat about" };

    public static string[] deepChatTopicStrings = { " their past", " the death of a loved one", " religon", " the afterlife"};
    public static string[] deepChatSetupsStrings = { " had a deep chat about", " had a discussion about" };

    public static string[] goodTeamworkTopicStrings = { " found a shortcut", " prevented an ambush", " hunted local game" };
    public static string[] goodTeamworkSetupsStrings = { " worked well together and" };

    public static string[] genericDisagreementTopicStrings = { " the best food toppings", " pinapple on pizza", " best lute melody", " which is the best school of magic" };
    public static string[] contractDisagreementTopicStrings = { " which direction to travel", " a solution to a puzzle", " how to handle some enemies" };
    public static string[] disagreementSetupsStrings = { " had a disagreement about" };

    //public static string[] argumentTopicStrings = { " which direction to travel", " the best food toppings", " pinapple on pizza" };
    public static string[] argumentSetupsStrings = { " had an argument about", " had a falling out over" };

    public static string[] smallFightSetupStrings = { " had a small fight about" };
    public static string[] bigFightSetupStrings = { " had a big fight about" };

    public static string[] goodDowntimeSetupStrings = { " enjoyed each others company while" };
    public static string[] goodDowntimeTopicStrings = { " drinking at the local tavern", " gambling with some locals", " fishing at a nearby fishing spot" };
}
