using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class StackController : MonoBehaviour
{
    public GameObject RemoveBadFoodFX, SellFX;
    public List<GameObject> FinalFX;
    public List<GameObject> Burger;
    [HideInInspector] public float money, burgerPrice, badIndex = 1;
    public float finalIndex;
    private TextMeshProUGUI moneyText;
    private GameObject Ingredient, foodQualityBarObject;
    private bool inBurger = false, finalRotation = false, firstIngredient = true;
    private Animation animation;
    public List<AnimationClip> animations;
    private CameraShake cameraShake;
    private CameraMove cameraMove;
    public FoodQualityBar foodQualityBar;
    int currentBurgerCount;
    private UIManager UIManager;
    private Vector3 finalPos;
    private CinemachineVirtualCamera virtualCamera;
    private AudioController audioController;

    private void Awake()
    {
        UIManager = GameObject.Find("[UI]").GetComponent<UIManager>();
        moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        moneyText.text =money + "";
        cameraShake = GameObject.Find("CinemachineCamera1").GetComponent<CameraShake>();
        cameraMove = GameObject.Find("CinemachineCamera1").GetComponent<CameraMove>();
        virtualCamera = GameObject.Find("CinemachineCamera1").GetComponent<CinemachineVirtualCamera>();
        foodQualityBar = GameObject.Find("FoodQualityBar").GetComponent<FoodQualityBar>();
        audioController = GameObject.Find("Player").GetComponent<AudioController>();

        currentBurgerCount = 0;
    }
    void FixedUpdate()
    {
        if (Ingredient != null)
        {
            if (inBurger == true)
            {
                Ingredient.transform.Translate(Vector3.down * Time.deltaTime * 5, Space.Self);
            }
            if(Ingredient.transform.position.y <= transform.position.y + (0.2f * (Burger.Count) + 0.2f) && !firstIngredient)
            {
                inBurger = false;
            }
            if (Ingredient.transform.position.y <= transform.position.y + (0.3f * (Burger.Count)) && firstIngredient)
            {
                inBurger = false;
                firstIngredient = false;
            }
        }
        if(finalRotation)
        {
            this.transform.Rotate(0, 0.1f, 0);
            gameObject.transform.position = Vector3.MoveTowards(transform.position, finalPos, 3*Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Food" || collider.tag == "BadFood")
        {
            LiftingFood(collider);
            BurgerPriceCalculation(collider);
            if (collider.tag == "Food")
            {
                money += 2;
                moneyText.text = money + "";
                audioController.PickUpGoodFoodAudio();
            }
            if (collider.tag == "BadFood")
            {
                cameraShake.Shake();
                Instantiate(RemoveBadFoodFX, collider.transform.position, Quaternion.identity);
                audioController.PickUpBadFoodAudio();
            }
            Destroy(collider.gameObject);
        }
        if (collider.tag == "SellArch")
        {
            SellArch();
        }
        if (collider.tag == "RemoveBadArch")
        {
            RemoveBadArch();
        }
        if(collider.tag == "FinalArch")
        {
            Invoke("Final", 0f);
            finalPos = new Vector3(collider.transform.position.x, collider.transform.position.y + 1.1f, collider.transform.position.z);
            audioController.FinishAudio();
        }    
    }

    private void BurgerPriceCalculation(Collider collider)
    {
        float countPrice = 0;
        for (int i = 0; i < Burger.Count; i++)
        {
            if (Burger[i].tag == "Food")
                countPrice += 10;
            if (Burger[i].tag == "BadFood")
            {
                if (badIndex == 0.3f)
                    badIndex = 0.3f;
                else if (badIndex > 0.3f)
                    badIndex -= 0.1f;

            }
        }
        foodQualityBar.UpdateFoodQualityBar();
        burgerPrice = (int)(countPrice * badIndex);
        badIndex = 1;
    }

    private void LiftingFood(Collider collider)
    {
        Ingredient = Instantiate(collider.gameObject, new Vector3(transform.position.x, transform.position.y + (0.2f * (Burger.Count + 1) + 2f), transform.position.z),
            Quaternion.Euler(transform.rotation.x, Random.Range(-75f, 75f), transform.rotation.z), transform);
        if (Ingredient.GetComponentInChildren<Animation>())
        {
            animation = Ingredient.GetComponentInChildren<Animation>();
            inBurger = true;
            ChangeAnimation();
        }

        Burger.Add(Ingredient);
        cameraMove.UpdateCamDistance(Burger.Count, Burger.Count - 1);
    }
    private void SellArch()
    {
        for (int i = 0; i < Burger.Count; i++)
        {
            Instantiate(SellFX, Burger[i].transform.position, Quaternion.identity, this.gameObject.transform);
            Destroy(Burger[i]);
        }
        money += burgerPrice;
        burgerPrice = 0;
        moneyText.text =money + "";
        badIndex = 1;
        foodQualityBar.UpdateFoodQualityBar();

        Burger.RemoveRange(0, Burger.Count);
        firstIngredient = true;
        audioController.ArchSellFoodAudio();
    }
    private void RemoveBadArch()
    {
        bool badFood = false;
        for (int i = 0; i < Burger.Count; i++)
        {
            if (Burger[i].tag == "BadFood")
            {
                badFood = true;
            }
        }
        if (money >= 10 && badFood)
        {
            money -= 10;
            moneyText.text =money + "";
            //Тут дитчь та ещё, продублирована проверка и удаление, но прикол в том что без этого оно не удаляет всё плохое, по абсолююто не ясной мне причине
            //Я бы разобрался но сроки жмут, по этому так
            BadCalculation();
            BadCalculation();
            BadCalculation();
            badIndex = 1;
            foodQualityBar.UpdateFoodQualityBar();
            if (Burger.Count == 0)
                firstIngredient = true;
            audioController.ArchBadFoodRemoveAudio();
        }
    }
    private void BadCalculation()
    {
        for (int i = 0; i < Burger.Count; i++)
        {
            if (Burger[i].tag == "BadFood")
            {
                Instantiate(RemoveBadFoodFX, Burger[i].transform.position, Quaternion.identity, this.gameObject.transform);
                Destroy(Burger[i]);
                Burger.RemoveAt(i);
            }
        }
        for (int i = 0; i < Burger.Count; i++)
        {
            Burger[i].transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f * (i + 1), transform.position.z);
        }
    }
    private void Final()
    {
        UIManager.isGameStarted = false;
        money += (int)(burgerPrice*finalIndex);
        moneyText.text =money + "";
        InvokeRepeating("FinalFireworks", 0f, 0.5f);
        finalRotation = true;
        virtualCamera.Follow = this.gameObject.transform;
        virtualCamera.LookAt = this.gameObject.transform;
        UIManager.OnGameDone();
    }
    private void FinalFireworks()
    {
        int i = Random.Range(0, 6);
        Instantiate(FinalFX[i], new Vector3(transform.position.x + Random.Range(-4, 4), transform.position.y + Random.Range(0, 7), transform.position.z + Random.Range(-4, 4)), Quaternion.identity, this.gameObject.transform);
    }
    public void ChangeAnimation()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            if (animations[i].name == "flip" && inBurger)
            {
                animation.clip = animations[i];
                animation.AddClip(animations[i], "flip");
                animation.Play();
            }
            else if (animations[i].name == "food" && !inBurger)
            {
                animation.clip = animations[i];
                animation.AddClip(animations[i], "food");
                animation.Play();
            }

        }
    }
}
