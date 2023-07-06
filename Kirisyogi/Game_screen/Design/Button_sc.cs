using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

public class Button_sc : MonoBehaviour
{
    public GameObject game_obj;
    public GameObject game_obj_2;

    public Button testaa;

    public void Transition_Battle() {
        SceneManager.LoadScene("Scenes/Battle");
    }

    public void Transition_Org() {
        SceneManager.LoadScene("Scenes/Org");
    }

    public void Transition_Title() {
        SceneManager.LoadScene("Scenes/Title");
    }

    public void Transition_Config() {
        SceneManager.LoadScene("Scenes/Config");
    }

    public void End_game() {
        Application.Quit();
    }

    public void Surrender_proc() {
        Main.game.Surrender_proc();
    }

    public void Save() {
        Org_main.game.Save();
        SceneManager.LoadScene("Scenes/Battle");
    }

    public void Delete() {
        Org_main.game.Delete_chara();
    }

    public void Hide_clicked() {
        game_obj.SetActive (false);
    }

    public void Disp_clicked() {
        game_obj.SetActive (true);
    }

    public void Change_user_data() {
        Ref_user_data.Asg_user_data(game_obj);
        Ref_user_data.Refl_user_data(game_obj_2);
    }

    public void Disp_add_chara_list() {
        game_obj.SetActive(true);
        Org_main.game.Disp_add_chara_list();
    }

    public void test() {
        print(1);
    }

    void Update() {
        if (testaa) {
            testaa.interactable = ((Time_manager.second_time % 2) == 0) ? true: false;
        }
        
    }


}
