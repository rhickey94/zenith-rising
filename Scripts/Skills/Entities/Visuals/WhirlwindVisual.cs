using Godot;
using ZenithRising.Scripts.Skills.Base;

namespace ZenithRising.Scripts.Skills.Entities.Visuals;

public partial class WhirlwindVisual : Node2D
{
    private float _duration;
    private float _rotationSpeed;
    private float _radius;
    private int _rotationCount = 1; // New: How many full rotations

    private float _timeElapsed = 0f;
    private float _alpha = 0.6f; // Current alpha for fade effect

    /// <summary>
    /// Initialize with skill parameters (called after instantiation)
    /// </summary>
    public void Initialize(Skill sourceSkill)
    {
        _duration = sourceSkill.Duration;
        _radius = sourceSkill.Radius;
        _rotationCount = sourceSkill.WhirlwindRotations;
        _rotationSpeed = Mathf.Pi * 2 * _rotationCount / _duration;
        _alpha = Mathf.Clamp(0.4f + (_rotationCount * 0.1f), 0.4f, 0.8f);
    }

    public override void _Ready()
    {
        // Fallback if Initialize() wasn't called
        if (_duration <= 0)
        {
            GD.PrintErr("WhirlwindVisual: Initialize() not called! Using fallback values.");
            _duration = 0.8f;
            _radius = 150f;
            _rotationCount = 1;
            _rotationSpeed = Mathf.Pi * 2 / _duration;
        }

        GetTree().CreateTimer(_duration).Timeout += QueueFree;
    }

    public override void _Process(double delta)
    {
        _timeElapsed += (float)delta;
        Rotation += _rotationSpeed * (float)delta;

        // Fade out during last 20% of duration
        float fadeStartTime = _duration * 0.8f;
        if (_timeElapsed > fadeStartTime)
        {
            float fadeProgress = (_timeElapsed - fadeStartTime) / (_duration * 0.2f);
            _alpha = Mathf.Lerp(0.6f, 0.0f, fadeProgress);
        }

        // Pulse effect (optional - scales with rotation speed)
        float pulseFreq = 8f * _rotationCount;
        float pulse = 1.0f + Mathf.Sin(_timeElapsed * pulseFreq) * 0.05f;
        Scale = Vector2.One * pulse;

        QueueRedraw(); // Redraw every frame to update alpha
    }

    public override void _Draw()
    {
        if (_radius <= 0)
        {
            GD.PrintErr($"WhirlwindVisual: Cannot draw with radius {_radius}!");
            return;
        }

        int segments = 8 * _rotationCount;
        if (segments <= 0)
        {
            GD.PrintErr($"WhirlwindVisual: Cannot draw with {segments} segments!");
            return;
        }

        for (int i = 0; i < segments; i++)
        {
            float startAngle = (Mathf.Pi * 2 / segments) * i;
            float endAngle = startAngle + Mathf.Pi / segments;

            DrawArc(Vector2.Zero, _radius, 0, Mathf.Pi * 2, 64, new Color(0.5f, 0.8f, 1.0f, _alpha), 2.0f);
        }

        DrawCircle(Vector2.Zero, _radius, new Color(0.5f, 0.8f, 1.0f, _alpha * 0.3f), false);
    }
}
