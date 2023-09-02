using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Letter
{
    public class Draw : MonoBehaviour
    {
        #region 편지 띄우는 ui
        public Text draw_LetterText;
        public Text draw_LetterNum;
        public Canvas LetterUi;
        #endregion
        #region ui 창 띄우기
        public Transform head;
        public float spawnDistance =2;
        #endregion

        //Trail Renderer의 position을 입력받을 코드

        //public Color newColor;
        private TrailRenderer trailRenderer;
        public FlexibleColorPicker fcp;
        public Material[] trailMaterial;
        public int trailMaterial_index = 1;

        void Start()
        {
            trailRenderer = GetComponent<TrailRenderer>();
            trailMaterial = trailRenderer.materials;
            draw_LetterText.text = DataManager.instance.nowPlayer.LetterText;
            draw_LetterNum.text = DataManager.instance.nowPlayer.LetterNum+"번째 편지";
            LetterUi.gameObject.SetActive(false);
           
        }

        void Update()
        {
            #region ui 창 player 앞에 띄우는 코드
            if (Input.GetKeyDown(KeyCode.Space)) //이곳에 VR 기기를 눌렀을 때로 변경
            {
                LetterUi.gameObject.SetActive(true);
                LetterUi.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized*spawnDistance;
            }
            LetterUi.transform.LookAt(new Vector3(head.position.x,LetterUi.transform.position.y,head.position.z));
            #endregion
            #region ui 창버튼을 누르면 나타나고 떼면 없어지고
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LetterUi.gameObject.SetActive(true);
                /*
                Material newMaterial = new Material(trailMaterial[0]);
                newMaterial.color = fcp.color; 
                trailMaterial[trailMaterial_index] = newMaterial;
                trailRenderer.materials = trailMaterial;
                trailMaterial_index++;
                */
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                LetterUi.gameObject.SetActive(false);
            }
            #endregion

            #region 색상변경
            //trailRenderer.startColor = fcp.color;
            //trailRenderer.endColor = fcp.color;
            trailRenderer.material.color = fcp.color;

            #endregion
        }
    

        //저장버튼
        public void Save()
        {
            DataManager.instance.SaveData();
            
        }


        public void Drawing()
        {
            //그림그리는 코드 입력
        }



        public void showLetter()
        {

        }


        

    
    }
    

}
