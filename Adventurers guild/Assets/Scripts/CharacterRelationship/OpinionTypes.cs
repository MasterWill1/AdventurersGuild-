using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OpinionTypes : MonoBehaviour
{
    public static List<opinionletTag> allOpinionletTags =
        Enum.GetValues(typeof(opinionletTag)).Cast<opinionletTag>().ToList();

    public enum opinionletTag
    {
        nullOpinionlet, successfulQuestTogether, unsuccessfulQuestTogether, smallGoodnessDifference, largeGoodnessDifference,
        sameGoodness, hadChat, hadDeepChat, goodTeamwork, hadDisagreement, hadArgument, hadSmallFight, hadBigFight, socialisedTogether
    }

    public static List<opinionletTag> getAllOpinionletTags()
    {
        List<opinionletTag> outputList = allOpinionletTags;
        outputList.Remove(opinionletTag.nullOpinionlet);
        return outputList;
    }
}
