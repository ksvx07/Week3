public class GameManager : Singleton<GameManager>
{


    public GameState CurrentState { get; private set; } = GameState.Home;

    // public event System.Action<GameState> OnStateChanged; // 이벤트 정의

    public int Money { get; private set; } = 10000;
    public int Power { get; private set; } = 1;
    public int Health { get; private set; } = 1;
    public int Day { get; private set; } = 1;
    public int Time { get; private set; } = 0;
    public int Love { get; private set; } = 0;

    public string ouroboros = "Ouroboros";
    public string ouroborosClone = "OuroborosClone";

    void Start()
    {
        InitDay();
    }
    public void InitDay()
    {
        Inventory.Instance.SetShopLists();
    }

    public void SetMoney(int money)
    {
        Money = money;
        UIManager.Instance.UpdateMoney(Money);
    }
    public void SetPower(int power)
    {
        Power = power;
        UIManager.Instance.UpdatePower(Power);
    }
    public void SetHealth(int health)
    {
        Health = health;
        UIManager.Instance.UpdateHealth(Health);

    }
    public void NextDay()
    {
        Day++;
        UIManager.Instance.UpdateDay(Day);
        InitDay();
    }
    public void NextTime()
    {
        Time++;
        if (Time == 3)
        {
            StateManager.ChangeState(GameState.NightBattle);
        }
        else if (Time > 3)
        {
            Time = 0;
            NextDay();
        }
        UIManager.Instance.UpdateTime(Time);
    }

    public void GetLove()
    {
        Love++;
    }
}
