using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Init_obj: MonoBehaviour
{
    //$p フォトン関連
    public Photon.Realtime.Player ally_p;
    public Photon.Realtime.Player opp_p;

    public int p_num;
    public int turn = 1;

    public Field_obj field;
    public Chara_obj chara;



    //$p 
    public void Start_game()
    {
        field.Init_field_data();
        UI.Edit_simple_info_text("通信待機中・・・");
    }



    //$p プレイヤー別の初期化
    public void Create_init() {
        ally_p = PhotonNetwork.LocalPlayer;
        //for (int i = 0; i < 10; i++)
        //{
        //    random_int = Random.Range (1, 255);
        //    if (ally_p.recv_int("test") == 0) {break;}
        //    if (ally_p.recv_int("test") != random_int) {break;}
        //}
        ally_p.InitHashtable();
        ////p_num = (PhotonNetwork.PlayerList[0] == ally_p) ? 0 : 1;
        if (PhotonNetwork.PlayerList[0] == ally_p) {p_num = 0;}
        else if (PhotonNetwork.PlayerList[1] == ally_p) {p_num = 1;}


        // 通信データ初期化処理
        //for (int i = 0; i < 100; i++)
        //{
        //    if (i == 99) {print("初期化不可能");}
        //    //if (ally_p.recv_int("test") != random_int) {print("初期化されていない"); print(ally_p.recv_int("test")); return 2;}
        //    if (ally_p.recv_int("test") != 0) {print("初期化されていない"); continue;}
        //    print(ally_p.recv_int("test"));
        //    ally_p.send_int("test", 1);
        //    break;
        //}


        ally_p.send_int_d2("init_li", Game_data.init_li);

        Ref_init(Game_data.init_li, p_num);
    }


    //$p init_liの受信
    public int Recv_init()
    {
        opp_p = PhotonNetwork.PlayerList[(p_num+1)%2];

        if (ally_p.recv_int_d2("init_li") != null && opp_p.recv_int_d2("init_li") != null) {
            int[][][] all_init_li = new int[][][] 
            {
                (p_num == 0) ? ally_p.recv_int_d2("init_li"): opp_p.recv_int_d2("init_li"),
                (p_num == 0) ? opp_p.recv_int_d2("init_li"): ally_p.recv_int_d2("init_li")
            };
            Ref_init(opp_p.recv_int_d2("init_li"), (p_num+1)%2);
        } else {return 2;}
        return 3;
    }

    //$c init_liの反映
    public void Ref_init(int[][] all_init_li, int now_p_num)
    {
        int[] info_li;

        //$p お互いのキャラの配置
        ////$b プレイヤー毎
        //for (int now_p_num = 0; now_p_num < all_init_li.Length; now_p_num++) {
        //$b キャラ毎
        for (int i = 0; i < all_init_li.Length; i++) {
            info_li = all_init_li[i];
            
            //$b プレイヤー別にフィールドのどちら側に置くかを判定
            int[] pos_li = (p_num == now_p_num) ? 
                new int[]{info_li[1], info_li[2]}: 
                new int[]{(Coord.x+1)-info_li[1], (Coord.y+1)-info_li[2]};
            
            Chara_sc chara_sc = f_k.Instance_chara(
                p_num, now_p_num, info_li[0], pos_li);

            chara_sc.Dup_check(chara.field_chara_li[now_p_num], chara_sc, false);
            //chara.field_chara_li[now_p_num].Add(chara_sc);

            field.Move_chara(chara_sc);
        }
    }
}
