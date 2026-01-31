using Godot;
using System;

public partial class Button : Control
{
	[Export] float rythm = 1.2f;
	[Export] Label textLabel;

	private void SetTimingProgression(float t)
	{
		var shaderMaterial = (ShaderMaterial)GetNode<ColorRect>("ColorRect").Material;
		shaderMaterial.SetShaderParameter("t", t);
	}

	public void Appear(string letter)
	{
		textLabel.Text = letter;
		Visible = true;
		SetTimingProgression(0.0f);
		Callable callableTimingProgression = new Callable(this, nameof(SetTimingProgression));
		GetTree().CreateTween().TweenMethod(callableTimingProgression, 0.0f, 1.0f, rythm);
		GetTree().CreateTween().TweenProperty(textLabel, "theme_override_colors/font_color:a", 1.0f, 0.15f);
		GetNode<Timer>("LabelDisappear").Start();
	}

	private void MakeLabelDisappear()
	{
		GetTree().CreateTween().TweenProperty(textLabel, "theme_override_colors/font_color:a", 0.0f, 0.15f);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("LabelDisappear").WaitTime = 0.85f * rythm;
		Appear("Y");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
