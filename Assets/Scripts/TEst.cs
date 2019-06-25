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
        _animator.ResetTrigger("hadouken");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("punch");
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetTrigger("hadouken");
        }

        var crouched = Input.GetKey(KeyCode.DownArrow);
        var walking = Input.GetKey(KeyCode.RightArrow);
        _animator.SetBool("crouched", crouched);
        _animator.SetBool("walking", walking);
    }
}
