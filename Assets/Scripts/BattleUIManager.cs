using UnityEngine;

public class BattleUI : MonoBehaviour
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


    private Vector2Int playerGridNum = new(1, 1);
    private Vector2Int enemyGridNum = new(1, 1);

    private int playerNum = 0;
    private int enemyNum = 0;

    private Vector2 playerScale = new(0.1f, 0.1f);

    public void InitRectNum(Vector3 thisScale)
    {
        playerScale = thisScale;
        playerNum = 0;
        enemyNum = 0;
        playerGridNum.x = (int)(playerRect.x / (playerSize.x * playerScale.x * rightMargin));
        playerGridNum.y = (int)(playerRect.y / (playerSize.y * playerScale.y * topMargin));
        enemyGridNum.x = (int)(enemyRect.x / (enemySize.x * rightMargin));
        enemyGridNum.y = (int)(enemyRect.y / (enemySize.y * topMargin));
    }
    private Vector2 SpawnPlayerPosition()
    {
        Vector2Int playerGrid = Vector2Int.zero;
        int i = playerNum;
        while (i >= playerGridNum.x)
        {
            if (playerGrid.y > playerGridNum.y)
                break;
            i -= playerGridNum.x;
            playerGrid.y++;
        }
        playerGrid.x = i;
        playerNum++;

        return new Vector2(playerGrid.x * playerSize.x * playerScale.x * topMargin, playerGrid.y * playerSize.y * playerScale.y * rightMargin);
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
}
