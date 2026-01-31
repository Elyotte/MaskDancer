using Godot;
using System;

public partial class Couple : Path3D
{
	private const float SPIN_DURATION = 0.8f;
	[Export] PathFollow3D anchor;
    [Export] Dancer dancer1;
    [Export] Dancer dancer2;

    float elapsed;
	private Vector3 center => (dancer1.Position + dancer2.Position) * 0.5f;
    private const float dancerDist = 0.13f;
    [Export] float moveSpeed = .3f;
    float distanceEachStepInMeter = .4f;
    float distanceToParkour;

    Action<double> action;

    float startProgress;
    float endProgress;

    private void SetRotation(float tp)
    {
        float t = tp;
        dancer1.Position = new Vector3(dancerDist * Mathf.Cos(Mathf.Tau * t), 0.0f, dancerDist * Mathf.Sin(Mathf.Tau * t));
        dancer2.Position = new Vector3(dancerDist * Mathf.Cos(Mathf.Tau * t + Mathf.Pi), 0.0f, dancerDist * Mathf.Sin(Mathf.Tau * t + Mathf.Pi));
    }

    private void Spin()
    {
        dancer1.Spin();
        dancer2.Spin();
        Callable callmdr = new Callable(this, nameof(SetRotation));
        GetTree().CreateTween().TweenMethod(callmdr, 0.0f, 1.0f, SPIN_DURATION);
    }

    public override void _Ready()
    {
        base._Ready();
        action = ListenInput;
        dancer2.Flip();
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

        if (Input.IsActionJustPressed("spin"))
        {
            Spin();
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
