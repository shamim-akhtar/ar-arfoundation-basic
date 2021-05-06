using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Spawner : MonoBehaviour
{
/// <summary>
/// The reference to ARRaycastManager. This will handle 
/// the ray casting towards the tracckable features.
/// </summary>
public ARRaycastManager m_ARRaycastManager;

/// <summary>
/// The prefab for the spawned object. You can have a list
/// of prefabs as well and create some mechanishm to let user
/// select what object they want to place in the scene. 
/// Think of the IKEA app.
/// </summary>
public GameObject m_spawnableObjectPrefab;

/// <summary>
/// Representation of a Position, and a Rotation in 3D Space. 
/// This structure is used primarily in XR applications to 
/// describe the current "pose" of a device in 3D space.
/// https://docs.unity3d.com/ScriptReference/Pose.html
/// </summary>
Pose m_placementPose;

/// <summary>
/// A temporary variable to hold the recently spawned object.
/// </summary>
GameObject m_spawnedObject = null;


    // Start is called before the first frame update
void Start()
{
    m_spawnedObject = null;
}

private void Update()
{
    //Check for touch inputs. If there is no touch event then return.
    if (Input.touchCount == 0)
        return;

    //If there is a touch event then get the touch position
    var touchPt = Input.GetTouch(0).position;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //Do a raycast using the ARRaycastManager to get the hits.
    //ARRaycastManager manages an XRRaycastSubsystem, exposing 
    //raycast functionality in ARFoundation.Use this component
    //to raycast against trackables(i.e., detected features in 
    //the physical environment) when they do not have a presence 
    //in the Physics world.
    //https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.ARRaycastManager.html

    m_ARRaycastManager.Raycast(touchPt, hits);
    if (hits.Count == 0)
        return;

    //If there is a hit with trackable features then keep a 
    //reference to the Pose.
    m_placementPose = hits[0].pose;
    if(Input.GetTouch(0).phase == TouchPhase.Began)
    {
        //The Spawn function. 
        Spawn(m_placementPose.position);
    }
    else if(Input.GetTouch(0).phase == TouchPhase.Moved && m_spawnedObject != null)
    {
        //If there is a TouchPhase.Moved event and the spawnedObject 
        //is not null then reposition the object based on our touch input. 
        //Move the object.
        m_spawnedObject.transform.position = m_placementPose.position;
    }
    if(Input.GetTouch(0).phase == TouchPhase.Ended)
    {
        //If the TouchPhase has ended then reset the m_spawnedObject 
        //to null so that we can handle another new spawned object.
        m_spawnedObject = null;
    }
}

    //private void UpdateARPlacementIndicator()
    //{
    //    if(m_placementValid)
    //    {
    //        m_placementPoseIndicator.SetActive(true);
    //        m_placementPoseIndicator.transform.SetPositionAndRotation(m_placementPose.position, m_placementPose.rotation);
    //    }
    //    else
    //    {
    //        m_placementPoseIndicator.SetActive(false);
    //    }
    //}

void Spawn(Vector3 position)
{
    m_spawnedObject = Instantiate(m_spawnableObjectPrefab, position, Quaternion.identity);
}
}
