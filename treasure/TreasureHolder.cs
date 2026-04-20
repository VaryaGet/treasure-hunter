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
    private int tier = 3;

    public TreasureHolder(IState state)
    {
        FillStates();
    }

    private void FillStates()
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

    public int GetMoney(int id)
    {
        return _treasuresDto[id].Income;
    }

    public int SpawnChance(int id)
    {
        return _treasuresDto[id].Income;
    }

    public TreasureDto GetRandomTreasure()
    {
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
            case 2 when chance <= spawnChance:
                return _treasuresDto[1];
            case 3 when chance <= spawnChance:
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

    public void UpdateStates(UpgradeType upgradeType, decimal value)
    {
        switch (upgradeType)
        {
            case UpgradeType.TREASURE_BRONSE:
                _treasuresDto[0].Income = (int)value;
                break;
            case UpgradeType.TREASURE_SILVER:
                _treasuresDto[1].Income = (int)value;
                break;
            case UpgradeType.TREASURE_GOLD:
                _treasuresDto[2].Income = (int)value;
                break;
            case UpgradeType.TREASURE_TIER:
                tier = (int)value;
                break;
            case UpgradeType.QUALITY:
                spawnChance = (float)value;
                break;
        }
    }

    public class TreasureDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Income { get; set; }
    }
}
