using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private GameObject panel;
    [SerializeField] private Vector2 offset = new Vector2(20f, -20f); // 마우스에서 살짝 떨어지게

    private bool isVisible = false;

    void Awake()
    {
        Hide();
    }

    void Update()
    {
        if (isVisible)
        {
            // 마우스 따라다니기
            panel.transform.position = Input.mousePosition + (Vector3)offset;

            // 👇 클릭 감지해서 닫기
            if (Input.GetMouseButtonUp(0)) // 0 = 좌클릭, 1 = 우클릭, 2 = 휠클릭
            {
                Hide();
            }
        }
    }

    public void Show(string text)
    {
        panel.SetActive(true);
        tooltipText.text = text;
        isVisible = true;
    }

    public void Hide()
    {
        panel.SetActive(false);
        isVisible = false;
    }
}
