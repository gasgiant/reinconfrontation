using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static UnityEvent ResetEvent = new UnityEvent();
    public bool EnableControl;

    private void Awake()
    {
        Instance = this;
        EnableControl = true;
    }

    public void FinishRound(bool win)
    {
        CommandManager.Instance.TerminateExecution();
        if (win)
            CommandManager.Instance.SaveRun();
        else
            CommandManager.Instance.ClearCurrentRun();
        StartCoroutine(FinishRoutine());
        
    }

    IEnumerator FinishRoutine()
    {
        EnableControl = false;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        BulletSpawner.Instance.DestroyAllBullets();
        ResetGame();
    }

    private void ResetGame()
    {
        if (ResetEvent != null)
            ResetEvent.Invoke();
        EnableControl = true;
    }
}
