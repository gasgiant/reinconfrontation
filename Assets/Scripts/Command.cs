using UnityEngine;

public struct Command
{
    public float time;
    public Vector3 impulse;
    public Vector3 position;
    public Quaternion rotation;

    public Command(float _time, Vector3 _impulse, Vector3 _position, Quaternion _rotation)
    {
        time = _time;
        impulse = _impulse;
        position = _position;
        rotation = _rotation;
    }
}
