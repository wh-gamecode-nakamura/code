using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara_sc : MonoBehaviour
{
    // プロパティの値の変更は、プレハブには反映されない
    public int chara_id;
    public int p_num;
    public int[] pos = new int[1];
    public GameObject obj;
    public bool mine;

    public string chara_name;
    public List<(int x, int y)> pos_var_li;
    public int promote_id;
    public bool promote_state = false;
    public int origin_id;
    public bool is_king = false;
    public float cost;

    public int dup_id = 1; // 同じIDのキャラを識別するためのID


    //$p キャラの初期情報の設定
    public void Set_chara_info(Chara_sc chara_sc) {
        Chara_data.chara_info_set(chara_sc, mine);
        //GameObject Text = transform.Find("Canvas/Text").gameObject;
        //Text.GetComponent<UnityEngine.UI.Text>().text = $"{chara_name}";
        obj.name = chara_name;
    }

    public string Set_chara_info_text() {
        string text = "";
        text += $"　駒名：　{chara_name}";
        text += "<br>　種類：　";
        text += (is_king == true) ? "王" : "通常";
        text += $"<br>コスト：　{cost}";
        text += "<br>成るか：　";
        if (promote_state) {text += "既に成り<br><br>";}
        else if (promote_id == 0) {text += "成らない<br><br>";}
        else {
            text += "成る<br>";
            text += $"成り先：　{Chara_data.Get_chara_name(promote_id)}";
        }
        return text;
    }

    //$p dup_idの更新
    public void Dup_check(List<Chara_sc> hand_or_chara_li, Chara_sc now_chara_sc, bool is_add_hand) {
        //$b 手札に加える場合
        if (is_add_hand) {now_chara_sc.dup_id = 0;}
        else {
            //$b 手札から盤上に出すときに既に同じIDのキャラがいるとき
            List<Chara_sc> dup_chara_sc_li = hand_or_chara_li.FindAll(
                chara_sc => chara_sc.chara_id == now_chara_sc.chara_id);
            int max_dup_id = 1;
            foreach (Chara_sc chara_sc_t in dup_chara_sc_li) {
                if (max_dup_id <= chara_sc_t.dup_id) {max_dup_id = chara_sc_t.dup_id + 1;}
            }
            now_chara_sc.dup_id = max_dup_id;
        }
        hand_or_chara_li.Add(now_chara_sc);
    }

    public static List<(int x, int y)> place_range(bool mine, int? sp_pos, params int[] dir_list)
    {
        List<(int x, int y)> move_list = new List<(int, int)>{};
        int p = (mine) ? 1 : -1;
        switch (sp_pos)
        {
            case 0: move_list.Add((0, 0)); break; // 現在地
        }
        foreach (int direction in dir_list)
        {
            switch (direction)
            {
                case 0: // 十字
                    move_list.Add((0, 1)); move_list.Add((1, 0)); 
                    move_list.Add((0, -1)); move_list.Add((-1, 0)); break;
                case 1: // 斜め
                    move_list.Add((1, 1)); move_list.Add((1, -1));
                    move_list.Add((-1, -1)); move_list.Add((-1, 1)); break;
                case 2: move_list.Add((0, 1*p)); break;   // 北
                case 3: move_list.Add((1*p, 1*p)); break;   // 北東
                case 4: move_list.Add((1*p, 0)); break;   // 東
                case 5: move_list.Add((1*p, -1*p)); break;  // 南東
                case 6: move_list.Add((0, -1*p)); break;  // 南
                case 7: move_list.Add((-1*p, -1*p)); break; // 南西
                case 8: move_list.Add((-1*p, 0)); break;  // 西
                case 9: move_list.Add((-1*p, 1*p)); break;  // 北西
                
                case -2: move_list.Remove((0, 1*p)); break;   // 北
                case -3: move_list.Remove((1*p, 1*p)); break;   // 北東
                case -4: move_list.Remove((1*p, 0)); break;   // 東
                case -5: move_list.Remove((1*p, -1*p)); break;  // 南東
                case -6: move_list.Remove((0, -1*p)); break;  // 南
                case -7: move_list.Remove((-1*p, -1*p)); break; // 南西
                case -8: move_list.Remove((-1*p, 0)); break;  // 西
                case -9: move_list.Remove((-1*p, 1*p)); break;  // 北西
            }
        }
        return move_list;
    }
}
