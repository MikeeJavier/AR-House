using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseManage : MonoBehaviour
{

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "AR_Menu")
        {
             Get_HouseMeasurement();
        }
    }

    void Get_HouseMeasurement()
    {
        if (this.gameObject.transform.childCount > 0)
        {
            Vector3 center = Vector3.zero;

            foreach (Transform child in this.gameObject.transform.transform)
            {
                center += child.gameObject.GetComponent<MeshRenderer>().bounds.center;
            }
            center /= this.gameObject.transform.childCount;

            Bounds bounds = new Bounds(center, Vector3.zero);

            foreach (Transform child in this.gameObject.transform.transform)
            {
                bounds.Encapsulate(child.gameObject.GetComponent<MeshRenderer>().bounds);
            }


            decimal myWidth = Math.Round((decimal)bounds.size.x, 2);
            decimal myHeight = Math.Round((decimal)bounds.size.z, 2);


            ARSpawnManager.Instance.HouseInfoText.text = myWidth + " feet x " + myHeight + " feet";


        }




    }
}
