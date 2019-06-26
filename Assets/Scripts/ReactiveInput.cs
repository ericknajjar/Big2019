using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public partial class ReactiveInput : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Time.timeScale = 0.3f;
    }

    void Start()
    {
        var punch = CreateKeyStream(KeyCode.Space);
        var crouch = CreateKeyStream(KeyCode.DownArrow);
        var walk = CreateKeyStream(KeyCode.RightArrow);

        punch.Where(_ => _.Pressed).Subscribe(_ => _animator.SetTrigger("punch"));
        punch.Where(_ => !_.Pressed).Subscribe(_ => _animator.ResetTrigger("punch"));
        crouch.Subscribe(_ => _animator.SetBool("crouched", _.Pressed));
        walk.Subscribe(_ => _animator.SetBool("walking", _.Pressed));

        var hadoukenStream = SequenceDetector(punch.Merge(crouch).Merge(walk), 0.3f, InputSequences._hadoukenSequence, InputSequences._alternativeHadoukenSequence);
        var sheikoHaduken = hadoukenStream.SelectMany(_ => hadoukenStream.TakeUntil(Observable.Timer(TimeSpan.FromSeconds(0.9f))).Take(1));

        hadoukenStream.Subscribe(_ =>
        {

            _animator.SetTrigger("hadouken");
        });

        sheikoHaduken.Subscribe(_ =>
        {
            _animator.ResetTrigger("hadouken");
            _animator.SetTrigger("cancelHadouken");
            _animator.SetTrigger("sheikoHadouken");
        });
    }

    IObservable<Unit> SequenceDetector(IObservable<KeyEvent> source, float windowInSeconds, params IList<KeyEvent>[] sequences)
    {
        return sequences.Select(sequence =>
        {

            var startWindow = source.Where(_ => _.Equals(sequence.First())).Share();

            var sequenceWithoutFirst = sequence.Skip(1);

            return startWindow.SelectMany(keyEvent =>
            {

                return source.Buffer(TimeSpan.FromSeconds(windowInSeconds), sequenceWithoutFirst.Count())
                .TakeUntil(startWindow)
                .Where(_ => _.SequenceEqual(sequenceWithoutFirst)).Take(1);

            }).AsUnitObservable();

        }).Merge().Share();
    }

    static IObservable<KeyEvent> CreateKeyStream(KeyCode key)
    {
        var down = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(key)).Select(_ => new KeyEvent(key, true));
        var up = Observable.EveryUpdate().Where(_ => Input.GetKeyUp(key)).Select(_ => new KeyEvent(key, false));

        return down.Merge(up).Share();
    }
}