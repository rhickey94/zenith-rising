using Godot;

namespace SpaceTower.Scripts.Effects;

public partial class WhirlwindEffect : Node2D
{
		[Export] public float Duration { get; set; } = 1.0f; // Duration in seconds

		[Export] public float RotationSpeed { get; set; } = 10.0f;

		private float _elapsedTime = 0.0f;

		public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
				Rotation += RotationSpeed * (float)delta;

				_elapsedTime += (float)delta;
				var fadeProgress = _elapsedTime / Duration;
				Modulate = new Color(1, 1, 1, 1 - fadeProgress);

				if (_elapsedTime >= Duration)
				{
						QueueFree();
				}
		}
}
