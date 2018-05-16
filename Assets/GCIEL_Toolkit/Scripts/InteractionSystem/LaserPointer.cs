using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCIEL.Toolkit;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public Transform cameraRigTransform;
    public GameObject teleportReticlePrefab;
    public GameObject teleportPointerPrefab;
    private GameObject reticle;
    private GameObject pointer;
    private Transform teleportReticleTransform;
    public Transform headTransform;
    public Vector3 teleportReticleOffset;
    public Vector3 teleportPointerOffset;
    public LayerMask teleportMask;
    public int maxTeleportDistance;
    private bool shouldTeleport;
    private bool canTeleport = true;

    private float y;
    private float x;

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
    }


    // Z: Still think we should be using events here?
    private void Update()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;
            y = Controller.GetAxis().y;
            x = Controller.GetAxis().x;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, maxTeleportDistance, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                reticle.SetActive(true);
                pointer.SetActive(true);
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                pointer.transform.position = teleportReticleTransform.position + teleportPointerOffset;
                shouldTeleport = true;
                pointer.transform.rotation =  Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up);
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
            pointer.SetActive(false);
        }

        if(Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport && canTeleport)
        {
            RotateCamera();
            Teleport();
        }
    }

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;

        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;

        pointer = Instantiate(teleportPointerPrefab);
    }

    private void Teleport()
    {
        shouldTeleport = false;
        reticle.SetActive(false);
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }

    private void RotateCamera()
    {
        Camera.main.gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(x, 0, -y), Vector3.up);
        //cameraRigTransform.rotation = cameraRigTransform.rotation * Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up);
    }

    public void EnableTelportation()
    {
        canTeleport = true;
    }
    public void DisableTelportation()
    {
        canTeleport = false;
    }
}
