using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class CoupleManager : Node
{
	private static CoupleManager instance;
	public static CoupleManager GetInstance()
	{
		if (instance == null) { 
			instance = new CoupleManager();
			((SceneTree)Engine.GetMainLoop()).Root.AddChild(instance);
		}
		return instance;
	}

	List<Couple> coupleList = new List<Couple>();

	protected CoupleManager() {
		if (instance == null) instance = this;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public List<Couple> ClosestCouples(Couple fromWho, int max = 4)
	{
		const float radius = 1.0f;
		List<Couple> ans = new List<Couple>();
		foreach (Couple who in coupleList)
		{
			if((who.GlobalPosition - fromWho.GlobalPosition).Length() <= radius && who != fromWho)
				ans.Add(who);
		}
		// ans.sort() ? Pas la peine en vrai on aura jamais + de 3 couples a la fois
		return ans.GetRange(0, Mathf.Min(max, ans.Count));
	}



	public static void AddCouple(Couple couple)
	{
		GetInstance().coupleList.Add(couple);
	}

	public static void RemoveCouple(Couple couple)
	{
		GetInstance().coupleList.Remove(couple);
	}

    public override void _ExitTree()
    {
		if (instance == this)
		{
			instance = null;
		}
        base._ExitTree();
    }
}
