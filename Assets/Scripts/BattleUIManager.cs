using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private Vector2 playerRect = new(1, 1);
    [SerializeField] private Vector2 playerSize = new(1, 1);
    [SerializeField] private Vector2 enemyRect = new(1, 1);
    [SerializeField] private Vector2 enemySize = new(1, 1);
    [SerializeField] private float topMargin = 0.6f;
    [SerializeField] private float rightMargin = 0.6f;

    [SerializeField] private Vector2 randPosRange = new(0.1f, 0.1f);

    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;


    public List<Vector2Int> playerGridList;
    public List<Vector2Int> enemyGridList;

    private Vector2Int playerGridNum = new(1, 1);
    private Vector2Int enemyGridNum = new(1, 1);
    private int enemyNum = 0;

    private Vector2 playerScale = new(1f, 1f);

    private DamagePopupSpawner damagePopupSpawner;

    void Start()
    {
        damagePopupSpawner = GetComponent<DamagePopupSpawner>();
    }

    public void EndBattle()
    {
        foreach (Transform child in playerParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void InitBattle(Unit player, Unit enemy)
    {
        playerGridList = new();
        enemyGridList = new();
        SetPlayerScale(player, enemy);
        InitRectNum();
        attackCount = 0;
    }

    private void SetPlayerScale(Unit player, Unit enemy)
    {
        float playerValue = player.AttackPower + player.MaxHP;
        float enemyValue = enemy.AttackPower + enemy.MaxHP;

        playerScale = enemyPrefab.transform.localScale * Mathf.Sqrt(playerValue / enemyValue);
    }

    private void InitRectNum()
    {
        enemyNum = 0;
        playerGridNum.x = (int)(playerRect.x / (playerSize.x * playerScale.x * rightMargin));
        playerGridNum.y = (int)(playerRect.y / (playerSize.y * playerScale.y * topMargin));
        enemyGridNum.x = (int)(enemyRect.x / (enemySize.x * rightMargin));
        enemyGridNum.y = (int)(enemyRect.y / (enemySize.y * topMargin));
    }

    public async void DeletePlayerUI(int num)
    {
        playerGridList.RemoveAt(num);
        await Task.Delay(700);
        Destroy(playerParent.GetChild(num).gameObject);
    }

    private Vector2 SpawnPlayerPosition()
    {
        for (int y = 0; y < playerGridNum.y; y++)
        {
            for (int x = 0; x < playerGridNum.x; x++)
            {
                var pos = new Vector2Int(x, y);
                if (!playerGridList.Contains(pos))
                {
                    playerGridList.Add(pos);
                    return new Vector2(pos.x * playerSize.x * playerScale.x * topMargin, pos.y * playerSize.y * playerScale.y * rightMargin);
                }
            }
        }

        int z = 0;
        while (true)
        {
            var pos = new Vector2Int(z, playerGridNum.y);
            if (!playerGridList.Contains(pos))
            {
                playerGridList.Add(pos);
                return new Vector2(pos.x * playerSize.x * playerScale.x * topMargin, pos.y * playerSize.y * playerScale.y * rightMargin);
            }
            z++;
        }

    }
    private Vector2 SpawnEnemyPosition()
    {
        Vector2Int enemyGrid = Vector2Int.zero;
        int i = enemyNum;
        while (i >= enemyGridNum.x)
        {
            if (enemyGrid.y > enemyGridNum.y)
                break;
            i -= enemyGridNum.x;
            enemyGrid.y++;
        }
        enemyGrid.x = i;
        enemyNum++;
        enemyGridList.Add(enemyGrid);
        return new Vector2(enemyGrid.x * enemySize.x * topMargin, enemyGrid.y * enemySize.y * rightMargin);
    }

    public void SpawnPlayer()
    {
        Vector2 parentPos = new(playerParent.position.x, playerParent.position.y);
        Vector2 randPos = new(Random.Range(-randPosRange.x, randPosRange.x) * playerScale.x, Random.Range(-randPosRange.y, randPosRange.y) * playerScale.y);
        GameObject obj = Instantiate(playerPrefab, SpawnPlayerPosition() + parentPos + randPos, Quaternion.identity, playerParent);
        obj.transform.localScale = playerScale;
    }

    public void SpawnEnemy()
    {
        Vector2 parentPos = new(enemyParent.position.x, enemyParent.position.y);
        Vector2 randPos = new(Random.Range(-randPosRange.x, randPosRange.x), Random.Range(-randPosRange.y, randPosRange.y));
        Instantiate(enemyPrefab, -SpawnEnemyPosition() + parentPos + randPos, Quaternion.identity, enemyParent);
    }

    private int playerAttackNum = 0;

    private int attackCount = 0;
    public void AttackPlayerAction(int damage, int targetNum)
    {
        playerParent.GetChild(playerAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, enemyParent.GetChild(targetNum).transform.position);
        attackCount++;
        if (attackCount >= BattleManager.Instance.attackCount)
        {
            attackCount = 0;
            playerAttackNum++;
        }
    }

    public void EndPlayerTurn()
    {
        playerAttackNum = 0;
    }

    private int enemyAttackNum = 0;
    public void AttackEnemyAction(int damage, int targetNum)
    {
        enemyParent.GetChild(enemyAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, playerParent.GetChild(targetNum).transform.position);
        enemyAttackNum++;
    }

    public void AttackAllEnemyAction(int damage)
    {
        enemyParent.GetChild(enemyAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, playerParent.transform.position);
        enemyAttackNum++;
    }

    public void EndEnemyTurn()
    {
        enemyAttackNum = 0;
    }
}
