using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerScript : MonoBehaviour
{
    public void HoverChara() {
        Org_main.game.hover_obj = gameObject;
        Org_main.game.hover_chara_sc = gameObject.GetComponent<Chara_sc>();
    }

    public void ReturnObject() {
        Org_main.game.click_obj = gameObject;
        Org_main.game.UI_click_flag = true;
        print(Org_main.game.click_obj);
    }

}
