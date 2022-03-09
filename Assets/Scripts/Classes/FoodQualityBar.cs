using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodQualityBar : MonoBehaviour
{
    public Image fillImage;
    private int maximum, current;
    private StackController stackController;
    private bool fill = false, unfill = false;
    private float badIndex = 1;
    public GameObject FoodQualityBarBackground;
    void Awake()
    {
        //FoodQualityBarBackground = GameObject.Find("FoodQualityBarBackground").gameObject;
        fillImage = gameObject.GetComponent<Image>();
        stackController = GameObject.Find("PlayerObj").GetComponent<StackController>();
    }

    public void UpdateFoodQualityBar()
    {
        this.gameObject.active = true;
        if(stackController.badIndex < badIndex)
        {
            badIndex = stackController.badIndex;
            fill = true;
        }
        else if(stackController.badIndex > badIndex)
        {
            badIndex = stackController.badIndex;
            unfill = true;
        }
    }

    private void FixedUpdate()
    {
        if(fill)
        {
            fillImage.fillAmount -= 0.005f;
            if (fillImage.fillAmount <= badIndex)
                fill = false;
        }
        else if(unfill)
        {
            fillImage.fillAmount += 0.005f;
            if (fillImage.fillAmount >= badIndex)
                unfill = false;
        }
    }
}
