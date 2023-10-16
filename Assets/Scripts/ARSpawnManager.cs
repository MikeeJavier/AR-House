using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using UnityEngine.UI;

public class ARSpawnManager : MonoBehaviour
{
    public static ARSpawnManager Instance;
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    public GameObject[] housePrefabs;
    public TextMeshProUGUI HouseInfoText;
    public Button GoToFloorButton;
    public GameObject myHouseSpawned;
    public Vector3 myPosHitPose;
    public Quaternion myRotHitPose;
    public bool isGo2ndFloor;

    public bool useCursor = true;
    bool hasAlreadySpawned;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        cursorChildObject.SetActive(useCursor);

        switch (PlayerPrefs.GetString("HouseName"))
        {
            case "house1":
                objectToPlace = housePrefabs[0];
                break;
            case "house2":
                objectToPlace = housePrefabs[1];
                break;
        }

    }
    private void Update()
    {

        if (!hasAlreadySpawned)
        {
            if (useCursor)
            {
                UpdateCursor();
            }


            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (useCursor)
                {
                    Vector3 pos1 = new Vector3(transform.position.x, transform.position.y, transform.position.z + 14.2f);
                    GameObject GO_Cursor =  GameObject.Instantiate(objectToPlace, pos1, transform.rotation);
                    GO_Cursor.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    // hasAlreadySpawned = true; 


                }
                else
                {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                    if (hits.Count > 0)
                    {
                        if(!isGo2ndFloor)
                        {

                            if (myHouseSpawned == null)
                            {
                                Vector3 pos = new Vector3(hits[0].pose.position.x, hits[0].pose.position.y, hits[0].pose.position.z + 14.2f);
                                myPosHitPose = pos;
                                myRotHitPose = hits[0].pose.rotation;
                                myHouseSpawned = GameObject.Instantiate(objectToPlace, pos, hits[0].pose.rotation);
                                myHouseSpawned.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            }
                        }
                        else
                        {
                            if (myHouseSpawned == null)
                            {
                                Vector3 pos = new Vector3(hits[0].pose.position.x, hits[0].pose.position.y, hits[0].pose.position.z + 14.2f);
                                myPosHitPose = pos;
                                myRotHitPose = hits[0].pose.rotation;

                                myHouseSpawned = GameObject.Instantiate(objectToPlace, pos, hits[0].pose.rotation);
                                myHouseSpawned.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            }
                           /* myHouseSpawned.transform.GetChild(0).position = new Vector3(myHouseSpawned.transform.GetChild(0).position.x,
                                myHouseSpawned.transform.GetChild(0).position.y - 3f,
                                myHouseSpawned.transform.GetChild(0).position.z);*/
                        }
                       // hasAlreadySpawned = true; 
                    }
                }
            }
        }



    }

    private void UpdateCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }


    public void GoTo2ndFloorHouse(TextMeshProUGUI _WhatFloorText)
    {
        if(!isGo2ndFloor)
        {
            isGo2ndFloor = true;
            _WhatFloorText.text = "Go To 1st Floor";
            myHouseSpawned.transform.GetChild(0).position = new Vector3(myHouseSpawned.transform.GetChild(0).position.x,
                myHouseSpawned.transform.GetChild(0).position.y + 4f,
                myHouseSpawned.transform.GetChild(0).position.z);

        }
        else
        {
            isGo2ndFloor = false;
            _WhatFloorText.text = "Go To 2nd Floor";

            myHouseSpawned.transform.GetChild(0).position = new Vector3(myHouseSpawned.transform.GetChild(0).position.x,
                myHouseSpawned.transform.GetChild(0).position.y - 4f,
                myHouseSpawned.transform.GetChild(0).position.z);
        }
    }

 

}
