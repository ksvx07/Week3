using UnityEngine;

public class NightBattleState : BaseState
{
    public NightBattleState() : base(GameState.NightBattle) { }


    public override void EnterState()
    {
        BattleManager.Instance.StartNightBattle();
        UIManager.Instance.SetActiveHomeOuro(false);
    }

    // public override void ExitState()
    // {
    // }
}
