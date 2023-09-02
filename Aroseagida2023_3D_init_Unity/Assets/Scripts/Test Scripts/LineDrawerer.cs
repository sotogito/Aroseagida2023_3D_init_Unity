using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Letter
{
    public class LineDrawerer : MonoBehaviour
    {
    //따로 작성한 뒤 D.M처럼 ㅇ니스턴스 생성해서 받는게 깔끔할 거 같음
        #region 편지 띄우는 ui
        public Text draw_LetterText;
        public Text draw_LetterNum;
        #endregion
        //DataManager로 넘기기 전 임의의 list 생성
        List<DrawnInfo> tempDrawnLines = new List<DrawnInfo>(); 

        List<Vector3> linePoints;
        float timer;
        public float timerdelay;

        GameObject newLine;
        LineRenderer drawLine;
        public float lineWidth;

        //팬촉 오브젝트 넣어야됨
        public GameObject drawingObject;

        public FlexibleColorPicker flexibleColorPicker;
        public Canvas ColorPickerUi;


        void Start()
        {            
            ColorPickerUi.gameObject.SetActive(false);


            linePoints = new List<Vector3>();
            timer = timerdelay;

            //편지 ui
            draw_LetterText.text = DataManager.instance.nowPlayer.LetterText;
            draw_LetterNum.text = DataManager.instance.nowPlayer.LetterNum+"번째 편지";

            //tempDrawnLines = new List<DrawnInfo>();
            //tempDrawnLines = DataManager.instance.nowPlayer.DrawnLines;
            
            //일단 데이터가 있으면 그림그림
            LoadLines(DataManager.instance.nowPlayer.DrawnLines.ToArray());
        
        }

        // Update is called once per frame
        void Update()
        { 

            if(Input.GetKeyDown(KeyCode.Space))
            {   
                ColorPickerUi.gameObject.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                ColorPickerUi.gameObject.SetActive(false);
            }

            #region 그림그리기 코드
            if(Input.GetKeyDown(KeyCode.E))             //마우스를 누를 때 -> 이때 저장 시작, Color를 받음
            {
                //Material도 따로 지정해줘야됨
                //Debug.Log(linePoints.Count);
                newLine = new GameObject();
                drawLine = newLine.AddComponent<LineRenderer>();
                drawLine.material = new Material(Shader.Find("Sprites/Default"));
                //Color c = randomColor();
                drawLine.startColor = flexibleColorPicker.color;
                drawLine.endColor = flexibleColorPicker.color;
                //drawLine.material.color = flexibleColorPicker.color;
                drawLine.startWidth = lineWidth;
                drawLine.endWidth = lineWidth;

            }
            
            if(Input.GetKey(KeyCode.E))               //마우스를 누르는 동안   -> 값을 계속 받음
            {
                //마우스의 위치에 따라서 선이 생겨 따라옴
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), GetMousePosition(),Color.red);
                timer -= Time.deltaTime;
                if(timer<=0)
                {
                    drawLine.positionCount = linePoints.Count;
                    drawLine.SetPositions(linePoints.ToArray());
                    linePoints.Add(GetMousePosition());

                    timer = timerdelay;
                }
            }

            if(Input.GetKeyUp(KeyCode.E))           //마우스를 누르는 동안  -> 이때 저장완료 집어 넣으면 됨
            {
                /*
                #region 선 하나당 linePoints에 저장된 Vector3, 사용도니 Color 출력 = JSON에 저장될 데이터는 이렇게 저장하면 됨
                foreach (Vector3 point in linePoints)
                {
                    Debug.Log("X: " + point.x + ", Y: " + point.y + ", Z: " + point.z);
                }
                Debug.Log("flexibleColorPicker Color"+flexibleColorPicker.color);
                #endregion
                */

                //데이터 받기
                DrawnInfo drawninfo = new DrawnInfo
                {
                    Points = linePoints.ToArray(),
                    Color = flexibleColorPicker.color
                };
                tempDrawnLines.Add(drawninfo);
            

                linePoints.Clear();
            }
            #endregion
        
        }

        Vector3 GetMousePosition()
        {
            /*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray.origin + ray.direction * 10;
            */
            return drawingObject.transform.position;
        }

        //저장하기 버튼을 누르면 LetterText데이터를 업데이트하고 Main 씬으로 이동, 
        public void Save()
        {
            DataManager.instance.nowPlayer.LetterText += "한번 저장된 데이터입니다";
            //임의의 list를 D.M 으로 넘기기ㄴ
            DataManager.instance.nowPlayer.DrawnLines = tempDrawnLines;
            SceneManager.LoadScene(0);
            DataManager.instance.SaveData();
        }
        public void Out()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadLines(DrawnInfo[] drawnInfos)
        {
            // 저장된 그림 정보를 이용하여 화면에 그리는 작업 수행
            foreach (DrawnInfo drawnInfo in drawnInfos)
            {
                GameObject newLine = new GameObject();
                LineRenderer drawLine = newLine.AddComponent<LineRenderer>();
                drawLine.material = new Material(Shader.Find("Sprites/Default"));
                drawLine.startColor = drawnInfo.Color;
                drawLine.endColor = drawnInfo.Color;
                drawLine.startWidth = lineWidth;
                drawLine.endWidth = lineWidth;
                drawLine.positionCount = drawnInfo.Points.Length;
                drawLine.SetPositions(drawnInfo.Points);
            }
        }
    }
    [System.Serializable]
    public class DrawnInfo
    {
        public Vector3[] Points;
        public Color Color;
    }

}