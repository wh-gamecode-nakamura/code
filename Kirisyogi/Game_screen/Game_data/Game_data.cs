using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_data : MonoBehaviour
{
    public static string user_name = "1-20";

    public static int[][] init_li = new int[][] // {chara_id, x, y}
    {
        new int[] {2, 1, 1}, // 十
        new int[] {4, 2, 1}, // 王
        new int[] {3, 3, 1}, // 斜
        new int[] {1, 2, 2}, // 歩
    };
}
