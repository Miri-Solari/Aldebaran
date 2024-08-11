using UnityEngine;

public class FiringLaser : MonoBehaviour
{
    [SerializeField] Material material;
    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = material;
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.11f;
        _lineRenderer.enabled = false;
    }


    public void OpenFire(Transform target)
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, target.position);
        _lineRenderer.enabled = true;
    }

    public void StopFire()
    {
        _lineRenderer.enabled = false;
    }
}
