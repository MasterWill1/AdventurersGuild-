using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MoodTypes : MonoBehaviour
{
    public static List<moodletTag> allMoodletTags =
        Enum.GetValues(typeof(moodletTag)).Cast<moodletTag>().ToList();

    public enum moodletTag
    {
        nullMoodlet, optimistic, pessimistic, newlyRecruited, successfulQuest, unsuccessfulQuest, wasPayed, injuredMinor,
        injuredMajor, enjoyedDowntimeMinor, enjoyedDowntimeMajor, onMisalignedContract, onAlignedContract, completedMisalignedQuest, 
        completedAlignedQuest, dislikeColleague, likeColleague
    }

    public static List<moodletTag> getAllMoodletTags()
    {
        List<moodletTag> outputList = allMoodletTags;
        outputList.Remove(moodletTag.nullMoodlet);
        return outputList;
    }
}
