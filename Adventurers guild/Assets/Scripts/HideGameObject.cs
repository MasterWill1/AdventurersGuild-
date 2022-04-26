using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideGameObject : MonoBehaviour
{
    public GameObject objToClose;
    public Button closeButton;
    // Start is called before the first frame update
    void Start()
    {
        Button closeBtn = closeButton.GetComponent<Button>();
        closeBtn.onClick.AddListener(hideObject);
    }

    void hideObject()
    {
        objToClose.SetActive(false);
    }
}
