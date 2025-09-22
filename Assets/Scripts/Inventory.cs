using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : Singleton<Inventory>
{
    public List<Item> items = new();
    public List<Item> stampsList = new();
    public List<Item> myStamps = new();
    public string[] shopLists = new string[6];
    public List<Item> nightReward = new();

    public string cheapFood = "삼각김밥";
    public string expensiveFood = "스테이크";
    public string water = "콜라";


    public string doublePowerAfterAttackStamp = "분노";
    public string moreLifeStamp = "불사";
    public string increaseMaxHealthAfterHitStamp = "철갑";
    public string fiveTimesPowerTempAfterHitAndSurvive = "역전";
    public string duplicateAfterAttackStamp = "분신";
    public string doubleAttackStamp = "신속";

    public int stampPrice = 30000;

    protected override void Awake()
    {
        base.Awake(); // 부모 Awake()도 실행

        items.Add(new(cheapFood, 2000, 1));
        items.Add(new(expensiveFood, 20000, 0));
        items.Add(new(water, 1000, 1));


        stampsList.Add(new(doublePowerAfterAttackStamp, stampPrice, 0));
        stampsList.Add(new(moreLifeStamp, stampPrice, 0));
        stampsList.Add(new(increaseMaxHealthAfterHitStamp, stampPrice, 0));
        stampsList.Add(new(fiveTimesPowerTempAfterHitAndSurvive, stampPrice, 0));
        stampsList.Add(new(duplicateAfterAttackStamp, stampPrice, 0));
        stampsList.Add(new(doubleAttackStamp, stampPrice, 0));

        // myStamps.Add(new(moreLifeStamp, stampPrice, 0));
        // myStamps.Add(new(moreLifeStamp, stampPrice, 0));
        // myStamps.Add(new(moreLifeStamp, stampPrice, 0));
        // myStamps.Add(new(duplicateAfterAttackStamp, stampPrice, 0));
        // myStamps.Add(new(doubleAttackStamp, stampPrice, 0));
    }


    public void BuyItem(int itemNum)
    {
        foreach (Item item in items)
        {
            if (item.itemName == shopLists[itemNum])
            {
                if (GameManager.Instance.Money >= item.price)
                {
                    GameManager.Instance.SetMoney(GameManager.Instance.Money - item.price);
                    item.quantity++;
                    Debug.Log($"구매 완료! 현재 수량 : {item.quantity}");

                    if (item.itemName == cheapFood)
                        UIManager.Instance.SetCheapFoodNumUI(item.quantity);
                    else if (item.itemName == expensiveFood)
                        UIManager.Instance.SetExpensiveFoodNumUI(item.quantity);
                    else if (item.itemName == water)
                        UIManager.Instance.SetWaterNumUI(item.quantity);
                }
            }
        }

        foreach (Item stamp in stampsList)
        {
            if (stamp.itemName == shopLists[itemNum])
            {
                if (GameManager.Instance.Money >= stamp.price)
                {
                    GameManager.Instance.SetMoney(GameManager.Instance.Money - stamp.price);
                    AddMyStamp(stamp);
                }
            }
        }
    }

    public void FeedButton(string foodName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == foodName)
            {
                if (item.quantity > 0)
                {
                    item.quantity--;
                    if (foodName == cheapFood)
                    {
                        GameManager.Instance.SetHealth(GameManager.Instance.Health + 2);
                        GameManager.Instance.SetHeart(GameManager.Instance.Heart + 1);
                        UIManager.Instance.SetCheapFoodNumUI(item.quantity);
                    }
                    else if (foodName == expensiveFood)
                    {
                        GameManager.Instance.SetHealth(GameManager.Instance.Health * 2);
                        GameManager.Instance.SetHeart(GameManager.Instance.Heart + 3);
                        UIManager.Instance.SetExpensiveFoodNumUI(item.quantity);
                    }
                    else if (foodName == water)
                    {
                        GameManager.Instance.SetPower(GameManager.Instance.Power + 1);
                        UIManager.Instance.SetWaterNumUI(item.quantity);
                    }
                }
                else
                {
                    Debug.Log($"{foodName} 없어...");
                }
            }
        }
    }

    private void AddMyStamp(Item thisStamp)
    {
        myStamps.Add(thisStamp);
        UIManager.Instance.AddStamp(thisStamp.itemName);
    }


    public void UpdateNightReward()
    {
        nightReward = stampsList.Select(i => new Item(i.itemName, i.price, i.quantity)).ToList();
        nightReward = nightReward.OrderBy(x => Random.value).Take(3).ToList();
        UIManager.Instance.UpdateNightRewardLists(nightReward);
    }

    public void UpdateDayReward()
    {
        UIManager.Instance.UpdateDayRewardLists();
    }

    public void GetReward(int index)
    {
        if (BattleManager.Instance.dayTime)
        {
            if (index == 0)
                GameManager.Instance.SetMoney(GameManager.Instance.Money + 4000 * GameManager.Instance.Day);
            else if (index == 1)
                GameManager.Instance.SetPower(GameManager.Instance.Power + 10 * GameManager.Instance.Day);
            else if (index == 2)
                GameManager.Instance.SetHealth(GameManager.Instance.Health + 10 * GameManager.Instance.Day);

        }
        else
        {
            AddMyStamp(nightReward[index]);
        }

        UIManager.Instance.SetActiveReward(false);

        BattleManager.Instance.GoToHome();
    }


    public void SetShopLists()
    {
        List<Item> selectedItems = GetRandomItems(items, 3);
        List<Item> selectedStamps = GetRandomItems(stampsList, 3);
        int i = 0;
        foreach (Item selectedItem in selectedItems)
        {
            shopLists[i] = selectedItem.itemName;
            i++;
        }
        foreach (Item selectedStamp in selectedStamps)
        {
            shopLists[i] = selectedStamp.itemName;
            i++;
        }
        UIManager.Instance.UpdateShopLists(selectedItems, selectedStamps);
    }

    public List<Item> GetRandomItems(List<Item> items, int count)
    {
        List<Item> temp = new List<Item>(items); // 원본 안 건드리고 복사
        return temp.Take(count).ToList();
    }
}
