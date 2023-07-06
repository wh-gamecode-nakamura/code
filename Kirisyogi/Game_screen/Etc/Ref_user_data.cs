using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

public class Ref_user_data : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Refl_user_data(this.gameObject);
    }

    public static void Refl_user_data(GameObject game_obj) {
        switch (game_obj.name)
        {
            case "User_name_placeholder":
                game_obj.GetComponent<TextMeshProUGUI>().text = User_data.user_name;
                break;
        }
    }

    public static void Asg_user_data(GameObject game_obj) {
        switch (game_obj.name)
        {
            case "User_name_text":
                User_data.user_name = game_obj.GetComponent<TextMeshProUGUI>().text;
                break;
        }
    }
}
