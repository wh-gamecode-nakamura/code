using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldParent: MonoBehaviour {
    [SerializeField]
    //$p ゲームオブジェクト定数
    public const string Collision_mark = "Syogi/Mark/Collision_mark";
    public const string Now_chara_mark = "Syogi/Mark/Now_chara_mark";
    public const string Now_field_mark = "Syogi/Mark/Now_field_mark";

    public List<Field_sc> ch_field_li = new List<Field_sc>();
    public List<GameObject> move_pos_li = new List<GameObject>();
    
    public Field_sc[, ] field_data = new Field_sc[Coord.y + 1, Coord.x + 1];
    public Chara_sc[, ] field_chara_data = new Chara_sc[Coord.y + 1, Coord.x + 1];

    public GameObject now_pos_mark;



    //$p 
    public void Init_field_data() {
        GameObject field_obj;
        for (int y = Coord.o; y <= Coord.y; y++) {
            for (int x = Coord.o; x <= Coord.x; x++) {
                field_obj = f_k.Instance_field(new int[] {x, y});
                field_obj.name += $"({x}, {y})";
                field_data[y, x] = field_obj.transform.GetChild(0).GetComponent<Field_sc>();
                field_data[y, x].obj = field_obj;
                
            }
        }
        Init_field_mist();
    }

    //$c 最初の霧の情報の代入
    public void Init_field_mist() {
        for (int y = 1; y <= Coord.mistRow; y++) {
            // 霧の生成
            for (int x = 1; x <= Coord.x; x++) {
                field_data[y, x].mist_st = Mist.ally_mist;
                field_data[Coord.y - (y-1), x].mist_st = Mist.opp_mist;
            }
        }
    }

    //$p 移動先を表す色を元に戻す
    public virtual void Del_move_color() {
        foreach (Field_sc field in ch_field_li) {field.status = 1;}
        ch_field_li = new List<Field_sc>();
        foreach (var item in move_pos_li) {Destroy(item);}
        move_pos_li = new List<GameObject>();
    }

    //$p 移動範囲の表示
    public virtual void disp_movable_pos(List<List<Chara_sc>> field_chara_li, Chara_sc now_obj_sc, int state_num, int hand_len) {
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

    //$c 手札から動かすときの座標表示
    public void Disp_from_hand(List<List<Chara_sc>> field_chara_li, Chara_sc now_obj_sc, int state_num) {
        for (int y = Coord.o; y <= Coord.y; y++) {
            for (int x = Coord.o; x <= Coord.x; x++) {

                //$b キャラのいる座標の除外
                try {if (field_chara_data[y, x]/*p_num <= 1*/) {continue;}} //$d 無意味処理
                catch (System.NullReferenceException) {/*座標除外1*/;}

                //$b 歩兵と同じ座標の除外
                //if (x == Exc_pos(field_chara_li, now_obj_sc)) {/*座標除外2*/; continue;}
                
                Create_pos_mark(x, y, state_num);
            }
        }
    }

    //$c 駒特有の行けない位置を除外
    public int Exc_pos(List<List<Chara_sc>> field_chara_li, Chara_sc now_obj_sc) {
        int rm_pos = 0;

        //$b 歩兵なら
        if (now_obj_sc.chara_id == 1) {
            try {
                rm_pos = field_chara_li[now_obj_sc.p_num].Find(
                    chara_sc => chara_sc.p_num == now_obj_sc.p_num & 
                    chara_sc.chara_id == now_obj_sc.chara_id).pos[0];
            }
            catch (System.NullReferenceException) {;}
        }
        return rm_pos;
    }

    //$c 盤上から動かすときの座標表示
    public virtual void Disp_from_field(Chara_sc now_obj_sc, Vector3 now_obj_pos, int state_num, int hand_len) {
        for (int i = 0; i < now_obj_sc.pos_var_li.Count; i++) {
            int pos_x = (int)now_obj_pos.x + now_obj_sc.pos_var_li[i].x;
            int pos_y = (int)now_obj_pos.y + now_obj_sc.pos_var_li[i].y;

            //$b 盤外の除外
            if (pos_x < Coord.o || Coord.x < pos_x || pos_y < Coord.o || Coord.y < pos_y) {continue;}

            //$b 自キャラの除外
            try {if (field_chara_data[pos_y, pos_x].p_num == now_obj_sc.p_num) {continue;}}
            catch (System.NullReferenceException) {;}

            //$b 手札に二体取っている場合
            if (hand_len == 2) {
                try {if (field_chara_data[pos_y, pos_x].p_num != now_obj_sc.p_num) {continue;}}
                catch (System.NullReferenceException) {;}
            }

            //$b 移動可能座標表示時にその座標に相手の霧がかかっているなら
            if (state_num == Field_color.can_move & 
            field_data[pos_y, pos_x].mist_st == Mist.opp_mist) {continue;}
            
            Create_pos_mark(pos_x, pos_y, state_num);
        }
    }

    //$c 座標の表示
    public void Create_pos_mark(int x, int y, int state_num) {
        // 移動可能座標の色変え
        ch_field_li.Add(field_data[y, x]);
        field_data[y, x].status = state_num;

        // 移動可能座標の当たり判定
        move_pos_li.Add(Instantiate((GameObject)Resources.Load(Collision_mark), new Vector3(
            (float)x, (float)y, Layer.mark), Quaternion.identity) as GameObject);
    }

    //$p プレイヤーの現在地の更新
    public void Now_pos_update(GameObject hover_obj) {
        //$b 前の表示があるなら削除
        if (now_pos_mark) {Destroy(now_pos_mark); now_pos_mark = null;}
        //$b オブジェクトをホバーしているなら
        if (hover_obj) {
            now_pos_mark = Instantiate((GameObject)Resources.Load(
                (hover_obj.CompareTag("Chara")) ?  Now_chara_mark : Now_field_mark), 
            new Vector3(hover_obj.transform.position.x, 
                hover_obj.transform.position.y, 
                Layer.mark), Quaternion.identity) as GameObject;
        }
    }


    //$t
    //$p キャラの移動
    public void Move_chara(Chara_sc now_chara_sc) {
        Vector3 pos = now_chara_sc.obj.transform.position; // 現在地
        //print("現在地"); print(pos.x); print(pos.y);
        //print("移動先"); print(now_chara_sc.pos[0]); print(now_chara_sc.pos[1]);
        if (Is_in_field(new int[] {(int)pos.x, (int)pos.y})) {field_chara_data[(int)pos.y, (int)pos.x] = null;}
        field_chara_data[now_chara_sc.pos[1], now_chara_sc.pos[0]] = now_chara_sc;
        pos.x = (float)now_chara_sc.pos[0];
        pos.y = (float)now_chara_sc.pos[1];
        now_chara_sc.obj.transform.position = pos;
        
    }

    //$p その座標が盤面内か
    public bool Is_in_field(int[] pos) {
        //** (1 <= x <= 3) & (1 <= y <= 4)
        return (((Coord.o <= pos[0]) && (pos[0] <= Coord.x)) &&
            ((Coord.o <= pos[1]) && (pos[1] <= Coord.y))) ? true : false;
    }
}