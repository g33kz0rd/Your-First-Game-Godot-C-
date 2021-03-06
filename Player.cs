using Godot;
using System;

public class Player : Area2D
{
    [Signal] public delegate void Hit();

    [Export] private int _speed = 400;

    private Vector2 _screenSize;
    private AnimatedSprite _animatedSprite;
    private CollisionShape2D _collisionShape2D;

    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _animatedSprite = GetNode("AnimatedSprite") as AnimatedSprite;
        _collisionShape2D = GetNode("CollisionShape2D") as CollisionShape2D;

        Hide();
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        _collisionShape2D.Disabled = false;
    }

    public override void _Process(float delta)
    {
        var velocity = new Vector2();

        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * _speed;

            _animatedSprite.Play();
        }
        else
        {
            _animatedSprite.Stop();
        }

        var tempPosition = Position + velocity * delta;
        tempPosition.x = Mathf.Clamp(tempPosition.x, 0, _screenSize.x);
        tempPosition.y = Mathf.Clamp(tempPosition.y, 0, _screenSize.y);
        Position = tempPosition;

        if (velocity.x != 0)
        {
            _animatedSprite.Animation = "right";
            _animatedSprite.FlipV = false;
            _animatedSprite.FlipH = velocity.x < 0;
        }
        else if (velocity.y != 0)
        {
            _animatedSprite.Animation = "up";
            _animatedSprite.FlipV = velocity.y > 0;
        }
    }

    private void OnPlayerBodyEntered(Godot.Object body)
    {
        Hide();
        EmitSignal(nameof(Hit));
        _collisionShape2D.Disabled = true;
    }
}