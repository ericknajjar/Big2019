using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TimedKeyEvent = System.Tuple<KeyEvent, float>;

public class ClassicInput : MonoBehaviour
{
    Animator _animator;
    List<TimedKeyEvent> _keyBufferSequence1 = new List<TimedKeyEvent>();
    List<TimedKeyEvent> _keyBufferSequence2 = new List<TimedKeyEvent>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        var thisFrameEvents = GetKeyEvents(KeyCode.Space, KeyCode.DownArrow, KeyCode.RightArrow);

        if(thisFrameEvents.Count>0)
        {
            _keyBufferSequence1.AddRange(thisFrameEvents.Select(_ => Tuple.Create(_, Time.time)));
            _keyBufferSequence2.AddRange(thisFrameEvents.Select(_ => Tuple.Create(_, Time.time)));
        }
            
        var punch = thisFrameEvents.FirstOrDefault(_ => _.Key == KeyCode.Space);
        var crouch = thisFrameEvents.FirstOrDefault(_ => _.Key == KeyCode.DownArrow);
        var walk = thisFrameEvents.FirstOrDefault(_ => _.Key == KeyCode.RightArrow);
    
        if (punch.Key!=KeyCode.None)
        {
            if(!punch.Pressed)
                _animator.ResetTrigger("punch");
            else
                _animator.SetTrigger("punch");
        }

        if (crouch.Key != KeyCode.None)
        {
            _animator.SetBool("crouched", crouch.Pressed);
        }

        if (walk.Key != KeyCode.None)
        {
            _animator.SetBool("walking", walk.Pressed);
        }

        if(SequenceDetector(_keyBufferSequence1,0.3f,InputSequences._hadoukenSequence)
            || SequenceDetector(_keyBufferSequence2, 0.3f, InputSequences._alternativeHadoukenSequence))
        {
            _keyBufferSequence1.Clear();
            _keyBufferSequence2.Clear();
            _animator.SetTrigger("hadouken");
        }
    }

    bool SequenceDetector(IList<TimedKeyEvent> sequenceBuffer, float windowInSeconds, IList<KeyEvent> sequence)
    {
        if(sequenceBuffer.Count()  != 0)
        {
            var first = sequenceBuffer.First();
            var deltaTimeFirstInput = Time.time - first.Item2;

            if (deltaTimeFirstInput > windowInSeconds 
                || !first.Item1.Equals(sequence.First()))
            {
                sequenceBuffer.Clear();
                return false;
            }

            if(sequenceBuffer.Count() == sequence.Count())
            {
                if (sequenceBuffer.Select(_ => _.Item1).SequenceEqual(sequence))
                {
                    sequenceBuffer.Clear();
                    return true;
                }
            }         
        }

        return false;
    }

    IList<KeyEvent> GetKeyEvents(params KeyCode[] keys)
    {
        var ret = new List<KeyEvent>(keys.Length);

        for (int i=0;i<keys.Length;++i)
        {
            var key = keys[i];
            if(Input.GetKeyDown(key))
            {
                ret.Add(new KeyEvent(key, true));
            }
            else if(Input.GetKeyUp(key))
            {
                ret.Add(new KeyEvent(key, false));
            }
        }

        return ret;
    }
}