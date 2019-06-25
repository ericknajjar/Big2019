using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public partial class ReactiveInput : MonoBehaviour
{
    Animator _animator;

    static readonly KeyEvent[] _hadoukenSequence = new KeyEvent[] { new KeyEvent(KeyCode.DownArrow, true),
        new KeyEvent(KeyCode.RightArrow, true),
        new KeyEvent(KeyCode.DownArrow, false),
        new KeyEvent(KeyCode.Space, true)};

    static readonly KeyEvent[] _alternativeHadoukenSequence = new KeyEvent[] { new KeyEvent(KeyCode.DownArrow, true),
        new KeyEvent(KeyCode.RightArrow, true),
        new KeyEvent(KeyCode.DownArrow, false),
        new KeyEvent(KeyCode.RightArrow, false),
        new KeyEvent(KeyCode.Space, true)};

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        var punch = Create(KeyCode.Space);
        var crouch = Create(KeyCode.DownArrow);
        var walk = Create(KeyCode.RightArrow);

        punch.Where(_ => _.Pressed).Subscribe(_ => _animator.SetTrigger("punch"));
        punch.Where(_ => !_.Pressed).Subscribe(_ => _animator.ResetTrigger("punch"));
        crouch.Subscribe(_ => _animator.SetBool("crouched",_.Pressed));
        walk.Subscribe(_ => _animator.SetBool("walking", _.Pressed));

        SequenceDetector(punch.Merge(crouch).Merge(walk), 0.3f,_hadoukenSequence).Subscribe(_=> {

            _animator.SetTrigger("hadouken");
        });

    }


    IObservable<Unit> SequenceDetector(IObservable<KeyEvent> source, float windowInSeconds,params IList<KeyEvent>[] sequences)
    {
        return sequences.Select(sequence => {

            var startWindow = source.Where(_ => _.Equals(sequence.First())).Share();

            var sequenceWithoutFirst = sequence.Skip(1);

            return startWindow.SelectMany(keyEvent => {

                return source.Buffer(TimeSpan.FromSeconds(windowInSeconds), sequenceWithoutFirst.Count())
                .TakeUntil(startWindow)
                .Where(_ => _.SequenceEqual(sequenceWithoutFirst)).Take(1);

            }).AsUnitObservable().Share();

        }).Merge().Share();
       
    }


    static IObservable<KeyEvent> Create(KeyCode key)
    {
        var down = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(key)).Select(_=> new KeyEvent(key,true));
        var up = Observable.EveryUpdate().Where(_ => Input.GetKeyUp(key)).Select(_ => new KeyEvent(key, false));

        return down.Merge(up).Share();
    }


}
