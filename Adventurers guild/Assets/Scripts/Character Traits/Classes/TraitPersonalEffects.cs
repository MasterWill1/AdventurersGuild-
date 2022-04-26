using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitPersonalEffects
{
    public List<MoodTypes.moodletTag> perminantMoods;
    public List<MoodTypes.moodletTag> disabledMoods;
    public List<TraitClashEffect> traitClashEffects;
    public int stressThresholdEffect;
    public TraitPersonalEffects(List<MoodTypes.moodletTag> _perminantMoods, List<MoodTypes.moodletTag> _disabledMoods,
        List<TraitClashEffect> _traitClashEffects, int _stressThresholdEffect)
    {
        perminantMoods = _perminantMoods;
        disabledMoods = _disabledMoods;
        traitClashEffects = _traitClashEffects;
        stressThresholdEffect = _stressThresholdEffect;
    }
}
