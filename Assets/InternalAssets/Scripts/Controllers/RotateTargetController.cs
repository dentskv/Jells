using System.Collections;
using UnityEngine;

public class RotateTargetController : MonoBehaviour
{
    public bool isRotated { get; set; }

    private IEnumerator ResetState()
    {
        yield return new WaitForSeconds(2f);
        isRotated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ResetState());
        }
    }
    
}
