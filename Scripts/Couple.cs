using Godot;
using System;

public partial class Couple : Path3D
{
    [Export] PathFollow3D anchor;

    float stepDuration = 0.7f;
    float elapsed;
    private const float dancerDist = 0.13f;
    [Export] float moveSpeed = .3f;
    float distanceEachStepInMeter = .4f;
    float distanceToParkour;

    Action<double> action;
    Tween currentTween;

    float startProgress;
    float endProgress;
    float rotationOffset = 0.35f;


    public override void _Ready()
    {
        CoupleManager.AddCouple(this);
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
        if (Input.IsActionJustPressed("spin"))
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
    void PlayStep(double delta)
    {
        float lerp = Mathf.Pow(elapsed, 2) / moveSpeed;
        anchor.Progress = Mathf.Lerp(startProgress, endProgress, lerp);
        elapsed += (float)delta;
        if (lerp >= 1)
        {
            anchor.Progress = endProgress;
            elapsed = 0;
            SetListenInput();
        }
    }

    public override void _ExitTree()
    {
        CoupleManager.RemoveCouple(this);
        base._ExitTree();
    }
}
