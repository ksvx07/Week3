using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI txt;

    [Header("Anim")]
    [SerializeField] float life = 0.8f;          // 전체 지속
    [SerializeField] Vector2 move = new(0, 80f); // 위로 이동량(px)
    [SerializeField] AnimationCurve posCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0.6f, 1, 1f);

    RectTransform rt;
    Color baseColor;
    Vector2 startPos;

    void Awake()
    {
        rt = (RectTransform)transform;
        if (!txt) txt = GetComponent<TextMeshProUGUI>();
        baseColor = txt.color;
    }

    public void Show(int dmg, Vector2 screenPos)
    {
        txt.text = dmg.ToString();
        startPos = screenPos;
        StopAllCoroutines();
        StartCoroutine(CoRun());
    }

    System.Collections.IEnumerator CoRun()
    {
        float t = 0f;
        while (t < life)
        {
            float n = t / life;

            // 위치
            rt.anchoredPosition = startPos + move * posCurve.Evaluate(n);
            // 스케일
            float s = scaleCurve.Evaluate(n);
            rt.localScale = new Vector3(s, s, 1);
            // 알파
            var c = baseColor;
            c.a = alphaCurve.Evaluate(n);
            txt.color = c;

            t += Time.unscaledDeltaTime; // timescale 영향 안받게
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
