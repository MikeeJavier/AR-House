using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation; 



[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject OpeningToOpenAreaSpawnHouseScene;
    [SerializeField] private Button NextSceneButton;
    public TextMeshProUGUI textInfo;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }


    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    } 

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        { 
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        { 
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }



    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        textInfo.text = "House name : "+name;

        GameObject prefab = spawnedPrefabs[name]; 
        prefab.transform.position = position;
        prefab.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        prefab.SetActive(true);


        if(name == "Smallhouse1")
        {
            PlayerPrefs.SetString("HouseName", "house1");
            PlayerPrefs.Save();
        }
        else if (name == "Smallhouse2")
        {
            PlayerPrefs.SetString("HouseName", "house2");
            PlayerPrefs.Save();
        }


        if (name != "")
        { 
            textInfo.text = "After scanning the area make sure a mesh will appear and touch the screen's device where you want to spawn.";
            NextSceneButton.interactable = true;
            NextSceneButton.GetComponentInChildren<TextMeshProUGUI>().text = "Scan Open Area";
        }
        else
        {
            textInfo.text = "Scan the image.";
            NextSceneButton.interactable = false;
        }


        foreach (GameObject GO in spawnedPrefabs.Values)
        {
            if(GO.name != name)
            {
                GO.SetActive(false);
            } 
        }

    }
    

    public void DelayGoingToSpawnHouseScene()
    {
        SceneManager.LoadScene("AR_Menu");
    }
    

    public void ScanNewHouse()
    {
        SceneManager.LoadScene("ScanImageConfiguration");
    }

}
