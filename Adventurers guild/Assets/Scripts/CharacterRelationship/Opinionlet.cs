using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opinionlet : MonoBehaviour
{
    public int charId; //character opinionlet is assigned to 
    public int targetCharId; //character that the opinionlet is of
    public OpinionTypes.opinionletTag opinionletTag; //Id for the mood event
    public int opinionBuff; //how it affects characters mood
    public int timeLeft; //days left of the moodlet
    public bool canRepeat; //flag to determine whether a moodlet can happen multiple times or not
    public bool isPerminant;

    public Opinionlet(int _charId, int _targetCharId, OpinionTypes.opinionletTag _opinionletTag, int _opinionBuff, int _timeLeft, bool _canRepeat, bool _isPerminant)
    {
        charId = _charId;
        targetCharId = _targetCharId;
        opinionletTag = _opinionletTag;
        opinionBuff = _opinionBuff;
        timeLeft = _timeLeft;
        canRepeat = _canRepeat;
        isPerminant = _isPerminant;
    }

    public void updateUsingOpinionletDef(OpinionletDef opinionletDef)
    {
        opinionBuff = opinionletDef.opinionBuff;
        timeLeft = opinionletDef.duration;
        isPerminant = opinionletDef.isPermininant;
        canRepeat = opinionletDef.canRepeat;
    }
}
