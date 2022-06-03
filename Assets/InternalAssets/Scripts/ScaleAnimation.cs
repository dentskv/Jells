using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    private Transform _buttonTransform;

    private void Awake()
    {
        _buttonTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        SetDefaultTransform();
        _buttonTransform.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.OutElastic, -1f);
    }

    private void OnDisable()
    {
        SetDefaultTransform();
    }

    private void SetDefaultTransform()
    {
        _buttonTransform.localScale = new Vector3(0, 0, 1);
    }
}
