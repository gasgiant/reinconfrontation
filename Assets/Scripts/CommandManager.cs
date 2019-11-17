using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance;
    [SerializeField]
    private GameObject spawnSquare;
    [SerializeField]
    private PlayerController executorPrefab;
    private List<PlayerController> enemies = new List<PlayerController>();
    private List<List<Command>> allRuns = new List<List<Command>>();
    private List<Command> currentRun = new List<Command>();
    
    private List<Coroutine> executionRotines = new List<Coroutine>();
    private float recordStartTime;
    private float TimeSinceRecordStart { get { return Time.time - recordStartTime; } }
    private Coroutine executionRoutine;
    private bool recording;

    public int EnemiesCount { get { return enemies.Count; } }

    private void Awake()
    {
        Instance = this;
        allRuns.Add(new List<Command>());
    }

    public void ResetRuns()
    {
        allRuns.Clear();
        allRuns.Add(new List<Command>());
    }

    public void RemoveEnemy(PlayerController enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (enemies.Count == 0)
            GameManager.Instance.FinishRound(true);
    }

    public void RememberCommand(Vector3 impulse, Vector3 position, Quaternion rotation)
    {
        if (!recording)
        {
            spawnSquare.SetActive(false);
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
        List<Command> previouseRun = new List<Command>();
        foreach (var command in currentRun)
        {
            previouseRun.Add(command);
        }
        allRuns.Add(previouseRun);
        ClearCurrentRun();
    }

    public void ExecuteCommands()
    {
        foreach (var run in allRuns)
        {
            PlayerController enemy = Instantiate(executorPrefab, Vector3.right * 5, Quaternion.identity);
            enemies.Add(enemy);
        }

        for (int i = 0; i < allRuns.Count; i++)
        {
            executionRotines.Add(StartCoroutine(Execution(allRuns[i], enemies[i])));
        }
    }

    public void TerminateExecution()
    {
        spawnSquare.SetActive(true);
        foreach (var routine in executionRotines)
        {
            if (routine != null)
                StopCoroutine(routine);
        }
        executionRotines.Clear();
        enemies.Clear();
    }

    IEnumerator Execution(List<Command> commands, PlayerController executor)
    {
        float startTime = Time.time;
        float previousTime = 0;
        foreach (var command in commands)
        {
            float t = 0;
            Quaternion startRotation = new Quaternion();
            if (executor != null)
                startRotation = executor.transform.rotation;
            while (Time.time < startTime + command.time)
            {
                t += Time.deltaTime / (command.time - previousTime) * 3;
                if (executor != null)
                    executor.transform.rotation = Quaternion.Lerp(startRotation, command.rotation, t);
                yield return null;
            }
            if (executor != null)
            {
                previousTime = command.time;
                executor.ApplyImpulse(-command.impulse);
                BulletSpawner.Instance.SpawnBullet(-command.position,
                    command.impulse.normalized, 0, true);//Time.time - startTime - command.time);
                executor.transform.position = -command.position;
                executor.transform.rotation = command.rotation;
            }
        }
    }

}
