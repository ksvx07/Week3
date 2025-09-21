using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    [SerializeField] Camera cam;                 // 메인 카메라
    [SerializeField] Canvas canvas;              // Overlay Canvas
    [SerializeField] DamagePopup popupPrefab;

    public void SpawnDamage(int dmg, Vector3 worldPos)
    {
        // 1. 월드 → 스크린 좌표
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        // 2. 스크린 → 캔버스 로컬 좌표
        RectTransform canvasRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screen,
            null, // Overlay 모드라 카메라 필요 없음
            out Vector2 localPos
        );

        // 3. 팝업 생성
        var popup = Instantiate(popupPrefab, canvas.transform);
        popup.gameObject.SetActive(true);
        popup.Show(dmg, localPos); // local 좌표 넘겨줌
    }
}
