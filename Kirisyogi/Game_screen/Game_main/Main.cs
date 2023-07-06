using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Photon.Pun;

public class Main : MonoBehaviourPunCallbacks
{

    public static bool start_pass = false;
    public int init_pass = 2;

    public static Init_obj init;
    public static Game_obj game;
    public static Chara_obj chara;
    public static Field_obj field;
    public static Hand_obj hand;

    
    
    void Start() {
        SaveManager.Load();
        SaveManager.LoadOrg();
        init = f.Instant_obj("System_obj/Init_obj", 0).GetComponent<Init_obj>();
        game = f.Instant_obj("System_obj/Game_obj", 0).GetComponent<Game_obj>();
        chara = f.Instant_obj("System_obj/Chara_obj", 0).GetComponent<Chara_obj>();
        field = f.Instant_obj("System_obj/Field_obj", 0).GetComponent<Field_obj>();
        hand = f.Instant_obj("System_obj/Hand_obj", 0).GetComponent<Hand_obj>();
        init.transform.parent = GameObject.Find("Obj_parent").transform;
        game.transform.parent = GameObject.Find("Obj_parent").transform;
        chara.transform.parent = GameObject.Find("Obj_parent").transform;
        field.transform.parent = GameObject.Find("Obj_parent").transform;
        hand.transform.parent = GameObject.Find("Obj_parent").transform;
        

        init.chara = chara; init.field = field;
        init.Start_game();
    }

    public static void Create_init() {
        init.Create_init();
        start_pass = true;
    }

    void Update() {
        if (start_pass) {
            //$b プレイヤーが二人になったとき
            if (init_pass == PhotonNetwork.PlayerList.Length) {
                init_pass = init.Recv_init(); //** 2 or 3
                game.chara = chara; game.field = field;
                game.hand = hand;
                game.p_num = init.p_num; game.turn = init.turn;
                game.ally_p = init.ally_p; game.opp_p = init.opp_p;
                //print(game.ally_p); print(game.opp_p);
                UI.Able_surrender();
            }
            //$b 初期化完了
            else if (init_pass == 3) {game.Update_game();}
            
        }
        
    }
    
}