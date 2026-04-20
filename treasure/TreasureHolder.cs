using System;
using System.Collections.Generic;
using Godot;
using TreasureHunter.balance;
using TreasureHunter.save;

namespace TreasureHunter.treasure;

public class TreasureHolder
{
    private List<TreasureDto> _treasuresDto;
    private float spawnChance = 0.5f;
    private float searcherSpawnChance = 0.5f;
    private int tier = 3;

    public TreasureHolder(StateGd state)
    {
        FillStates(state);
    }

    private void InitList()
    {
        _treasuresDto = new List<TreasureDto>(3);
        _treasuresDto.Add(new TreasureDto
        {
            Id = 1,
            Type = "BRONSE",
            Income = 2
        });

        _treasuresDto.Add(new TreasureDto
        {
            Id = 2,
            Type = "SILVER",
            Income = 20
        });

        _treasuresDto.Add(new TreasureDto
        {
            Id = 3,
            Type = "GOLD",
            Income = 200
        });
    }

    public void FillStates(StateGd state)
    {
        InitList();
        foreach (var st in state.state.current())
        {
            UpdateStates(st.Type, (float)state.balance.Balanced(st.Type, st.Level).Value);
        }
    }

    public TreasureDto GetRandomTreasure(bool isForPlayer = false)
    {
        var targetChance = isForPlayer ? spawnChance : searcherSpawnChance;
        if (tier == 1)
        {
            return _treasuresDto[0];
        }

        var chance = GD.Randf();
        if (chance == 0f)
        {
            return _treasuresDto[0];
        }

        switch (tier)
        {
            case 2 when chance <= targetChance:
                return _treasuresDto[1];
            case 3 when chance <= targetChance:
                return _treasuresDto[2];
            case 3:
            {
                chance = GD.Randf();
                if (chance <= 0.5)
                {
                    return _treasuresDto[1];
                }

                break;
            }
        }

        return _treasuresDto[0];
    }

    public bool UpdateStates(UpgradeType upgradeType, float value)
    {
        switch (upgradeType)
        {
            case UpgradeType.TREASURE_BRONSE:
                _treasuresDto[0].Income = value;
                return true;
            case UpgradeType.TREASURE_SILVER:
                _treasuresDto[1].Income = value;
                return true;
            case UpgradeType.TREASURE_GOLD:
                _treasuresDto[2].Income = value;
                return true;
            case UpgradeType.TREASURE_TIER:
                tier = (int)value;
                return false;
            case UpgradeType.QUALITY:
                spawnChance = value;
                return false;
            case UpgradeType.SEARCHER_QUALITY:
                searcherSpawnChance = value;
                return false;
            default:
                return false;
        }
    }

    public class TreasureDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public float Income { get; set; }
    }
}
