using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArchController : MonoBehaviour
{
    private TextMeshPro burgerCostText;
    private StackController stackController;

    void Awake()
    {
        burgerCostText = this.gameObject.GetComponent<TextMeshPro>();
        stackController = GameObject.Find("PlayerObj").GetComponent<StackController>();
        InvokeRepeating("ArchPriceCalculation", 1f, 0.5f);
    }

    public void ArchPriceCalculation()
    {
        burgerCostText.text = "SELL \n" + stackController.burgerPrice + "$"; 
    }
}
