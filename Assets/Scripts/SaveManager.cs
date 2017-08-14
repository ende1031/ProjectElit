using UnityEngine;
using System.Collections;
using System.IO;



public class SaveManager : MonoBehaviour
{
    public int level = 20;//언록된 스테이지 수(패치시엔 삭제 예정)

    string m_strPath = "Assets/Save/";

    void Start()
    {
        WriteData(level.ToString());//11번째 스테이지 (3-1)까지 클리어 임시로 강제 세이브 작성 (삭제예정)
    }

    public void WriteData(string strData)//strData를 txt에 저장 (0 = 1-1스테이지)
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Create, FileAccess.ReadWrite);
        StreamWriter writer = new StreamWriter(f);
        writer.WriteLine(strData);
        writer.Close();
        f.Close();
    }



    public int Parse()
    {
        FileStream f = new FileStream(m_strPath + "Data.txt", FileMode.Open, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(f);

        //한줄을 읽는다. 
        string source = sr.ReadLine();
        int Temp = int.Parse(source);
        sr.Close();
        f.Close();
        return Temp;
        //string[] values;    // 쉼표로 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        //while (source != null)
        //{
        //    values = source.Split(',');  // 쉼표로 구분한다. 저장시에 쉼표로 구분하여 저장하였다.
        //    if (values.Length == 0)
        //    {
        //        sr.Close();
        //        return;
        //    }
        //    source = sr.ReadLine();    // 한줄 읽는다.
    }
}