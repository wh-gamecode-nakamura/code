using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Org_init_obj : MonoBehaviour
{
    public Org_field_obj field;
    public Chara_obj chara;


    //$p init_liの反映
    public void Ref_init()
    {
        //$b キャラ毎
        for (int i = 0; i < Game_data.init_li.Length; i++) {
            int[] info_li = Game_data.init_li[i];
            Chara_sc chara_sc = f_k.Instance_chara(
                0, 0, info_li[0], new int[]{info_li[1], info_li[2]});
            chara.field_chara_li[0].Add(chara_sc);
            field.Move_chara(chara_sc);
        }
    }
}
