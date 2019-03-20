using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.PlugStudio.Patterns;

public class Intro : State
{
    public override IEnumerator Initialize(params object[] _data)
    {
        yield return null;
    }

    public override void FirstFrame()
    {
    }

    public override void Execute()
    {
    }

    public override void Release()
    {
    }
}
