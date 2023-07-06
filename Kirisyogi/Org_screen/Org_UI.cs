using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

[System.Serializable]
public class Org_UI : MonoBehaviour
{

    //$p ゲームオブジェクト
    public GameObject cost_text;
    public GameObject info_window_text;
    public GameObject info_window_image;
    public GameObject info_window_promote_image;
    public GameObject chara_scroll_list;
    public GameObject buttons;
    public Button Battle_transition_button;

    public static Org_UI test = null;

    void Awake() {
        if (test == null) {
            test = this;
        }
    }
    

    public static void Edit_text(GameObject text_obj, string text_t) {
        text_obj.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void Edit_cost_text(string text_t) {
        test.cost_text.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    //$p 勝敗表示
    public static string Create_result_text(bool win, int result_reason) {
        string result_text = "";
        switch (result_reason) {
            case Because.take_king:
                result_text = win ? "王を取りました": "王が取られました";
                break;
            case Because.arrival_king:
                result_text = win ? "あなたの王が\r\n端に着きました": "相手の王が\r\n端に着きました";
                break;
            case Because.surrender:
                result_text = win ? "相手が降参しました": "降参を選択しました";
                break;
            case Because.time_out:
                result_text = win ? "相手の時間切れです": "時間切れです";
                break;
            case Because.disconnect:
                result_text = win ? "相手の通信が\r\n切断されました": "あなたの通信が\r\n切断されました";
                break;
        }
        result_text += win ? "\r\nあなたの勝利です！": "\r\nあなたの敗北です";
        
        return result_text;
    }
}
