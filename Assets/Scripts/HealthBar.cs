using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage; // Slider 대신 Image.fillAmount로 구현
    [SerializeField] private Vector2 offset = new(0, 50f); // 머리 위 위치 보정

    private RectTransform rt;
    private int maxHp;
    private int curHp;
    private Vector2 startPos; // 캔버스 로컬 기준 위치

    void Awake()
    {
        rt = (RectTransform)transform;
    }

    public void Initialize(Vector2 localPos, int maxHp, Vector3 scale)
    {
        transform.localScale = scale;
        startPos = localPos;
        this.maxHp = maxHp;
        this.curHp = maxHp;
        UpdateBar();
    }

    public void SetHealth(int value)
    {
        curHp = Mathf.Clamp(value, 0, maxHp);
        UpdateBar();
    }

    private void UpdateBar()
    {
        float ratio = (float)curHp / maxHp;
        fillImage.fillAmount = ratio;

        // 위치 갱신 (머리 위로 오프셋)
        rt.anchoredPosition = startPos + offset;
    }

    public void SetToMaxHealth()
    {
        curHp = maxHp;
        UpdateBar();
    }
}
