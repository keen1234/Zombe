using Godot;

public partial class Camera3d : Camera3D
{
	[Export]
	public NodePath TargetPath { get; set; } = new("../RigidBody3D");

	[Export]
	public float MouseSensitivity { get; set; } = 0.003f;

	[Export]
	public float MinimumPitch { get; set; } = -1.2f;

	[Export]
	public float MaximumPitch { get; set; } = 1.2f;

	private Node3D _target;
	private Vector3 _offset;
	private float _yaw;
	private float _pitch;

	public override void _Ready()
	{
		_target = GetNode<Node3D>(TargetPath);
		_offset = GlobalPosition - _target.GlobalPosition;
		var horizontalDistance = new Vector2(_offset.X, _offset.Z).Length();
		_yaw = Mathf.Atan2(_offset.X, _offset.Z);
		_pitch = Mathf.Atan2(_offset.Y, horizontalDistance);
		Current = true;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{
		var distance = _offset.Length();
		var horizontalDistance = Mathf.Cos(_pitch) * distance;
		var orbitOffset = new Vector3(
			Mathf.Sin(_yaw) * horizontalDistance,
			Mathf.Sin(_pitch) * distance,
			Mathf.Cos(_yaw) * horizontalDistance);

		GlobalPosition = _target.GlobalPosition + orbitOffset;
		LookAt(_target.GlobalPosition);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton &&
			mouseButton.ButtonIndex == MouseButton.Left &&
			mouseButton.Pressed)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else if (@event is InputEventKey keyEvent &&
			keyEvent.Keycode == Key.Escape &&
			keyEvent.Pressed)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else if (@event is InputEventMouseMotion mouseMotion &&
			Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			_yaw -= mouseMotion.Relative.X * MouseSensitivity;
			_pitch = Mathf.Clamp(
				_pitch - mouseMotion.Relative.Y * MouseSensitivity,
				MinimumPitch,
				MaximumPitch);
		}
	}
}
