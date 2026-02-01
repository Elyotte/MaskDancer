using Godot;
using System;
using System.Collections.Generic;

public partial class improMod : Node
{
    List<AudioStreamPlayer> players = new List<AudioStreamPlayer>();
    int playersNumber = 10;
    [Export] AudioStream[] notes1;
    [Export] AudioStream[] notes2;
    [Export] AudioStream wrong;
    public float tolerance = 0.4f;
    public Instrumental currentOST;
    public float signature;

    float latestNote;
    bool probablyPlaying;
    public override void _Ready()
    {
        CreatePlayers();
        currentOST = new Instrumental();
        currentOST = musicPlayer.Instance.GetCurrentInstrumental();
        GD.Print(currentOST.music.GetLength());
        signature = currentOST.binaire ? 2 : 3;
    }
    public override void _Process(double delta)
    {
        if (latestNote > 5f) { latestNote = 0; probablyPlaying = false; }
        if (Input.IsActionJustPressed("tap"))
        {
            Play();
        }
    }

    void CreatePlayers()
    {
        for (int i = 0; i < playersNumber; i++)
        {
            AudioStreamPlayer player = new AudioStreamPlayer();
            AddChild(player);
            players.Add(player);
        }
    }
    public bool Play()
    {
        return PlayNote(currentOST);
    }

    bool PlayNote(Instrumental OST)
    {
        float time = (float)musicPlayer.Instance.player.GetPlaybackPosition();
        signature = OST.binaire ? 2 : 3;
        float timing = signature*60 / OST.BPM;
        GD.Print(time % timing<tolerance);

        //Picking note algorithm
        int nombreDeBattementsParMesure = OST.binaire ? 4 : 3;
        float tempsPourUnBattement = 60 / OST.BPM;
        float chordUnit = tempsPourUnBattement * nombreDeBattementsParMesure;
        int currentChordSection = Mathf.FloorToInt(time / chordUnit) % 8; //le 8 est une constante qui devrait pas exister mais game jam

        //Actually playing note

        float fix = musicPlayer.Instance.music_played == 1 ? 1 : 2;
        for (int i = 0; i < playersNumber; i++)
        {
            if (players[i].Playing)
            {
                continue;
            }
            else
            {
                GD.Print(currentChordSection);

                if (musicPlayer.Instance.player.Stream == musicPlayer.Instance.music1)
                {
                    players[i].Stream = notes1[currentChordSection];
                }
                else { players[i].Stream = notes2[currentChordSection]; }
                if (!((time - tempsPourUnBattement * fix) % (chordUnit) < tolerance)){ players[i].Stream = wrong;  }
                players[i].Play();
                break;
            }
        }
        return ((time - tempsPourUnBattement* fix) % (chordUnit) < tolerance);
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
