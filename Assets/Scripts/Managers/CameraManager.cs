using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CameraRotationZones
{
    ZoneA,
    ZoneB,
    ZoneC,
    ZoneD
}
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

    public CameraRotationZones GetCameraZone(Vector3 playerPos)
    {
        float radius = thirdPersonCam.m_Orbits[1].m_Radius;
        float d = (float)(radius / Math.Sqrt(2.0)); // Distància x,z mitjana del centre de la circumferència al quadrant

        float x = thirdPersonCam.transform.position.x - playerPos.x;
        float z = thirdPersonCam.transform.position.z - playerPos.z;

        if (z > 0 && (-d <= x && x <= d))
        {
            return CameraRotationZones.ZoneA;
        }
        else if (x > 0 && (-d < z && z < d))
        {
            return CameraRotationZones.ZoneB;
        }
        else if (z < 0 && (-d <= x && x <= d))
        {
            return CameraRotationZones.ZoneC;
        }
        else
        {
            return CameraRotationZones.ZoneD;
        }
    }
    public void SetCameraTopPriority(Cameras camera)
    {
        foreach (var cam in camMap.Values) cam.Priority = 1;
        camMap[camera].Priority = 2;
    }
    public float GetFPCamRotationXValue() { return firstPersonCam.m_XAxis.Value; }
    public Vector3 GetFPCamPosition() { return firstPersonCam.transform.position; }
}