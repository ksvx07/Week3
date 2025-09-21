using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private float battleDelay = 0.5f;

    private List<Unit> playerTeam;
    private List<Unit> enemyTeam;
    private List<Unit> addedPlayerTeam;
    private List<Unit> addedEnemyTeam;

    public bool dayTime;
    public int attackCount = 1;
    private BattleUIManager battleUIManager;


    void Start()
    {
        battleUIManager = GetComponent<BattleUIManager>();
        addedPlayerTeam = new();
        addedEnemyTeam = new();
    }

    public void SetBattleSpeed(float speed)
    {
        battleDelay = speed;
    }
    public void StartBattle()
    {
        battleDelay = 0.5f;
        int life = 0;
        attackCount = 1;
        foreach (Item stamp in Inventory.Instance.myStamps)
        {
            if (stamp.itemName == Inventory.Instance.moreLifeStamp)
                life++;
            if (stamp.itemName == Inventory.Instance.doubleAttackStamp)
                attackCount++;
        }

        playerTeam = new()
        {
            new(GameManager.Instance.ouroboros, GameManager.Instance.Power, GameManager.Instance.Health, life)
        };
        battleUIManager.InitBattle(playerTeam[0], enemyTeam[0]);
        battleUIManager.SpawnPlayer();
        battleUIManager.SpawnEnemy();

        StartCoroutine(BattleLoop());
    }

    public void StartDayBattle()
    {
        dayTime = true;
        enemyTeam = new()
        {
            UnitList.enemies[GameManager.Instance.Day - 1].Clone()
        };
        StartBattle();
    }

    public void StartNightBattle()
    {
        dayTime = false;
        enemyTeam = new()
        {
            UnitList.bosses[GameManager.Instance.Day - 1].Clone()
        };
        StartBattle();
    }

    public void EndBattle()
    {
        if (playerTeam.Count > 0)
        {
            if (dayTime)
            {
                // 낮에 이겼을 시
                UIManager.Instance.SetActiveReward(true);
            }
            else
            {
                // 밤에 이겼을 시
                Inventory.Instance.UpdateNightReward();
                UIManager.Instance.SetActiveReward(true);
            }


            // GameManager.Instance.GetLove();
        }
        else
        {
            GoToHome(); // 집으로 돌아가기
        }

    }

    public void GoToHome()
    {
        battleUIManager.EndBattle();
        StateManager.ChangeState(GameState.Home);
    }

    private IEnumerator BattleLoop()
    {
        Debug.Log("전투 시작!");

        int bossAttackCount = 0;

        while (playerTeam.Count > 0 && enemyTeam.Count > 0)
        {

            // 적군 턴
            foreach (var enemy in enemyTeam)
            {

                if (!enemy.IsDead() && playerTeam.Count > 0)
                {

                    if (dayTime || bossAttackCount < 2)
                    {
                        int targetNum = Random.Range(0, playerTeam.Count);
                        Unit target = playerTeam[targetNum];
                        Debug.Log($"{enemy.name} attacks {target.name}!");
                        enemy.Attack(target);
                        battleUIManager.AttackEnemyAction(enemy.AttackPower, targetNum);
                        ApplyHitStamps(target);
                        if (target.IsDead())
                        {
                            GameManager.Instance.SetPower(GameManager.Instance.Power + 1);
                            GameManager.Instance.SetHealth(GameManager.Instance.Health + 1);
                        }
                        target.Revive();
                        if (target.IsDead())
                        {
                            playerTeam.Remove(target);
                            battleUIManager.DeletePlayerUI(targetNum);
                        }
                    }
                    else
                    {
                        List<int> deadList = new();
                        int i = 0;
                        enemy.AttackAll(playerTeam);
                        battleUIManager.AttackAllEnemyAction(enemy.AttackPower);
                        foreach (Unit tar in playerTeam)
                        {
                            ApplyHitStamps(tar);
                            if (tar.IsDead())
                            {
                                GameManager.Instance.SetPower(GameManager.Instance.Power + 1);
                                GameManager.Instance.SetHealth(GameManager.Instance.Health + 1);
                            }
                            tar.Revive();
                            if (tar.IsDead())
                                deadList.Add(i);
                            i++;
                        }
                        Debug.Log(playerTeam.Count);
                        for (int k = playerTeam.Count - 1; k >= 0; k--)
                        {
                            if (playerTeam[k].IsDead())
                            {
                                playerTeam.RemoveAt(k);
                                battleUIManager.DeletePlayerUI(k);
                            }
                        }
                        Debug.Log(playerTeam.Count);
                    }

                    yield return new WaitForSeconds(battleDelay);

                }
            }

            if (!dayTime)
            {
                bossAttackCount++;
                if (bossAttackCount > 2)
                {
                    bossAttackCount = 0;
                }

            }

            enemyTeam.AddRange(addedEnemyTeam);
            addedEnemyTeam = new();
            battleUIManager.EndEnemyTurn();

            // 아군 턴 (모든 살아있는 유닛이 공격)
            foreach (var player in playerTeam)
            {
                for (int i = 0; i < attackCount; i++)
                {
                    if (!player.IsDead() && enemyTeam.Count > 0)
                    {
                        int targetNum = Random.Range(0, enemyTeam.Count);
                        var target = enemyTeam[targetNum];
                        player.Attack(target);
                        battleUIManager.AttackPlayerAction(player.AttackPower, targetNum);
                        ApplyAttackStamps(player, target);
                        target.Revive();
                        if (target.IsDead())
                            enemyTeam.Remove(target);
                        yield return new WaitForSeconds(battleDelay); // 턴 텀
                    }
                }

            }
            yield return new WaitForSeconds(battleDelay);
            battleUIManager.EndPlayerTurn();
            playerTeam.AddRange(addedPlayerTeam);
            addedPlayerTeam = new();
        }

        EndBattle();
        Debug.Log(playerTeam.Count > 0 ? "플레이어 승리!" : "적 승리!");
    }

    // Stamp 능력들
    private void ApplyAttackStamps(Unit player, Unit target)
    {
        foreach (Item stamp in Inventory.Instance.myStamps)
        {
            if (stamp.itemName == Inventory.Instance.doublePowerAfterAttackStamp)
                ApplyDoublePowerAfterAttackStamp(player);
            if (stamp.itemName == Inventory.Instance.duplicateAfterAttackStamp)
                ApplyDuplicateAfterAttackStamp(player);
        }
    }

    private void ApplyDoublePowerAfterAttackStamp(Unit player)
    {
        player.AttackPower *= 2;
        if (player.name == GameManager.Instance.ouroboros)
            GameManager.Instance.SetPower(GameManager.Instance.Power * 2);
    }

    private void ApplyDuplicateAfterAttackStamp(Unit player)
    {
        Unit clone = player.Clone();
        clone.name = GameManager.Instance.ouroborosClone;
        addedPlayerTeam.Add(clone);
        battleUIManager.SpawnPlayer();
    }

    private void ApplyHitStamps(Unit player)
    {
        foreach (Item stamp in Inventory.Instance.myStamps)
        {
            if (stamp.itemName == Inventory.Instance.increaseMaxHealthAfterHitStamp)
                ApplyIncreaseMaxHealthAfterHitStamp(player);
            if (stamp.itemName == Inventory.Instance.fiveTimesPowerTempAfterHitAndSurvive)
                ApplyFiveTimesPowerTempAfterHitAndSurvive(player);
        }
    }

    private void ApplyIncreaseMaxHealthAfterHitStamp(Unit player)
    {
        if (player.name == GameManager.Instance.ouroboros)
            GameManager.Instance.SetHealth(GameManager.Instance.Health + 10);
    }

    private void ApplyFiveTimesPowerTempAfterHitAndSurvive(Unit player)
    {
        if (!player.IsDead())
            player.AttackPower *= 5;
    }

}