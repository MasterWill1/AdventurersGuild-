using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyWorldDetailsHandler : MonoBehaviour
{
    public GameObject allCharacterHandler, ContractHandler;
    AllCharacterStorage AllCharacterStorage;
    ContractStorage ContractStorage;

    [HideInInspector]
    public int[] keyWorldDetailsArray;
    public Text goldText;
    public Text dateText, renownText, reputationText;
    //public GameObject Generator, ContractHandler;
    //Generator generatorScript;
    //ContractHandler contractScript;

    public Button pauseTimeButton;
    public Button playTimeButton;
    bool isTimePassing = false;

    float secondsCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        AllCharacterStorage = allCharacterHandler.GetComponent<AllCharacterStorage>();
        ContractStorage = ContractHandler.GetComponent<ContractStorage>();

        // [totalGold, date, day, renown, reputation]
        keyWorldDetailsArray = new int[5];
        //starting gold
        keyWorldDetailsArray[0] = 100;
        //starting day
        keyWorldDetailsArray[1] = 1;
        //days until payday/new week
        keyWorldDetailsArray[2] = 7;
        //renown
        keyWorldDetailsArray[3] = 0;
        //reputation
        keyWorldDetailsArray[4] = 50;

        updateGoldVisual();
        updateDateVisual();
        updateRenownVisual();
        updateRepuatationVisual();

        //generatorScript = Generator.GetComponent<Generator>();
        //contractScript = ContractHandler.GetComponent<ContractHandler>();

        pauseTimeButton.interactable = false;

        Button pauseTimeBtn = pauseTimeButton.GetComponent<Button>();
        pauseTimeBtn.onClick.AddListener(pauseTime);

        Button playTimeBtn = playTimeButton.GetComponent<Button>();
        playTimeBtn.onClick.AddListener(playTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimePassing == true)
        {
            updateTime();
        }
    }

    public void changeTotalGold(int amountChanged)
    {
        keyWorldDetailsArray[0] = keyWorldDetailsArray[0] + amountChanged;
        updateGoldVisual();

        //gold has changed, update whether can afford each recruitable char
        GameObject[] recruitableChars = GameObject.FindGameObjectsWithTag("RecruitableCharacter");
        for (int g = 0; g < recruitableChars.Length; g++)
        {
            RecruitableCharacter rc = recruitableChars[g].GetComponent<RecruitableCharacter>();
            int charCost = rc.getCharCost();

            if (keyWorldDetailsArray[0] < charCost)
            {
                rc.recruitButton.interactable = false;
            }
            else
            {
                rc.recruitButton.interactable = true;
            }
        }
    }



    void updateDateVisual()
    {
        dateText.text = returnCurrentDateString();
    }

    string returnCurrentDateString()
    {
        return "Day: " + getDate().ToString() + ", " + HelperFunctions.whatDayIsIt(keyWorldDetailsArray[2]);
    }

    public int getDate()
    {
        return keyWorldDetailsArray[1];
    }

    void updateGoldVisual()
    {
        goldText.text = "Gold: " + keyWorldDetailsArray[0].ToString();
    }

    void updateRenownVisual()
    {
        renownText.text = "Renown: " + keyWorldDetailsArray[3].ToString();
    }

    void updateRepuatationVisual()
    {
        reputationText.text = "Reputation: " + keyWorldDetailsArray[4].ToString() + ", " + 
            HelperFunctions.goodnessRatingFromBigInt(keyWorldDetailsArray[4]);
    }

    public void loadKeyWorldData(int[] kwd)
    {
        System.Array.Copy(kwd, keyWorldDetailsArray, kwd.Length);
        updateGoldVisual();
        updateDateVisual();
        updateRenownVisual();
        updateRepuatationVisual();
    }

    void updateTime()
    {
        secondsCount += Time.deltaTime;

        //a day passes
        if(secondsCount > 2.99f)
        {
            //increase date, set visual for it
            keyWorldDetailsArray[1]++;
            secondsCount = 0;

            //activate all functions from time ticker
            timeTicker();

            //update visuals
            updateDateVisual();
        }
    }

    public void pauseTime()
    {
        isTimePassing = false;
        pauseTimeButton.interactable = false;
        playTimeButton.interactable = true;
    }

    void playTime()
    {
        isTimePassing = true;
        playTimeButton.interactable = false;
        pauseTimeButton.interactable = true;
    }

    //all the functions that are called when a day passes
    void timeTicker()
    {
        tickDayDown();
        Debug.Log("======A day passes. New Date: " + returnCurrentDateString());

        AllCharacterStorage.dailyHealRecovery();
        ContractStorage.tickContractsDown();
        AllCharacterStorage.tickAllMoodletsDown();
        AllCharacterStorage.tickAllOpinionletsDown();
        AllCharacterStorage.updateAllGoodnessDifferenceOpinionlets();
        AllCharacterStorage.updateAllCharMoodFromPartyMembers();
        AllCharacterStorage.doEachCharsDailySocialEvent();
    }

    //day out of 7. character get payed on final day
    void tickDayDown()
    {
        keyWorldDetailsArray[2]--;
        //if day is negative, reset it to monday (payday)
        if (keyWorldDetailsArray[2] < 1)
        {
            keyWorldDetailsArray[2] = 7;
            payWages();
        }
    }

    public void changeRenown(int renownChange)
    {
        keyWorldDetailsArray[3] = keyWorldDetailsArray[3] + renownChange;
        updateRenownVisual();
    }

    public void changeReputation(int repChange)
    {
        keyWorldDetailsArray[4] = keyWorldDetailsArray[4] + repChange;

        // catchers to stop goodness from going out of bounds
        if (keyWorldDetailsArray[4] < 5)
        {
            keyWorldDetailsArray[4] = 5;
        }
        if (keyWorldDetailsArray[4] > 105)
        {
            keyWorldDetailsArray[4] = 105;
        }
        updateRepuatationVisual();
    }

    
    public void payWages()
    {
        Debug.Log("--Begin Paying Wages--");
        foreach(Character c in AllCharacterStorage.allAliveCharactersList)
        {
            if (c.characterDetails.isRecruited == true)
            {
                int thisCharWage = HelperFunctions.wageFromWorth(c.characterDetails.cost);
                changeTotalGold(-thisCharWage);
                Debug.Log("Payed " + c.characterDetails.charName + " " + thisCharWage + "gp");
                AllCharacterStorage.addMoodletToChar(c.charId, MoodTypes.moodletTag.wasPayed);
            }
        }
    }
}
