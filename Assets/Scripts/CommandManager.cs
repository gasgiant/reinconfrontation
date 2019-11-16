using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance;

    [SerializeField]
    private PlayerController executor;

    private List<Command> currentRun = new List<Command>();
    private List<Command> previouseRun = new List<Command>();
    private float recordStartTime;
    private float TimeSinceRecordStart { get { return Time.time - recordStartTime; } }
    private Coroutine executionRoutine;
    private bool recording;



    private void Awake()
    {
        Instance = this;
    }

    public void RememberCommand(Vector3 impulse, Vector3 position, Quaternion rotation)
    {
        if (!recording)
        {
            recording = true;
            recordStartTime = Time.time;
            ExecuteCommands();
        }
        currentRun.Add(new Command(TimeSinceRecordStart, impulse, position, rotation));
    }

    public void ClearCurrentRun()
    {
        recording = false;
        currentRun.Clear();
    }

    public void SaveRun()
    {
        previouseRun.Clear();
        foreach (var command in currentRun)
        {
            previouseRun.Add(command);
        }
        ClearCurrentRun();
    }

    public void ExecuteCommands()
    {
        executionRoutine = StartCoroutine(Execution(previouseRun));
    }

    public void TerminateExecution()
    {
        if (executionRoutine != null)
            StopCoroutine(executionRoutine);
    }

    IEnumerator Execution(List<Command> commands)
    {
        bool firstRun = true;
        while (true)
        {
            float startTime = Time.time;
            float previousTime = 0;
            foreach (var command in commands)
            {
                float t = 0;
                Quaternion startRotation = executor.transform.rotation;
                while (Time.time < startTime + command.time)
                {
                    t += Time.deltaTime / (command.time - previousTime) * 3;
                    executor.transform.rotation = Quaternion.Lerp(startRotation, command.rotation, t);
                    yield return null;
                }
                previousTime = command.time;
                executor.ApplyImpulse(-command.impulse);
                BulletSpawner.Instance.SpawnBullet(-command.position,
                    command.impulse.normalized, 0);//Time.time - startTime - command.time);
                if (firstRun)
                {
                    executor.transform.position = -command.position;
                    executor.transform.rotation = command.rotation;
                }
            }
            yield return new WaitForSeconds(2);
            firstRun = false;
        }
    }

}
