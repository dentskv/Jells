using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private bool isTurnRight;

    public bool GetTurnDirection()
    {
        return isTurnRight;
    }
}
