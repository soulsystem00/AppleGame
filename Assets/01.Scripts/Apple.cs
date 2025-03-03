using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] TextMeshPro numberText;

    int number;

    public void Init(int num)
    {
        this.number = num;
        numberText.text = num.ToString();
    }

    public bool IsInside(Vector2 minPos, Vector2 maxPos)
    {
        if (minPos.x <= transform.position.x && transform.position.x <= maxPos.x &&
            minPos.y <= transform.position.y && transform.position.y <= maxPos.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetNumber()
    {
        return number;
    }
}
