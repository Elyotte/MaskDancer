using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class CoupleManager : Node
{
	[Export] private Couple player;
	[Export] private int maxNbClosestCouples;

	private List<string> inputLetters = null;
	private int nbInputs;

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

	public override void _Ready()
	{
		base._Ready();
		inputLetters = new List<string> { "H", "J", "K", "L" };
		nbInputs = Mathf.Min(maxNbClosestCouples, inputLetters.Count);
    }

	public override void _Process(double delta)
	{
		base._Process(delta);

		// eliott si tu veux encapsuler ça dans un ::ListenInput ou quoi vas-y
		List<Couple> closest = ClosestCouples(player, nbInputs);
		for (int i = 0; i < nbInputs; i++)
		{
			if (Input.IsActionJustPressed("dance" + i))
			{
				bool goodTiming = true;
				if (goodTiming)
				{
					if (inputLetters[i] == player.GetLetter())
					{
						// TODO : green feedback
						player.StartPlayStep();
					}
					else
					{
						// on cherche parmi les couples du voisinage si y'en a qui ont cette lettre
						for (int j = 0; j < closest.Count; j++)
						{
							if (inputLetters[i] == closest[j].GetLetter())
							{
								// TODO : green feedback
								player.SwapWith(closest[j]);
							}
						}
					}
				}
				else
				{
					// TODO : red feedback
				}
			}
		}
	}

	// flemme de chercher List::Shuffle() je vais juste le recoder
	List<string> Shuffle(List<string> l)
	{
		for (int i = inputLetters.Count - 1; i > 0; i--)
		{
			int j = (int)GD.Randi() % (i + 1);
			string tmp = l[i];
			l[i] = l[j];
			l[j] = tmp;
		}
		return l;
	}

	public void OnOffBeat()
	{
		List<Couple> closest = ClosestCouples(player, nbInputs);
		inputLetters = Shuffle(inputLetters);

		player.Appear(inputLetters[0]);
		for (int i = 1; i < Math.Min(closest.Count, nbInputs); i++)
		{
			closest[i].Appear(inputLetters[i]);
		}
	}

	public void OnBeat()
	{
		foreach (Couple who in coupleList)
		{
			who.StartPlayStep();
		}
	}

	// TODO : supprimer ça dès qu'on intègre le code de teddy code dur
	public void StartBeating()
	{
		GetNode<Timer>("Beat").Start();
	}

	public List<Couple> ClosestCouples(Couple fromWho, int max = 4)
	{
		const float radius = 1.0f;
		List<Couple> ans = new List<Couple>();
		foreach (Couple who in coupleList)
		{
			if ((who.GlobalPosition - fromWho.GlobalPosition).Length() <= radius && who != fromWho)
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
