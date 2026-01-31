using Godot;
using System;

public partial class Couple : Path3D
{
	[Export] PathFollow3D anchor;

    float elapsed;
    [Export] float moveSpeed = .3f;
    float distanceEachStepInMeter = .4f;
    float distanceToParkour;

    Action<double> action;

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

    void SetListenInput() { action = ListenInput; }
    void ListenInput(double delta)
    {
        if (Input.IsActionJustPressed("tap"))
        {
            StartPlayStep();
        }
    }

    void StartPlayStep()
    {
        distanceToParkour = distanceEachStepInMeter;
        startProgress = anchor.Progress;
        endProgress = startProgress + distanceEachStepInMeter;
        elapsed = 0;
        action = PlayStep;
    }
    void PlayStep(double delta){
        float lerp = Mathf.Pow(elapsed,2) / moveSpeed;
        anchor.Progress = Mathf.Lerp(startProgress, endProgress, lerp);
        elapsed += (float)delta;
        if (lerp >= 1)
        {
            anchor.Progress = endProgress;
            elapsed = 0;
            SetListenInput();
        }
    }

    
}
