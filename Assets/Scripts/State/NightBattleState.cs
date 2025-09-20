using UnityEngine;

public class NightBattleState : BaseState
{
    public NightBattleState() : base(GameState.NightBattle) { }


    public override void EnterState()
    {
        BattleManager.Instance.StartNightBattle();
    }

    public override void ExitState()
    {
        // GameManager.Instance.NextTime();
    }
}
