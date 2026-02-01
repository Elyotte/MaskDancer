using Godot;
using System;

public partial class DancerAnchor : Node3D
{
	// _____________________private_________________________

	private const float SPIN_DURATION = 0.8f;
	[Export] public Dancer dancer;
	[Export] private Timer turnAround;

	private void UpdateAnim()
	{
		dancer.UpdateAnim();
	}

	private void SetBackwards(bool backwards)
	{
		dancer.SetBackwards(backwards);
	}

	// _____________________public_________________________

	public override void _Ready()
	{
		turnAround.WaitTime = SPIN_DURATION / 2.0f;
	}

	public void Flip()
	{
		SetBackwards(!dancer.backwards);
	}

	public void Step()
	{
		dancer.Play();
	}

	public void Spin(bool clockwise)
	{
		float target = clockwise ? Mathf.Pi : -Mathf.Pi;
		GetTree().CreateTween().TweenProperty(dancer, "rotation:y", target, SPIN_DURATION).AsRelative();
		turnAround.Start();
	}
}
