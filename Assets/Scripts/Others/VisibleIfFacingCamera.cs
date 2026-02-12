using System.Collections.Generic;
using UnityEngine;

public class VisibleIfFacingCamera : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera minimapCamera;

    private Renderer[] renderers;
    private Light[] lights;
    private void Update() 
    { 
        foreach (GameObject obj in objects)
        {
            renderers = obj.GetComponentsInChildren<Renderer>();
            lights = obj.GetComponentsInChildren<Light>();
            foreach (Renderer rend in renderers) rend.enabled = IsVisible();
            foreach (Light light in lights) light.enabled = IsVisible();
        }
    }

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
