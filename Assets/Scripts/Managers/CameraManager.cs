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
    [SerializeField] private Camera minimapCam;
    [SerializeField] private GameObject player;

    private Dictionary<Cameras, CinemachineVirtualCameraBase> camMap;
    private float minimapCamInitialY;

    private void Awake()
    {
        Instance = this;
        minimapCamInitialY = minimapCam.transform.position.y;

        camMap = new Dictionary<Cameras, CinemachineVirtualCameraBase>{
            { Cameras.ThirdPerson, thirdPersonCam },
            { Cameras.FirstPerson, firstPersonCam },
            { Cameras.Dance, danceCam }};
    }
    private void Update() => UpdateMinimapCamera();

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
    private void UpdateMinimapCamera()
    {
        minimapCam.transform.position = new Vector3(
            player.transform.position.x, 
            minimapCamInitialY + player.transform.position.y,
            player.transform.position.z);
    }
}