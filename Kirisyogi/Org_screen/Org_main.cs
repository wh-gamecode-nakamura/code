using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Org_main : MonoBehaviour
{
    public static Org_init_obj init;
    public static Org_game_obj game;
    public static Chara_obj chara;
    public static Org_field_obj field;
    //public static Org_Hand_obj hand;

    void Start()
    {
        SaveManager.Load();
        SaveManager.LoadOrg();
        init = f.Instant_obj("System_obj/Org_init_obj", 0).GetComponent<Org_init_obj>();
        game = f.Instant_obj("System_obj/Org_game_obj", 0).GetComponent<Org_game_obj>();
        chara = f.Instant_obj("System_obj/Chara_obj", 0).GetComponent<Chara_obj>();
        field = f.Instant_obj("System_obj/Org_field_obj", 0).GetComponent<Org_field_obj>();
        init.transform.parent = GameObject.Find("Obj_parent").transform;
        game.transform.parent = GameObject.Find("Obj_parent").transform;
        chara.transform.parent = GameObject.Find("Obj_parent").transform;
        field.transform.parent = GameObject.Find("Obj_parent").transform;

        init.chara = chara; init.field = field;
        field.Create_field_data();
        init.Ref_init();

        game.chara = chara; game.field = field;
    }


    void Update()
    {
        game.Update_game();
    }
}
