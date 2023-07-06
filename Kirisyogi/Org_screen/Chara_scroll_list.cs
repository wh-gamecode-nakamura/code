using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

public class Chara_scroll_list : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Org_UI.test.chara_scroll_list.SetActive(true);
        GameObject chara_content;
        for (int i = 1; i <= 12; i++) //$t iをキャラ数の定数に
        {
            chara_content = GameObject.Find($"UI_canvas/Chara_scroll_list/Viewport/Contents/Chara_{i}");
            if ((Chara_data.chara_prefab_li.Length-1) < i) {chara_content.SetActive (false); continue;}
            Org_set_chara_info(chara_content, 0, 0, i, new int[] {0, 0});
            
            chara_content.transform.Find("Chara_image").GetComponent<Image>().sprite = 
                Resources.Load<Sprite>(Chara_data.Collation_prefab(chara_content.GetComponent<Chara_sc>().chara_id));
            chara_content.transform.Find("Chara_name_text").GetComponent<TextMeshProUGUI>().text = 
                Chara_data.Get_chara_name(chara_content.GetComponent<Chara_sc>().chara_id);
        }
        Org_UI.test.chara_scroll_list.SetActive(false);
    }

    public void Org_set_chara_info(GameObject chara_obj, int p_num, int chara_p_num, int chara_id, int[] pos_li) {
        Chara_sc chara_sc = chara_obj.GetComponent<Chara_sc>();
        chara_sc.obj = chara_obj;

        chara_obj.transform.Find("Chara_image").GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(Chara_data.Collation_prefab(chara_id));

        chara_sc.p_num = chara_p_num;
        chara_sc.mine = (p_num == chara_p_num);

        chara_sc.chara_id = chara_id;
        chara_sc.pos = new int[] {pos_li[0], pos_li[1]};
        chara_sc.Set_chara_info(chara_sc);
    }

}
