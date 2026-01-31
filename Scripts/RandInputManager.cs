using Godot;
using System;
using System.Collections.Generic;

public partial class RandInputManager : Control
{
	Control button;
	private static RandInputManager instance;
	public static RandInputManager GetInstance()
	{
		if (instance == null)
		{
			return new RandInputManager();
		}
		return instance;
	}

	const string INPUT_NAME = "dance";
	List<string> inputsLettersToDisplay = new List<string>() { "Z",
	"Q",
	"S",
	"D"};

	private RandInputManager()
	{
		if (instance == null) { instance = this; }
	}

	


    public override void _ExitTree()
    {
		if (instance !=null && instance == this)
		{
			instance = null;
		}
        base._ExitTree();
    }
}
