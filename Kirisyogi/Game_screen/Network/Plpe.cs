using ExitGames.Client.Photon;
using Photon.Realtime;

using System.Collections.Generic;

public static class Plpe // PlayerPropertiesExtensions
{
    public static Hashtable send_li = new Hashtable();

    public static void InitHashtable(this Player player)
    {
        send_li = new Hashtable();
        send_li["move_chara_li"] = null;
        send_li["init_li"] = null;
        send_li["result_reason"] = 0;
        send_li["check_result"] = false;
        send_li["test"] = 0;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    //$p 送信
    public static void send_bool(this Player player, string key, bool num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }
    
    public static void send_int(this Player player, string key, int? num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    public static void send_int_d1(this Player player, string key, int[] num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    public static void send_int_d2(this Player player, string key, int[][] num)
    {
        send_li[key] = num;
        player.SetCustomProperties(send_li);
        send_li.Clear();
    }

    //$p 受信
    public static bool recv_bool(this Player player, string key) {
        if (player.CustomProperties[key] == null) {return false;}
        return (bool)player.CustomProperties[key];
    }
    public static int recv_int(this Player player, string key) {
        if (player.CustomProperties[key] == null) {return 0;}
        return (int)player.CustomProperties[key];
    }
    public static int[] recv_int_d1(this Player player, string key) {
        return (int[])player.CustomProperties[key];
    }
    public static int[][] recv_int_d2(this Player player, string key) {
        return (int[][])player.CustomProperties[key];
    }
}