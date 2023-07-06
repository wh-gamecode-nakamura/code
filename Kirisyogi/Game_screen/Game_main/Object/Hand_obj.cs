using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hand_obj: MonoBehaviour
{
    public List<List<Chara_sc>> hand_li = new List<List<Chara_sc>>(){
        new List<Chara_sc>(){},
        new List<Chara_sc>(){},
    };

    //$p 手札の駒の更新
    public void hand_update(List<List<Chara_sc>> hand_li) {
        
        foreach (List<Chara_sc> hand_t in hand_li) {
            float num_i = 0f;
            foreach (Chara_sc hand_chara_sc in hand_t) {
                try {
                    Vector3 pos = hand_chara_sc.obj.transform.position;
                    if (hand_chara_sc.mine) {
                        pos.x = (float)Coord.o + num_i;
                        pos.y = (float)Coord.o - 1f;
                    }
                    else {
                        pos.x = (float)Coord.x - num_i;
                        pos.y = (float)Coord.y + 1f;
                    }
                    num_i += 1f;

                    hand_chara_sc.obj.transform.position = pos;
                    hand_chara_sc.pos = new int[]{(int)pos.x, (int)pos.y};
                    
                    ////Active_collider(hand_chara_sc.obj);
                } catch (System.NullReferenceException) {continue;}
            }
        }
    }
}