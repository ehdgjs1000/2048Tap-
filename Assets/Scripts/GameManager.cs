using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas gameOverCanvas; //게임이 끝났을떄 setactive = true

    //현재, 다음 숫자 변수
    public Text nowNumTxt;
    public Text nextNumTxt;

    private int nowNum;
    private int nextNum;
    //다음 숫자 확률 변수
    int highNum; //최고 숫자

    //점수 변수
    public Text nowScoreTxt;
    int nowScore = 2;

    public Text highScoreTxt;
    int highScore = 8;


    //아이템 button
    public Button itemBackBtn;
    public Text itemBackTxt;
    int itemBackCount;

    public Button itemEraseBtn;
    public Text itemEraseTxt;
    int itemEraseCount;
    public Text finalScoreTxt;

    public Button storeBtn;

    public Text goldTxt;
    int gold;

    int[] numSet = new int[25];
    int[] RanNumVal = new int[] {2,2,2,2,2,4,4,8};
    public Button[] NumBtnSet;

    AudioSource clickAudio;
    public AudioClip clickClip;

    void Start()
    {
        //최고 점수 playerprefab에서 가져와서 넣기
        //최고 점수 갱신하하면 추후에 계속 update 시키기
        nowNum = 2;
        highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();
        clickAudio = GetComponent<AudioSource>();
    }
    void Awake()
    {
        for(int a = 0; a < 25; a++)
        {
            NumBtnSet[a].GetComponentInChildren<Text>().text = null;
            visited[a] = false;
            isFilled[a] = false;
        }

        

        //#초기 랜덤 숫자 3개 생성
        for(int a = 0; a < 4; a++)
        {
            int val_a = Random.Range(0,5);
            int val_b = Random.Range(0,5);
            int val_nul = Random.Range(0,8);

            SetNum(val_a,val_b, RanNumVal[val_nul]);
        }//#초기 랜덤 숫자 3개 생성
        nextNum = 2;

    }
    bool[] isFilled = new bool[25];
    void Update()
    {
        UpdateInfo();
        UpdateTextColor();
    }
    void UpdateTextColor()
    {
        for (int a = 0; a < 25; a++)
        {
            if (NumBtnSet[a].GetComponentInChildren<Text>().text == "2")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = Color.black;
            }
            else if (NumBtnSet[a].GetComponentInChildren<Text>().text == "4")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = 
                    new Color(150/255f, 65 / 255f, 65 / 255f);
            }
            else if (NumBtnSet[a].GetComponentInChildren<Text>().text == "8")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = 
                    new Color(25 / 255f, 60 / 255f, 150 / 255f);
            }
            else if (NumBtnSet[a].GetComponentInChildren<Text>().text == "16")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = 
                    new Color(25 / 255f, 150 / 255f, 150 / 255f);
            }
            else if (NumBtnSet[a].GetComponentInChildren<Text>().text == "32")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = 
                    new Color(60 / 255f, 70 / 255f, 10 / 255f);
            }
            else if (NumBtnSet[a].GetComponentInChildren<Text>().text == "64")
            {
                NumBtnSet[a].GetComponentInChildren<Text>().color = 
                    new Color(100 / 255f, 30 / 255f, 170 / 255f);
            }
        }
    }
    void UpdateInfo()
    {
        nowNumTxt.text = nowNum.ToString();
        nextNumTxt.text = nextNum.ToString();

        nowScoreTxt.text = nowScore.ToString();
        itemBackTxt.text = itemBackCount.ToString();
        itemEraseTxt.text = itemEraseCount.ToString();

        goldTxt.text = gold.ToString();

        int filledCount = 0;
        for (int a = 0; a < 25; a++)
        {
            //칸 순회하면서 있는지 없는지 확인하기
            //꽉 차 있으면 GameOver
            if (numSet[a] != 0)
            {
                filledCount++;
            }
        }
        if (filledCount >= 25) GameOver();
    }
    void GameOver()
    {
        gameOverCanvas.gameObject.SetActive(true);
        if(PlayerPrefs.GetInt("HighScore") <= nowScore)
        {
            PlayerPrefs.SetInt("HighScore", nowScore);
        }
        finalScoreTxt.text = nowScore.ToString();
    }
    void SetNum(int pos_x,int pos_y, int val) //번호 입력
    {
        int realPos = pos_x * 5 + pos_y;
        numSet[realPos] = val;
        NumBtnSet[realPos].GetComponentInChildren<Text>().text = val.ToString();
    }
    void SetNextNum() // 버튼 누르면 실행
    {
         if(highScore <= 8)
        {
            int a = Random.Range(0,4);
            if (a == 3)
            {
                nextNum = 4;
            } else nextNum = 2;
        }else if(highScore == 16)
        {
            int a = Random.Range(0, 100);
            if (a <= 65) nextNum = 2;
            else if (a > 65 && a <= 90) nextNum = 4;
            else nextNum = 8;
        }else if(highScore == 32)
        {
            int a = Random.Range(0, 100);
            if (a <= 50) nextNum = 2;
            else if(a > 50 && a <= 87) nextNum = 4;
            else if(a > 87 && a <= 97) nextNum = 8;
            else nextNum = 16;
        }else if(highScore == 64)
        {
            int a = Random.Range(0, 100);
            if (a <= 50) nextNum = 2;
            else if (a > 50 && a <= 80) nextNum = 4;
            else if (a > 80 && a <= 94) nextNum = 8;
            else nextNum = 16;
        }else if(highScore == 128)
        {
            int a = Random.Range(0, 100);
            if (a <= 45) nextNum = 2;
            else if (a > 45 && a <= 75) nextNum = 4;
            else if (a > 75 && a <= 91) nextNum = 8;
            else if (a > 91 && a <= 98) nextNum = 16;
            else nextNum = 32;
        }else if(highScore == 256)
        {
            int a = Random.Range(0, 100);
            if (a <= 43) nextNum = 2;
            else if (a > 43 && a <= 65) nextNum = 4;
            else if (a > 65 && a <= 85) nextNum = 8;
            else if (a > 85 && a <= 96) nextNum = 16;
            else nextNum = 32;
        }else if(highScore > 256)
        {
            int a = Random.Range(0, 100);
            if (a <= 40) nextNum = 2;
            else if (a > 40 && a <= 62) nextNum = 4;
            else if (a > 62 && a <= 79) nextNum = 8;
            else if (a > 79 && a <= 92) nextNum = 16;
            else nextNum = 32;
        }
    }
    public void RetryBtnClicked()
    {
        SceneManager.LoadScene(0);

    }

    public void BtnOnClicked()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        if(clickedObject.GetComponentInChildren<Text>().text == "") // 칸이 비어있을 떄
        {
            clickedObject.GetComponentInChildren<Text>().text = nowNum.ToString();

            string tempClickedName = clickedObject.name;
            string clickedNameNum = tempClickedName.Substring(6,2); //클릭 된 버튼 배열 번호
            int num_x = int.Parse(clickedNameNum.Substring(0, 1));
            int num_y = int.Parse(clickedNameNum.Substring(1, 1));
            numSet[num_x*5 + num_y] = nowNum;
            clickedPos = num_x * 5 + num_y;
            sameSpaceNum[0] = clickedPos;

            //같은값 3개 이상일 경우 합치기
            FindSameNum(clickedPos);

            nowNum = nextNum;
            SetNextNum();
        }
        //소리 실행
        clickAudio.PlayOneShot(clickClip);
    }

    bool[] visited = new bool[30];
    int[] sameSpaceNum = new int[30]; //지워저야할 숫자 칸
    int sameCount = 0; //같은 숫자가 몇개 있는지
    int clickedPos;
    
    void TestFind(int pos) // pos = 16
    {
        visited[pos] = true;
        int sameCountTemp = 0;
        int a = pos % 5; //a = 1
        if (pos - 1 >= 0 && a != 0 && !visited[pos - 1] &&
            NumBtnSet[pos - 1].GetComponentInChildren<Text>().text == nowNum.ToString())
        {
            sameCount++;
            sameCountTemp++;
            visited[pos - 1] = true;
            sameSpaceNum[sameCount] = pos - 1;
        }
        if (pos + 1 < 25 && a != 4 && !visited[pos + 1] &&
            NumBtnSet[pos + 1].GetComponentInChildren<Text>().text == nowNum.ToString())
        {
            sameCount++;
            sameCountTemp++;
            visited[pos + 1] = true;
            sameSpaceNum[sameCount] = pos + 1;
        }
        if (pos - 5 >= 0 && !visited[pos - 5] &&
           NumBtnSet[pos - 5].GetComponentInChildren<Text>().text == nowNum.ToString())
        {
            sameCount++;
            sameCountTemp++;
            visited[pos - 5] = true;
            sameSpaceNum[sameCount] = pos - 5;
        }
        if (pos + 5 < 25 && !visited[pos + 5] &&
          NumBtnSet[pos + 5].GetComponentInChildren<Text>().text == nowNum.ToString())
        {
            sameCount++;
            sameCountTemp++;
            visited[pos + 5] = true;
            sameSpaceNum[sameCount] = pos + 5;
        }
        //이어진 같은수 찾기
        for (int turn = 1; turn <= sameCountTemp; turn++)
        {
            TestFind(sameSpaceNum[turn]);
        }
    }

    void FindSameNum(int pos) //주변에 같은 숫자 3개가 있는지 확인 
    {
        TestFind(pos);
        while (sameCount >= 2)
        {
            for(int b = 0; b <= sameCount; b++)
            {
                numSet[sameSpaceNum[b]] = 0;
                NumBtnSet[sameSpaceNum[b]].GetComponentInChildren<Text>().text = "";
            }
            nowNum *= 2;
            numSet[clickedPos] = nowNum;
            NumBtnSet[clickedPos].GetComponentInChildren<Text>().text = nowNum.ToString();

            int tempClickedPos = clickedPos;
            ResetValue();
            sameSpaceNum[0] = tempClickedPos;

            TestFind(pos);
        }

        if(highScore < nowNum)
        {
            highScore = nowNum;
            nowScoreTxt.text = nowNum.ToString();
        }

        nowScore += nowNum;
        //변수 초기화 해주기!!!
        ResetValue();
        clickedPos = 0;
    }
    void ResetValue()
    {
        for (int a = 0; a < 30; a++)
        {
            sameSpaceNum[a] = 0;
            visited[a] = false;
        }
        sameCount = 0;
    }
    

}
