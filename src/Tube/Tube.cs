using Godot;
using System;
using System.Collections.Generic;

namespace GodotBeginnerSeries2024;

public partial class Tube : Node3D {
    [Export] private float _radius = 3;
    [Export] private int _sides = 8;
    [Export] private int _capacity = 20;

    [ExportGroup("Resources")]
    [Export] private PackedScene _platformScene;

    private List<Platform> _platforms = new();
    private Transform3D[] _transforms;

    private Random _random = new();
    private int _index;

    public override void _Ready() {
        _transforms = new Transform3D[_sides];
        var angle = Mathf.Tau / _sides;
        var theta = 0f;

        for (int i = 0; i < _sides; i++, theta += angle) {
            var origin = new Vector3(_radius * Mathf.Sin(theta), _radius - _radius * Mathf.Cos(theta), 0);
            var basis = Basis.Identity.Rotated(Vector3.Forward, -theta);
            var transform = new Transform3D(basis, origin);
            _transforms[i] = transform;
        }

        var platform = _platformScene.Instantiate<Platform>();
        AddChild(platform);

        for (int i = 1; i < _capacity; i++) {
            platform = _platformScene.Instantiate<Platform>();
            AddChild(platform);

            _index = GetNextIndex(_index);
            var transform = _transforms[_index];
            platform.Basis = transform.Basis;
            platform.Position = new Vector3(transform.Origin.X, transform.Origin.Y, -(i * Platform.LENGTH));
        }
    }

    public void Spin(bool right) {

    }

    public void Traverse() {

    }

    private int GetNextIndex(int index) {
        index += _random.NextDouble() > 0.5 ? 1 : -1;
        if (index < 0) {
            index += _sides;
        } else if (index >= _sides) {
            index -= _sides;
        }
        return index;
    }
}
