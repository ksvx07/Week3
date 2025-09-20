using System.Threading.Tasks;

public class WorkState : BaseState
{
    public WorkState() : base(GameState.Work) { }


    public override async void EnterState()
    {
        GameManager.Instance.SetMoney(GameManager.Instance.Money + 10000); // 돈 100 증가

        await Task.Delay(100); // 1초 대기
        StateManager.ChangeState(GameState.Home); // 집으로 돌아가기
    }

    // public override void ExitState()
    // {
    // }
}
