using Godot;
using System;
using System.Threading.Tasks;

public class Hud : CanvasLayer
{
    [Signal] public delegate void StartGame();

    private Label _messageLabel;
    private Label _scoreLabel;
    private Timer _messageTimer;
    private Button _startButton;

    public override void _Ready()
    {
        _messageLabel = GetNode("MessageLabel") as Label;
        _scoreLabel = GetNode("ScoreLabel") as Label;
        _messageTimer = GetNode("MessageTimer") as Timer;
        _startButton = GetNode("StartButton") as Button;
    }

    public void ShowMessage(string message)
    {
        _messageLabel.Text = message;
        _messageLabel.Show();
        _messageTimer.Start();

        while (_messageLabel.Visible)
            continue;
    }

    public void ShowGameOver()
    {
        Task.Run(() => GameOver());
    }

    private void GameOver()
    {
        Task.Run(() => ShowMessage("Game Over")).Wait(); 

        _startButton.Show();

        _messageLabel.Text = "Dodge the Creeps!";
        _messageLabel.Show();
    }

    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }

    private void OnMessageTimerTimeout()
    {
        _messageLabel.Hide();
    }

    private void OnStartButtonButtonUp()
    {
        _startButton.Hide();

        EmitSignal(nameof(StartGame));
    }
}
