using UnityEngine;

public class HomeState : BaseState
{
    public HomeState() : base(GameState.Home)
    {
    }

    public override void EnterState()
    {
        UIManager.Instance.SetActiveHomeOuro(true);
        GameManager.Instance.NextTime();
    }
}