using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

namespace Letter
{
    public class ExhibitList_Select : MonoBehaviour
    {
        
        public Text[] SlotText;
        public bool[] savefile = new bool[5];
        List<int> falseIndexes = new List<int>(); 

        void Start()
        {
            falseIndexes.Add(-2);
            for(int i = 0; i<5; i++)
            {
                if(File.Exists(DataManager.instance.path + $"{i}"))
                {
                    savefile[i] = true;
                    DataManager.instance.nowSlot = i;
                    DataManager.instance.LoadData();
                    SlotText[i].text = DataManager.instance.nowPlayer.LetterText;
                    //print("ExhibitList_Select"+DataManager.instance.nowPlayer.LetterText);
                }
                else
                {
                    SlotText[i].text = "없음!";
                    falseIndexes.Add(i);
                }  
            }
            DataManager.instance.DataClear(); 
            MakenowSlot();
        }

    
        void Update()
        {
        
        }

        public void Slot(int slotnum) //전시장List에서 게임 로드
        {
            DataManager.instance.nowSlot = slotnum;
            print(slotnum);

            if(savefile[slotnum])
            {
                DataManager.instance.LoadData();
            }
            GoDrew();
        }

        public void GoDrew()
        {
            SceneManager.LoadScene(2);
        }

        
        public void MakenowSlot() //DataManager로 넘길 nowSlot
        {
            print("MakenowSlot에 들어왔씁니다");
            if(falseIndexes.Count > 1)
            {
                int randomIndex = Random.Range(1, falseIndexes.Count);
                DataManager.instance.nowSlot = falseIndexes[randomIndex];
            }
            else if(falseIndexes.Count <= 1)
            {
                int randomIndex = Random.Range(0, 5);
                DataManager.instance.DataClear(); 
                DataManager.instance.nowSlot = randomIndex;
            }

        }
        
    }

}
