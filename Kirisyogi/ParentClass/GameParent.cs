using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

using UnityEngine.UI;
using Photon.Pun;

using TMPro; //TextMeshProを扱う際に必要


public class GameParent: MonoBehaviour
{

    //$p ホバー
    public GameObject hover_obj = null;
    public Chara_sc hover_chara_sc = null;
    public Field_sc hover_field_sc = null;
    
    //$p クリック
    public GameObject click_obj = null;
    public Chara_sc click_chara_sc = null;

    // クリック時の比較用
    public GameObject clicked_obj;
    public Vector3 clicked_mouse_pos;
    
    //{{
    //$p フォトン関連
    public Photon.Realtime.Player ally_p;
    public Photon.Realtime.Player opp_p;

    public Time_bar_manager time_bar_sc;

    public int turn;
    public int win_p_num = 2;
    private int game_phase_pass = 1;
    public int p_num;
    public int result_reason;
    //}}

    public bool isStartTurn = true;

    //$p System_obj
    public FieldParent fieldParent;
    public Hand_obj hand;
    public Chara_obj chara;


    //$c ゲーム全体の処理
    private void Update_game() {
        switch (game_phase_pass) {
            //$b 対戦中
            case Phase.while_game:
                //$b 相手の通信の切断時
                if (PhotonNetwork.PlayerList.Length == 1) { //$t 観戦にも対応できるようにする
                    Disconnect_opp_proc();
                }
                //$b このターンの最初なら
                if (isStartTurn) {
                    Start_turn_proc();
                }
                //$b 勝敗がついた場合
                if (win_p_num != 2) {
                    result_proc();
                }
                //$b 勝敗がついていない場合
                else {
                    Get_mouse_info();
                    
                    Hover_disp_movable_pos();
                    
                    Main_game();

                    fieldParent.Now_pos_update(hover_obj);
                }
                break;

            //$b 対戦終了
            case Phase.result_game:
                try {
                    if (result_reason == Because.disconnect || opp_p.recv_bool("check_result")) {
                        Invoke("InvokeDiscon", 0.5f);
                        //Network_manager.Discon();
                        game_phase_pass = Phase.end_game;
                    }
                } catch (System.NullReferenceException) {;}
                break;
        }
    }

    //!! override
    private void Start_turn_proc() {
        //$b 自分のターンなら
        if (p_num == turn) {time_bar_sc.count_pass = true;}

        UI.Edit_simple_info_text((turn == p_num) ? "自分のターン" : "相手のターン");
        time_bar_sc.slider.value = time_bar_sc.max_value;
        isStartTurn = false;
    }

    //!! クリックとホバーを関数化してoverride
    //$c マウスからオブジェクト情報取得
    private void Get_mouse_info() {
        //$p クリック
        GetClickInfo();

        //$p ホバー
        GetHoverObj();
    }

    private void GetClickInfo() {
        if (Input.GetMouseButtonDown(0)) {
            clicked_obj = Mouse.Get_down_obj();
            clicked_mouse_pos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) {
            click_obj = Mouse.Get_up_obj();
            if (clicked_mouse_pos != Input.mousePosition) {click_obj = null;}
        }
    }

    private void GetHoverObj() {
        hover_obj = Mouse.Get_hover_obj();
        if (hover_obj) {
            hover_chara_sc = (hover_obj.CompareTag("Chara")) ? hover_obj.GetComponent<Chara_sc>() : null;
            hover_field_sc = (hover_obj.CompareTag("FieldParent")) ? hover_obj.GetComponent<Field_sc>() : null;
        }
    }

    //$c ホバー時の移動先情報表示
    private void Hover_disp_movable_pos(int handCount = 0) {

        //$b 動かすキャラを選択している場合
        if (click_chara_sc) {return;}

        //$b キャラをホバーしていないなら
        if (!hover_chara_sc) {
            fieldParent.Del_move_color();
            return;
        }
        //$b キャラをホバーしているなら
        fieldParent.disp_movable_pos(chara.field_chara_li, hover_chara_sc, 
            Field_color.can_move, handCount);
        

        //$p キャラの詳細情報表示
        
        // 詳細情報のアクティブ化
        UI.test.info_window_text.SetActive (true);
        UI.test.info_window_image.SetActive (true);
        UI.test.info_window_promote_image.SetActive (false);

        // 詳細情報の代入
        UI.test.info_window_text.GetComponent<TextMeshProUGUI>().text = 
            hover_chara_sc.Set_chara_info_text();
        UI.test.info_window_image.GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(Chara_data.Collation_prefab(hover_chara_sc.chara_id));

        //$b 成れる駒なら
        if (hover_chara_sc.promote_id != 0) {
            Add_info_window(hover_chara_sc.promote_id);
        }
        //$b 成っている駒なら
        else if (hover_chara_sc.promote_state) {
            Add_info_window(hover_chara_sc.origin_id);
        }
    }

    //$repeat キャラの成り関連の情報の追加
    private void Add_info_window(int chara_id)
    {
        UI.test.info_window_promote_image.SetActive (true);
        UI.test.info_window_text.GetComponent<TextMeshProUGUI>().text += 
            $"<br><br><br>　駒名：　{Chara_data.Get_chara_name(chara_id)}";
        UI.test.info_window_promote_image.GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(Chara_data.Collation_prefab(chara_id));
    }

    //$aggregation(集合) ゲーム処理
    private void Main_game() {
        //$b 自分ターン
        if (p_num == turn) {
            //$b オブジェクトを選択していないなら
            if (!click_obj) {
                Not_click_obj_proc();
            }
            else {
                Choice_object();
            }
        }
        //$b 相手ターン
        else {
            //$t 関数化
            //$b 相手の情報を受け取ったら
            if (opp_p.recv_int_d1("move_chara_li") != null) {
                ////f.print_r(opp_p.recv_int_d1("move_chara_li"));
                Ref_recv(opp_p.recv_int_d1("move_chara_li"));
                opp_p.send_int_d1("move_chara_li", null);
            }
        }

        //$t 関数化
        //$b 相手から正規以外の勝敗が送られた場合
        switch (opp_p.recv_int("result_reason")) {
            case Because.none:
                return;
            case Because.surrender:
                result_reason = Because.surrender; break;
            case Because.time_out:
                result_reason = Because.time_out; break;
            default:
                print(opp_p.recv_int("result_reason")); break;
        }
        win_p_num = p_num;
        opp_p.send_int("result_reason", 0);
    }

    //$c 使用オブジェクト以外を選択した場合の処理
    private void Not_click_obj_proc() {
        click_chara_sc = null;
        //fieldParent.Del_move_color();
    }

    //$c オブジェクト選択時の処理
    private void Choice_object() {
        //$b 移動先を選択したなら
        if (click_obj.CompareTag("Mark")) {
            Choice_mark_proc();
        }
        //$b キャラを選択したなら
        else if (click_obj.CompareTag("Chara")) {
            Choice_chara_proc();
        }
        //$b 上記以外のオブジェクトを選択したなら
        else {
            Not_click_obj_proc();
        }
    }

    //$c 移動先選択時の処理
    private void Choice_mark_proc() {
        // 選択したマークの座標
        int[] dest = f.return_pos(click_obj); // destination 行き先

        int[] move_chara_li = new int[] { //** {1, 3, 4, 1} 座標は移動後の相手目線での座標
            click_chara_sc.chara_id, (Coord.x+1)-dest[0], (Coord.y+1)-dest[1], click_chara_sc.dup_id};

        //$b 手札から移動する場合
        if (!fieldParent.Is_in_field(f.return_pos(click_chara_sc.obj))) {
            click_chara_sc.Dup_check(chara.field_chara_li[click_chara_sc.p_num], click_chara_sc, false);
            hand.hand_li[click_chara_sc.p_num].Remove(click_chara_sc);
        }

        ally_p.send_int_d1("move_chara_li", move_chara_li);
        
        Move_piece(dest);

        // 移動１
        click_chara_sc.pos = new int[] {dest[0], dest[1]};
        fieldParent.Move_chara(click_chara_sc);

        End_turn();
    }

    //$c キャラ選択時の処理
    private void Choice_chara_proc() {
        click_chara_sc = click_obj.GetComponent<Chara_sc>();
        if (click_chara_sc.mine) {
            // 移動先表示
            fieldParent.disp_movable_pos(chara.field_chara_li, click_chara_sc, 
                Field_color.attack_move, hand.hand_li[p_num].Count);
        } else {
            click_chara_sc = null;
        }
    }

    //!! override
    //$p ターン終了時の処理
    private void End_turn()
    {
        //$p 表示更新
        hand.hand_update(hand.hand_li);

        fieldParent.Del_move_color();

        click_chara_sc = null;
        time_bar_sc.count_pass = false;
        isStartTurn = true;

        opp_p.send_int_d1("move_chara_li", null); // 受信データの削除
        turn = (turn+1)%2;
    }

    //$p 勝敗時の処理
    private void result_proc()
    {
        time_bar_sc.count_pass = false;

        UI.Disp_result();
        
        UI.Edit_text(UI.test.Result_text, 
            UI.Create_result_text(win_p_num == p_num, result_reason));

        game_phase_pass = Phase.result_game;
        
        // 自分の画面でゲームの勝敗が決したことを送信
        opp_p.send_bool("check_result", true);
    }

    //$p サレンダー処理
    public void Surrender_proc() {
        win_p_num = (p_num+1)%2;
        result_reason = Because.surrender;
        ally_p.send_int("result_reason", (int)result_reason);
        
    }

    //$p 時間切れ処理
    public void Time_out_proc() {
        win_p_num = (p_num+1)%2;
        result_reason = Because.time_out;
        ally_p.send_int("result_reason", (int)result_reason);
    }

    //$p 相手の通信切断時の処理
    public void Disconnect_opp_proc() {
        win_p_num = p_num;
        result_reason = Because.disconnect;
    }
    
    //$p ゲーム終了時の通信切断の遅延
    private void InvokeDiscon() {
        Network_manager.Discon();
    }

    //$p 相手から受け取った情報の反映
    public void Ref_recv(int[] move_chara_li) {
        //$p 受信したキャラの検索
        // chara_idとdup_idから盤上検索
        click_chara_sc = chara.field_chara_li[(p_num+1)%2].Find(
            chara_sc => chara_sc.chara_id == move_chara_li[0] & 
            chara_sc.dup_id == move_chara_li[3]);
        // chara_idから手札検索
        if (!click_chara_sc) {
            click_chara_sc = hand.hand_li[(p_num+1)%2].Find(
                chara_sc => chara_sc.chara_id == move_chara_li[0]);
        }

        //$p 手札から移動する場合
        if (!fieldParent.Is_in_field(click_chara_sc.pos)) {
            click_chara_sc.Dup_check(chara.field_chara_li[click_chara_sc.p_num], click_chara_sc, false);
            hand.hand_li[click_chara_sc.p_num].Remove(click_chara_sc);
        }
        // 移動１
        click_chara_sc.pos = new int[] {move_chara_li[1], move_chara_li[2]};
        Move_piece(click_chara_sc.pos);
        fieldParent.Move_chara(click_chara_sc);

        End_turn();
    }

    //$c 駒移動時の処理
    public void Move_piece(int[] dest) {
        //$b 移動先に駒があった場合
        if (fieldParent.field_chara_data[dest[1], dest[0]]) {
            Chara_sc delete_chara_sc = fieldParent.field_chara_data[dest[1], dest[0]];
            int hand_chara_id = (delete_chara_sc.promote_state) ? 
                delete_chara_sc.origin_id : delete_chara_sc.chara_id;

            //$p 手駒の生成
            Chara_sc chara_sc = f_k.Instance_chara(
                p_num, click_chara_sc.p_num, hand_chara_id, new int[] {0, 0});

            //$p 取られた駒の処理
            chara_sc.Dup_check(hand.hand_li[chara_sc.p_num], chara_sc, true);
            chara.field_chara_li[delete_chara_sc.p_num].Remove(delete_chara_sc);
            Destroy(delete_chara_sc.obj);

            //$b 王がとられた場合
            if (delete_chara_sc.is_king) {
                win_p_num = click_chara_sc.p_num;
                result_reason = Because.take_king;
            }
        }

        print(dest[1]);
        //$b プレイヤーごとの成り座標に移動した場合
        if (((dest[1] > Coord.y - Coord.mistRow) && (click_chara_sc.mine == true)) ||
        ((dest[1] < Coord.o + Coord.mistRow) && (click_chara_sc.mine == false))) {
            //$p 霧の処理
            fieldParent.field_data[dest[1], dest[0]].mist_st = 0;

            //$b その駒が成っていない場合
            if (!click_chara_sc.promote_state) {
                //$b 成る駒の場合
                if (click_chara_sc.promote_id != 0) {
                    //$p 駒が成る処理
                    click_chara_sc = Consists_piece();;
                }
            }
        }
        //$b プレイヤー毎の相手の端に移動した場合
        if (((dest[1] == Coord.y) && (click_chara_sc.mine == true)) ||
        ((dest[1] == Coord.o) && (click_chara_sc.mine == false))) {
            //$b 王が移動した場合
            if (click_chara_sc.is_king) {
                win_p_num = click_chara_sc.p_num;
                result_reason = Because.arrival_king;
            }
        }
    }

    //$c 駒が成る処理
    private Chara_sc Consists_piece() {
        // キャラ情報の設定
        Chara_sc consists_chara_sc = f_k.Instance_chara(
            p_num, click_chara_sc.p_num, click_chara_sc.promote_id, click_chara_sc.pos);
        consists_chara_sc.obj.transform.position = click_chara_sc.obj.transform.position;
        consists_chara_sc.origin_id = click_chara_sc.chara_id;
        consists_chara_sc.cost = click_chara_sc.cost;
        consists_chara_sc.promote_state = true;
        
        // 
        chara.field_chara_li[click_chara_sc.p_num].Remove(click_chara_sc);
        chara.field_chara_li[click_chara_sc.p_num].Add(consists_chara_sc);
        Destroy(click_chara_sc.obj);

        return consists_chara_sc;
    }
}
