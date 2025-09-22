using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject homeOuro;

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
    [SerializeField] private Button homeToNightBattleButton;


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
    [SerializeField] private Button speedUpButton5;
    [SerializeField] private Button speedUpButton50;
    [SerializeField] private TooltipTrigger battleTooltip;




    [Header("Items")]
    [SerializeField] private Transform stampParent;
    [SerializeField] private GameObject stampPrefab;
    [SerializeField] private Tooltip StampListTooltip;
    [SerializeField] private TextMeshProUGUI cheapFoodNumText;
    [SerializeField] private TextMeshProUGUI expensiveFoodNumText;
    [SerializeField] private TextMeshProUGUI waterNumText;
    [SerializeField] private GameObject itemListPanel;



    [Header("Feed Buttons")]
    [SerializeField] private Button feedToHomeButton;
    [SerializeField] private Button feedButton1;
    [SerializeField] private Button feedButton2;
    [SerializeField] private Button feedButton3;
    // [SerializeField] private Button feedButton4;
    // [SerializeField] private Button feedButton5;
    // [SerializeField] private Button feedButton6;

    [Header("Heart")]
    [SerializeField] private TextMeshProUGUI heartText;
    [SerializeField] private GameObject heartObj;

    [Header("Next Boss")]
    [SerializeField] private GameObject nextBossPanel;
    [SerializeField] private TextMeshProUGUI nextBossPowerText;
    [SerializeField] private TextMeshProUGUI nextBossHealthText;

    [Header("Title")]

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject title2;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button titleButton2;
    [SerializeField] private GameObject ending;
    [SerializeField] private GameObject badEnding;
    [SerializeField] private GameObject me;

    public void Ending()
    {
        ending.SetActive(true);
        if (GameManager.Instance.Heart >= 10)
            me.SetActive(true);
        else
            me.SetActive(false);
        ending.GetComponent<Animator>().SetTrigger("Ending");
    }

    public void BadEnding()
    {
        badEnding.SetActive(true);
    }


    public void OnHomeToWorkButton() => StateManager.ChangeState(GameState.Work);
    public void OnHomeToShopButton() => StateManager.ChangeState(GameState.Shop);
    public void OnHomeToBattleButton() => StateManager.ChangeState(GameState.DayBattle);
    public void OnToHomeButton() => StateManager.ChangeState(GameState.Home);
    private void OnHomeToNightBattleButton() => StateManager.ChangeState(GameState.NightBattle);
    public void BuyItem1() => Inventory.Instance.BuyItem(0);
    public void BuyItem2() => Inventory.Instance.BuyItem(1);
    public void BuyItem3() => Inventory.Instance.BuyItem(2);
    public void BuyItem4() => Inventory.Instance.BuyItem(3);
    public void BuyItem5() => Inventory.Instance.BuyItem(4);
    public void BuyItem6() => Inventory.Instance.BuyItem(5);
    public void GetReward1() => Inventory.Instance.GetReward(0);
    public void GetReward2() => Inventory.Instance.GetReward(1);
    public void GetReward3() => Inventory.Instance.GetReward(2);
    public void FeedButton1() => Inventory.Instance.FeedButton(Inventory.Instance.cheapFood);
    public void FeedButton2() => Inventory.Instance.FeedButton(Inventory.Instance.expensiveFood);
    public void FeedButton3() => Inventory.Instance.FeedButton(Inventory.Instance.water);
    // public void FeedButton4() => Inventory.Instance.FeedButton(Inventory.Instance.cheapFood);
    // public void FeedButton5() => Inventory.Instance.FeedButton(Inventory.Instance.cheapFood);
    // public void FeedButton6() => Inventory.Instance.FeedButton(Inventory.Instance.cheapFood);
    public void SpeedUpButton5() => BattleManager.Instance.SetBattleSpeed(0.1f);
    public void SpeedUpButton50() => BattleManager.Instance.SetBattleSpeed(0.01f);

    private string doublePowerAfterAttackStampTooltip = "공격할 때마다 공격력이\n영구적으로 2배 증가한다";
    private string moreLifeStampTooltip = "체력이 0이 되었을때 한 번\n최대 체력으로 부활한다";
    private string increaseMaxHealthAfterHitStampTooltip = "공격을 받을 때마다 최대 체력이\n영구적으로 20 증가한다";
    private string fiveTimesPowerTempAfterHitAndSurviveTooltip = "공격을 받고 생존할 때마다\n공격력이 5배 증가한다";
    private string duplicateAfterAttackStampTooltip = "공격할 때마다 동일한 분신을 소환한다";
    private string doubleAttackStampTooltip = "매 턴 두번 공격한다";



    public void SetCheapFoodNumUI(int num) => cheapFoodNumText.text = num.ToString();
    public void SetExpensiveFoodNumUI(int num) => expensiveFoodNumText.text = num.ToString();
    public void SetWaterNumUI(int num) => waterNumText.text = num.ToString();

    private void OnTitle1() => Destroy(title);
    private void OnTitle2() => Destroy(title2);
    protected override void Awake()
    {
        base.Awake();

        stampTooltipText.Add(Inventory.Instance.doublePowerAfterAttackStamp, doublePowerAfterAttackStampTooltip);
        stampTooltipText.Add(Inventory.Instance.moreLifeStamp, moreLifeStampTooltip);
        stampTooltipText.Add(Inventory.Instance.increaseMaxHealthAfterHitStamp, increaseMaxHealthAfterHitStampTooltip);
        stampTooltipText.Add(Inventory.Instance.fiveTimesPowerTempAfterHitAndSurvive, fiveTimesPowerTempAfterHitAndSurviveTooltip);
        stampTooltipText.Add(Inventory.Instance.duplicateAfterAttackStamp, duplicateAfterAttackStampTooltip);
        stampTooltipText.Add(Inventory.Instance.doubleAttackStamp, doubleAttackStampTooltip);
    }

    void Start()
    {
        homeToWorkButton.onClick.AddListener(OnHomeToWorkButton);
        homeToShopButton.onClick.AddListener(OnHomeToShopButton);
        homeToFeedButton.onClick.AddListener(OpenFeedMenu);
        homeToBattleButton.onClick.AddListener(OnHomeToBattleButton);
        homeToNightBattleButton.onClick.AddListener(OnHomeToNightBattleButton);
        shopToHomeButton.onClick.AddListener(OnToHomeButton);
        shopBuyItem1.onClick.AddListener(BuyItem1);
        shopBuyItem2.onClick.AddListener(BuyItem2);
        shopBuyItem3.onClick.AddListener(BuyItem3);
        shopBuyItem4.onClick.AddListener(BuyItem4);
        shopBuyItem5.onClick.AddListener(BuyItem5);
        shopBuyItem6.onClick.AddListener(BuyItem6);
        rewardButton1.onClick.AddListener(GetReward1);
        rewardButton2.onClick.AddListener(GetReward2);
        rewardButton3.onClick.AddListener(GetReward3);
        feedButton1.onClick.AddListener(FeedButton1);
        feedButton2.onClick.AddListener(FeedButton2);
        feedButton3.onClick.AddListener(FeedButton3);
        // feedButton4.onClick.AddListener(FeedButton4);
        // feedButton5.onClick.AddListener(FeedButton5);
        // feedButton6.onClick.AddListener(FeedButton6);
        feedToHomeButton.onClick.AddListener(CloseFeedMenu);
        speedUpButton5.onClick.AddListener(SpeedUpButton5);
        speedUpButton50.onClick.AddListener(SpeedUpButton50);
        UIManager.Instance.SetDayBattleTooltip(UnitList.enemies[GameManager.Instance.Day - 1].AttackPower, UnitList.bosses[GameManager.Instance.Day - 1].MaxHP);
        titleButton.onClick.AddListener(OnTitle1);
        titleButton2.onClick.AddListener(OnTitle2);
    }

    public void SetDayBattleTooltip(int power, int health)
    {
        battleTooltip.tooltipText = $"돌아다니면서 시비건다\n왠지 {power}/{health}의 적이 나올 것 같다";
    }

    public void SetNextBossInfo(int power, int health)
    {
        nextBossPowerText.text = power.ToString();
        nextBossHealthText.text = health.ToString();
    }

    public void SetActiveItemListPanel(bool value)
    {
        itemListPanel.SetActive(value);
        nextBossPanel.SetActive(value);
    }

    public void SetUIWhenNight()
    {
        homeToNightBattleButton.gameObject.SetActive(true);
        homeToWorkButton.interactable = false;
        homeToShopButton.interactable = false;
        homeToBattleButton.interactable = false;
    }

    public void SetUIWhenDay()
    {
        homeToNightBattleButton.gameObject.SetActive(false);
        homeToWorkButton.interactable = true;
        homeToShopButton.interactable = true;
        homeToBattleButton.interactable = true;
    }


    public void OpenFeedMenu()
    {
        homeUI.SetActive(false);
        feedUI.SetActive(true);
    }

    public void CloseFeedMenu()
    {
        feedUI.SetActive(false);
        homeUI.SetActive(true);
    }

    // public void SetActiveHeart(bool value)
    // {
    //     heartObj.SetActive(value);
    // }

    public void SetHeart(int heart)
    {
        if (heart >= 10)
        {
            heartText.text = "MAX";
        }
        else
        {
            heartText.text = heart.ToString();
        }
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

        shopBuyItem4.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[0].itemName];
        shopBuyItem5.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[1].itemName];
        shopBuyItem6.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[2].itemName];
    }

    Dictionary<string, string> stampTooltipText = new();


    public void UpdateNightRewardLists(List<Item> stamps)
    {
        rewardButton1.GetComponentInChildren<TextMeshProUGUI>().text = stamps[0].itemName;
        rewardButton2.GetComponentInChildren<TextMeshProUGUI>().text = stamps[1].itemName;
        rewardButton3.GetComponentInChildren<TextMeshProUGUI>().text = stamps[2].itemName;

        rewardButton1.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[0].itemName];
        rewardButton2.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[1].itemName];
        rewardButton3.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stamps[2].itemName];
    }

    public void UpdateDayRewardLists()
    {
        rewardButton1.GetComponentInChildren<TextMeshProUGUI>().text = $"{4000 * GameManager.Instance.Day}원";
        rewardButton2.GetComponentInChildren<TextMeshProUGUI>().text = $"공격력 + {10 * GameManager.Instance.Day}";
        rewardButton3.GetComponentInChildren<TextMeshProUGUI>().text = $"체력 + {10 * GameManager.Instance.Day}";

        rewardButton1.GetComponent<TooltipTrigger>().tooltipText = "";
        rewardButton2.GetComponent<TooltipTrigger>().tooltipText = "";
        rewardButton3.GetComponent<TooltipTrigger>().tooltipText = "";
    }


    public void SetActiveReward(bool active)
    {
        rewardButton1.gameObject.SetActive(active);
        rewardButton2.gameObject.SetActive(active);
        rewardButton3.gameObject.SetActive(active);
    }

    public void SetActiveHomeOuro(bool value)
    {
        homeOuro.SetActive(value);
    }

    private float bornScale = 0.3f;
    public void UpdateHomeOuro()
    {
        float scaleMultiply = Mathf.Sqrt(GameManager.Instance.Power + GameManager.Instance.Health);
        float newScale = bornScale * scaleMultiply;
        homeOuro.transform.localScale = new(newScale, newScale);
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
        UpdateHomeOuro();
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
        UpdateHomeOuro();
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
        stampObject.GetComponent<TooltipTrigger>().tooltip = StampListTooltip;
        stampObject.GetComponent<TooltipTrigger>().tooltipText = stampTooltipText[stampName];
    }

}
