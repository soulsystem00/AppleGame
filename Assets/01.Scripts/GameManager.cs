using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] AppleSpawner appleSpawner;
    [SerializeField] InputManager inputManager;

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

        inputManager.OnAppleMouseUp += InputManager_OnAppleMouseUp;
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

        foreach (var apple in insideApples)
        {
            apples.Remove(apple);
            Destroy(apple.gameObject);
            Debug.Log("Apple Destoryed");
        }
    }
}
