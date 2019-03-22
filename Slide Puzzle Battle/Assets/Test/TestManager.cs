using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.PlugStudio.Patterns;

public class TestManager : Singleton<TestManager>
{
    public Canvas canvas;
    public Test testPrefab;
    public int energy;
    public int maxEnergy;
    public float recoveryTime;

    private bool isRecovery;
    private Test createdObject = null;
    private float currentRecoveryTime;

    public bool IsRecovery { get { return isRecovery; } }
    public float RecoveryTime { get { return currentRecoveryTime; } }

    private void Start()
    {
        maxEnergy = 5;
        energy = maxEnergy;
    }

    public void Create()
    {
        if(createdObject != null)
        {
            return;
        }

        createdObject = Instantiate(testPrefab, canvas.transform);
    }

    public void Remove()
    {
        if(createdObject == null)
        {
            return;
        }

        Destroy(createdObject.gameObject);
        createdObject = null;
    }

    public void Use()
    {
        if(energy > 0)
        {
            energy--;

            if(createdObject != null)
            {
                createdObject.SetImages(false);
            }

            if(isRecovery == false)
            {
                isRecovery = true;
                StartCoroutine(Recovery());
            }
        }
    }

    public IEnumerator Recovery()
    {
        currentRecoveryTime = recoveryTime;

        while(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;

            yield return null;
        }

        energy++;
        if (createdObject != null)
        {
            createdObject.SetImages(true);
        }

        if (energy < maxEnergy)
        {
            StartCoroutine(Recovery());
        }
        else
        {
            isRecovery = false;
        }
    }
}
