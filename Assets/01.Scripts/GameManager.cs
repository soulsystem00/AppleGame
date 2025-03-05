using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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

        if (IsAppleAvailable() == true)
        {
            Debug.Log("Apple Available");
        }
        else
        {
            Debug.Log("No Apple Available");
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

        foreach (Apple apple in apples)
        {
            apple.SetAvailable(false);
        }

        foreach (Apple apple in apples)
        {
            Vector2Int minPos = apple.GetPos();

            for (int i = 0; i < widthCount; i++)
            {
                for (int j = 0; j < heightCount; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    Vector2Int maxPos = new Vector2Int(i, j);
                    var availableApples = apples.Where(x => x.IsInside(minPos, maxPos)).ToList();

                    if (availableApples.Count > 0)
                    {
                        int sum = availableApples.Sum(x => x.GetNumber());

                        if (sum == 10)
                        {
                            result = true;

                            foreach (var item in availableApples)
                            {
                                item.SetAvailable(true);
                            }
                        }
                        else if (sum > 10)
                        {
                            break;
                        }
                    }
                }
            }
        }

        return result;
    }

    private void InputManager_OnKeyPressed(bool value)
    {
        var apples = appleSpawner.GetApples();

        foreach (Apple apple in apples)
        {
            apple.SetSpriteColor(value);
        }
    }
}
