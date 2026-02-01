using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using static Godot.Control;

public partial class CoupleManager : Node
{
	[Export] public Couple player;
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
		improMod teddyMonGoat = GetNode<improMod>("Jazz/ImproModule");
		float tpub = 60 / teddyMonGoat.currentOST.BPM * teddyMonGoat.signature;
		GD.Print("tpub = ", tpub);
		GetNode<Timer>("Beat").WaitTime = tpub;
		GetNode<Timer>("OffBeat").WaitTime = tpub;
		GetNode<Timer>("HalfBeat").WaitTime = tpub / 2.0f;
    }



	int NumberOfLetter(string letter)
	{
		switch (letter)
		{
		case "H":
			return 0;
		case "J":
			return 1;
		case "K":
			return 2;
		case "L":
			return 3;
		default:
			return -1;
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		// eliott si tu veux encapsuler ça dans un ::ListenInput ou quoi vas-y
		List<Couple> closest = ClosestCouples(player, nbInputs);
		for (int i = 0; i < nbInputs; i++)
		{
			if (Input.IsActionJustPressed("dance" + NumberOfLetter(inputLetters[i])))
			{
				bool goodTiming = GetNode<improMod>("Jazz/ImproModule").Play();
				if (goodTiming)
				{
					GD.Print("Good timing ! player letter : " + player.GetLetter().ToUpper() + "vs input = " + inputLetters[i]);
					if (inputLetters[i] == player.GetLetter().ToUpper())
					{
						// TODO : green feedback
						player.StartPlayStep();
						player.GetNode<CoupleAnimator>("PathFollow3D").Spin();
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
								player = closest[j];
								break;
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
		for (int i = l.Count - 1; i > 0; i--)
		{
			int j = (int)(GD.Randi() % (i + 1));
			string tmp = l[i];
			l[i] = l[j];
			l[j] = tmp;
		}
		return l;
	}

	public void OnOffBeat()
	{
		List<Couple> closest = ClosestCouples(player, nbInputs);
		GD.Print("nb closest =", closest.Count);
		inputLetters = Shuffle(inputLetters);

		player.Appear(inputLetters[0]);
		for (int i = 0; i < Math.Min(closest.Count, nbInputs); i++)
		{
			closest[i].Appear(inputLetters[i + 1]);
		}
	}

	public void OnBeat()
	{
        List<Couple> closest = ClosestCouples(player, nbInputs);
        foreach (Couple who in coupleList)
		{
			if (who != player)
			{
				who.StartPlayStep();
				bool found = false;
				foreach(Couple who2 in closest)
				{
					if (who == who2) found = true;

				}
				if(!found) who.GetNode<CoupleAnimator>("PathFollow3D").Spin();
			}
		}
	}

	// TODO : supprimer ça dès qu'on intègre le code de teddy code dur
	public void StartBeating()
	{
		GetNode<Timer>("Beat").Start();
	}

	public List<Couple> ClosestCouples(Couple fromWho, int max = 4)
	{
		const float radius = 2.5f;
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

	public static void Swap(Dancer dancerA, Dancer dancerB)
	{
		DancerAnchor anchorA= dancerA.GetParent() as DancerAnchor;
		DancerAnchor anchorB= dancerB.GetParent() as DancerAnchor;

		//CoupleAnimator anchorParentA = anchorA.GetParent() as CoupleAnimator;
		//CoupleAnimator anchorParentB = anchorB.GetParent() as CoupleAnimator;


		Vector3 offset = new Vector3(0, 0.337f, 0);

		Tween tween = GetInstance().CreateTween();
		tween.TweenProperty(dancerA, "global_position", anchorB.GlobalPosition + offset, 0.3f);
		tween.Parallel();
		tween.TweenProperty(dancerB, "global_position", anchorA.GlobalPosition + offset, 0.3f);

        tween.TweenCallback(new Callable(GetInstance(), nameof(WithThisTreasureISummon)));



         void WithThisTreasureISummon()
		{
			dancerA.Reparent(anchorB);
			dancerA.Position = offset;
			dancerB.Reparent(anchorA);
			dancerB.Position = offset;	
		}
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
