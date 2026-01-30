using Godot;
using System;

public partial class musicPlayer : Node
{
    public static musicPlayer Instance {  get; private set; }
    [Export] public AudioStream music;
    public AudioStreamPlayer player;
    public override void _Ready()
    {
        Instance = this;
        player = new AudioStreamPlayer();
        player.Autoplay = true;
        player.Stream = music;
        AddChild(player);
    }
}
