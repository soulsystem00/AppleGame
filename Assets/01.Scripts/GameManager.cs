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

    //    // ��� ����鿡 ���� SetAvailable(false) ȣ���Ͽ� ���� �ʱ�ȭ
    //    foreach (var apple in apples)
    //    {
    //        apple.SetAvailable(false);  // ��� ����� �ʱ�ȭ
    //    }

    //    // ����� ��ġ���� ����
    //    var applePositions = new Dictionary<Vector2Int, int>();
    //    foreach (var apple in apples)
    //    {
    //        Vector2Int pos = apple.GetPos();
    //        applePositions[pos] = apple.GetNumber();
    //    }

    //    bool foundValidRectangle = false;

    //    // ������ �簢�� ���� Ž�� (1x1 ���� widthCount x heightCount����)
    //    for (int x1 = 0; x1 < widthCount; x1++)
    //    {
    //        for (int y1 = 0; y1 < heightCount; y1++)
    //        {
    //            // (x1, y1)���� �����ؼ� ���� x2, y2�� ����
    //            for (int x2 = x1; x2 < widthCount; x2++)
    //            {
    //                for (int y2 = y1; y2 < heightCount; y2++)
    //                {
    //                    // �� �簢�� ���� ���� ���� ���
    //                    int sum = CalculateRectangleSum(applePositions, x1, y1, x2, y2);

    //                    if (sum == 10)
    //                    {
    //                        // ���� 10�� ���, �ش� ������ ���Ե� ������� SetAvailable(true)
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
    //                        // ���� 10�� �ʰ��ϸ� �� �̻� ������ �ʿ� ����
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return foundValidRectangle; // ��� �ϳ��� ��ȿ�� �簢���� ã���� true
    //}

    //// �簢�� ���� ���� ���� ����ϴ� �Լ�
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
    //                sum += applePositions[pos];  // �ش� ��ġ�� ����� ������ �� ���� ����
    //            }
    //        }
    //    }

    //    return sum;
    //}

    //// �簢�� ���� ���� ���� ���ԵǴ��� Ȯ���ϴ� �Լ�
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
