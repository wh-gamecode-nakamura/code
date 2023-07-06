using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//$p 霧将棋の便利関数置き場
public class f_k : MonoBehaviour
{
    //$p キャラのインスタンス作成
    public static Chara_sc Instance_chara(int p_num, int chara_p_num, int chara_id, int[] pos_li)
    {
        GameObject chara_obj = Instantiate((GameObject)Resources.Load(
            "Syogi/Def_piece"), new Vector3(0, 0, Layer.piece), 
            Quaternion.identity) as GameObject;
        
        // 生成場所指定
        chara_obj.transform.parent = GameObject.Find(
            "Piece_parent/" + ((p_num == chara_p_num) ? "Ally_parent": "Opp_parent")).transform;
        

        return Set_chara_info(chara_obj, p_num, chara_p_num, chara_id, pos_li);
    }

    //$p フィールドのインスタンス作成
    public static GameObject Instance_field(int[] coord)
    {
        GameObject field_obj = Instantiate((GameObject)Resources.Load(
            "Syogi/Field/Fields"), new Vector3(coord[0], coord[1], Layer.field), 
            Quaternion.identity) as GameObject;
        Instance_after(field_obj, "Field_parent");

        return field_obj;
    }

    //$p Instantiate後の後処理
    public static void Instance_after(GameObject game_object, string parent_name) {
        game_object.transform.parent = GameObject.Find(parent_name).transform;
        game_object.name = game_object.name.Replace( "(Clone)", "" );
    }


    //$p キャラ情報の設定
    public static Chara_sc Set_chara_info(GameObject chara_obj, int p_num, int chara_p_num, int chara_id, int[] pos_li)
    {
        Chara_sc chara_sc = chara_obj.GetComponent<Chara_sc>();
        chara_sc.obj = chara_obj;

        chara_obj.GetComponent<SpriteRenderer>().sprite = 
            Resources.Load<Sprite>(Chara_data.Collation_prefab(chara_id));

        chara_sc.p_num = chara_p_num;
        chara_sc.mine = (p_num == chara_p_num);
        if (!chara_sc.mine) {
            chara_obj.GetComponent<SpriteRenderer>().flipX = true;
            chara_obj.GetComponent<SpriteRenderer>().flipY = true;
        }

        chara_sc.chara_id = chara_id;
        chara_sc.pos = new int[] {pos_li[0], pos_li[1]};
        chara_sc.Set_chara_info(chara_sc);

        return chara_sc;
    }
}
