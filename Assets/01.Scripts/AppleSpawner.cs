using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    [SerializeField] GameObject applePrefab;
    [SerializeField] int widthCount;
    [SerializeField] int heightCount;

    public int width = 1;
    public int height = 1;
    public float widthOffset = -8.5f;
    public float heightOffset = -4.5f;

    List<Apple> appleList = new List<Apple>();

    public void SpawnApples()
    {
        if (appleList == null)
        {
            appleList = new List<Apple>();
        }

        for (int i = 0; i < widthCount; i++)
        {
            for (int j = 0; j < heightCount; j++)
            {
                var apple = Instantiate(applePrefab, new Vector3(i * width + widthOffset, j * height + heightOffset, 0), Quaternion.identity).GetComponent<Apple>();
                int randomNumber = Random.Range(1, 10);
                apple.Init(randomNumber);

                apple.transform.SetParent(this.transform);

                appleList.Add(apple);
            }
        }
    }

    public List<Apple> GetApples()
    {
        return appleList;
    }
}
