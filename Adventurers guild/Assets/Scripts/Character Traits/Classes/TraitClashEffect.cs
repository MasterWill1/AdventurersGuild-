using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitClashEffect
{
    //when 2 chars have the same or opposing trait, list the effect of this
    public TraitTypes.traitTag otherCharTraitTag;
    public int opinionEffect; //can be positive or negative
    public TraitClashEffect(TraitTypes.traitTag _otherCharTraitTag, int _opinionEffect)
    {
        otherCharTraitTag = _otherCharTraitTag;
        opinionEffect = _opinionEffect;
    }
}
