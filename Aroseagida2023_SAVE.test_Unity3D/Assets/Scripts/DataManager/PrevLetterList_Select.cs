using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;


namespace Letter
{
    public class PrevLetterList_Select : MonoBehaviour
    {   
        private const string DjangoApiUrl = "http://127.0.0.1:8000/api/get_prevletter_list/";

        //인스턴스 생성
        public static PrevLetterList_Select instance; 

        #region 변수들
        
        //각 button과 text의 값을 지정해준다. 
        public int BtnNum;
        public Text[] BtnText;
    
        //서버에서 받아온 데이터를 저장하는 곳
        public string[] PrevLetter_text;
        public int[] PrevLetter_id;

        //그림을 다 그렸나 안그렸나, 저장된 데이터의 편지인가 아닌가를 판별
        bool[] isActive = new bool[10];


        public int nowBtnNum;
  
        public Button drawButton;
        public Button skipButton;

        #endregion

     
        void Start()
        {
            StartCoroutine(GetPrevLetterList());
            drawButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }

        //클릭된 LetterList 버튼의 index값을 받음
        public void nowClick(int btnnum)
        {
            BtnNum = btnnum;
            print(BtnNum);

            print(PrevLetter_text[BtnNum]);
            print(PrevLetter_id[BtnNum]);

            //버튼 실행
            drawButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
    
        }

        #region 버튼ui에 지정할 Function
        #region [그림그리기]
        public void nowDraw() //[그림그리기]
        {
            DataManager.instance.nowPlayer.LetterNum = PrevLetter_id[BtnNum];
            DataManager.instance.nowPlayer.LetterText = PrevLetter_text[BtnNum];
            DataManager.instance.nowPlayer.IsDrawn = true;
            

            print("PrevLetterList_Select"+DataManager.instance.nowPlayer.LetterNum);
            print("해당 버튼의 index값  :  "+BtnNum);
            DataManager.instance.test();
            print("현재 데이터의 nowSlot은 "+DataManager.instance.nowSlot);
            DataManager.instance.SaveData();
            GoDraw();
        }

        #endregion
        #region [넘어가기]
        public void nowSkip() //[넘어가기]
        {
            print("해당 버튼의 index값  :  "+BtnNum);
            print("넘어가기를 선택하셨습니다");
            StartCoroutine(UpdateIsActve(PrevLetter_id[BtnNum]));
            BtnText[BtnNum].text = "삭제 되었습니다";
        }
        #endregion
        #endregion

        public void GoDraw() //draw 씬으로 이동
        {
            print("여기에 이동하면 됨");
            SceneManager.LoadScene(3);
        }


         public void MakeLetterList()
        {
            for(int i = PrevLetter_id.Length-1; i>=0; i--)
            {
                BtnText[i].text = PrevLetter_text[i];
            }
        }



        #region GetPrevLetterList()+[System.Serializable]
        private IEnumerator GetPrevLetterList()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(DjangoApiUrl))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    print(request.downloadHandler.text);
                    PrevLetterListResponse response = JsonUtility.FromJson<PrevLetterListResponse>(jsonResponse);
                    PrevLetter_text = response.prevletters;
                    PrevLetter_id = response.prevletters_id;
                    MakeLetterList();
                }
            }
        }
        [System.Serializable]
        public class PrevLetterListResponse
        {  
            public string[] prevletters;
            public int[] prevletters_id;
        }
        #endregion
    

        #region UpdateIsActve() [넘어가기]->is_active:false
        private IEnumerator UpdateIsActve(int dataId)
        {
            string apiUrl = "http://127.0.0.1:8000/api/update_is_active/";
            WWWForm form = new WWWForm();
            form.AddField("data_id", dataId);

            using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, form))
            {

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                }
                else
                {
                    Debug.Log("is_active field updated successfully.");
                }
            }
        }
        #endregion


    }
}



