using Godot;
using System;

public partial class improMod : Node
{
    AudioStreamPlayer improPlayer;
    [Export] AudioStream[] notes;
    float tolerance = 0.2f;
    Instrumental currentOST;

    float latestNote;
    bool probablyPlaying;
    public override void _Ready()
    {
        improPlayer = new AudioStreamPlayer(); AddChild(improPlayer);
        currentOST = new Instrumental();
        currentOST.music = musicPlayer.Instance.player.Stream;
        currentOST.BPM = 93;
        GD.Print(currentOST.music.GetLength());
    }
    public override void _Process(double delta)
    {
        if (latestNote > 5f) { latestNote = 0; probablyPlaying = false; }
        if (Input.IsActionJustPressed("tap"))
        {
            PlayNote(currentOST);
        }
    }
    void PlayNote(Instrumental OST)
    {
        float time = (float)musicPlayer.Instance.player.GetPlaybackPosition();
        float timing = 60 / OST.BPM;
            PickNote(time, timing);
    }

    void PickNote(float time, float timing)
    {
        bool onBeat = time % timing < tolerance;
    }
}

public partial class Instrumental
{
    public AudioStream music;
    public float BPM;
    public int scale;
}
