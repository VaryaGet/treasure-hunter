using Godot;
using System;
using System.Collections.Generic;
using TreasureHunter.balance;
using TreasureHunter.save;

public partial class Btn : StaticBody2D
{
    [Signal]
    public delegate void UpgradedEventHandler(UpgradeType type, int level, float value, float cost);

    [Export] public UpgradeType type { get; set; }

    public IState state;
    public IBalance balance;
    public Score score;
    public bool enabled;
    private StateGd stateGd;

    private Sprite2D _sprite;

    private Tween _clickTween;
    private Vector2 _originalScale;
    private Color _originalModulate;

    private Dictionary<UpgradeType, string> upgrades = new()
    {
        { UpgradeType.DIGGER_QUANTITY, "Diggers" },
        { UpgradeType.DIGGER_SHOVEL, "Digger dig delay" },
        { UpgradeType.DIGGER_RUN, "Digger run speed" },
        { UpgradeType.TREASURE_BRONSE, "Bronze value" },
        { UpgradeType.TREASURE_SILVER, "Silver value" },
        { UpgradeType.TREASURE_GOLD, "Gold value" },
        { UpgradeType.SEARCHER_QUANTITY, "Searchers" },
        { UpgradeType.SEARCHER_SEARCH, "Searcher speed" },
        { UpgradeType.SEARCHER_QUALITY, "Searcher skill" },
        { UpgradeType.TREASURE_TIER, "Coin tier" },
        { UpgradeType.QUALITY, "Detector quality" }
    };

    public override void _Ready()
    {
        stateGd = this.GetStateGd();
        state = stateGd.state;
        balance = stateGd.balance;
        InputEvent += MyInputEvent;
        score = this.GetScore();
        enabled = false;
        GetNode<LabelNextCost>("Labels/NextCost").init();
        GetNode<LabelNextLvl>("Labels/NextLvl").init();

        _sprite = GetNode<Sprite2D>("Sprite");
        _originalScale = _sprite.Scale;
        _originalModulate = _sprite.Modulate;

        MouseEntered += SHovered;
        MouseExited += SNotHovered;
    }

    public override void _Process(double delta)
    {
        if (!balance.Checked(type, state.currentLevel(type) + 1))
        {
            enabled = false;
        }
        else
        {
            enabled = (float)balance.Balanced(
                type,
                state.currentLevel(type) + 1
            ).Cost <= score.score;
        }

        UpdateSpriteColor();
    }

    private void MyInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true } && enabled == true)
        {
            AnimateClick();
            var nextLevel = state.currentLevel(type) + 1;
            if (balance.Checked(type, nextLevel))
            {
                GD.Print(nextLevel);
                var nextValue = balance.Balanced(type, nextLevel);
                state.persist(type, nextLevel);
                EmitSignalUpgraded(
                    type,
                    nextLevel,
                    (float)nextValue.Value,
                    (float)nextValue.Cost
                );
                score.AddScore(-1 * (float)nextValue.Cost);
            }
        }
    }

    public string Named()
    {
        return upgrades[type];
    }

    private void SHovered()
    {
        if (enabled)
        {
            _sprite.Modulate = new Color(1.2f, 1.2f, 0.8f);
            _sprite.Scale = new Vector2(1.05f, 1.05f);
        }
    }

    private void SNotHovered()
    {
        if (enabled)
        {
            _sprite.Modulate = Colors.White;
        }
        else
        {
            _sprite.Modulate = new Color(0.7f, 0.7f, 0.7f);
        }

        _sprite.Scale = Vector2.One;
    }

    private void UpdateSpriteColor()
    {
        if (!enabled)
        {
            // Делаем спрайт сереньким
            _sprite.Modulate = new Color(0.7f, 0.7f, 0.7f);
        }
        else if (_sprite.Modulate != Colors.White && _sprite.Modulate.R == 0.7f)
        {
            // Возвращаем белый цвет, если кнопка снова стала доступна
            _sprite.Modulate = Colors.White;
        }
    }

    private void AnimateClick()
    {
        // Останавливаем предыдущую анимацию, если есть
        _clickTween?.Kill();

        // Создаем новый Tween
        _clickTween = CreateTween();

        // Анимация сжатия и поворота (качание)
        _clickTween.SetTrans(Tween.TransitionType.Back);
        _clickTween.SetEase(Tween.EaseType.Out);

        // Сжимаем спрайт
        _clickTween.TweenProperty(_sprite, "scale", new Vector2(0.95f, 0.95f), 0.08f);

        // Небольшой наклон
        _clickTween.Parallel().TweenProperty(_sprite, "rotation", Mathf.DegToRad(5f), 0.08f);

        // Возвращаем в исходное состояние с эффектом "пружинки"
        _clickTween.TweenProperty(_sprite, "scale", _originalScale, 0.12f);
        _clickTween.Parallel().TweenProperty(_sprite, "rotation", 0f, 0.12f);
    }
}
