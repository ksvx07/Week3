using UnityEngine;

public class HomeState : BaseState
{
    public HomeState() : base(GameState.Home)
    {
    }

    public override void EnterState()
    {
        UIManager.Instance.SetUIWhenDay();
        UIManager.Instance.SetActiveHomeOuro(true);
        UIManager.Instance.SetActiveItemListPanel(true);
        GameManager.Instance.NextTime();
        UIManager.Instance.SetNextBossInfo(UnitList.bosses[GameManager.Instance.Day - 1].AttackPower, UnitList.bosses[GameManager.Instance.Day - 1].MaxHP);
        UIManager.Instance.SetDayBattleTooltip(UnitList.enemies[GameManager.Instance.Day - 1].AttackPower, UnitList.bosses[GameManager.Instance.Day - 1].MaxHP);
    }
}