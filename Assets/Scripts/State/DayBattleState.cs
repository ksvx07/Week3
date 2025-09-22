using UnityEngine;

public class DayBattleState : BaseState
{
    public DayBattleState() : base(GameState.DayBattle) { }

    public override void EnterState()
    {
        BattleManager.Instance.StartDayBattle();
        UIManager.Instance.SetActiveHomeOuro(false);
        UIManager.Instance.SetActiveItemListPanel(false);
    }

    // public override void ExitState()
    // {
    // }
}
