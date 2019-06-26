using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var clickStream = Observable.EveryUpdate()
    .Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Buffer(TimeSpan.FromMilliseconds(250), 2)
            .Where(xs => xs.Count >= 2)
            .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
    }


}
