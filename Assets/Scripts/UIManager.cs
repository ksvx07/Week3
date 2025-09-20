using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject workUI;
    [SerializeField] private GameObject feedUI;
    [SerializeField] private GameObject battleUI;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI TimeText;

    [Header("Home Buttons")]
    [SerializeField] private Button homeToWorkButton;
    [SerializeField] private Button homeToShopButton;
    [SerializeField] private Button homeToFeedButton;
    [SerializeField] private Button homeToBattleButton;


    [Header("Shop Buttons")]
    [SerializeField] private Button shopToHomeButton;
    [SerializeField] private Button shopBuyItem1;
    [SerializeField] private Button shopBuyItem2;
    [SerializeField] private Button shopBuyItem3;
    [SerializeField] private Button shopBuyItem4;
    [SerializeField] private Button shopBuyItem5;
    [SerializeField] private Button shopBuyItem6;


    [Header("Battle Buttons")]
    [SerializeField] private Button rewardButton1;
    [SerializeField] private Button rewardButton2;
    [SerializeField] private Button rewardButton3;



    [Header("Items")]
    [SerializeField] private Transform stampParent;
    [SerializeField] private GameObject stampPrefab;



    public void OnHomeToWorkButton() => StateManager.ChangeState(GameState.Work);
    public void OnHomeToShopButton() => StateManager.ChangeState(GameState.Shop);
    public void OnHomeToFeedButton() => StateManager.ChangeState(GameState.Feed);
    public void OnHomeToBattleButton() => StateManager.ChangeState(GameState.DayBattle);
    public void OnShopToHomeButton() => StateManager.ChangeState(GameState.Home);
    public void BuyItem1() => Inventory.Instance.BuyItem(0);
    public void BuyItem2() => Inventory.Instance.BuyItem(1);
    public void BuyItem3() => Inventory.Instance.BuyItem(2);
    public void BuyItem4() => Inventory.Instance.BuyItem(3);
    public void BuyItem5() => Inventory.Instance.BuyItem(4);
    public void BuyItem6() => Inventory.Instance.BuyItem(5);
    public void GetReward1() => Inventory.Instance.GetReward(0);
    public void GetReward2() => Inventory.Instance.GetReward(1);
    public void GetReward3() => Inventory.Instance.GetReward(2);

    void Start()
    {
        homeToWorkButton.onClick.AddListener(OnHomeToWorkButton);
        homeToShopButton.onClick.AddListener(OnHomeToShopButton);
        homeToFeedButton.onClick.AddListener(OnHomeToFeedButton);
        homeToBattleButton.onClick.AddListener(OnHomeToBattleButton);
        shopToHomeButton.onClick.AddListener(OnShopToHomeButton);
        shopBuyItem1.onClick.AddListener(BuyItem1);
        shopBuyItem2.onClick.AddListener(BuyItem2);
        shopBuyItem3.onClick.AddListener(BuyItem3);
        shopBuyItem4.onClick.AddListener(BuyItem4);
        shopBuyItem5.onClick.AddListener(BuyItem5);
        shopBuyItem6.onClick.AddListener(BuyItem6);
        rewardButton1.onClick.AddListener(GetReward1);
        rewardButton2.onClick.AddListener(GetReward2);
        rewardButton3.onClick.AddListener(GetReward3);
    }
    public void ShowState(GameState state)
    {
        homeUI.SetActive(state == GameState.Home);
        shopUI.SetActive(state == GameState.Shop);
        workUI.SetActive(state == GameState.Work);
        battleUI.SetActive(state == GameState.DayBattle || state == GameState.NightBattle);
        feedUI.SetActive(state == GameState.Feed);
    }

    public void UpdateShopLists(List<Item> items, List<Item> stamps)
    {
        shopBuyItem1.GetComponentInChildren<TextMeshProUGUI>().text = items[0].itemName;
        shopBuyItem2.GetComponentInChildren<TextMeshProUGUI>().text = items[1].itemName;
        shopBuyItem3.GetComponentInChildren<TextMeshProUGUI>().text = items[2].itemName;
        shopBuyItem4.GetComponentInChildren<TextMeshProUGUI>().text = stamps[0].itemName;
        shopBuyItem5.GetComponentInChildren<TextMeshProUGUI>().text = stamps[1].itemName;
        shopBuyItem6.GetComponentInChildren<TextMeshProUGUI>().text = stamps[2].itemName;
    }

    public void UpdateNightRewardLists(List<Item> stamps)
    {
        rewardButton1.GetComponentInChildren<TextMeshProUGUI>().text = stamps[0].itemName;
        rewardButton2.GetComponentInChildren<TextMeshProUGUI>().text = stamps[1].itemName;
        rewardButton3.GetComponentInChildren<TextMeshProUGUI>().text = stamps[2].itemName;
    }


    public void SetActiveReward(bool active)
    {
        rewardButton1.gameObject.SetActive(active);
        rewardButton2.gameObject.SetActive(active);
        rewardButton3.gameObject.SetActive(active);
    }


    public void UpdateMoney(int amount)
    {
        moneyText.text = amount.ToString();
    }

    public void UpdateDay(int day)
    {
        dayText.text = day.ToString();
    }

    public void UpdatePower(int power)
    {
        powerText.text = power.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateTime(int time)
    {
        string timeText = "아침";
        timeText = time switch
        {
            0 => "아침",
            1 => "점심",
            2 => "저녁",
            3 => "밤",
            _ => "???",
        };

        TimeText.text = timeText;
    }

    public void AddStamp(string stampName)
    {
        GameObject stampObject = Instantiate(stampPrefab, stampParent);
        stampObject.GetComponent<TextMeshProUGUI>().text = stampName;
    }

}
