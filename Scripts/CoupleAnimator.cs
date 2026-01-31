using Godot;
using System;

public partial class CoupleAnimator : PathFollow3D
{
    private const float SPIN_DURATION = 0.8f;
    [Export] DancerAnchor dancer1;
    [Export] DancerAnchor dancer2;
    [Export] Timer endRotationTimer;

    float elapsed;
    private Vector3 center => (dancer1.Position + dancer2.Position) * 0.5f;
    private const float dancerDist = 0.13f;
    [Export] float moveSpeed = .3f;
    float distanceEachStepInMeter = .4f;
    float distanceToParkour;

    float startProgress;
    float endProgress;
    float rotationOffset = 0.35f;

    bool spinning = false;

    public override void _Ready()
    {
        base._Ready();
        dancer1.Flip();
        SetRotation(0.0f);
        endRotationTimer.WaitTime = SPIN_DURATION;
    }

    public override void _Process(double delta)
    {
        ListenInput(delta);
    }

    void ListenInput(double delta)
    {
        if (Input.IsActionJustPressed("spin"))
        {
            Spin();

        }
    }
    private void SetRotation(float tp)
    {
        float t = tp + rotationOffset;
        dancer1.Position = new Vector3(dancerDist * Mathf.Cos(Mathf.Pi * t), 0.0f, dancerDist * Mathf.Sin(Mathf.Pi * t));
        dancer2.Position = new Vector3(dancerDist * Mathf.Cos(Mathf.Pi * t + Mathf.Pi), 0.0f, dancerDist * Mathf.Sin(Mathf.Pi * t + Mathf.Pi));
    }

    private void EndRotation()
    {
        SetRotation(1.0f);
        rotationOffset += 1.0f;
        GD.Print(rotationOffset);
    }

    private void Spin()
    {
        if (spinning)
            return;
        spinning = true;
        bool clockwise = Mathf.FloorToInt(rotationOffset) % 2 == 0;
        dancer1.Spin(clockwise);
        dancer2.Spin(!clockwise);
        Callable SetRotation = new Callable(this, nameof(SetRotation));
        Tween tween = GetTree().CreateTween();
        tween.TweenMethod(SetRotation, 0.0f, 1.0f, SPIN_DURATION);
        endRotationTimer.Start();
        tween.TweenCallback(new Callable(this,nameof(ResetSpin)));
        GD.Print("Spining started");
    }

    private void ResetSpin()
    {
        spinning = false;
    }
}
