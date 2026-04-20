using Godot;
using System;
using System.Globalization;
using TreasureHunter.balance;

public partial class LabelNextCost : Label
{
	private Btn parent;

	public void init()
	{
		parent = GetParent<Btn>();
		parent.Upgraded += UpdateText;
		GD.Print(parent.balance.Balanced(parent.type, 1).Cost);
		BValue value = parent.balance.Balanced(parent.type, parent.state.currentLevel(parent.type));
		UpdateText(parent.type, parent.state.currentLevel(parent.type), (float) value.Value, (float) value.Cost);
	}

	private void UpdateText(UpgradeType type, int level, float value, float cost)
	{
		if (!parent.balance.Checked(type, level + 1))
		{
			Text = "MAX";
		}
		else
		{
			Text = parent.balance.Balanced(
				type,
				level + 1
			).Cost.ToString(CultureInfo.InvariantCulture);
		}
	}
}
