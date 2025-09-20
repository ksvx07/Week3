using UnityEngine;

public abstract class BaseState
{
    /* [SerializeField]*/
    public GameState myState
    {
        get; private set;
    }

    public BaseState(GameState state)
    {
        myState = state;
    }

    /// <summary>
    /// 상태에 들어왔을 때 실행할 로직
    /// </summary>
    public virtual void EnterState()
    {
        // UIManager.Instance.ShowState(myState);
    }

    public virtual void UpdateState()
    {
        // 매 프레임마다 실행할 로직 (필요시 오버라이드)
    }

    /// <summary>
    /// 상태에서 나갈 때 실행할 로직
    /// </summary>
    public virtual void ExitState()
    {
        // UIManager.Instance.ShowState(myState);
    }
}
