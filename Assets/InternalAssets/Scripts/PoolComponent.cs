using UnityEngine;

[AddComponentMenu("Pool/Pool Component")]

public class PoolComponent : MonoBehaviour
{
    [SerializeField] private Transform startConnectPoint;
    [SerializeField] private Transform endConnectPoint;

    public Transform GetStartPoint => startConnectPoint;
    public Transform GetEndPoint => endConnectPoint;
}
