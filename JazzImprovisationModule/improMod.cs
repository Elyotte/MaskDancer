using Godot;
using System;

public partial class improMod : Node
{
    AudioStreamPlayer[] improPlayer;
    [Export] AudioStream[] notes1;
    [Export] AudioStream[] notes2;
    float tolerance = 0.2f;
    Instrumental currentOST;

    float latestNote;
    bool probablyPlaying;
    public override void _Ready()
    {
        improPlayer = new AudioStreamPlayer[10]; 
        for (int i = 0; i < improPlayer.Length; i++) { AddChild(improPlayer[i]); }
        currentOST = new Instrumental();
        currentOST = musicPlayer.Instance.GetCurrentInstrumental();
        GD.Print(currentOST.music.GetLength());
    }
    public override void _Process(double delta)
    {
        if (latestNote > 5f) { latestNote = 0; probablyPlaying = false; }
        if (Input.IsActionJustPressed("tap"))
        {
            Play();
        }
    }

    public bool Play()
    {
        return PlayNote(currentOST);
    }
    bool PlayNote(Instrumental OST)
    {
        float time = (float)musicPlayer.Instance.player.GetPlaybackPosition();
        float signature = OST.binaire ? 2 : 3;
        float timing = signature*60 / OST.BPM;
        GD.Print(time % timing<tolerance);

        //Picking note algorithm
        return (time % timing < tolerance);
    }



}
//j'ai la flemme mais il faut un entier nombre d'accords et ici ce sera toujours 8
public partial class Instrumental
{
    public AudioStream music;
    public float BPM;
    public int scale;
    public bool binaire;
}
