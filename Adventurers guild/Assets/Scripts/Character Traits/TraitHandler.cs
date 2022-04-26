using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TraitHandler : MonoBehaviour
{
    [HideInInspector]
    public List<TraitReference> charTraitList;
    [HideInInspector]
    public List<CharacterAttributes> charAttributesList;

    public IDictionary<int, Trait> traitDictionary = new Dictionary<int, Trait>();

    XmlDocument targetXmlDocument;
    //public GameObject GeneratorHandlerAccessor;
    //Generator GeneratorAccessor;

    private void Start()
    {
        Debug.Log("---Begining creating Traits.---");
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/Traits");

        targetXmlDocument = new XmlDocument();
        targetXmlDocument.LoadXml(xmlTextAsset.text);

        XmlNodeList targets = targetXmlDocument.SelectNodes("/Defs/Trait");

        foreach (XmlNode target in targets)
        {
            createTraitDatum(target);
        }
        Debug.Log("---Finished creating Traits. Created " + traitDictionary.Count + " traits---");

        startupChecks();
    }
    void createTraitDatum(XmlNode trait)
    {
        Trait newTrait = new Trait(trait);

        traitDictionary.Add((int)newTrait.traitTag, newTrait);
        Debug.Log("Created trait: " + newTrait.traitTag);
    }
    /// <summary>
    /// Generates character ID, gender, goodness, lawfulness and home biome. TODO Sexuality 
    /// </summary>
    /// <param name="characterID"></param>
    /// <returns></returns>
    public CharacterAttributes getRandomAttributes(int characterID)
    {
        int gender = Random.Range(0, 2);
        int lawfulness = Random.Range(1, 11);
        int goodness = Random.Range(1, 11);

        CharacterAttributes thisCharAttribute = gameObject.AddComponent<CharacterAttributes>();
        thisCharAttribute.charID = characterID;
        thisCharAttribute.gender = gender;
        thisCharAttribute.goodness = goodness;
        thisCharAttribute.lawfulness = lawfulness;
        thisCharAttribute.nativeBiome = LocationTypes.getRandomBiome();

        charAttributesList.Add(thisCharAttribute);
        return thisCharAttribute;
    }

    /// <summary>
    /// Generates random traits for a character such as strong, brave etc
    /// </summary>
    /// <param name="characterID"></param>
    /// <returns></returns>
    public TraitReference getRandomTraits(int characterID)
    {
        int numberOfTraits = Random.Range(1, 4);
        List<TraitTypes.traitTag> availabletraitList = TraitTypes.getAllTraitTags();
        List<TraitTypes.traitTag> chosenTraits = new List<TraitTypes.traitTag>();

        for (int i = 0; i < numberOfTraits;  i++)
        {
            TraitTypes.traitTag thisTraitTag = availabletraitList[Random.Range(0, availabletraitList.Count)];
            availabletraitList.Remove(thisTraitTag);

            //remove incompatable traits from potential trait list
            traitDictionary.TryGetValue((int)thisTraitTag, out Trait thisTrait);
            foreach(TraitTypes.traitTag incompatableTrait in thisTrait.incompatableTraits)
            {
                availabletraitList.Remove(incompatableTrait);
            }

            chosenTraits.Add(thisTraitTag);
        }

        TraitReference thisTraitReference = gameObject.AddComponent<TraitReference>();
        thisTraitReference.ID = characterID;
        thisTraitReference.traitList = new List<TraitTypes.traitTag>(chosenTraits);

        Debug.Log("Created char with " + chosenTraits.Count + " traits");

        charTraitList.Add(thisTraitReference);
        return thisTraitReference;
    }

    public CharacterAttributes getAttributesFromID(int charID)
    {
        foreach(CharacterAttributes a in charAttributesList)
        {
            if(a.charID == charID)
            {
                return a;
            }
        }
        Debug.LogError("Invalid ID passed: " + charID);
        return null;
    }

    public TraitReference getTraitsReferenceFromID(int charID)
    {
        foreach (TraitReference t in charTraitList)
        {
            if (t.ID == charID)
            {
                return t;
            }
        }
        Debug.LogError("Invalid ID passed: " + charID);
        return null;
    }

    public void loadAttribute(int charID, int gender, int lawfulness, int goodness, LocationTypes.locationBiomeTag nativeBiome)
    {
        CharacterAttributes thisAttributeReference = gameObject.AddComponent<CharacterAttributes>();
        thisAttributeReference.charID = charID;
        thisAttributeReference.gender = gender;
        thisAttributeReference.lawfulness = lawfulness;
        thisAttributeReference.goodness = goodness;
        thisAttributeReference.nativeBiome = nativeBiome;

        charAttributesList.Add(thisAttributeReference);
    }

    public void loadTraits(int charID, List<int> traitsIntList)
    {

        TraitReference thisTraitReference = gameObject.AddComponent<TraitReference>();
        thisTraitReference.ID = charID;

        foreach(int i in traitsIntList)
        {
            if(i != 0)
            {
                thisTraitReference.traitList.Add((TraitTypes.traitTag)i);
            }
        }

        charTraitList.Add(thisTraitReference);
    }

    public string traitsListAsStringFromID(int charID)
    {
        List<TraitTypes.traitTag> traitTagList = getTraitsReferenceFromID(charID).traitList;

        string traitsString = getTraitTitleFromTag(traitTagList[0]);
        for(int t = 1; t < traitTagList.Count; t++)
        {
            traitsString = traitsString + ", " + getTraitTitleFromTag(traitTagList[t]);
        }
        return traitsString;
    }

    public List<Trait> getTraitListFromTagList(List<TraitTypes.traitTag> traitTagList)
    {
        List<Trait> outputList = new List<Trait>();

        foreach (TraitTypes.traitTag traitTag in traitTagList)
        {
            traitDictionary.TryGetValue((int)traitTag, out Trait thisTrait);
            outputList.Add(thisTrait);
        }
        return outputList;
    }

    string getTraitTitleFromTag(TraitTypes.traitTag traitTag)
    {
        traitDictionary.TryGetValue((int)traitTag, out Trait thisTrait);

        return thisTrait.traitTitle;
    }

    public void clearTraitData()
    {
        Debug.Log("Clearing trait data");
        charTraitList.Clear();
        charAttributesList.Clear();
    }

    void startupChecks()
    {
        if(traitDictionary.Count != TraitTypes.getAllTraitTags().Count)
        {
            Debug.LogError("Mismatch between number of traits generated and number of trait enums. Traits generated: " + traitDictionary.Count
                + ", trait enums: " + TraitTypes.getAllTraitTags().Count + "Traits in enum but not dictionary: ");

            foreach(TraitTypes.traitTag traitTag in TraitTypes.getAllTraitTags())
            {
                traitDictionary.TryGetValue((int)traitTag, out Trait thisTrait);
                if(thisTrait == null)
                {
                    Debug.LogError(traitTag);
                }
            }
        }
    }
}
