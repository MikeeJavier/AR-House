
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Estimate_light : MonoBehaviour
{

    public ARCameraManager arcamman;

    Light our_light;
     void OnEnable()
    {
        arcamman.frameReceived += getlight;
    }

     void OnDisable()
    {
        arcamman.frameReceived -= getlight;
    }
    // Start is called before the first frame update
    void Start()
    {
        our_light = GetComponent<Light>();
    }
       
    void getlight(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.mainLightColor.HasValue)
        {
            our_light.color = args.lightEstimation.mainLightColor.Value;
        }
    }

}
