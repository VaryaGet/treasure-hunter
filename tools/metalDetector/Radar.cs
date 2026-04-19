using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Radar : Area2D
{
    private HashSet<Node2D> _treasuresInZone = new();
    public float Radius { get; private set; }
    private GradientTexture1D _texture;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        Radius = ((CircleShape2D)GetNode<CollisionShape2D>("CollisionShape2D").Shape).Radius;
        _texture = GetNode<Sprite2D>("Level").Texture as GradientTexture1D;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup(Groups.TreasureCollision))
        {
            _treasuresInZone.Add(body);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup(Groups.TreasureCollision))
        {
            _treasuresInZone.Remove(body);
        }
    }

    public override void _Process(double delta)
    {
        _treasuresInZone.RemoveWhere(t => !IsInstanceValid(t));

        if (_treasuresInZone.Count != 0)
        {
            var distance = _treasuresInZone
                .Select(treasure => GlobalPosition.DistanceTo(treasure.GlobalPosition))
                .Select(dist => Mathf.Clamp(dist, 0, Radius))
                .Min();

            if (distance < Radius)
            {
                var value = Mathf.InverseLerp(0, Radius, distance);
                UpdateTexture(value);
            }
            else
            {
                UpdateTexture(1);
            }
        }
        else
        {
            UpdateTexture(1);
        }
    }

    private void UpdateTexture(float value)
    {
        _texture.Gradient.SetOffset(1, value);
    }
}
