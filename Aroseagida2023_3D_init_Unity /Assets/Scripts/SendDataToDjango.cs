// Unity C# Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Letter
{
    public class SendDataToDjango : MonoBehaviour
    {
        private const string DjangoApiUrl = "http://127.0.0.1:8000/api/receive_unity_data/";
        private RandomLetter randomLetter;
        private void Start()
        {
            randomLetter = GetComponent<RandomLetter>();
        }
        

        public void SendData(int questionType, int levelType, int question, int option, string questionText, bool isActive)
        {
            StartCoroutine(SendDataCoroutine(questionType, levelType, question, option, questionText, isActive));
        }

        private IEnumerator SendDataCoroutine(int questionType, int levelType, int question, int option, string questionText, bool isActive)
        {
            PrevLetterData data = new PrevLetterData
            {
                question_type = questionType,
                level_type = levelType,
                question = question,
                option = option,
                question_text = questionText,
                is_active = isActive
            };

            string jsonData = JsonUtility.ToJson(data);
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

            UnityWebRequest request = new UnityWebRequest(DjangoApiUrl, "POST");
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent to Django successfully.");
                print(randomLetter.LetterText +"중복이 아닌 질문xxxxxxxxx");
            }
            else
            {
                Debug.LogError("Failed to send data to Django: " + request.error);
                print(randomLetter.LetterText +"중복인 질문ooooooo");
                if (request.responseCode == 400)
                {
                    // 중복이면 RandomLetter 클래스의 MakeLetter 메서드 호출
                    //print("-----------중복되어 다시생성");
                    randomLetter.MakeLetter();
                }
            }
        }

        [System.Serializable]
        private class PrevLetterData
        {
            public int question_type;
            public int level_type;
            public int question;
            public int option;
            public string question_text;
            public bool is_active;
        }
    }
}
