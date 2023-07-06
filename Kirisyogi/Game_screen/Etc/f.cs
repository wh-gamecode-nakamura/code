using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class f : MonoBehaviour
{
    public static GameObject test_s;

    public static double Floor2(double num)
    {
        num = Math.Floor(num*10);
        return num * 0.1;
    }

    public static void sleep(int num)
    {
        print($"{num}ミリ秒");
        Thread.Sleep(num);
    }

    public static void print_t(object x) {
        print(x);
    }

    public static void Destroy_t(GameObject obj) {
        Destroy(obj);
    }

    public static GameObject Instant_obj(string obj_name, float layer) {
        return Instantiate((GameObject)Resources.Load(obj_name), 
            new Vector3(0, 0, layer), Quaternion.identity) as GameObject;
    }

    public static int[] return_pos(GameObject obj) {
        int pos_x = (int)obj.transform.position.x;
        int pos_y = (int)obj.transform.position.y;
        return new int[] {pos_x, pos_y};
    }

    public static GameObject Cre_text(float x, float y, string t)
    {
        GameObject text_canvas = Instantiate(
            (GameObject)Resources.Load("Canvas"), 
            new Vector3(x, y, Layer.UI), 
            Quaternion.identity) as GameObject;
        GameObject text_t = text_canvas.transform.GetChild(0).gameObject;
        text_t.GetComponent<UnityEngine.UI.Text>().text = t;
        return text_canvas;
    }

    public static void Edit_text(GameObject text_t, string t)
    {
        text_t = text_t.transform.GetChild(0).gameObject;
        text_t.GetComponent<UnityEngine.UI.Text>().text = t;
    }

    public static void print_r(object disp_li) {
        Type type = disp_li.GetType();

        if (type == typeof(int[])) {
            foreach (int item in (int[])disp_li) {
                print(item);
            }
        }
        else if (type == typeof(Photon.Realtime.Player[])) {
            foreach (Photon.Realtime.Player item in (Photon.Realtime.Player[])disp_li) {
                print(item);
            }
        }
    }


    ////public static void print_r(int[][] disp_li)
    ////{
    ////    string disp_str = "[";
    ////    bool pas = true;
    ////    int len = disp_li.Length;
    ////    int i = 1;
////
    ////    foreach (var item in disp_li)
    ////    {
    ////        //// if (item.GetType().IsArray) // 型宣言によって次元ごとに分けられないため
    ////        disp_str += "[";
    ////        foreach (var item2 in item)
    ////        {
    ////            disp_str += (pas) ? $"{item2}" : $", {item2}";
    ////            pas = false;
    ////        }
    ////        disp_str += (i == len) ? "]" : "], ";
    ////        pas = true; i += 1;
    ////    }
    ////    disp_str += "]";
    ////    print(disp_str);
    ////}
}
