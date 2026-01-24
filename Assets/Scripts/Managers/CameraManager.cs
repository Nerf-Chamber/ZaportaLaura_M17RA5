using Cinemachine;
using UnityEngine;
using System.Collections.Generic;

public enum Cameras
{
    ThirdPerson,
    FirstPerson,
    Dance
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineFreeLook thirdPersonCam;
    [SerializeField] private CinemachineFreeLook firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera danceCam;

    private Dictionary<Cameras, CinemachineVirtualCameraBase> camMap;

    private void Awake()
    {
        Instance = this;

        camMap = new Dictionary<Cameras, CinemachineVirtualCameraBase>{
            { Cameras.ThirdPerson, thirdPersonCam },
            { Cameras.FirstPerson, firstPersonCam },
            { Cameras.Dance, danceCam }};
    }
    public void SetCameraTopPriority(Cameras camera)
    {
        foreach (var cam in camMap.Values) cam.Priority = 1;
        camMap[camera].Priority = 2;
    }
    public float GetCamRotationXValue(Cameras camera) 
    {
        if (camMap.TryGetValue(camera, out var cam) && cam is CinemachineFreeLook freeLook) return freeLook.m_XAxis.Value;
        return 0f;
    }
    public Vector3 GetCamPosition(Cameras camera) 
    { 
        return camMap[camera].transform.position;
    }
    public float GetAngleThirdPerson(Vector2 tempDirection)
    {
        if (tempDirection != Vector2.zero) return Mathf.Atan2(tempDirection.x, tempDirection.y) * Mathf.Rad2Deg;
        return 0f;
    }
    public float GetAngleFirstPerson(Vector2 tempDirection)
    {
        if (tempDirection.y < 0 || tempDirection == Vector2.zero) return 0f;
        else return Mathf.Atan2(tempDirection.x, tempDirection.y) * Mathf.Rad2Deg;
    }
}