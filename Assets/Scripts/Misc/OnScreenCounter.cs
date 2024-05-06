using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnScreenCounter : MonoBehaviour
{

    public TextMeshProUGUI _itemCounter;
    public int Counter = 0;

    // Update is called once per frame
    void Update()
    {
        _itemCounter.SetText($"Fish Collected - {Counter}");
    }

    public void counter() 
    {
        Counter++;
    }
}
