using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

namespace Letter
{
    public class RandomLetter : MonoBehaviour
    {
        #region API Url
        private const string DjangoApiUrl = "http://127.0.0.1:8000/api/get_user_level/";
        //private const string ageURL = "http://127.0.0.1:8000/api/get_user_age/";
        #endregion

        //public UnityEngine.UI.Text LetterText;
  
        public int maxStringListSize = 10;



        #region 질문 생성 관련 함수 및 인스턴스
        //user의 level과 전체 편지 개수
        public int level;
        public int totalLetter;

        // 질문을 생성하는 배열 4가지
        public int QuestionType;
        public int RandomLevel;
        public int RandomQuestion;
        public int RandomOption;
        public string LetterText;

        //인스턴스 생성
        private SendDataToDjango sendDataToDjango;
        private RandomLetterList rLetterList;
        #endregion

        void Start()
        {   
            rLetterList = new RandomLetterList(); 
            sendDataToDjango = GetComponent<SendDataToDjango>();

            //StartCoroutine(GetAgeOption());
            StartCoroutine(GetLevel());
        
           
        }


        #region 질문 생성 함수
        public void MakeLetter()
        {

            level = 5;
                //index 초기화
            // indext 0
            QuestionType = UnityEngine.Random.Range(0,2);
            // indext 1
            //RandomLevel = UnityEngine.Random.Range(0,level); => RandomLetterList.cs에서 함
            // indext 2
            RandomQuestion = 0;
            // indext 3
            RandomOption = 0;
            
            #region 랜덤 범위 알아오기
            rLetterList.QuestionLevel(QuestionType,level);
            RandomOption = rLetterList.randomOption;
            RandomLevel = rLetterList.randomLevel;
            //print("추가된 나이옵션 총 길이"+rLetterList.ageOptions.Count);
            rLetterList.Question(QuestionType,RandomLevel,0,RandomOption);
            RandomQuestion = rLetterList.randomQuestion;
            #endregion
            
            //최종
            rLetterList.Question(QuestionType,RandomLevel,RandomQuestion,RandomOption);
            //LetterText.text = rLetterList.resultQuestion;
            LetterText = rLetterList.resultQuestion;
            //서버로 데이터 전송 
            sendDataToDjango.SendData(QuestionType, RandomLevel, RandomQuestion, RandomOption, LetterText, true);   
        }
        #endregion



        #region 레벨 받아오기
        IEnumerator GetLevel()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(DjangoApiUrl))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                    yield break; // 에러 발생 시 중단
                }

                string jsonResponse = request.downloadHandler.text;
                LevelResponseData response = JsonUtility.FromJson<LevelResponseData>(jsonResponse);

                if (response != null)
                {
                    level = response.level;
                    totalLetter = response.active_question_count+1;
                    Debug.Log("Received Level: " + level);
                    Debug.Log("Total Letter: " + totalLetter);
                    MakeLetter();
                    //MakeLetter();
                    //MakeLetter();
                }
                else{
                    Debug.LogError("Failed to parse response data.");
                }
                request.Dispose();
            }
        }
        #endregion

        /*
        #region 나이 받아오기
        IEnumerator GetAgeOption()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(ageURL))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("나이받기 Error: " + request.error);
                }
                else{   
                    string jsonResponse = request.downloadHandler.text;
                    userAgeData ageresponse = JsonUtility.FromJson<userAgeData>(jsonResponse);
                    for (int a = ageresponse.user_age_num; a>=1; a--)
                    {
                        rLetterList.ageOptions.Add((a * 10)+"대에");
                    }
                }
            }
        }
        #endregion
        */

        #region System.Serializable
        [System.Serializable]
        public class LevelResponseData
        {   
            public int level;
            public int active_question_count;
        }
        #endregion

        /*
        [System.Serializable]
        public class userAgeData
        {
            public int user_age;
            public int user_age_num;
        }
        
        */
        
    }
}


