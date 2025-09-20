using UnityEngine;

public class HomeState : BaseState
{
    public HomeState() : base(GameState.Home)
    {
    }

    public override void EnterState()
    {
        GameManager.Instance.NextTime();
        // Debug.Log("시간지남");
    }
}