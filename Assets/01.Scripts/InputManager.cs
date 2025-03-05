using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] LineRenderer line;

    Camera mainCam;
    Vector2 startMousePos;
    Vector2 endMousePos;

    bool isDrawing = false;

    private event Action<Vector2, Vector2> onAppleMouseUp;
    public event Action<Vector2, Vector2> OnAppleMouseUp
    {
        add
        {
            onAppleMouseUp -= value;
            onAppleMouseUp += value;
        }
        remove { onAppleMouseUp -= value; }
    }

    private event Action<bool> onKeyPressed;
    public event Action<bool> OnKeyPressed
    {
        add
        {
            onKeyPressed -= value;
            onKeyPressed += value;
        }
        remove { onKeyPressed -= value; }
    }

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        MouseInput();

        HotKeyInput();

        DrawingRectangle();
    }

    private void HotKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            onKeyPressed?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.F1))
        {
            onKeyPressed?.Invoke(false);
        }
    }

    private void DrawingRectangle()
    {
        if (isDrawing == true)
        {
            line.enabled = true;

            line.positionCount = 4;

            float minX = Mathf.Min(startMousePos.x, endMousePos.x);
            float maxX = Mathf.Max(startMousePos.x, endMousePos.x);
            float minY = Mathf.Min(startMousePos.y, endMousePos.y);
            float maxY = Mathf.Max(startMousePos.y, endMousePos.y);

            line.SetPosition(0, new Vector2(minX, maxY));
            line.SetPosition(1, new Vector2(maxX, maxY));
            line.SetPosition(2, new Vector2(maxX, minY));
            line.SetPosition(3, new Vector2(minX, minY));
        }
        else
        {
            line.enabled = false;
            line.positionCount = 0;
        }
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            endMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            isDrawing = true;
        }

        if (Input.GetMouseButton(0))
        {
            endMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            float minX = Mathf.Min(startMousePos.x, endMousePos.x);
            float maxX = Mathf.Max(startMousePos.x, endMousePos.x);
            float minY = Mathf.Min(startMousePos.y, endMousePos.y);
            float maxY = Mathf.Max(startMousePos.y, endMousePos.y);

            Vector2 minPos = new Vector2(minX, minY);
            Vector2 maxPos = new Vector2(maxX, maxY);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            onAppleMouseUp?.Invoke(minPos, maxPos);
            sw.Stop();

            Debug.Log($"Elapsed Time : {sw.ElapsedMilliseconds} ms");

            startMousePos = Vector2.zero;
            endMousePos = Vector2.zero;
            isDrawing = false;
        }
    }
}
