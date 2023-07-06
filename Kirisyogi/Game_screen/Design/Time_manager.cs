using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Time_manager : MonoBehaviour
{
    public static int hour_time;
    public static int minute_time;
    public static int second_time;

    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        hour_time = DateTime.Now.Hour;
        minute_time = DateTime.Now.Minute;
        second_time = DateTime.Now.Second;
        test.GetComponent<TextMeshProUGUI>().text = $"{hour_time}/{minute_time}/{second_time}";
    }

    // Update is called once per frame
    void Update()
    {
        hour_time = DateTime.Now.Hour;
        minute_time = DateTime.Now.Minute;
        second_time = DateTime.Now.Second;
        test.GetComponent<TextMeshProUGUI>().text = $"{hour_time}/{minute_time}/{second_time}";
    }
}
