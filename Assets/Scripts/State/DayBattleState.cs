using UnityEngine;

public class DayBattleState : BaseState
{
    public DayBattleState() : base(GameState.DayBattle) { }

    public override void EnterState()
    {
        BattleManager.Instance.StartDayBattle();
    }

    // public override void ExitState()
    // {
    // }
}
