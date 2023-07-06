using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chara_data : MonoBehaviour
{
    public static string[] chara_prefab_li = new string[] {"", 
            "hohei", "juuji", "syaji", "ousyou", "ginnsyou",
            "kinnsyou", "uhei", "sahei", "taisyou", "jakuhei",
            "boukunn", "jakuou",
        };

    public static string[] promote_chara_prefab_li = new string[] {"", 
            "tokin", "ginnnari", "ryouhei", "seija",
        };


    //$p 表示するpieceの照合
    public static string Collation_prefab(int chara_id) {
        string chara_prefab = $"Syogi/Picture/Piece/Piece_";
        
        if (chara_id < 100) {return chara_prefab + chara_prefab_li[chara_id];}
        else {return chara_prefab + promote_chara_prefab_li[chara_id-100];}
    }

    public static string Get_chara_name(int chara_id) {
        string chara_name = "";
        switch (chara_id)
        {
            case  1: chara_name = "歩兵"; break;
            case  2: chara_name = "十字"; break;
            case  3: chara_name = "斜字"; break;
            case  4: chara_name = "王将"; break;
            case  5: chara_name = "銀将"; break;
            case  6: chara_name = "金将"; break;
            case  7: chara_name = "右兵"; break;
            case  8: chara_name = "左兵"; break;
            case  9: chara_name = "大将"; break;
            case 10: chara_name = "弱兵"; break;
            case 11: chara_name = "暴君"; break;
            case 12: chara_name = "弱王"; break;
            //case : chara_name = ""; break;
            
            case 101: chara_name = "と金"; break;
            case 102: chara_name = "銀成"; break;
            case 103: chara_name = "両兵"; break;
            case 104: chara_name = "聖者"; break;
            //case : chara_name = ""; break;
        }
        return chara_name;
    }

    public static void chara_info_set(Chara_sc chara_sc, bool mine){
        //?? 何故chara_idを引数としないか：chara_scに情報を代入するため

        chara_sc.promote_id = 0;
        chara_sc.cost = 3;
        //"""
        //923 2   2  1.5   5.5
        //8 4 1      1.5   2.5
        //765 0.5 1  1     2.5
        //923 0.5 0.5 0.5  1.5
        //8 4 1       1    2
        //765 1.5 1.5 1.5  4.5
        //"""


        switch (chara_sc.chara_id)
        {
            case 1:
                chara_sc.chara_name = "歩兵";
                chara_sc.pos_var_li = place_range(mine, null, 2);
                chara_sc.cost += 3.5f; // 6.5   2 + ((5-2)/2)
                chara_sc.promote_id = 101;
                break;
            case 2:
                chara_sc.chara_name = "十字";
                chara_sc.pos_var_li = place_range(mine, null, 0);
                chara_sc.cost += 5.5f; // 8.5
                break;
            case 3:
                chara_sc.chara_name = "斜字";
                chara_sc.pos_var_li = place_range(mine, null, 1);
                chara_sc.cost += 5; // 8
                break;
            case 4:
                chara_sc.chara_name = "王将";
                chara_sc.pos_var_li = place_range(mine, null, 0, 1);
                chara_sc.cost += 12.5f; // 15.5   2 + 10.5f
                chara_sc.is_king = true;
                break;
            case 5:
                chara_sc.chara_name = "銀将";
                chara_sc.pos_var_li = place_range(mine, null, 1, 2);
                chara_sc.cost += 7; // 10
                chara_sc.promote_id = 102;
                break;
            case 6:
                chara_sc.chara_name = "金将";
                chara_sc.pos_var_li = place_range(mine, null, 0, 3, 9);
                chara_sc.cost += 9; // 12
                break;
            case 7:
                chara_sc.chara_name = "右兵";
                chara_sc.pos_var_li = place_range(mine, null, 2, 3, 4, 5);
                chara_sc.cost += 6; // 9   6 + ((6-6)/2)  (6.5f)
                chara_sc.promote_id = 103;
                break;
            case 8:
                chara_sc.chara_name = "左兵";
                chara_sc.pos_var_li = place_range(mine, null, 2, 7, 8, 9);
                chara_sc.cost += 6; // 9   5.5f + ((6.5f-5.5f)/2)
                chara_sc.promote_id = 103;
                break;
            case 9:
                chara_sc.chara_name = "大将";
                chara_sc.pos_var_li = place_range(mine, null, 0, 1);
                chara_sc.cost += 10.5f; // 13.5
                break;
            case 10:
                chara_sc.chara_name = "弱兵";
                chara_sc.pos_var_li = place_range(mine, null, 2);
                chara_sc.cost += 2; // 5
                break;
            case 11:
                chara_sc.chara_name = "暴君";
                chara_sc.pos_var_li = place_range(mine, null, 0, 3, 9, -6);
                chara_sc.cost += 5; // 8   8 + ((2-8)/2)  (1.5f)
                chara_sc.promote_id = 104;
                break;
            case 12:
                chara_sc.chara_name = "弱王";
                chara_sc.pos_var_li = place_range(mine, null, 2, 5, 7);
                chara_sc.cost += 5.5f; // 8.5   2 + 3.5f;
                chara_sc.is_king = true;
                break;


            case 101:
                chara_sc.chara_name = "と金";
                chara_sc.pos_var_li = place_range(mine, null, 0, 3, 9);
                break;
            case 102:
                chara_sc.chara_name = "銀成";
                chara_sc.pos_var_li = place_range(mine, null, 0, 3, 9);
                break;
            case 103:
                chara_sc.chara_name = "両兵";
                chara_sc.pos_var_li = place_range(mine, null, 0, 1, -6);
                break;
            case 104:
                chara_sc.chara_name = "聖者";
                chara_sc.pos_var_li = place_range(mine, null, 6);
                break;
        }
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


