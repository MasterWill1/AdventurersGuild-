using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class MoodletDef
{
    public MoodTypes.moodletTag moodletTag;
    public string title;
    public string description;
    public int moodBuff;
    public int duration;
    public bool isPermininant;
    public bool canRepeat;

    public MoodletDef(XmlNode curOpinionletNode)
    {
        Enum.TryParse(curOpinionletNode.Attributes["label"].Value, out moodletTag);

        title = curOpinionletNode["Title"].InnerText;
        description = curOpinionletNode["Description"].InnerText;

        moodBuff = int.Parse(curOpinionletNode["MoodBuff"].InnerText);
        duration = int.Parse(curOpinionletNode["Duration"].InnerText);

        isPermininant = HelperFunctions.isXMLStringTrueOrFalse(curOpinionletNode["IsPerminant"].InnerText);

        canRepeat = HelperFunctions.isXMLStringTrueOrFalse(curOpinionletNode["CanRepeat"].InnerText);
    }
}
