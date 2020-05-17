using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]

public class ARPlacement : MonoBehaviour
{
    /// <summary>
    /// This controls the placement of Augmented Reality objects and the placement logo. 
    /// </summary>
    ///

    [SerializeField]

    public GameObject placementIndicator;
    public GameObject vehicle1;
    public GameObject vehicle2;
    public GameObject vehicle3;
    public MenuScript menuScript;
    public GameObject slider;

    private ARRaycastManager arRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool vehicleIsPlaced = false;
    private GameObject instantiatedVehicle;

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuScript.currentMenuState)
        {
            case (MenuScript.MenuStates.Main):
                break;

            case (MenuScript.MenuStates.AR):
                UpdatePlacementPose();
                UpdatePlacementIndicator();
                break;
        }
    }


    private void UpdatePlacementIndicator()
    {
        //controls the position of the placement logo on the plane
        if (placementPoseIsValid && !vehicleIsPlaced)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }

    }


    private void UpdatePlacementPose()
    {
        try
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
            placementPoseIsValid = hits.Count > 0;

            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;
                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
            Debug.Log(placementPose.position);
        }
        catch (System.NullReferenceException)
        {
            placementPose = new Pose();
        }
    }


    private void placeVehicle()
    {
        GameObject vehicleObj = vehicle1;
        Debug.Log("VEHICLE IS PLACED");
        switch (menuScript.currentARState) {

            case (MenuScript.ARStates.Vehicle1):
                vehicleObj = vehicle1;
                break;

            case (MenuScript.ARStates.Vehicle2):
                vehicleObj = vehicle2;
                break;

            case (MenuScript.ARStates.Vehicle3):
                vehicleObj = vehicle3;
                break;
        }

        instantiatedVehicle = Instantiate(vehicleObj, placementPose.position, placementPose.rotation);
        float value = getSliderValue();
        instantiatedVehicle.transform.localScale = new Vector3(value, value, value);
        vehicleIsPlaced = true;
    }


    private void DestroyVehicle()
    {
        if (vehicleIsPlaced)
        {
            Destroy(instantiatedVehicle);
            vehicleIsPlaced = false;
        }
    }

    public void placeOrRemoveVehicle()
    {
        if (vehicleIsPlaced)
        {
            DestroyVehicle();
        }
        else
        {
            placeVehicle();
        }
    }

    public void scaleVehicle(float value)
    {
        if (vehicleIsPlaced) { 
            instantiatedVehicle.transform.localScale = new Vector3(value, value, value);
        }
    }


    public float getSliderValue()
    {
        return slider.GetComponent<Slider>().value;
    }

    public void resetAR()
    {
        DestroyVehicle();
    }

    public bool isVehiclePlaced()
    {
        return vehicleIsPlaced;
    }
}
