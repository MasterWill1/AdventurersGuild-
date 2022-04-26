using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aMoodBar : MonoBehaviour
{
    [HideInInspector]
    public Moodlet thisMoodlet;

    [HideInInspector]
    public MoodletDef thisMoodletDef;

    string moodTitle = "";
    int moodBuff = 0;

    public GameObject moodTitleText, moodBuffText;

    public void setSelf(Moodlet moodlet, MoodletDef moodletDef)
    {
        thisMoodlet = moodlet;
        thisMoodletDef = moodletDef;

        moodTitle = moodletDef.title;
        moodBuff = moodletDef.moodBuff;

        updateVisuals();
    }

    public void updateVisuals()
    {
        moodTitleText.GetComponent<Text>().text = moodTitle;
        moodBuffText.GetComponent<Text>().text = "Mood Buff: " + moodBuff.ToString();
    }
}
