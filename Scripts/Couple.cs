using Godot;
using System;

public partial class Couple : Path3D
{
	[Export] PathFollow3D anchor;
	[Export] float rythm = 1.2f;
	[Export] Label3D textLabel;

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
		GetNode<Timer>("LabelDisappear").WaitTime = 0.85f * rythm;

	}

	public override void _Process(double delta)
	{
		action?.Invoke(delta);
	}

	private void SetTimingProgression(float t)
	{
		var shaderMaterial = (ShaderMaterial)GetNode<Sprite3D>("Sprite3D").MaterialOverride;
		shaderMaterial.SetShaderParameter("t", t);
	}

	public void Appear(string letter)
	{
		textLabel.Text = letter;
		Visible = true;
		SetTimingProgression(0.0f);
		Callable callableTimingProgression = new Callable(this, nameof(SetTimingProgression));
		GetTree().CreateTween().TweenMethod(callableTimingProgression, 0.0f, 1.0f, rythm);
		GetTree().CreateTween().TweenProperty(textLabel, "modulate:a", 1.0f, 0.15f * rythm);
		GetTree().CreateTween().TweenProperty(textLabel, "outline_modulate:a", 1.0f, 0.15f * rythm);
		GetNode<Timer>("LabelDisappear").Start();
	}
	
	private void MakeLabelDisappear()
	{
		GetTree().CreateTween().TweenProperty(textLabel, "modulate:a", 0.0f, 0.15f * rythm);
		GetTree().CreateTween().TweenProperty(textLabel, "outline_modulate:a", 0.0f, 0.15f * rythm);
	}

	void SetListenInput() { 
		action = ListenInput; 
	
	}
	void ListenInput(double delta)
	{
		if (Input.IsActionJustPressed("spin"))
		{
			StartPlayStep();
			Appear("Y");
		}
	}

	void StartPlayStep()
	{
		action = null;

		startProgress = anchor.Progress;
		endProgress = startProgress + distanceEachStepInMeter;
		
		currentTween = GetTree().CreateTween();
		currentTween.TweenProperty(anchor, "progress", endProgress, stepDuration);

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
