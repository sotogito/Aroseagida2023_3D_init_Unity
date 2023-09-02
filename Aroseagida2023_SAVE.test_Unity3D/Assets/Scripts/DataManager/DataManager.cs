using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

namespace Letter
{
    public class PlayerData
    {
        public bool IsDrawn;
        public string LetterText;
        public int LetterNum;
        //Draw에서 받아올 Trail Renderer - posiotion 함수
        //Draw에서 받아올 Trail Renderer - color 함수
        public List<DrawnInfo> DrawnLines;
    }
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance; //DataManager 인스턴스
        public PlayerData nowPlayer = new PlayerData(); //PlayerData인스턴스

        //데이터가 저장된 경로 = path+nowSlot
        public string path;
        public int nowSlot;

        private void Awake()
        {
            #region 싱글톤
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(instance.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
            #endregion

            path = Application.persistentDataPath + "/AROSEAGIDA SAVE";
            print(path);
        }

        void Start()
        {
         
        }

        
        #region SaveData(), LoadData(), DataClear() 저장하기,불러오기,데이터클리어
        public void SaveData() //저장하기 함수
        {
            string data = JsonUtility.ToJson(nowPlayer);
            File.WriteAllText(path+ nowSlot.ToString(), data); 
        }
        public void LoadData() //불러오기 함수
        {
            string data = File.ReadAllText(path + nowSlot.ToString());
            nowPlayer = JsonUtility.FromJson<PlayerData>(data);
        }

        public void DataClear()
        {
            print("초기화 되었습니다");
            nowSlot = -1;
            nowPlayer = new PlayerData();
        }
        #endregion


        #region test() [그림그리기]가 선택될 때 출력됨
        public void test()
        {
            print("");
            print("");
            print("DataManager로 넘어간 데이터를 출력합니다");
            print("id : "+nowPlayer.LetterNum);
            print("Letter : "+nowPlayer.LetterText);
            print("nowSlot : "+nowSlot);
            print("");
            print("");
        }
        #endregion

        
    
    }

}
