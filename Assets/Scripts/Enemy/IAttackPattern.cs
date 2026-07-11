using UnityEngine;

public interface IAttackPattern
{
     bool IsRunning { get; }
     void Execute(Transform target,MonoBehaviour owner);
}