using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitClassEffects
{
    public int fighterCombatScoreEffect;
    public int wizardCombatScoreEffect;
    public int bardCombatScoreEffect;
    public int rangerCombatScoreEffect;
    public int paladinCombatScoreEffect;
    public int rogueCombatScoreEffect;
    
    public TraitClassEffects(int _fighterCombatScoreEffect, int _wizardCombatScoreEffect, int _bardCombatScoreEffect,
        int _rangerCombatScoreEffect, int _paladinCombatScoreEffect, int _rogueCombatScoreEffect)
    {
        fighterCombatScoreEffect = _fighterCombatScoreEffect;
        wizardCombatScoreEffect = _wizardCombatScoreEffect;
        bardCombatScoreEffect = _bardCombatScoreEffect;
        rangerCombatScoreEffect = _rangerCombatScoreEffect;
        paladinCombatScoreEffect = _paladinCombatScoreEffect;
        rogueCombatScoreEffect = _rogueCombatScoreEffect;
    }
}
