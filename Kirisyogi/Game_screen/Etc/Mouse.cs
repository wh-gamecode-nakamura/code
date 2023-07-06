using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mouse
{

    //$p 押下したオブジェクトを取得
    public static GameObject Get_down_obj() {
        GameObject down_obj = null;

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {down_obj = hit2d.transform.gameObject;}
        }
        return down_obj;
    }

    //$p クリックしたオブジェクトを取得
    public static GameObject Get_up_obj() {
        GameObject up_obj = null;

        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {up_obj = hit2d.transform.gameObject;}
        }
        return up_obj;
    }

    //$p 長押しまたはホバーしたオブジェクトを取得
    public static GameObject Get_hover_obj() {
        GameObject clicked_game_obj = null;

        //$p スマホ用
        //$b 長押し
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {clicked_game_obj = hit2d.transform.gameObject;}
        //$p PC用
        //$b ホバー
        } else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d) {clicked_game_obj = hit2d.transform.gameObject;}
        }

        return clicked_game_obj;
    }
}