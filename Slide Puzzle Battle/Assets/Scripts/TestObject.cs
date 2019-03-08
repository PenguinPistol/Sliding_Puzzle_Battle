﻿using UnityEngine;
using System.Collections;
using com.PlugStudio.Input;

public class TestObject : MonoBehaviour, ITouchObservable
{
    public int index;

    private void Start()
    {
        InputController.Instance.AddObservable(this);
    }

    public void TouchBegan(Vector3 _touchPosition, int _index)
    {
        RaycastHit2D hit = Physics2D.Raycast(_touchPosition, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            hit.collider.SendMessage("MoveRight");
        }
    }

    public void TouchCancel(Vector3 _touchPosition, int _index)
    {
    }

    public void TouchEnded(Vector3 _touchPosition, int _index)
    {
    }

    public void TouchMoved(Vector3 _touchPosition, int _index)
    {
    }
}
