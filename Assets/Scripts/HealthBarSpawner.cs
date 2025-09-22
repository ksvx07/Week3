using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
    [SerializeField] private Camera cam;         // 메인 카메라
    [SerializeField] private Transform playerRoot;      // Overlay Canvas
    [SerializeField] private Transform enemyRoot;      // Overlay Canvas
    [SerializeField] private HealthBar healthBarPrefab;

    /// <summary>
    /// worldPos : 체력바를 붙일 대상의 월드 위치 (예: 머리 위 Pivot)
    /// maxHp    : 초기 최대 체력
    /// </summary>
    public HealthBar SpawnPlayerHealthBar(Vector3 worldPos, int maxHp, Vector3 scale)
    {
        // 월드 → 스크린
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        // 스크린 → 캔버스 로컬
        RectTransform canvasRect = playerRoot.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screen,
            null, // Overlay 모드라 카메라 필요 없음
            out Vector2 localPos
        );

        // 프리팹 생성
        var hb = Instantiate(healthBarPrefab, playerRoot.transform);
        hb.gameObject.SetActive(true);
        hb.Initialize(localPos, maxHp, scale);

        return hb;
    }

    public HealthBar SpawnEnemyHealthBar(Vector3 worldPos, int maxHp)
    {
        // 월드 → 스크린
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        // 스크린 → 캔버스 로컬
        RectTransform canvasRect = enemyRoot.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screen,
            null, // Overlay 모드라 카메라 필요 없음
            out Vector2 localPos
        );

        // 프리팹 생성
        var hb = Instantiate(healthBarPrefab, enemyRoot.transform);
        hb.gameObject.SetActive(true);
        hb.Initialize(localPos, maxHp, new(1, 1, 1));

        return hb;
    }
}
