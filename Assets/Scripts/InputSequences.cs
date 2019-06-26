using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSequences 
{

    public static readonly KeyEvent[] _hadoukenSequence = new KeyEvent[] { new KeyEvent(KeyCode.DownArrow, true),
        new KeyEvent(KeyCode.RightArrow, true),
        new KeyEvent(KeyCode.DownArrow, false),
        new KeyEvent(KeyCode.Space, true)};

    public static readonly KeyEvent[] _alternativeHadoukenSequence = new KeyEvent[] { new KeyEvent(KeyCode.DownArrow, true),
        new KeyEvent(KeyCode.RightArrow, true),
        new KeyEvent(KeyCode.DownArrow, false),
        new KeyEvent(KeyCode.RightArrow, false),
        new KeyEvent(KeyCode.Space, true)};
}
