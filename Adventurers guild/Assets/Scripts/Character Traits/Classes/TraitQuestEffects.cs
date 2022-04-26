using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitQuestEffects
{
    public int speed = 0;
    public int critSuccessChance = 0;
    public int critFailChance = 0;

    public TraitQuestEffects(int _speed=0, int _critsuccessChance = 0, int _critFailChance = 0)
    {
        speed = _speed;
        critSuccessChance = _critsuccessChance;
        critFailChance = _critFailChance;
    }
}
