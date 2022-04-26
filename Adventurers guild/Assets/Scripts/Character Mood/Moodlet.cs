using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moodlet : MonoBehaviour
{
    public int charId; //character moodlet is assigned to 
    public MoodTypes.moodletTag moodEventEnum; //tag for the mood event
    public int moodBuff; //how it affects characters mood
    public int timeLeft; //days left of the moodlet
    public bool isPerminant;
    public bool canRepeat; //flag to determine whether a moodlet can happen multiple times or not

    public Moodlet(int _charId, MoodTypes.moodletTag _moodEventEnum, int _moodBuff, int _timeLeft, bool _isPerminant, bool _canRepeat)
    {
        charId = _charId;
        moodEventEnum = _moodEventEnum;
        moodBuff = _moodBuff;
        timeLeft = _timeLeft;
        isPerminant = _isPerminant;
        canRepeat = _canRepeat;
    }

    public void updateUsingMoodletDef(MoodletDef moodletDef)
    {
        moodBuff = moodletDef.moodBuff;
        timeLeft = moodletDef.duration;
        isPerminant = moodletDef.isPermininant;
        canRepeat = moodletDef.canRepeat;
    }
}
