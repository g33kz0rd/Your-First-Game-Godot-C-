using Godot;
using System;

public class Main : Node
{
    [Export] private PackedScene _mob;

    private Timer _startTimer;
    private Timer _scoreTimer;
    private Timer _mobTimer;

    private Player _player;
    private Position2D _startPosition;

    private PathFollow2D _mobSpawnLocation;

    private int _score;

    private Hud _hud;

    public override void _Ready()
    {
        _startTimer = GetNode("StartTimer") as Timer;
        _scoreTimer = GetNode("ScoreTimer") as Timer;
        _mobTimer = GetNode("MobTimer") as Timer;
        _player = GetNode("Player") as Player;
        _startPosition = GetNode("StartPosition") as Position2D;
        _mobSpawnLocation = GetNode("MobPath/MobSpawnLocation") as PathFollow2D;
        _hud = GetNode("Hud") as Hud;
    }

    private void OnPlayerHit() 
    {
        GameOver();
    }

    private void GameOver()
    {
        _scoreTimer.Stop();
        _mobTimer.Stop();
        _hud.ShowGameOver();
    }

    private void NewGame()
    {
        _hud.UpdateScore(_score);
        _hud.ShowMessage("Get Ready");
        _player.Start(_startPosition.Position);
        _startTimer.Start();
    }


    private void OnStartTimerTimeout()
    {
        _mobTimer.Start();
        _scoreTimer.Start();
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        _hud.UpdateScore(_score);
    }

    private void OnMobTimerTimeout()
    {
        var random = new Random();
        _mobSpawnLocation.SetOffset((float)random.NextDouble());

        var mob = _mob.Instance() as Mob;

        AddChild(mob);

        var direction = _mobSpawnLocation.Rotation + Math.PI / 2;

        mob.Position = _mobSpawnLocation.Position;

        direction += random.Next((int)((-Math.PI / 4)*1000000d), (int)((Math.PI / 4) * 1000000d)) / 1000000f;

        mob.SetLinearVelocity(new Vector2(random.Next(mob.MinSpeed, mob.MaxSpeed), 0).Rotated((float)direction));
    }

    public void OnHudStartGame()
    {
        NewGame();
    }
}
