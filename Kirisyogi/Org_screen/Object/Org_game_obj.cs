using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;


public class Org_game_obj : GameParent
{
    public bool add_chara_flag = false;
    public Chara_sc add_chara_sc = null;

    public int max_cost = 50;
    public bool cost_flag = false;
    public bool is_king_flag = false;

    // 追加ボタン押下時にリストが消えることを防ぐ変数
    public bool add_button_flag = false;

    public bool UI_click_flag = false;

    public Org_field_obj field;

    //$c ゲーム全体の処理
    public void Update_game() {
        //$b このターンの最初なら
        if (isStartTurn) {
            Org_UI.test.cost_text.GetComponent<TextMeshProUGUI>().text = Create_cost_text();
            Org_UI.test.Battle_transition_button.interactable = (cost_flag && is_king_flag) ? true : false;
            isStartTurn = false;
        }
        
        Get_mouse_info();
        
        Hover_disp_movable_pos();
        
        Myturn_proc();

        if (!add_chara_flag) {
            field.Now_pos_update(hover_obj);
            Org_UI.test.buttons.SetActive(true);
        }
        else {
            field.Now_pos_update(null);
            Org_UI.test.buttons.SetActive(false);
        }
        
    }

    //$c マウスからオブジェクト情報取得
    private void Get_mouse_info() {
        //$p クリック
        if (!UI_click_flag) {
            click_obj = Mouse.Get_up_obj();
            //$b 追加ボタン押下時
            if (add_button_flag) {
                // 追加ボタンを押した際にclick_objがnullになり、リストが消えてしまうので、
                // 一旦適当なオブジェクトを入れて消えないようにしている
                click_obj = gameObject;
                add_button_flag = false;
            }
        } UI_click_flag = false;

        //$p ホバー
        if (!add_chara_flag) {
            hover_obj = Mouse.Get_hover_obj();
            if (hover_obj) {
                hover_chara_sc = (hover_obj.CompareTag("Chara")) ? hover_obj.GetComponent<Chara_sc>() : null;
                hover_field_sc = (hover_obj.CompareTag("Field")) ? hover_obj.GetComponent<Field_sc>() : null;
            }
        }
    }


    //$c ホバー時の移動先情報表示
    private void Hover_disp_movable_pos() {
        //$b まだ動かすキャラを選択していない場合
        if (!click_chara_sc) {
            //$b キャラをホバーしていないなら
            if (!hover_chara_sc) {
                field.Del_move_color();
            }
            else {
                if (!add_chara_flag) {
                    field.disp_movable_pos(chara.field_chara_li, hover_chara_sc, 
                        Field_color.can_move, 0);
                }
                // info_window
                Org_UI.test.info_window_text.SetActive(true);
                Org_UI.test.info_window_image.SetActive(true);
                Org_UI.test.info_window_promote_image.SetActive(true);

                Org_UI.test.info_window_text.GetComponent<TextMeshProUGUI>().text = 
                    hover_chara_sc.Set_chara_info_text();
                Org_UI.test.info_window_image.GetComponent<Image>().sprite = 
                    Resources.Load<Sprite>(Chara_data.Collation_prefab(hover_chara_sc.chara_id));
                //$p 成れる駒なら
                //$b 成る駒かつ、まだ成っていないなら
                if ((hover_chara_sc.promote_id != 0) && !hover_chara_sc.promote_state) {
                    Org_UI.test.info_window_text.GetComponent<TextMeshProUGUI>().text += 
                        $"<br><br><br><br><br>　駒名：　{Chara_data.Get_chara_name(hover_chara_sc.promote_id)}";
                    Org_UI.test.info_window_promote_image.GetComponent<Image>().sprite = 
                        Resources.Load<Sprite>(Chara_data.Collation_prefab(hover_chara_sc.promote_id));
                }
                else {
                    Org_UI.test.info_window_promote_image.SetActive(false);
                }
            }
        }
    }

    //$c 自分ターンの処理
    private void Myturn_proc() {
        //$b クリック時
        if (Input.GetMouseButtonUp(0)) {
            //$b オブジェクトを選択していないなら
            if (!click_obj) {
                click_chara_sc = null;
                add_chara_sc = null;
                field.Del_move_color();

                End_add_chara();
            }
            else {
                Choice_object();
            }
        }
    }

    //$c オブジェクト選択時の処理
    private void Choice_object() {
        //$b キャラを選択したなら
        if (click_obj.CompareTag("Chara")) {
            Choice_chara_proc();

            if (add_chara_flag) {
                add_chara_sc = click_obj.GetComponent<Chara_sc>();
            }
            End_add_chara();
        }
        //$b 盤面を選択したなら
        else if (click_obj.CompareTag("Field")) {
            Choice_field_proc();
        }
        //$b 上記以外のオブジェクトを選択したなら
        else {
            click_chara_sc = null;
            add_chara_sc = null;
            field.Del_move_color();
            //End_add_chara();
        }
    }

    //$c キャラ選択時の処理
    private void Choice_chara_proc() {
        //$b 既に追加キャラを選択していた場合
        if (add_chara_sc) {
            Chara_sc after_chara_sc = click_obj.GetComponent<Chara_sc>();
            int[] pos_t = new int[] {after_chara_sc.pos[0], after_chara_sc.pos[1]};
            Delete_chara(after_chara_sc);

            Chara_sc add_chara_sc_t = f_k.Instance_chara(0, 0, add_chara_sc.chara_id, pos_t);
            field.field_chara_data[pos_t[1], pos_t[0]] = add_chara_sc_t;
            chara.field_chara_li[0].Add(add_chara_sc_t);
            field.Move_chara(add_chara_sc_t);
            End_move();
        }
        //$b 既に盤面のキャラを選択していた場合
        else if (click_chara_sc) {
            Chara_sc after_chara_sc = click_obj.GetComponent<Chara_sc>();
            int[] pos_t = new int[] {after_chara_sc.pos[0], after_chara_sc.pos[1]};
            after_chara_sc.pos = click_chara_sc.pos;
            click_chara_sc.pos = pos_t;
            field.Move_chara(click_chara_sc);
            field.Move_chara(after_chara_sc);
            End_move();
        }
        //$b まだキャラを選択していなかった場合
        else {
            click_chara_sc = click_obj.GetComponent<Chara_sc>();
            // 移動先表示
            if (!add_chara_flag) {
                field.disp_movable_pos(chara.field_chara_li, click_chara_sc, 
                    Field_color.attack_move, 0);
            }
        }
    }

    //$c 盤面選択時の処理
    private void Choice_field_proc() {
        //$b 選択画面のキャラを選択している状態なら
        if (add_chara_sc) {
            int[] pos_t = new int[] {(int)click_obj.transform.position.x, (int)click_obj.transform.position.y};
            //$b 自陣なら
            if ((int)click_obj.transform.position.y <= ((Coord.y - (Coord.y % 2)) / 2)) {
                Chara_sc add_chara_sc_t = f_k.Instance_chara(0, 0, add_chara_sc.chara_id, pos_t);
                field.field_chara_data[pos_t[1], pos_t[0]] = add_chara_sc_t;
                chara.field_chara_li[0].Add(add_chara_sc_t);
                field.Move_chara(add_chara_sc_t);
            }
        }
        //$b 盤面のキャラを選択している状態なら
        else if (click_chara_sc) {
            int[] pos_t = new int[] {(int)click_obj.transform.position.x, (int)click_obj.transform.position.y};
            //$b 自陣なら
            if ((int)click_obj.transform.position.y <= ((Coord.y - (Coord.y % 2)) / 2)) {
                click_chara_sc.pos = pos_t;
                field.Move_chara(click_chara_sc);
            }
        }
        //$b まだキャラを選択していなかった場合
        else {
            //
        }
        End_move();
    }

    //$p ターン終了時の処理
    private void End_move()
    {
        click_chara_sc = null;
        add_chara_sc = null;

        isStartTurn = true;

        // 移動先表示を削除する
        field.Del_move_color();

        Save();
    }

    private string Create_cost_text() {
        float cost = 0;
        bool is_king = false;
        string cost_text = $"コスト：";

        foreach (Chara_sc chara_sc_t in chara.field_chara_li[0])
        {
            cost += chara_sc_t.cost;
            if (chara_sc_t.is_king) {is_king = true;}
        }
        cost_flag = (cost <= max_cost) ? true : false;
        is_king_flag = (is_king) ? true : false;
        if (cost_flag) {cost_text += $"<color=blue>{cost}</color>/{max_cost} 王：";}
        else {cost_text += $"<color=red>{cost}</color>/{max_cost} 王：";}
        
        cost_text += (is_king) ? "<color=blue>有</color>" : "<color=red>無</color>";
        return cost_text;
    }

    public void End_add_chara() {
        Org_UI.test.chara_scroll_list.SetActive (false);
        add_chara_flag = false;
    }

    public void Save() {
        List<int[]> init_li = new List<int[]> {};
        foreach (Chara_sc chara_t in chara.field_chara_li[0]) {
            init_li.Add(new int[] {chara_t.chara_id, chara_t.pos[0], chara_t.pos[1]});
        }
        Game_data.init_li = init_li.ToArray();
        SaveManager.Save();
    }

    public void Delete_chara(Chara_sc chara_sc_t = null) {
        if (!click_chara_sc) {return;}
        if (!chara_sc_t) {chara_sc_t = click_chara_sc;}
        Destroy(chara_sc_t.obj);
        chara.field_chara_li[0].Remove(chara_sc_t);
        field.field_chara_data[chara_sc_t.pos[1], chara_sc_t.pos[0]] = null;
        isStartTurn = true;
        Save();
    }

    public void Disp_add_chara_list() {
        add_chara_flag = true;
        add_button_flag = true;
    }
}
