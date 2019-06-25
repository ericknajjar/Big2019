using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.ResetTrigger("punch");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("punch");
        }
    }
}
