using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Letter
{

    public class ScreenShot : MonoBehaviour
    {
        //스크린 샷을 찍을동안은 잠시 펜이 사라짐
        public GameObject playerPen;
        public GameObject playerPenPoint;


        
        void Start()
        {  
            CaptureScreen();
                //사진 초기화?
                //어차피 새로 찍으면 덮어씌기댐ㄴ
                //drawn 씬에는 넣지 않기
                //사진초기화를 해두지 않으면 다음 게임에서 만약 캡쳐를 하지 않았을 시 이전에 저장되었던 사진이 뜬다
                //이를 방지하고자 게임이 시작되면 임의의 이미지를 띄우는 것도 좋은 방법
            
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                playerPen.gameObject.SetActive(false);
                playerPenPoint.gameObject.SetActive(false);
                CaptureScreen();
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                playerPen.gameObject.SetActive(true);
                playerPenPoint.gameObject.SetActive(true);
            }
        }

        void CaptureScreen()
        {
            int NowSlot = DataManager.instance.nowSlot;
            //string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName ="_Screenshot_" + NowSlot + ".png";
            string filePath = Application.persistentDataPath +  "/AROSEAGIDA SAVE" + fileName;

            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log("Screenshot captured: " + fileName);
            Debug.Log("혀ㄴ재 스립nowslot " + NowSlot);
        }

    }

}
