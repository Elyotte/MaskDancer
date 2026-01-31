using Godot;
using System;

public partial class Couple : Path3D
{
	[Export] PathFollow3D anchor;

    float elapsed;
    [Export] float stepDuration = .5f;
    float distanceEachStepInMeter = .4f;
    float distanceToParkour;

    Action<double> action;
    Tween currentTween;

    float startProgress;
    float endProgress;

    public override void _Ready()
    {
        base._Ready();
        action = ListenInput;
    }

    public override void _Process(double delta)
    {
        action?.Invoke(delta);
    }

    void SetListenInput() { 
        action = ListenInput; 
    
    }
    void ListenInput(double delta)
    {
        if (Input.IsActionJustPressed("tap"))
        {
            StartPlayStep();
        }
    }

    void StartPlayStep()
    {
        action = null;

        startProgress = anchor.Progress;
        endProgress = startProgress + distanceEachStepInMeter;
        
        currentTween = GetTree().CreateTween();
        currentTween.TweenProperty(anchor, "progress", endProgress, stepDuration).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Circ);

        currentTween.Finished += SetListenInput;
    }

    void PlayStep()
    {

    }


    
}
