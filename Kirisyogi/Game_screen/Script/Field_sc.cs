using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field_sc : MonoBehaviour
{
    public GameObject obj;

    public int status = Field_color.basic;
    public Color now_Field_color;

    public const string Mist_ally = "Syogi/Mark/Mist_ally";
    public const string Mist_opp = "Syogi/Mark/Mist_opp";
    ////public int[] mist_li = new int[] {0, 0}; //$e 0:霧無し、1:自分の霧、2:相手の霧
    public int mist_st = 0;
    public GameObject mist;
    // 霧種類、誰の霧か、霧の関連
    // 1, 2, 3  [1, 2]   []

    void Start() {
        // フィールドのデフォルトカラー
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 215, 160, 255);
    }

    void Update() {
        //$p 盤面の色の切替
        switch (status) {
            case Field_color.basic:
                now_Field_color = new Color32(255, 215, 160, 255); break;
            case Field_color.can_move:
                now_Field_color = new Color32(170, 255, 255, 255); break;
            case Field_color.attack_move:
                now_Field_color = new Color32(255, 100, 75, 255); break;
            case Field_color.noMove:
                now_Field_color = new Color32(100, 100, 100, 100); break;
        }
        this.GetComponent<SpriteRenderer>().color = now_Field_color;
            
        
            
        switch (mist_st) {
            case 0: {
                if (mist) {Destroy(mist);} break;
            }
            case 1: // 自分の霧
                if (!mist) {
                    float x = obj.transform.position.x;
                    float y = obj.transform.position.y;

                    mist = Instantiate((GameObject)Resources.Load(
                        Mist_ally), new Vector3(x, y, Layer.mist), 
                        Quaternion.identity) as GameObject;
                    f_k.Instance_after(mist, "Mist_parent");
                    mist.name += $"({x}, {y})";
                } break;
            case 2: // 相手の霧
                if (!mist) {
                    float x = obj.transform.position.x;
                    float y = obj.transform.position.y;

                    mist = Instantiate((GameObject)Resources.Load(
                        Mist_opp), new Vector3(x, y, Layer.mist), 
                        Quaternion.identity) as GameObject;
                    f_k.Instance_after(mist, "Mist_parent");
                    mist.name += $"({x}, {y})";
                } break;
        }

        //$t p_num持ってこれない問題
        //////$p 霧の表示
        //////$b 霧無し
        ////if (mist_li[0] == 0 && mist_li[1] == 0) {
        ////    if (mist) {Destroy(mist);}
        ////}
        ////else {
        ////    if (!mist) {
        ////        float x = obj.transform.position.x;
        ////        float y = obj.transform.position.y;
        ////        
        ////        mist = Instantiate((GameObject)Resources.Load(
        ////            ((mist_li[p_num] <= mist_li[p_num^1])) ? Mist_opp : Mist_ally), new Vector3(x, y, Layer.mark), //$b 相手の霧があるか
        ////            Quaternion.identity) as GameObject;
        ////    }
        ////}
        
    }
}
