using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool checkAvailable = false;
    [SerializeField] AppleSpawner appleSpawner;
    [SerializeField] InputManager inputManager;
    [SerializeField] AudioSource audio;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float maxTime;

    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Button resultButton;

    int score = 0;
    int widthCount;
    int heightCount;

    void Start()
    {
        if (appleSpawner == null)
        {
            appleSpawner = FindObjectOfType<AppleSpawner>();

            if (appleSpawner == null)
            {
                appleSpawner = gameObject.AddComponent<AppleSpawner>();
            }
        }

        appleSpawner.SpawnApples();
        resultPanel.SetActive(false);
        scoreText.text = $"Score : {score.ToString()}";

        resultButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });


        widthCount = appleSpawner.GetWidthCount();
        heightCount = appleSpawner.GetHeightCount();

        inputManager.OnAppleMouseUp += InputManager_OnAppleMouseUp;
        inputManager.OnKeyPressed += InputManager_OnKeyPressed;

        IsAppleAvailable();
    }

    private void Update()
    {
        if (maxTime > 0)
        {
            maxTime -= Time.deltaTime;
            timerText.text = maxTime.ToString("F0");
        }


        if (maxTime <= 0)
        {
            maxTime = 0;
            timerText.text = maxTime.ToString("F0");

            inputManager.gameObject.SetActive(false);
            audio.Stop();

            resultPanel.SetActive(true);
            resultText.text = $"Your Score : {score.ToString()}";
        }
    }

    private void InputManager_OnAppleMouseUp(Vector2 minPos, Vector2 maxPos)
    {
        var apples = appleSpawner.GetApples();
        var insideApples = apples.Where(x => x.IsInside(minPos, maxPos) == true).ToList();

        if (insideApples.Count <= 0)
        {
            return;
        }

        int sum = insideApples.Sum(x => x.GetNumber());

        if (sum != 10)
        {
            Debug.Log($"Sum = {sum}");
            return;
        }

        score += insideApples.Count;
        scoreText.text = $"Score : {score.ToString()}";

        foreach (var apple in insideApples)
        {
            apples.Remove(apple);
            Destroy(apple.gameObject);
            Debug.Log("Apple Destoryed");
        }

        if (checkAvailable == true)
        {
            if (IsAppleAvailable() == true)
            {
                Debug.Log("Apple Available");
            }
            else
            {
                Debug.Log("No Apple Available");
            }
        }
    }

    private bool IsAppleAvailable()
    {
        bool result = false;

        var apples = appleSpawner.GetApples();

        if (apples.Count <= 0)
        {
            return false;
        }

        Dictionary<Vector2Int, Apple> appleDict = new Dictionary<Vector2Int, Apple>();

        foreach (Apple apple in apples)
        {
            Vector2Int pos = apple.GetPos();
            appleDict[pos] = apple;

            apple.SetAvailable(false);
        }

        foreach (Apple apple in apples)
        {
            Vector2Int minPos = apple.GetPos();

            for (int i = minPos.x; i < widthCount; i++)
            {
                for (int j = minPos.y; j < heightCount; j++)
                {
                    if (i == minPos.x && j == minPos.y)
                    {
                        continue;
                    }

                    Vector2Int maxPos = new Vector2Int(i, j);

                    int sum = GetRectangleSum(appleDict, minPos, maxPos);

                    if (sum == 10)
                    {
                        result = true;
                        SetAvailable(appleDict, minPos, maxPos);
                    }
                    else if (sum > 10)
                    {
                        break;
                    }
                }
            }
        }

        return result;
    }

    private int GetRectangleSum(Dictionary<Vector2Int, Apple> grids, Vector2Int minPos, Vector2Int maxPos)
    {
        int sum = 0;

        for (int i = minPos.x; i <= maxPos.x; i++)
        {
            for (int j = minPos.y; j <= maxPos.y; j++)
            {
                var curPos = new Vector2Int(i, j);
                if (grids.ContainsKey(curPos))
                {
                    sum += grids[curPos].GetNumber();
                }
            }
        }


        return sum;
    }

    private void SetAvailable(Dictionary<Vector2Int, Apple> grids, Vector2Int minPos, Vector2Int maxPos)
    {
        for (int i = minPos.x; i <= maxPos.x; i++)
        {
            for (int j = minPos.y; j <= maxPos.y; j++)
            {
                var curPos = new Vector2Int(i, j);

                if (grids.ContainsKey(curPos))
                {
                    grids[curPos].SetAvailable(true);
                }
            }
        }
    }


    #region GPT Code

    //private bool IsAppleAvailable()
    //{
    //    var apples = appleSpawner.GetApples();
    //    if (apples.Count <= 0) return false;

    //    // 모든 사과들에 대해 SetAvailable(false) 호출하여 상태 초기화
    //    foreach (var apple in apples)
    //    {
    //        apple.SetAvailable(false);  // 모든 사과를 초기화
    //    }

    //    // 사과를 위치별로 저장
    //    var applePositions = new Dictionary<Vector2Int, int>();
    //    foreach (var apple in apples)
    //    {
    //        Vector2Int pos = apple.GetPos();
    //        applePositions[pos] = apple.GetNumber();
    //    }

    //    bool foundValidRectangle = false;

    //    // 가능한 사각형 범위 탐색 (1x1 부터 widthCount x heightCount까지)
    //    for (int x1 = 0; x1 < widthCount; x1++)
    //    {
    //        for (int y1 = 0; y1 < heightCount; y1++)
    //        {
    //            // (x1, y1)에서 시작해서 끝을 x2, y2로 설정
    //            for (int x2 = x1; x2 < widthCount; x2++)
    //            {
    //                for (int y2 = y1; y2 < heightCount; y2++)
    //                {
    //                    // 이 사각형 범위 내의 합을 계산
    //                    int sum = CalculateRectangleSum(applePositions, x1, y1, x2, y2);

    //                    if (sum == 10)
    //                    {
    //                        // 합이 10인 경우, 해당 범위에 포함된 사과들을 SetAvailable(true)
    //                        foreach (var pos in applePositions.Keys)
    //                        {
    //                            if (IsInside(pos, new Vector2Int(x1, y1), new Vector2Int(x2, y2)))
    //                            {
    //                                var apple = apples.FirstOrDefault(a => a.GetPos() == pos);
    //                                if (apple != null)
    //                                {
    //                                    apple.SetAvailable(true);
    //                                }
    //                            }
    //                        }
    //                        foundValidRectangle = true;
    //                    }
    //                    else if (sum > 10)
    //                    {
    //                        // 합이 10을 초과하면 더 이상 진행할 필요 없음
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return foundValidRectangle; // 적어도 하나의 유효한 사각형을 찾으면 true
    //}

    //// 사각형 범위 내의 합을 계산하는 함수
    //private int CalculateRectangleSum(Dictionary<Vector2Int, int> applePositions, int x1, int y1, int x2, int y2)
    //{
    //    int sum = 0;

    //    for (int x = x1; x <= x2; x++)
    //    {
    //        for (int y = y1; y <= y2; y++)
    //        {
    //            var pos = new Vector2Int(x, y);
    //            if (applePositions.ContainsKey(pos))
    //            {
    //                sum += applePositions[pos];  // 해당 위치에 사과가 있으면 그 값을 더함
    //            }
    //        }
    //    }

    //    return sum;
    //}

    //// 사각형 범위 내의 점이 포함되는지 확인하는 함수
    //private bool IsInside(Vector2Int point, Vector2Int minPos, Vector2Int maxPos)
    //{
    //    return point.x >= minPos.x && point.x <= maxPos.x && point.y >= minPos.y && point.y <= maxPos.y;
    //}
    #endregion

    private void InputManager_OnKeyPressed(bool value)
    {
        var apples = appleSpawner.GetApples();

        foreach (Apple apple in apples)
        {
            apple.SetSpriteColor(value);
        }
    }
}
