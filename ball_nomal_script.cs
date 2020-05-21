using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_nomal_script : MonoBehaviour {

    Color color;
    GameObject white_ball;
    // Use this for initialization
   /* private bool is_exist = true;
    public bool Is_exist
    {
        get { return is_exist; }
        private set { is_exist = value; }
    }*/
    void Start()
    {

            color = GetComponentInChildren<Renderer>().material.color;
        white_ball = GameObject.FindGameObjectWithTag("white_ball");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cam_pos = Camera.main.transform.position;
        Vector3 ball_pos = white_ball.GetComponent<Rigidbody>().transform.position;
        // Debug.Log(cam_pos + "  " + ball_pos);
        Ray check_ray = new Ray(cam_pos, ball_pos - cam_pos);
        Debug.DrawRay(cam_pos, ball_pos - cam_pos);
        RaycastHit check_hit;
        bool check = Physics.Raycast(check_ray, out check_hit, (ball_pos - cam_pos).magnitude,1<<10);
        if (Input.touchCount > 0)
        {
            Touch finger1 = Input.GetTouch(0);
            if (finger1.phase == TouchPhase.Began && white_ball.GetComponent<Rigidbody>().velocity.magnitude == 0.0f)
            {
                GetComponent<SphereCollider>().enabled = false;
                //Debug.Log("こらいだきえたball");
            }
        }
        GetComponent<SphereCollider>().enabled = true;
        if (check)
        {
            //Debug.Log("透明bo-ru");
            color.a = 0.8f;
            GetComponentInChildren<Renderer>().material.color = color;
        }
        else if (!check)
        {
            //Debug.Log("元通り");
            color.a = 1.0f;
            GetComponentInChildren<Renderer>().material.color = color;
        }

        if(GetComponent<Rigidbody>().transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }

}
