using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] bool isAvailable = false;

    int number;
    int x;
    int y;

    public void Init(int num, int x, int y)
    {
        this.number = num;
        this.x = x;
        this.y = y;
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

    public bool IsInside(Vector2Int minPos, Vector2Int maxPos)
    {
        if (minPos.x <= x && x <= maxPos.x &&
            minPos.y <= y && y <= maxPos.y)
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

    public void SetAvailable(bool isAvailable)
    {
        this.isAvailable = isAvailable;
    }

    public Vector2Int GetPos()
    {
        return new Vector2Int(x, y);
    }

    public void SetSpriteColor(bool isSet)
    {
        if (isSet == true)
        {
            if (isAvailable == true)
            {
                sr.color = Color.black;
            }
            else
            {
                sr.color = Color.white;
            }
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
