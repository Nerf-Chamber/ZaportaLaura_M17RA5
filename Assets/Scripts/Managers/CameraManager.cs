using UnityEngine;
using Cinemachine;
using System;

public enum TPCameraRotationZones
{
    ZoneA,
    ZoneB,
    ZoneC,
    ZoneD
}

public class CameraManager : MonoBehaviour
{
    public static TPCameraRotationZones GetCameraZone(CinemachineFreeLook thirdPersonCam, Vector3 playerPos)
    {
        float radius = thirdPersonCam.m_Orbits[1].m_Radius;
        float d = (float)(radius / Math.Sqrt(2.0)); // Distància x,z mitjana del centre de la circumferència al quadrant

        float x = thirdPersonCam.transform.position.x - playerPos.x;
        float z = thirdPersonCam.transform.position.z - playerPos.z;

        if (z > 0 && (-d <= x && x <= d))
        {
            return TPCameraRotationZones.ZoneA;
        }
        else if (x > 0 && (-d < z && z < d))
        {
            return TPCameraRotationZones.ZoneB;
        }
        else if (z < 0 && (-d <= x && x <= d))
        {
            return TPCameraRotationZones.ZoneC;
        }
        else
        {
            return TPCameraRotationZones.ZoneD;
        }
    }
}