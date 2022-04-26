using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class Trait
{
    public TraitTypes.traitTag traitTag;
    public string traitTitle;
    public string traitDescription;
    public List<TraitTypes.traitTag> incompatableTraits;
    public int commonality;
    public TraitPersonalEffects traitPersonalEffects;
    public TraitQuestEffects traitQuestEffects;
    public TraitClassEffects TraitClassEffects;
    //TODO:
    //list of disabled events
    //list of enable trait specific events
    //events with increased likely hood?

    public Trait(XmlNode curItemNode)
    {
        incompatableTraits = new List<TraitTypes.traitTag>();
        List<MoodTypes.moodletTag> perminantMoods = new List<MoodTypes.moodletTag>();
        List<MoodTypes.moodletTag> disabledMoods = new List<MoodTypes.moodletTag>();
        List<TraitClashEffect> traitClashEffects = new List<TraitClashEffect>();
        traitPersonalEffects = new TraitPersonalEffects(perminantMoods, disabledMoods, traitClashEffects, 0);
        traitQuestEffects = new TraitQuestEffects(0, 0, 0);
        TraitClassEffects = new TraitClassEffects(0, 0, 0, 0, 0, 0);

        //trait tag
        Enum.TryParse(curItemNode.Attributes["label"].Value, out traitTag);
        Debug.Log("started creating trait: " + traitTag);

        //trait title
        traitTitle = curItemNode["Title"].InnerText;
        //description
        traitDescription = curItemNode["Description"].InnerText;

        //incompatable traits
        XmlNode incompatableTraitsContainer = curItemNode.SelectSingleNode("IncompatableTraits");
        XmlNodeList traitsList = incompatableTraitsContainer.SelectNodes("li");
        foreach (XmlNode xmlNode in traitsList)
        {
            TraitTypes.traitTag thisTrait;
            Enum.TryParse(xmlNode.InnerText, out thisTrait);
            incompatableTraits.Add(thisTrait);
        }

        //commonality
        commonality = int.Parse(curItemNode["Commonality"].InnerText);

        //EFFECTS
        XmlNode effectsNode = curItemNode.SelectSingleNode("Effects");
        if (effectsNode != null)
        {
            //personal effects
            if (effectsNode.SelectSingleNode("PersonalEffects") != null)
            {
                XmlNode personalEffectsNode = effectsNode.SelectSingleNode("PersonalEffects");

                if (curItemNode.SelectSingleNode("PerminantMoodlet") != null)
                {
                    XmlNodeList perminantMoodletList = personalEffectsNode.SelectNodes("PerminantMoodlet");
                    foreach (XmlNode moodletNode in perminantMoodletList)
                    {
                        MoodTypes.moodletTag thisMoodlet;
                        Enum.TryParse(moodletNode.InnerText, out thisMoodlet);
                        traitPersonalEffects.perminantMoods.Add(thisMoodlet);
                    }
                }

                if (curItemNode.SelectSingleNode("DisabledMoodlet") != null)
                {
                    XmlNodeList disabledMoodletList = personalEffectsNode.SelectNodes("DisabledMoodlet");
                    foreach (XmlNode disabledNode in disabledMoodletList)
                    {
                        MoodTypes.moodletTag thisMoodlet;
                        Enum.TryParse(disabledNode.InnerText, out thisMoodlet);
                        traitPersonalEffects.disabledMoods.Add(thisMoodlet);
                    }
                }

                if (curItemNode.SelectSingleNode("clashingTraits") != null)
                {
                    traitPersonalEffects.stressThresholdEffect = int.Parse(curItemNode["stressThreshold"].InnerText);
                }

                if (curItemNode.SelectSingleNode("stressThreshold") != null)
                {
                    XmlNodeList clashingTraitList = personalEffectsNode.SelectNodes("clashingTraits");
                    foreach (XmlNode clashingTraitNode in clashingTraitList)
                    {
                        TraitTypes.traitTag thisTrait;
                        Enum.TryParse(clashingTraitNode.Attributes["label"].Value, out thisTrait);

                        TraitClashEffect thisTraitClashEffect = new TraitClashEffect(thisTrait, int.Parse(clashingTraitNode.InnerText));

                        traitPersonalEffects.traitClashEffects.Add(thisTraitClashEffect);
                    }
                }
            }

            //quest effects
            if (effectsNode.SelectSingleNode("QuestEffects") != null)
            {
                XmlNode questEffectsNode = effectsNode.SelectSingleNode("QuestEffects");

                if (curItemNode.SelectSingleNode("Speed") != null)
                {
                    traitQuestEffects.speed = int.Parse(questEffectsNode["Speed"].InnerText);
                }
                if (curItemNode.SelectSingleNode("CritSuccChance") != null)
                {
                    traitQuestEffects.critSuccessChance = int.Parse(questEffectsNode["CritSuccChance"].InnerText);
                }
                if (curItemNode.SelectSingleNode("CritFailChance") != null)
                {
                    traitQuestEffects.critFailChance = int.Parse(questEffectsNode["CritFailChance"].InnerText);
                }
            }

            //class effects
            if (effectsNode.SelectSingleNode("ClassEffects") != null)
            {
                XmlNode classEffectsNode = effectsNode.SelectSingleNode("ClassEffects");

                if (classEffectsNode.SelectSingleNode("All") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("All").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                    TraitClassEffects.bardCombatScoreEffect = effect;
                    TraitClassEffects.paladinCombatScoreEffect = effect;
                    TraitClassEffects.rangerCombatScoreEffect = effect;
                    TraitClassEffects.wizardCombatScoreEffect = effect;
                    TraitClassEffects.rogueCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("Martial") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("Martial").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                    TraitClassEffects.rogueCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("Hybrid") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("Hybrid").Attributes["combatScore"].Value);

                    TraitClassEffects.paladinCombatScoreEffect = effect;
                    TraitClassEffects.rangerCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("Caster") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("Caster").Attributes["combatScore"].Value);

                    TraitClassEffects.bardCombatScoreEffect = effect;
                    TraitClassEffects.wizardCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("fighter") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("fighter").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("wizard") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("wizard").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("bard") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("bard").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("ranger") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("ranger").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("paladin") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("paladin").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
                if (classEffectsNode.SelectSingleNode("rogue") != null)
                {
                    int effect = int.Parse(classEffectsNode.SelectSingleNode("rogue").Attributes["combatScore"].Value);

                    TraitClassEffects.fighterCombatScoreEffect = effect;
                }
            }
        }
    }
}
