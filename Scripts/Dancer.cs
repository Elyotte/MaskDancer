using Godot;
using System;

public partial class Dancer : AnimatedSprite3D
{
    public bool backwards { get; private set; } = false;
    public void UpdateAnim()
    {
        Animation = backwards ? "back" : "front";
    }
    public void SetBackwards(bool newVal)
    {
        backwards = newVal;
        UpdateAnim();
    }
}
