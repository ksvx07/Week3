using UnityEngine;
using System.Collections;

public class SkillEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    private Coroutine hideRoutine;

    private void OnEnable()
    {
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(CoHide());
    }

    private IEnumerator CoHide()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        hideRoutine = null;
    }
}
