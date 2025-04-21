using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Serialization;
using System;
using System.IO;

public class DataManager 
{
    public int CurrentStage = 0;
    public List<int> Score = new List<int>();

    public bool[] StageisClear = new bool[3];
    public void Add(int score)
    {
        Score.Add(score);

        Score = Score.OrderByDescending(n => n).ToList();

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));
#if UNITY_EDITOR
        try
        {
            using (StreamWriter sw = new StreamWriter("Assets/Data/ranking.xml"))
            {
                xmlSerializer.Serialize(sw, Score);
            }
        }
        catch (Exception e)
        {
            Debug.Log("폴더가 없음");
        }
        return;
#endif
        try
        {
            using (StreamWriter sw = new StreamWriter(Application.streamingAssetsPath+"/Data/ranking.xml"))
            {
                xmlSerializer.Serialize(sw, Score);
            }
        }
        catch (Exception e)
        {
            Debug.Log("폴더가 없음");
        }
    }
    public int Get(int index)
    {
        return Score[index];
    }
    public DataManager()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<int>));
#if UNITY_EDITOR
        try
        {
            using(StreamReader sr = new StreamReader("Assets/Data/ranking.xml"))
            {
                Score = (List<int>)xmlSerializer.Deserialize(sr);
            }
        }
        catch (Exception e)
        {
            Score = new List<int>();
            Debug.Log("랭킹 데이터 없음");
        }
        return;
#endif
        try
        {
            using (StreamReader sr = new StreamReader(Application.streamingAssetsPath+"/Data/ranking.xml"))
            {
                Score = (List<int>)xmlSerializer.Deserialize(sr);
            }
        }
        catch (Exception e)
        {
            Score = new List<int>();
        }
    }
}
