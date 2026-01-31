using Godot;
using System;

public partial class Dancer : Node3D
{
	// _____________________private_________________________

	private const float SPIN_DURATION = 0.8f;
	private bool backwards = false;
	[Export] private AnimatedSprite3D sprite3D;
	[Export] private Timer turnAround;

	private void UpdateAnim()
	{
		sprite3D.Animation = backwards ? "back" : "front";
	}

	private void SetBackwards(bool newVal)
	{
		backwards = newVal;
		UpdateAnim();
	}

	// _____________________public_________________________

	public override void _Ready()
	{
		turnAround.WaitTime = SPIN_DURATION / 2.0f;
	}

	public override void _Process(double delta)
	{

	}

	public void Flip()
	{
		backwards = !backwards;
		SetBackwards(backwards);
	}

	public void Step()
	{
		sprite3D.Play();
	}

	public void Spin()
	{
		GetTree().CreateTween().TweenProperty(sprite3D, "rotation:y", Mathf.Pi, SPIN_DURATION).AsRelative();
		turnAround.Start();
	}
}
