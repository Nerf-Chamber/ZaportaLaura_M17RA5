using UnityEngine;
using Cinemachine;
using System;

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

    private CinemachineFreeLook currentFreelookCam;

    private void Awake()
    {
        Instance = this;
        currentFreelookCam = thirdPersonCam;
    }

    public CameraRotationZones GetCameraZone(Vector3 playerPos)
    {
        float radius = currentFreelookCam.m_Orbits[1].m_Radius;
        float d = (float)(radius / Math.Sqrt(2.0)); // Distància x,z mitjana del centre de la circumferència al quadrant

        float x = currentFreelookCam.transform.position.x - playerPos.x;
        float z = currentFreelookCam.transform.position.z - playerPos.z;

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
        if (camera == Cameras.ThirdPerson)
        {
            currentFreelookCam = thirdPersonCam;
            thirdPersonCam.Priority = 2;
            firstPersonCam.Priority = 1;
            danceCam.Priority = 1;
        }
        else if (camera == Cameras.FirstPerson)
        {
            currentFreelookCam = firstPersonCam;
            firstPersonCam.Priority = 2;
            thirdPersonCam.Priority = 1;
            danceCam.Priority = 1;
        }
        else
        {
            currentFreelookCam = thirdPersonCam;
            danceCam.Priority = 2;
            firstPersonCam.Priority = 1;
            thirdPersonCam.Priority = 1;
        }
    }
}