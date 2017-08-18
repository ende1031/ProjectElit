using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Reflection;

public class SaveManager : MonoBehaviour
{
    SaveData m_save = new SaveData();

    // Use this for initialization
    void Start()
    {
        InputStage(20); //개발용 코드 0 = 1스테이지 클리어(배포시 지울예정)
        if (!File.Exists(Application.dataPath + "/Resources/save.dat")) InputStage(20);  //세이브 데이터가 없으면 새로 만든다.
    }

    void InputData()//클래스에 저장된 값을 직렬화 후 save.dat에저장한다.
    {
        try
        {
            var b = new BinaryFormatter();

            var f = File.Create(Application.dataPath + "/Resources/save.dat");
            Debug.Log(Application.dataPath);

            b.Serialize(f, m_save);
            f.Close();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OutputData()   //save.dat의 값들을 SaveData클래스로 불러온다.
    {
        try
        {
            if (File.Exists(Application.dataPath + "/Resources/save.dat"))
            {
                Debug.Log("Exists!!");
                var b = new BinaryFormatter();
                var f = File.Open(Application.dataPath + "/Resources/save.dat", FileMode.Open);

                m_save = (SaveData)b.Deserialize(f);
                f.Close();
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void InputStage(int x)   //CleardStageNum를 바꾸고 파일로 저장한다.
    {
        m_save.CleardStageNum = x;
        InputData();
    }

    public int OutputStage()    //SaveData의 CleardStageNum를 반환한다.
    {
        OutputData();
        return m_save.CleardStageNum;
    }
}

class SaveData // 직렬화를 위한 클래스. 원하는 자료형 추가 가능
{
    public int CleardStageNum = 0;
}
