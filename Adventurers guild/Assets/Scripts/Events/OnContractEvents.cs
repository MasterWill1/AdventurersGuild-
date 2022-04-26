using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContractEvents : MonoBehaviour
{
    public void decideAndTriggerOnContractEvent(int conId)
    {

    }

    void isThereAEvent(out bool eventOccurs, out bool isGoodEvent)
    {
        isGoodEvent = false;

        double eventHappensChance = 0.8;
        double eventHappensScore = Random.Range(0f, 1f);

        if (eventHappensScore > eventHappensChance)
        {
            eventOccurs = true;
            double goodEventChance = 0.5;
            double goodEventScore = Random.Range(0f, 1f);
            if (goodEventScore > goodEventChance)
            {
                isGoodEvent = true;
            }
        }
        else
        {
            eventOccurs = false;
        }
    }
}
