using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private static BaseState currentState;
    private BaseState home;
    private BaseState work;
    private BaseState shop;
    private BaseState feed;
    private BaseState dayBattle;
    private BaseState nightBattle;

    private static Dictionary<GameState, BaseState> stateDictionary;

    void Start()
    {
        home = new HomeState();
        work = new WorkState();
        shop = new ShopState();
        feed = new FeedState();
        dayBattle = new DayBattleState();
        nightBattle = new NightBattleState();

        stateDictionary = new()
        {
            { GameState.Home, home },
            { GameState.Work, work },
            { GameState.Feed, feed },
            { GameState.Shop, shop },
            { GameState.DayBattle, dayBattle },
            { GameState.NightBattle, nightBattle },
        };
        currentState = home;
    }

    // void Update()
    // {
    //     if (currentState != null) currentState.UpdateState();
    // }

    public static void ChangeState(GameState newState)
    {
        if (currentState.myState == newState) return;
        currentState.ExitState();
        currentState = stateDictionary[newState];
        UIManager.Instance.ShowState(newState);
        currentState.EnterState();

    }
}
