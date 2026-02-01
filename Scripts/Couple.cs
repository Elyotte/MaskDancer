using Godot;
using System;

public partial class Couple : Path3D
{
	[Export] public CoupleAnimator anchor;
	[Export] float rythm = 1.2f; //1.4f pour musique 1 et environ 0.7f pour musique 2
	[Export] Label3D textLabel;
	[Export] float moveSpeed = .3f;

    private string currentLetter = "";

	float stepDuration = 0.7f;
	float elapsed;
	private const float dancerDist = 0.13f;
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
		GetNode<Timer>("LabelDisappear").WaitTime = 0.85f * rythm;
	}

	public override void _Process(double delta)
	{
		action?.Invoke(delta);

        rythm = musicPlayer.Instance.music_played == 1 ? 1.4f : 0.7f;
    }

    public string GetLetter()
    {
        return currentLetter;
    }

	private void SetTimingProgression(float t)
    {
        var shaderMaterial = (ShaderMaterial)GetNode<Sprite3D>("PathFollow3D/Sprite3D").MaterialOverride;
        shaderMaterial.SetShaderParameter("t", t);
    }

	public void Appear(string letter)
	{
        currentLetter = letter;
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


	public void StartPlayStep()
	{
		action = null;

		startProgress = anchor.Progress;
		endProgress = startProgress + distanceEachStepInMeter;
		action = PlayStep;
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
			action = null;
		}
	}

	public void SwapWith(Couple other)
	{
		GD.Print("swap !");

		Dancer me = anchor.dancer1.dancer;
		Dancer him = other.anchor.dancer1.dancer;

		CoupleManager.Swap(anchor.dancer1.dancer, other.anchor.dancer1.dancer);

		other.anchor.dancer1.dancer = me;
		anchor.dancer1.dancer = him;
	}

	public override void _ExitTree()
    {
        CoupleManager.RemoveCouple(this);
        base._ExitTree();
    }
}
