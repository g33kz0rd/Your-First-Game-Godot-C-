using Godot;
using System;

public class Mob : Area2D
{
    private string[] MobTypes = new string[] { "walk", "swim", "fly" };

    [Export] private int _minSpeed = 150;
    [Export] private int _maxSpeed = 250;
    private int _speed = 400;

    private Vector2 _screensize;
    private AnimatedSprite _animatedSprite;
    private CollisionShape2D _collisionShape2D;

    public override void _Ready()
    {
        _screensize = GetViewportRect().Size;

        _animatedSprite = GetNode("AnimatedSprite") as AnimatedSprite;
        Random random = new Random();
        _animatedSprite.Animation = MobTypes[random.Next(0, MobTypes.Length)];
        _animatedSprite.Play();

        _collisionShape2D = GetNode("CollisionShape2D") as CollisionShape2D;

        //Hide();
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        _collisionShape2D.Disabled = false;
    }

    private void OnMobVisibilityChanged()
    {
        QueueFree();
    }
}
