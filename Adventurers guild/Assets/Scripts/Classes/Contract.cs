using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract : MonoBehaviour
{
    public int ID;
    public int difficulty;
    public int reward;
    public int timeLeft;
    public bool isOngoing;
    public ContractDetails contractDetails;


    public Contract(int _ID, int _difficulty, int _reward, int _timeLeft, bool _isOngoing, ContractDetails _contractDetails)
    {
        ID = _ID;
        difficulty = _difficulty;
        reward = _reward;
        timeLeft = _timeLeft;
        isOngoing = _isOngoing;
        contractDetails = _contractDetails;
    }
}
