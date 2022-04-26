using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGameObjectOnKeyPressed : MonoBehaviour
{
    public GameObject objToShow;
    public KeyCode key;

    //void Update()
//    {
    //    if (Input.GetKeyDown(key))
   //     {
  //          showObject();
   //         Debug.Log("key pressed");
   //     }
  //  }

    void showObject()
    {
        objToShow.SetActive(true);
    }
}
