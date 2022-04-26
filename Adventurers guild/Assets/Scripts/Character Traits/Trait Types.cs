using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TraitTypes
{
    public static List<traitTag> allTraitTags =
        Enum.GetValues(typeof(traitTag)).Cast<traitTag>().ToList();
    public enum traitTag
    {
        none, brave, coward, strong, weak, determined, weakWilled , agile, clumsy , clever, stupid, attractive, ugly,
        kind, callous, lucky, unlucky, jogger, slowCoach, drunkard, teetotal, optimistic, pessimistic, stoic, emotional
    }
    public static List<traitTag> getAllTraitTags()
    {
        List<traitTag> outputList = allTraitTags;
        outputList.Remove(traitTag.none);
        return outputList;
    }

}
