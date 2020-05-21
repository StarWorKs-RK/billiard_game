using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_key_script : MonoBehaviour
{
    Vector3 bigin;
    Vector3 end;
    bool flg = false;
    bool double_tap_start = false;
    bool double_tap_on = false;
    float tap_time = 0.0f;
    bool same_time_touch=false;
    float bfo_distance=0.0f;
    float double_end_time = 0.0f;
    Vector3 direction=Vector3.zero;
    bool is_just_end = false;
    Vector3 ball_pos;
   // ball_nomal_script ball_nomal_scri;
    [SerializeField]
    private LineRenderer arrow = null;
    // Use this for initialization
    void Start()
    {
        Camera.main.transform.LookAt(gameObject.transform);
       arrow = GameObject.FindGameObjectWithTag("direction").GetComponent<LineRenderer>();
       ball_pos = GetComponent<Rigidbody>().transform.position;
       // bool a = ball_nomal_scri.Is_exist;
    }

    // Update is called once per frame
    void Update()
    {

        //Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit_ball;
        ball_pos = GetComponent<Rigidbody>().transform.position;
        
        if (double_tap_start)
        {
            tap_time += Time.deltaTime;
            //Debug.Log("タップ間の時間"+tap_time);
        }
        if (double_tap_start && tap_time > 0.3f)
        {
            double_tap_start = false;
        }
        //同時タップ後にカメラの回転が一瞬作動してしまうのを防ぐため、同時タップの直後かどうかを調べる。
        if (is_just_end)
        {
            double_end_time += Time.deltaTime;
            if (double_end_time > 0.1f)
            {
                is_just_end = !is_just_end;
            }
        }
        
        int touchcount = Input.touchCount;

        if (touchcount > 0)
        {
            Touch finger1 = Input.GetTouch(0);
            //二本指でのタッチ時
            if(/*double_tap_start && (tap_time < 0.1f||tap_time==0.0f) &&*/ Input.touchCount > 1 && flg==false){
                Touch finger2 = Input.GetTouch(1);
                same_time_touch = true;
                if (bfo_distance == 0.0f)
                {
                    bfo_distance = (finger1.position - finger2.position).magnitude;
                    direction = (/*GetComponent<Rigidbody>().transform.position*/ball_pos -Camera.main.transform.position).normalized;
                    Debug.Log("gamepos" + ball_pos/*GetComponent<Rigidbody>().transform.position*/);
                    Debug.Log("campos" + Camera.main.transform.position);
                }
                float now_distance = (finger1.position - finger2.position).magnitude;
                float x = Camera.main.transform.position.x;
                float y = Camera.main.transform.position.y;
                float z = Camera.main.transform.position.z;
                //縮小
                if ((now_distance-bfo_distance)<0 && (/*GetComponent<Rigidbody>().transform.position*/ball_pos-Camera.main.transform.position).magnitude<20)
                {
                    //Debug.Log("dir"+direction);
                    //Camera.main.transform.Translate(-direction*0.100f);
                    //Camera.main.transform.LookAt(gameObject.transform);
                    Camera.main.transform.position = new Vector3(x - direction.x * 0.2f, y - direction.y * 0.2f, z - direction.z * 0.2f);
                    bfo_distance = now_distance;
                }
                //拡大
                if((now_distance - bfo_distance) > 0&& (/*GetComponent<Rigidbody>().transform.position*/ball_pos - Camera.main.transform.position).magnitude > 4)
                {
                    Camera.main.transform.position=new Vector3(x+direction.x*0.2f,y+direction.y*0.2f,z+direction.z*0.2f);
                    //Debug.Log("direction" + direction);
                    //Camera.main.transform.LookAt(gameObject.transform.position);
                    bfo_distance = now_distance;
                }
                if (finger1.phase == TouchPhase.Ended || finger2.phase == TouchPhase.Ended)
                {
                    bfo_distance = 0.0f;
                    same_time_touch = false;
                    direction = Vector3.zero;
                    is_just_end = true;
                    double_end_time = 0.0f;
                }
            }
            Ray finger1_ray = Camera.main.ScreenPointToRay(finger1.position);
            //以下タッチされた（マウスでクリックされたとき）の処理
            if (/*Input.GetMouseButtonDown(0) ||*/ finger1.phase == TouchPhase.Began)
            {//ボールに触れた瞬間
                //arrow.enabled = true;
                //arrow.SetPosition(0, GetComponent<Rigidbody>().transform.position);
                //arrow.SetPosition(1, GetComponent<Rigidbody>().transform.position);
                if (!double_tap_start)
                {
                    double_tap_start = true;
                    //Debug.Log("start change to true");
                    tap_time = 0.0f;
                }
                if (double_tap_start && tap_time <= 0.3f && tap_time>0.0f)
                {
                    //Debug.Log("on ni narumade"+tap_time);
                    double_tap_on = true;
                    //Debug.Log("on change to true");
                    double_tap_start = false;
                    //Debug.Log("start change to false");
                }
                if (/*Physics.Raycast(mouse_ray, out hit_ball, 50) ||*/ Physics.Raycast(finger1_ray, out hit_ball, 50,1<<8))
                {
                    /*if (hit_ball.collider.gameObject.transform.position == gameObject.transform.position)
                    {*/
                        //白ボールが動いている間は新たに動かせない。
                        if (/*delta_ball_pos != Vector3.zero*/GetComponent<Rigidbody>().velocity.magnitude!=0.0f)
                        {
                            return;
                        }
                        
                            flg = true;
                            arrow.enabled = true;
                            //bigin = Input.mousePosition;
                            bigin = finger1.position;
                            GetComponent<Renderer>().material.color = Color.blue;
                        
                    //}

                }
            }
            //フラグあり＆タッチしているとき
            if ((Input.GetMouseButton(0) || finger1.phase == TouchPhase.Moved || finger1.phase == TouchPhase.Stationary) && flg)
            {
                //end = Input.mousePosition;
               end = finger1.position;
                Vector3 cam_rote0 = Camera.main.transform.localEulerAngles;
                Vector3 arrow_size =new Vector3(bigin.x - end.x,0,bigin.y - end.y);
                var arrow_size_dir = Quaternion.Euler(0, cam_rote0.y, 0)*arrow_size;
                arrow.SetPosition(0,ball_pos);
                arrow.SetPosition(1,ball_pos+arrow_size_dir*0.05f);
                GetComponent<Renderer>().material.color = Color.blue;
            }
            //フラグあり＆タッチが終わったとき
            if ((Input.GetMouseButtonUp(0) || finger1.phase == TouchPhase.Ended) && flg)
            {
                Vector3 cam_rote = Camera.main.transform.localEulerAngles;
                Vector3 power_size = new Vector3(bigin.x - end.x, 0, bigin.y - end.y);
                Vector3 power_result;
                var power = Quaternion.Euler(0, cam_rote.y, 0) * power_size;
                if (power.magnitude * 5 <= 1100)
                {
                    power_result = power * 5;
                    GetComponent<Rigidbody>().AddForce(power_result);
                    //Debug.Log("result" + power_result);
                }
                else if (power.magnitude * 5 > 1100)
                {
                    power_result = power.normalized * 1100;
                    GetComponent<Rigidbody>().AddForce(power_result);
                    //Debug.Log("res" + power_result);
                }
                GetComponent<Renderer>().material.color = Color.white;
                flg = false;
                arrow.enabled = false;
            }
            //ボールに触れていないとき&指一本のとき
            if (/*Physics.Raycast(finger1_ray, out hit_ball, 50) && hit_ball.collider.gameObject.transform.position != gameObject.transform.position && */flg == false && !same_time_touch && !is_just_end)
            {
                Vector3 finger_bigin_pos = Input.GetTouch(0).position;
                if (Input.GetKey(KeyCode.RightArrow) || finger1.deltaPosition.x > 0)
                {
                   // Debug.Log("x>0");
                    Camera.main.transform.RotateAround(new Vector3(transform.position.x, 5.5f, transform.position.z), Vector3.up, 0.5f * finger1.deltaPosition.magnitude);
                }
                if (Input.GetKey(KeyCode.LeftArrow) || finger1.deltaPosition.x < 0)
                {
                   // Debug.Log("x<0");
                    Camera.main.transform.RotateAround(new Vector3(transform.position.x, 5.5f, transform.position.z), Vector3.down, 0.5f * finger1.deltaPosition.magnitude);
                }
            }
            //ダブルタップ時
            if (Input.GetKeyDown(KeyCode.Space) || double_tap_on)
            {
                Vector3 ball_pos = transform.position;
                Camera.main.transform.position = new Vector3(ball_pos.x, 4.5f + ball_pos.y, -5.0f + ball_pos.z);
                Camera.main.transform.LookAt(gameObject.transform);
                double_tap_on = false;
            }
        }
    }
}
//プレイヤーを1,2設定
//一回目
//白ボールの場所を指定して、ショット
//最初に落としたプレイヤーのボールグループがそのプレイヤーのグループとする
//失敗したら、一からやり直し
//（一回目が成功した）２回目以降
//・手球がポケットしてしまった場合
//・手球がテーブルから落ちてしまった場合
//・手球をキューですくいあげるようにショットした場合
//・8番ボールを途中で落とした時
//・なににも当たらなかった場合
//最初に相手ボールに当たった場合
//は白ボールを任意位置において再開（おとしたボールはそのまま）
//8bo-ruをいれるポケットのみ指定→指定以外→失敗→ランダム生成

//ブレイクショット時を除き、８番ボールをポケットした時にファウルした場合
//コールした以外のポケットに８番ボールを入れた場合
//テーブルがオープンの時に８番ボールをポケットした場合
//自分のグループボール全てをポケットする以前に８番ボールをポケットした場合
//自分の最後のグループボールと８番ボールをダブルインした場合
//自分のグループボールを全てポケットした後にファウルした場合