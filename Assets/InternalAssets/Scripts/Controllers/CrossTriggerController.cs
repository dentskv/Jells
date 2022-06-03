using System.Collections;
using UnityEngine;

public class CrossTriggerController : MonoBehaviour
{
    [SerializeField] private ObstacleCombiner obstacleCombiner;
    [SerializeField] private MeshRenderer obstacleElement;

    private Color _startColor;

    private void Start()
    {
        _startColor = obstacleElement.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeColor(obstacleCombiner.IsTouched);
        }
    }

    public bool IsTouched => obstacleCombiner.IsTouched;

    private IEnumerator SetDefaultColors()
    {
        yield return new WaitForSeconds(2f);
        obstacleElement.material.color = _startColor;
        obstacleCombiner.IsTouched = false;
    }
    
    private void ChangeColor(bool isTouched)
    {
        if (!isTouched)
        {
            obstacleElement.material.color = new Color(0.9424089f, 0.8349056f, 1f);
            StartCoroutine(SetDefaultColors());
        }
        else
        {
            obstacleElement.material.color = Color.red;
            StartCoroutine(SetDefaultColors());
        }
    }
}
