using UnityEngine;

public class PlatformCombiner : MonoBehaviour
{
    [SerializeField] private MeshFilter[] meshFilters;
    private void Awake()
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        var i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
    }
}
