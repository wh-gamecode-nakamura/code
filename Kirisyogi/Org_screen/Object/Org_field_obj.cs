using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Org_field_obj: FieldParent {
    //$p フィールドの作成
    public void Create_field_data()
    {
        Init_field_data();
                
        //$b 相手フィールドの暗がり
        for (int y = Coord.o; y <= Coord.y; y++) {
            for (int x = Coord.o; x <= Coord.x; x++) {
                //$b 自陣以外の座標
                if (((Coord.y - (Coord.y % 2)) / 2) < y) { //$t 要調整
                    field_data[y, x].status = Field_color.noMove;
                }
            }
        }
    }

    //$p 移動範囲の表示
    public override void disp_movable_pos(List<List<Chara_sc>> field_chara_li, Chara_sc now_obj_sc, int state_num, int hand_len) {
        //$b キャラを選択しているか
        if (now_obj_sc) {
            Vector3 now_obj_pos = now_obj_sc.obj.transform.position;

            Del_move_color();

            //$b 手札を選択したか
            if (now_obj_pos.y < Coord.o || Coord.y < now_obj_pos.y) {
                ////Disp_from_hand(field_chara_li, now_obj_sc, state_num);
                Disp_from_field(now_obj_sc, now_obj_pos, state_num, hand_len);
            }
            else {
                Disp_from_field(now_obj_sc, now_obj_pos, state_num, hand_len);
            }
        }
    }

    //$c 盤上から動かすときの座標表示
    public override void Disp_from_field(Chara_sc now_obj_sc, Vector3 now_obj_pos, int state_num, int hand_len) {
        for (int i = 0; i < now_obj_sc.pos_var_li.Count; i++) {
            int pos_x = (int)now_obj_pos.x + now_obj_sc.pos_var_li[i].x;
            int pos_y = (int)now_obj_pos.y + now_obj_sc.pos_var_li[i].y;

            //$b 盤外の除外
            if (pos_x < Coord.o || Coord.x < pos_x || pos_y < Coord.o || Coord.y < pos_y) {continue;}

            //$b 移動先選択時
            if (state_num != Field_color.attack_move) {
                //$b 自キャラの除外
                try {if (field_chara_data[pos_y, pos_x].p_num == now_obj_sc.p_num) {continue;}}
                catch (System.NullReferenceException) {;}
            } else {
                // 現在地
                pos_x = (int)now_obj_pos.x; pos_y = (int)now_obj_pos.y;
            }

            //$b 移動可能座標表示時にその座標に相手の霧がかかっているなら
            if (state_num == Field_color.can_move & 
            field_data[pos_y, pos_x].mist_st == Mist.opp_mist) {continue;}
            
            Create_pos_mark(pos_x, pos_y, state_num);

            if (state_num == Field_color.attack_move) {return;}
        }
    }

    //$p 移動先を表す色を元に戻す&灰色もある
    public override void Del_move_color() {
        foreach (Field_sc field in ch_field_li) {
            field.status = (((Coord.y - (Coord.y % 2)) / 2) < //$t 要調整
                field.transform.position.y) ? Field_color.noMove : Field_color.basic;
        }
        ch_field_li = new List<Field_sc>();
        foreach (var item in move_pos_li) {Destroy(item);}
        move_pos_li = new List<GameObject>();
    }
}