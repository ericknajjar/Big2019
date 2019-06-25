using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadoukenBehaviour : StateMachineBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float _whenToSpawn = 0.75f;

    bool _spawned = false;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!_spawned && stateInfo.normalizedTime >= _whenToSpawn)
        {
            _spawned = true;
            animator.GetComponent<HadoukenSpawner>().SpawnHadouken();
        }

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _spawned = false;
    }
}