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
    [SerializeField] private GameObject earthPrefab;

    [SerializeField] private Transform playerHealthBarRoot;
    [SerializeField] private Transform enemyHealthBarRoot;
    [SerializeField] private GameObject bossAOE;




    public List<Vector2Int> playerGridList;
    public List<Vector2Int> enemyGridList;

    private Vector2Int playerGridNum = new(1, 1);
    private Vector2Int enemyGridNum = new(1, 1);
    private int enemyNum = 0;

    private Vector2 playerScale = new(1f, 1f);

    private DamagePopupSpawner damagePopupSpawner;
    private HealthBarSpawner healthBarSpanwer;

    void Start()
    {
        damagePopupSpawner = GetComponent<DamagePopupSpawner>();
        healthBarSpanwer = GetComponent<HealthBarSpawner>();
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
        foreach (Transform child in playerHealthBarRoot)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemyHealthBarRoot)
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

    public void ActiveBossAOE()
    {
        bossAOE.SetActive(true);
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

    public void SpawnPlayer(Unit player)
    {
        Vector2 parentPos = new(playerParent.position.x, playerParent.position.y);
        Vector2 randPos = new(Random.Range(-randPosRange.x, randPosRange.x) * playerScale.x, Random.Range(-randPosRange.y, randPosRange.y) * playerScale.y);
        Vector2 playerPos = SpawnPlayerPosition() + parentPos + randPos;
        GameObject obj = Instantiate(playerPrefab, playerPos, Quaternion.identity, playerParent);
        HealthBar thisHealthbar = healthBarSpanwer.SpawnPlayerHealthBar(new(playerPos.x, playerPos.y, 0f), player.MaxHP, playerScale);
        thisHealthbar.SetHealth(player.CurrentHP);
        obj.transform.localScale = playerScale;
    }

    public void SpawnEnemy(Unit enemy)
    {
        Vector2 parentPos = new(enemyParent.position.x, enemyParent.position.y);
        Vector2 randPos = new(Random.Range(-randPosRange.x, randPosRange.x), Random.Range(-randPosRange.y, randPosRange.y));
        Vector2 enemyPos = -SpawnEnemyPosition() + parentPos + randPos;
        if (GameManager.Instance.Day == 5 && GameManager.Instance.Time == 3)
        {
            Instantiate(earthPrefab, enemyPos, Quaternion.identity, enemyParent);
        }
        else
        {
            Instantiate(enemyPrefab, enemyPos, Quaternion.identity, enemyParent);
        }
        HealthBar thisHealthbar = healthBarSpanwer.SpawnEnemyHealthBar(new(enemyPos.x, enemyPos.y, 0f), enemy.MaxHP);
        thisHealthbar.SetHealth(enemy.CurrentHP);
    }

    private int playerAttackNum = 0;

    private int attackCount = 0;
    public void AttackPlayerAction(int damage, int targetNum, Unit target)
    {
        playerParent.GetChild(playerAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, enemyParent.GetChild(targetNum).transform.position);
        enemyHealthBarRoot.GetChild(targetNum).GetComponent<HealthBar>().SetHealth(target.CurrentHP);
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


    public void ReviveHealthbar(int targetNum)
    {
        playerHealthBarRoot.GetChild(targetNum).GetComponent<HealthBar>().SetToMaxHealth();
    }
    public void AttackEnemyAction(int damage, int targetNum, Unit target)
    {
        enemyParent.GetChild(enemyAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, playerParent.GetChild(targetNum).transform.position);
        playerHealthBarRoot.GetChild(targetNum).GetComponent<HealthBar>().SetHealth(target.CurrentHP);
        enemyAttackNum++;
    }

    public void AttackAllEnemyAction(int damage, List<Unit> playerList)
    {
        enemyParent.GetChild(enemyAttackNum).GetComponent<Animator>().SetTrigger("Attack");
        damagePopupSpawner.SpawnDamage(damage, playerParent.transform.position);
        int i = 0;
        foreach (Unit player in playerList)
        {
            playerHealthBarRoot.GetChild(i).GetComponent<HealthBar>().SetHealth(player.CurrentHP);
            i++;
        }
        enemyAttackNum++;
    }

    public void EndEnemyTurn()
    {
        enemyAttackNum = 0;
    }
}
