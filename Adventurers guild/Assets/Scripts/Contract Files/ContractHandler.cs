using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractHandler : MonoBehaviour
{
    ContractDetailsStorage ContractDetailsStorage;
    ContractStorage ContractStorage;

    void Start()
    {
        ContractDetailsStorage = gameObject.GetComponent<ContractDetailsStorage>();
        ContractStorage = gameObject.GetComponent<ContractStorage>();
    }

    public ContractDetails getContractDetailsFromID(int contractId)
    {
        return ContractDetailsStorage.getContractDetailsFromID(contractId);
    }

}
