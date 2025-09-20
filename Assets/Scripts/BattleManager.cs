using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private float battleDelay = 0.1f;

    public List<Unit> playerTeam;
    private List<Unit> enemyTeam;
    public List<Unit> addedPlayerTeam;

    public bool dayTime;

    public void StartBattle()
    {
        int life = 0;
        foreach (Item stamp in Inventory.Instance.myStamps)
        {
            if (stamp.itemName == Inventory.Instance.moreLifeStamp)
                life++;
        }

        playerTeam = new()
        {
            new(GameManager.Instance.ouroboros, GameManager.Instance.Power, GameManager.Instance.Health, life)
        };

        StartCoroutine(BattleLoop());
    }

    public void StartDayBattle()
    {
        dayTime = true;
        enemyTeam = new(UnitList.enemies);
        StartBattle();
    }

    public void StartNightBattle()
    {
        dayTime = false;
        enemyTeam = new(UnitList.bosses);
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
            StateManager.ChangeState(GameState.Home); // 집으로 돌아가기
        }

    }

    private IEnumerator BattleLoop()
    {
        while (playerTeam.Count > 0 && enemyTeam.Count > 0)
        {
            Debug.Log("전투 시작!");

            // 적군 턴
            foreach (var enemy in enemyTeam)
            {
                if (!enemy.IsDead())
                {
                    var target = playerTeam[Random.Range(0, playerTeam.Count)];
                    Debug.Log($"{enemy.name} attacks {target.name}!");
                    enemy.Attack(target);
                    ApplyHitStamps(target, enemy);
                    if (target.IsDead())
                    {
                        GameManager.Instance.SetPower(GameManager.Instance.Power + 1);
                        GameManager.Instance.SetHealth(GameManager.Instance.Health + 1);
                    }
                    target.Revive();
                    if (target.IsDead())
                        playerTeam.Remove(target);

                }
                yield return new WaitForSeconds(battleDelay);
            }

            // 아군 턴 (모든 살아있는 유닛이 공격)
            foreach (var player in playerTeam)
            {
                if (!player.IsDead())
                {
                    var target = enemyTeam[Random.Range(0, enemyTeam.Count)];
                    player.Attack(target);
                    ApplyAttackStamps(player, target);
                    target.Revive();
                    if (target.IsDead())
                        enemyTeam.Remove(target);
                }
                yield return new WaitForSeconds(battleDelay); // 턴 텀
            }
            playerTeam.AddRange(addedPlayerTeam);
        }

        EndBattle();
        Debug.Log(playerTeam.Count > 0 ? "플레이어 승리!" : "적 승리!");
    }

    private void ApplyAttackStamps(Unit player, Unit target)
    {
        foreach (Item stamp in Inventory.Instance.myStamps)
        {
            if (stamp.itemName == Inventory.Instance.doublePowerAfterAttackStamp)
                ApplyDoublePowerAfterAttackStamp(player);
            if (stamp.itemName == Inventory.Instance.duplicateAfterAttackStamp)
                ApplyDuplicateAfterAttackStamp(player);
            if (stamp.itemName == Inventory.Instance.doubleAttackStamp)
                ApplyDoubleAttackStamp(player, target);
        }
    }

    private void ApplyDoubleAttackStamp(Unit player, Unit target)
    {
        target.Revive();
        if (target.IsDead())
            enemyTeam.Remove(target);
        var newTarget = enemyTeam[Random.Range(0, enemyTeam.Count)];
        player.Attack(newTarget);
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
    }

    private void ApplyHitStamps(Unit player, Unit enemy)
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