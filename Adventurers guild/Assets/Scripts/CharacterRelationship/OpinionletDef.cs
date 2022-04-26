using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class OpinionletDef
{
    public OpinionTypes.opinionletTag opinionletTag;
    public string title;
    public string description;
    public int opinionBuff;
    public int duration;
    public bool isPermininant;
    public bool canRepeat;

    public OpinionletDef(XmlNode curOpinionletNode)
    {
        Enum.TryParse(curOpinionletNode.Attributes["label"].Value, out opinionletTag);

        title = curOpinionletNode["Title"].InnerText;
        description = curOpinionletNode["Description"].InnerText;

        opinionBuff = int.Parse(curOpinionletNode["OpinionBuff"].InnerText);
        duration = int.Parse(curOpinionletNode["Duration"].InnerText);

        isPermininant = HelperFunctions.isXMLStringTrueOrFalse(curOpinionletNode["IsPerminant"].InnerText);

        canRepeat = HelperFunctions.isXMLStringTrueOrFalse(curOpinionletNode["CanRepeat"].InnerText);
    }
}
