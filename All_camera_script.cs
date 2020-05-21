using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_camera_script : MonoBehaviour {

    bool device_dir_changed;
    Camera all_camera;
	// Use this for initialization
	void Start () {
		 device_dir_changed = false;
         all_camera =GameObject.Find("all_Camera").GetComponent<Camera>();
	}

    // Update is called once per frame
    void Update() {
       // Debug.Log(Input.deviceOrientation);
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Unknown:
                case DeviceOrientation.FaceUp:
                    all_camera.rect = new Rect(0.7f, 0.7f, 0.3f, 0.3f);
                    break;
                case DeviceOrientation.LandscapeLeft:
                    all_camera.rect = new Rect(0.85f, 0.6f, 0.15f, 0.45f);
                    Debug.Log("左横向き");
                    break;
                case DeviceOrientation.LandscapeRight:
                    all_camera.rect = new Rect(0.85f, 0.6f, 0.15f, 0.45f);
                    break;
                default:
                    if (Screen.orientation != (ScreenOrientation)Input.deviceOrientation)
                    {
                        device_dir_changed = !device_dir_changed;
                        //Debug.Log("方向変化");
                    }
                    break;
            }    
	}
}
