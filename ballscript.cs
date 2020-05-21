using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballscript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var touchCount = Input.touchCount;//触れているゆびの数取得
        RaycastHit hit;
        for (var i = 0; i < touchCount; i++)
        {
            var touch = Input.GetTouch(0);
            Ray finger_ray = Camera.main.ScreenPointToRay(touch.position);


            if (Physics.Raycast(finger_ray, out hit, 100) && gameObject == hit.collider.gameObject)
            {
                Vector3 finger_bigin = touch.position;
                Debug.Log(finger_bigin);
                GetComponent<Renderer>().material.color = Color.blue;

                switch (touch.phase)
                {
                    case TouchPhase.Began://指が触れたとき
                    case TouchPhase.Moved://指が動いているとき
                                          /*Vector3 finger_pos = touch.position;
                                          Vector3 finger_real_pos = Camera.main.ScreenToWorldPoint(finger_pos);
                                          finger_real_pos.y = 0.5f;
                                          Vector3 force_level = transform.position - finger_pos;*/
                        //Vector3 finger_end = touch.position;
                        break;
                    case TouchPhase.Stationary://指が止まっているとき
                                               /*finger_pos = touch.position;
                                               finger_real_pos = Camera.main.ScreenToWorldPoint(finger_pos);
                                               finger_real_pos.y = 0.5f;
                                               force_level = transform.position - finger_pos;
                                               Debug.Log("force" + force_level);*/
                        //finger_end = touch.position;
                        break;
                    case TouchPhase.Ended://指が離れたとき

                        /*finger_pos = touch.position;
                        finger_real_pos = Camera.main.ScreenToWorldPoint(finger_pos);
                        finger_real_pos.y = 0.5f;
                        force_level = transform.position - finger_pos;
                        GetComponent<Rigidbody>().AddForce(force_level);

                        break;*/
                        Vector3 finger_end = touch.position;
                        Debug.Log(finger_end);
                        GetComponent<Rigidbody>().AddForce(finger_end - finger_bigin*5);
                        break;
                    case TouchPhase.Canceled://システムがタッチ処理をキャンセルしたとき
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();

                }

                GetComponent<Renderer>().material.color = Color.red;
            }


        }
    }
}
