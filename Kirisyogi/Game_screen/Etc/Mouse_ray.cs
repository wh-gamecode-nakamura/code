using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class Mouse_ray : MonoBehaviour
{
    public GameObject game_obj = null;
    public GameObject game_obj_t = null;

    public float mouse_pos_x;
    public float mouse_pos_y;

    public float mouse_pos_x_t;
    public float mouse_pos_y_t;

    public Camera cam;

    public float pos_x_origin1;
    public float pos_x_origin2;
    public float pos_y_origin1;
    public float pos_y_origin2;

    public float camera_default_pos_x = 2.5f; // 4.125
    public float camera_default_pos_y = 4.5f; // 2.5

    void Start()
    {
        Vector3 Camera_position = this.transform.position;
        Camera_position.x += Coord.x*0.5f - 1.5f;
        Camera_position.y += Coord.y*0.5f - 2f;
        this.transform.position = Camera_position;
    }

    void Update()
    {
        Change_size_hover();
        try {
            if (!Org_main.game.add_chara_flag && (4 < Coord.y)) {
                Move_camera_drag();
                Zoom_camera_scroll();
            }
        }
        catch (System.NullReferenceException)
        {
            //
        }
        
        
    }

    //$p ホバーによるキャラオブジェクトのサイズ変更
    private void Change_size_hover()
    {
        game_obj = Mouse.Get_hover_obj();

        //$b オブジェクトをホバーしているか
        if (game_obj) {
            //$b ホバーしているオブジェクトの変更時
            if (game_obj != game_obj_t)
            {
                Vector3 scale = game_obj.transform.localScale;

                //$b キャラをホバーしているなら
                if (game_obj.CompareTag("Chara")) {
                    scale.x *= 1.1f; scale.y *= 1.1f;
                    game_obj.transform.localScale = scale;
                    game_obj_t = game_obj;
                }

                //$b 変更前のオブジェクトがキャラなら
                if (game_obj_t && game_obj_t.CompareTag("Chara")) {
                    scale = game_obj_t.transform.localScale;
                    scale.x /= 1.1f; scale.y /= 1.1f;
                    game_obj_t.transform.localScale = scale;
                    game_obj_t = null;
                }
            }
        //$b オブジェクトのホバーをしていない場合
        } else {
            //$b 直前までオブジェクトをホバーしていたなら
            if (game_obj_t) {
                Vector3 scale = game_obj_t.transform.localScale;
                scale.x /= 1.1f; scale.y /= 1.1f;
                game_obj_t.transform.localScale = scale;
                game_obj_t = null;
            }
        }
    }

    //$p ドラッグによるカメラの移動
    private void Move_camera_drag()
    {
        //$b 押した瞬間
        if (Input.GetMouseButtonDown(0)) {
            mouse_pos_x_t = Input.mousePosition.x;
            mouse_pos_y_t = Input.mousePosition.y;
        }
        //$b マウスを離した時
        else if (Input.GetMouseButtonUp(0)) {
            ////mouse_pos_x_t = 0;
            ////mouse_pos_y_t = 0;
        }
        
        else if (Input.GetMouseButton(0)) {
            //$b 押してから動かしていない時
            if (mouse_pos_x_t == Input.mousePosition.x &&
            mouse_pos_y_t == Input.mousePosition.y) {
                mouse_pos_x_t = Input.mousePosition.x;
                mouse_pos_y_t = Input.mousePosition.y;
            }
            //$b 押してから動かした時
            else if (mouse_pos_x_t != Input.mousePosition.x ||
            mouse_pos_y_t != Input.mousePosition.y) {
                //$b 押してから動かしてから止まった時
                if (mouse_pos_x_t == Input.mousePosition.x &&
                mouse_pos_y_t == Input.mousePosition.y) {
                    mouse_pos_x_t = Input.mousePosition.x;
                    mouse_pos_y_t = Input.mousePosition.y;
                }
                else {
                    // 移動処理
                    // クリック時の座標ー今のマウスの座標
                    //mouse_pos_x_t - Input.mousePosition.x;
                    //mouse_pos_y_t - Input.mousePosition.y;
                    // 結果をカメラに加算
                    this.transform.position += new Vector3(
                        //0.1f, 0.1f, 0);
                    //print(mouse_pos_x_t); print(Input.mousePosition.x);
                        (mouse_pos_x_t - Input.mousePosition.x)/70, 
                        (mouse_pos_y_t - Input.mousePosition.y)/70, 0);

                    Vector3 pos = this.transform.position;

                    //if (pos.x < cam.orthographicSize - 2) {pos.x = cam.orthographicSize - 2;}
                    //else if (12 - cam.orthographicSize < pos.x) {pos.x = 12 - cam.orthographicSize;}
                    //if (pos.y < cam.orthographicSize - 1) {pos.y = cam.orthographicSize - 1;}
                    //else if (9 - pos.y < cam.orthographicSize) {pos.y = 9 - cam.orthographicSize;}
                    //if (pos.x < cam.orthographicSize+0.5 - (6-cam.orthographicSize)*2) {pos.x = cam.orthographicSize - 2;}
                    //if (16 - pos.x < cam.orthographicSize) {pos.x = 16 - cam.orthographicSize;}
                    //if (pos.y < cam.orthographicSize - 1) {pos.y = cam.orthographicSize - 1;}
                    // 4.5 - 4.5 : 3 - 6 : 1 - 8 : 0 - 9
                    // 9/16/3/4   x = 16/3   y = 9/4    x = 16/7   y = 9/9
                    //5*   3.5  7.5  2.5  4
                    //4*   1.5   9   1.5  5
                    //3*   0.5
                    //if (cam.orthographicSize+0.5 - 2*(6-cam.orthographicSize) < pos.x) {pos.x = cam.orthographicSize - 2;}
                    pos = aaa(pos);

                    this.transform.position = pos;
                }
                
                mouse_pos_x_t = Input.mousePosition.x;
                mouse_pos_y_t = Input.mousePosition.y;
            }
        }
    }

    //$p スクロールによるカメラのズーム
    private void Zoom_camera_scroll() {
        if (2 <= Input.touchCount) {
            var touch1 = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);

            if (pos_x_origin1 != 10000f) {
                // x
                float pos_now_x_left = (touch1.position.x < touch2.position.x) ? touch1.position.x : touch2.position.x;
                float pos_now_x_right = (touch1.position.x > touch2.position.x) ? touch1.position.x : touch2.position.x;

                float pos_x_t_left = (pos_x_origin1 < pos_x_origin2) ? pos_x_origin1 : pos_x_origin2;
                float pos_x_t_right = (pos_x_origin1 > pos_x_origin2) ? pos_x_origin1 : pos_x_origin2;

                // in
                if ((pos_now_x_left - pos_x_t_left) <= 0 &&
                0 <= pos_now_x_right - pos_x_t_right) {
                    cam.orthographicSize -= 0.2f * 1;
                }

                // out
                if (0 <= (pos_now_x_left - pos_x_t_left) &&
                pos_now_x_right - pos_x_t_right <= 0) {
                    cam.orthographicSize -= 0.2f * -1;
                }
                if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
                else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}

                pos_x_origin1 = touch1.position.x;
                pos_x_origin2 = touch2.position.x;


                // y
                float pos_now_y_left = (touch1.position.y < touch2.position.y) ? touch1.position.y : touch2.position.y;
                float pos_now_y_right = (touch1.position.y > touch2.position.y) ? touch1.position.y : touch2.position.y;

                float pos_y_t_left = (pos_y_origin1 < pos_y_origin2) ? pos_y_origin1 : pos_y_origin2;
                float pos_y_t_right = (pos_y_origin1 > pos_y_origin2) ? pos_y_origin1 : pos_y_origin2;

                // in
                if ((pos_now_y_left - pos_y_t_left) <= 0 &&
                0 <= pos_now_y_right - pos_y_t_right) {
                    cam.orthographicSize -= 0.2f * 1;
                }

                // out
                if (0 <= (pos_now_y_left - pos_y_t_left) &&
                pos_now_y_right - pos_y_t_right <= 0) {
                    cam.orthographicSize -= 0.2f * -1;
                }
                if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
                else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}

                pos_y_origin1 = touch1.position.y;
                pos_y_origin2 = touch2.position.y;
            }
            pos_x_origin1 = touch1.position.x;
            pos_x_origin2 = touch2.position.x;
            pos_y_origin1 = touch1.position.y;
            pos_y_origin2 = touch2.position.y;
        }
        else {
            pos_x_origin1 = 10000f;
            pos_x_origin2 = 10000f;
            pos_y_origin1 = 10000f;
            pos_y_origin2 = 10000f;
        }

        //回転の取得
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        cam = this.GetComponent<Camera>();

        //$b スクロールしたなら
        if (wheel != 0) {
            int wheel_num = (0 < wheel) ? 1 : -1;
            cam.orthographicSize -= 0.3f * wheel_num;
            if (cam.orthographicSize < 1) {cam.orthographicSize = 1;}
            else if (5 < cam.orthographicSize) {cam.orthographicSize = 5;}
            else {Move_camera_zoom(wheel_num);}
        }
    }

    //$c リサイズによるカメラの移動
    private void Move_camera_zoom(int wheel_num) {
        // x = 0...0.25, x = 500...0.25, x = 250...0
        // y = 0...0.125, x = 250...0.125, y = 125...0
        // y = (x - 250)*0.001
        Vector3 pos = this.transform.position;
        pos.x += wheel_num * (Input.mousePosition.x - 250) / 500;
        pos.y += wheel_num * (Input.mousePosition.y - 125) / 500;
        pos = aaa(pos);
        this.transform.position = pos;
    }

    private Vector3 aaa(Vector3 pos) {
        //5*   3.5  7.5  2.5  4
        //4*   1.5   9   1.5  5
        //3*   0.5  11
        //2*  -1.5
        //1*  -3.5
        //if (pos.x < 1.5f*cam.orthographicSize - 4) {pos.x = 1.5f*cam.orthographicSize - 4;}
        //else if (3.5f*cam.orthographicSize - 3 < pos.x) {pos.x = 3.5f*cam.orthographicSize - 3;}
        //if (pos.y < cam.orthographicSize - 2.5f) {pos.y = cam.orthographicSize - 2.5f;}
        //else if (9 - cam.orthographicSize < pos.y) {pos.y = 9 - cam.orthographicSize;}
        if (pos.x < -4.5f) {pos.x = -4.5f;}
        else if (12.5f < pos.x) {pos.x = 12.5f;}
        if (pos.y < -2) {pos.y = -2;}
        else if (7 < pos.y) {pos.y = 7;}
        return pos;
    }
}