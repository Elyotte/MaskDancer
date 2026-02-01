using Godot;
using System;

public partial class BringMeToLife : Sprite3D
{
	[Export] Couple feur;

	public override void _Process(double delta)
	{
		Modulate = CoupleManager.GetInstance().player == feur ? Colors.Red : Colors.Black;
	}
}
