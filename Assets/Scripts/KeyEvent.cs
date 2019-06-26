using UnityEngine;

public struct KeyEvent
{
    public readonly KeyCode Key;
    public readonly bool Pressed;
 
    public KeyEvent(KeyCode key,bool pressed)
    {
        Key = key;
        Pressed = pressed;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is KeyEvent))
        {
            return false;
        }

        var @event = (KeyEvent)obj;
        return Key == @event.Key &&
                Pressed == @event.Pressed;
    }

    public override int GetHashCode()
    {
        var hashCode = -1420180815;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + Key.GetHashCode();
        hashCode = hashCode * -1521134295 + Pressed.GetHashCode();
        return hashCode;
    }
}

