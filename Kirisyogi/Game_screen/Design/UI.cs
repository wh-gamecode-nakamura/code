using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; //TextMeshProを扱う際に必要
using UnityEngine.UI;

[System.Serializable]
public class UI : MonoBehaviour
{

    //$p ゲームオブジェクト
    //public GameObject Retry_button_t;
    //public GameObject Result_text_t;
    public GameObject Result_obj;
    public GameObject Result_text;
    public GameObject simple_info_text;
    public GameObject info_window_text;
    public GameObject info_window_image;
    public GameObject info_window_promote_image;
    public Button surrender_button;
    public static GameObject info_text;
    public static GameObject test_text;

    public static UI test = null;

    void Awake() {
        if (test == null) {
            test = this;
        }
    }
    

    void Start() {
        //Retry_button = Retry_button_t;
        //Result_text = Result_text_t;
        //print(Retry_button);
        
    }

    public static void Hide() {
        UI.test.Result_obj.SetActive(false);
    }

    public static void Disp_result() {
        UI.test.Result_obj.SetActive(true);
    }

    public static void Edit_text(GameObject text_obj, string text_t) {
        text_obj.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void Edit_simple_info_text(string text_t) {
        test.simple_info_text.GetComponent<TextMeshProUGUI>().text = text_t;
    }

    public static void Able_surrender() {
        UI.test.surrender_button.interactable = true;
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
