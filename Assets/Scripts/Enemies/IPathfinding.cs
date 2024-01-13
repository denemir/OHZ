using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinding
{
    void FindInitialPath(Transform target); //find target immediately upon spawn
    void FindPath(Transform target);
}
