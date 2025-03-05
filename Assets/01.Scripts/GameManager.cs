using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool isCheatMode = false;
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

        while (IsAppleAvailable() == false)
        {
            appleSpawner.SpawnApples();
        }
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

        if (IsAppleAvailable() == true)
        {
            Debug.Log("Apple Available");
        }
        else
        {
            Debug.Log("No Apple Available");

            while (IsAppleAvailable() == false)
            {
                appleSpawner.SpawnApples();
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
            apple.SetAvailable(false);

            if (isCheatMode == true)
            {
                apple.SetSpriteColor(false);
            }

            Vector2Int pos = apple.GetPos();
            appleDict[pos] = apple;
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

                    if (isCheatMode == true)
                    {
                        grids[curPos].SetSpriteColor(true);
                    }
                }
            }
        }
    }

    private void InputManager_OnKeyPressed(bool value)
    {
        if (value == true)
        {
            isCheatMode = !isCheatMode;

            var apples = appleSpawner.GetApples();

            foreach (Apple apple in apples)
            {
                apple.SetSpriteColor(isCheatMode);
            }
        }
    }
}
