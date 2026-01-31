using Godot;
using System;

public partial class musicPlayer : Node
{
    public static musicPlayer Instance {  get; private set; }
    [Export] public AudioStream music1;
    [Export] public AudioStream music2;
    public AudioStreamPlayer player;

    public Instrumental ost1;
    public Instrumental ost2;
    public override void _Ready()
    {
        Instance = this;
        player = new AudioStreamPlayer();
        player.Autoplay = true;
        player.Stream = music1;
        AddChild(player);
        MusicDatas();
    }

    public Instrumental GetCurrentInstrumental()
    {
        if (player.Stream == music1) { return ost1; }
        else { return ost2; }
    }
    void MusicDatas()
    {
        ost1 = new Instrumental();
        ost1.music = music1;
        ost1.BPM = 151;
        ost1.binaire = false;
        ost2 = new Instrumental();
        ost2.music = music2;
        ost2.BPM = 275;
        ost2.binaire = true;
    }
}
