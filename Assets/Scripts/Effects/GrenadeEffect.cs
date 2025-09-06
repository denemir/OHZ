using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrenadeEffect : MonoBehaviour
{
    public float EffectDuration { get; set; }

    public abstract void ExecuteEffect(Vector3 position, GameObject executer);
}
