using UnityEngine;

public class VisibleIfFacingCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    private Renderer[] renderers;

    private void Awake() => renderers = GetComponentsInChildren<Renderer>();

    private void Update() { foreach (Renderer rend in renderers) rend.enabled = IsVisible(); }

    private bool IsVisible()
    {
        Plane[] mainPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Plane[] minimapPlanes = GeometryUtility.CalculateFrustumPlanes(minimapCamera);

        foreach (Renderer rend in renderers)
        {
            if (GeometryUtility.TestPlanesAABB(mainPlanes, rend.bounds) ||
                GeometryUtility.TestPlanesAABB(minimapPlanes, rend.bounds))
            {
                return true;
            }
        }
        return false;
    }
}
