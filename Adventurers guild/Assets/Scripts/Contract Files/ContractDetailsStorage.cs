using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractDetailsStorage : MonoBehaviour
{
    ContractStorage ContractStorage;

    public List<ContractDetails> AllContractDetailsList;

    private void Start()
    {
        ContractStorage = gameObject.GetComponent<ContractStorage>();
    }
    public ContractDetails genRandomContractDetails(int contractId, int difficulty)
    {
        Debug.Log("----Started generating contract details for contract Id: " + contractId + "----");
        
        int goodness = Random.Range(1, 11);
        Debug.Log("Got goodness score: " + goodness);

        ContractTypes.contractGoalTag contractGoalTag =
            ContractTypes.getRandomContractGoalTag();
        Debug.Log("Got goal Tag: " + contractGoalTag);

        ContractTypes.contractTargetTag contractTargetTag =
            getRandomContractTargetTag(difficulty, contractGoalTag, goodness);
        Debug.Log("Got target Tag: " + contractTargetTag);

        ///TODO: make caves, dungeon etc overriding - meaning they can be in any biome
        LocationTypes.locationSpecificTag locationSpecificTag =
            getLocationForTarget(contractTargetTag);
        Debug.Log("Got location Tag: " + locationSpecificTag);

        LocationTypes.locationBiomeTag locationBiomeTag =
            getBiomeForTarget(contractTargetTag);
        Debug.Log("Got biome Tag: " + locationBiomeTag);



        return loadContractDetails(contractId, goodness, 0, locationBiomeTag, locationSpecificTag, contractTargetTag, contractGoalTag);
    }


    public ContractTypes.contractTargetTag getRandomContractTargetTag(int difficulty, 
        ContractTypes.contractGoalTag contractGoalTag, int goodness)
    {
        List<ContractTypes.contractTargetTag> availableTargets = ContractTypes.getAllTargetTags();

        //First we limit potential targets by difficulty
        availableTargets = limitTargetTagListByDifficultyRating(difficulty, availableTargets, contractGoalTag);

        //Second we limit potential targets by contract goal
        availableTargets = limitTargetTagListByContractGoal(contractGoalTag, availableTargets);

        ///TODO add weighting of targets chosen according to rarity
        ///

        return availableTargets[Random.Range(0, availableTargets.Count)];
    }

    public List<ContractTypes.contractTargetTag> limitTargetTagListByDifficultyRating
        (int difficulty, List<ContractTypes.contractTargetTag> contractTargetTagList, ContractTypes.contractGoalTag contractGoalTag)
    {
        List<ContractTypes.contractTargetTag> outputList = contractTargetTagList;

        foreach (ContractTypes.contractTargetTag contractTargetTag in contractTargetTagList)
        {
            if (!doesDifficultyFitMonsterRating(difficulty, contractTargetTag, contractGoalTag))
            {
                outputList.Remove(contractTargetTag);
            }
        }
        if (outputList.Count == 0)
        {
            Debug.LogError("No available contract targets left after difficulty reduction! : Difficulty: " + difficulty);
        }
        return outputList;
    }
    public bool doesDifficultyFitMonsterRating(int contractDifficulty, ContractTypes.contractTargetTag contractTargetTag,
        ContractTypes.contractGoalTag contractGoalTag)
    {
        Target target = ContractStorage.getTargetFromDictionary(contractTargetTag);

        int difficultyMin = 0;
        int difficultyMax = 0;

        foreach (applicableGoal applicableGoal in target.applicableGoalsList)
        {
            if(applicableGoal.contractGoalTag == contractGoalTag)
            {
                difficultyMin = applicableGoal.difficultyMin;
                difficultyMax = applicableGoal.difficultyMax;
            }
        }
        if(difficultyMin == 0 && difficultyMax == 0)
        {
            Debug.LogError("Difficulty not found for goal match. contractTargetTag: " 
                + contractTargetTag + ", contractGoalTag: " + contractGoalTag);
        }

        if (difficultyMin <= contractDifficulty && contractDifficulty <= difficultyMax)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public List<ContractTypes.contractTargetTag> limitTargetTagListByContractGoal
        (ContractTypes.contractGoalTag contractGoalTag, List<ContractTypes.contractTargetTag> contractTargetTagList)
    {
        List<ContractTypes.contractTargetTag> outputList = contractTargetTagList;

        foreach(ContractTypes.contractTargetTag contractTargetTag in contractTargetTagList)
        {
            Target target = ContractStorage.getTargetFromDictionary(contractTargetTag);

            //if target does not contain the applicable goal, remove it from the list
            if (!isGoalTagInGoalList(target.applicableGoalsList, contractGoalTag))
            {
                outputList.Remove(contractTargetTag);
            }
        }
        if (outputList.Count == 0)
        {
            Debug.LogError("No available contract targets left after contractGoalTag reduction! : contractGoalTag: " + contractGoalTag);
        }
        return outputList;
    }

    bool isGoalTagInGoalList(List<applicableGoal> applicableGoalsList, ContractTypes.contractGoalTag contractGoalTag)
    {
        bool isIncluded = false;
        foreach(applicableGoal applicableGoal in applicableGoalsList)
        {
            if(applicableGoal.contractGoalTag == contractGoalTag)
            {
                isIncluded = true;
                break;
            }
        }
        return isIncluded;
    }

    LocationTypes.locationBiomeTag getBiomeForTarget(ContractTypes.contractTargetTag contractTargetTag)
    {
        Target target = ContractStorage.getTargetFromDictionary(contractTargetTag);

        return target.allowedBiomes[Random.Range(0, target.allowedBiomes.Count)];
    }

    LocationTypes.locationSpecificTag getLocationForTarget(ContractTypes.contractTargetTag contractTargetTag)
    {
        Target target = ContractStorage.getTargetFromDictionary(contractTargetTag);

        return target.allowedLocations[Random.Range(0, target.allowedLocations.Count)];
    }


    public ContractDetails loadContractDetails(int conID, int goodness, int locationCoordinate, LocationTypes.locationBiomeTag locationBiomeTag,
        LocationTypes.locationSpecificTag locationSpecificTag, ContractTypes.contractTargetTag contractTargetTag, ContractTypes.contractGoalTag contractGoalTag)
    {
        ContractDetails thisContractsDetails = gameObject.AddComponent<ContractDetails>();
        thisContractsDetails.contractID = conID;
        thisContractsDetails.goodness = goodness;
        thisContractsDetails.locationCoordinate = locationCoordinate;
        thisContractsDetails.locationBiomeTag = locationBiomeTag;
        thisContractsDetails.locationSpecificTag = locationSpecificTag;
        thisContractsDetails.contractTargetTag = contractTargetTag;
        thisContractsDetails.contractGoalTag = contractGoalTag;

        AllContractDetailsList.Add(thisContractsDetails);

        Debug.Log("----Finished generating contract details for contract Id: " + conID + "----");
        return thisContractsDetails;
    }

    public ContractDetails getContractDetailsFromID(int conID)
    {
        foreach (ContractDetails c in AllContractDetailsList)
        {
            if (c.contractID == conID)
            {
                return c;
            }
        }
        Debug.LogError("Invalid ID passed: " + conID);
        return null;
    }

    public void clearData()
    {
        AllContractDetailsList.Clear();
    }
}
