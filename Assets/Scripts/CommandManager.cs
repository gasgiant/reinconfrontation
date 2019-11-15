using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance;

    [SerializeField]
    private PlayerController executor;

    private List<Command> commands = new List<Command>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteCommands();
        }
    }

    public void ExecuteCommands()
    {
        StartCoroutine(Execution());
    }

    public void RememberCommand(Vector3 impulse, Vector3 position, Quaternion rotation)
    {
        commands.Add(new Command(Time.time, impulse, position, rotation));
    }

    IEnumerator Execution()
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
            executor.ApplyImpulse(command.impulse);
            executor.transform.position = command.position;
            executor.transform.rotation = command.rotation;
        }
    }

}
