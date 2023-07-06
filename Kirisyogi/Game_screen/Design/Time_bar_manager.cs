using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_bar_manager : MonoBehaviour
{
    private bool start_pass = true;

    public bool count_pass = false;

    public float max_value;

    public Slider slider;
    public Color fill_color;

    
    void Update()
    {
        //$b Startの代わり
        if (Main.game && start_pass) {
            Main.game.time_bar_sc = this;
            start_pass = false;
            fill_color = GameObject.Find("Time_bar/Fill_area/Fill").GetComponent<Image>().color;
        }
        if (count_pass) {
            slider.value -= Time.deltaTime;

            //$b 時間切れになった場合
            if (slider.value == 0) {
                Main.game.Time_out_proc();
                count_pass = false;
            }
            //$b 残り時間4分の1の場合
            else if (slider.value < max_value/4) {
                fill_color = new Color32(255, 100, 75, 255);
            }
            //$b 残り時間半分の場合
            else if (slider.value < max_value/2) {
                fill_color = new Color32(255, 200, 30, 255);
            }
            //$b 時間に余裕がある場合
            else {
                fill_color = new Color32(140, 255, 220, 255);
            }
        }
        //$b 相手ターンの場合
        else {
            fill_color = new Color32(160, 160, 160, 255);
        }
        GameObject.Find("Time_bar/Fill_area/Fill").GetComponent<Image>().color = fill_color;
    }
}
