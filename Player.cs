using Godot;


public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    private int _speed = 400;
    private Vector2 _screensize;
    private AnimatedSprite _animatedSprite;

    public override void _Ready()
    {
        _screensize = GetViewportRect().Size;
        _animatedSprite = GetNode("AnimatedSprite") as AnimatedSprite;

        Hide();
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
        tempPosition.x = Mathf.Clamp(tempPosition.x, 0, _screensize.x);
        tempPosition.y = Mathf.Clamp(tempPosition.y, 0, _screensize.y);
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
}
