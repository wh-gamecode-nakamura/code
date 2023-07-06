using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;               // ファイル読み込みに必要です
using Newtonsoft.Json;         // Json.net用に必要です

[Serializable]
public class SaveData
{
    public int org_len = 0;
    public int[] org_li_1 = new int[3];
    public int[] org_li_2 = new int[3];
    public int[] org_li_3 = new int[3];
    public int[] org_li_4 = new int[3];
    public int[] org_li_5 = new int[3];
    public int[] org_li_6 = new int[3];

    public int score = 1500;
    public int stone;
}

public class SaveManager : MonoBehaviour
{

    static string filePath = Application.dataPath + "/Script/Kirisyogi/Savedata.json"; // /Assets
    static SaveData saveData = new SaveData();
    public static int[][] org_li_li = {saveData.org_li_1, saveData.org_li_2, saveData.org_li_3, saveData.org_li_4, saveData.org_li_5, saveData.org_li_6};
    
    //$c 
    public static void Save()
    {
        print("save");

        saveData.org_len = Game_data.init_li.Length;
        for (int i = 0; i < saveData.org_len; i++) {
            switch (i) {
                case 0: saveData.org_li_1 = Game_data.init_li[i]; break;
                case 1: saveData.org_li_2 = Game_data.init_li[i]; break;
                case 2: saveData.org_li_3 = Game_data.init_li[i]; break;
                case 3: saveData.org_li_4 = Game_data.init_li[i]; break;
                case 4: saveData.org_li_5 = Game_data.init_li[i]; break;
                case 5: saveData.org_li_6 = Game_data.init_li[i]; break;
                default: print("エラー"); break;
            }
            ////for (int j = 0; j < Game_data.init_li[i].Length; j++) {
            ////    org_li_li[i][j] = Game_data.init_li[i][j];
            ////}
            
        }
        f.print_r(org_li_li[0]);
        f.print_r(saveData.org_li_1);

        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter;
        
        try { // ワークスペースにjsonファイルを置くための処理
            streamWriter = new StreamWriter(filePath);
        } catch (System.IO.DirectoryNotFoundException) {
            filePath = Application.dataPath + "/Savedata.json";
            streamWriter = new StreamWriter(filePath);
        }
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }
    
    public static void Load()
    { 
        if (File.Exists(filePath))
        {
            print("load");

            StreamReader streamReader;

            try { // ワークスペースにjsonファイルを置くための処理
                streamReader = new StreamReader(filePath);
            } catch (System.IO.DirectoryNotFoundException) {
                filePath = Application.dataPath + "/Savedata.json";
                streamReader = new StreamReader(filePath);
            }
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            SaveData loadData = JsonUtility.FromJson<SaveData>(data);
            if (loadData == null) {return;}
            LoadData(loadData);
        } else {
            print("指定したファイルパスが存在しません");
        }
    }

    // jsonファイルからのロード
    public static void LoadData(SaveData loadData) {
        saveData.org_len = loadData.org_len;
        saveData.org_li_1 = loadData.org_li_1;
        saveData.org_li_2 = loadData.org_li_2;
        saveData.org_li_3 = loadData.org_li_3;
        saveData.org_li_4 = loadData.org_li_4;
        saveData.org_li_5 = loadData.org_li_5;
        saveData.org_li_6 = loadData.org_li_6;
        saveData.score = loadData.score;
        saveData.stone = loadData.stone;
    }

    // 編成データのロード
    public static void LoadOrg() {
        int[][] org_li_li = {saveData.org_li_1, saveData.org_li_2, saveData.org_li_3, saveData.org_li_4, saveData.org_li_5, saveData.org_li_6};
        for (int i = 0; i < saveData.org_len; i++) {
            for (int j = 0; j < Game_data.init_li[i].Length; j++) {
                Game_data.init_li[i][j] = org_li_li[i][j];
            }
        }
    }
}
