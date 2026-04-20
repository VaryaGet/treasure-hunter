using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreasureHunter.balance;

namespace TreasureHunter.save;

public class StateCsv : IState
{
    private IState source;
    private string path;

    public StateCsv(string path) : this(new StateMemo(persisted(path)), path)
    {
    }

    public StateCsv(IState source, string path)
    {
        this.source = source;
        this.path = path;
    }

    public int currentLevel(UpgradeType type)
    {
        return this.source.currentLevel(type);
    }

    public List<BState> current()
    {
        return this.source.current();
    }

    public void persist(UpgradeType type, int level)
    {
        this.source.persist(type, level);
        File.WriteAllLines(
            this.path + "save.txt",
            this.source.current().Select(s => $"{s.Type};{s.Level}")
        );
    }

    private static List<BState> persisted(string path)
    {
        List<BState> states;
        if (!File.Exists(path))
        {
            states = Enum.GetValues<UpgradeType>()
                .Select(type =>
                {
                    // if (UpgradeType.DIGGER_QUANTITY == type || UpgradeType.SEARCHER_QUALITY == type)
                    // {
                    //     return new BState(0, type);
                    // }
                    return new BState(1, type);
                })
                .ToList();
        }
        else
        {
            states = File.ReadAllLines("states.txt")
                .Select(line => line.Split(';'))
                .Where(parts => parts.Length == 2)
                .Select(parts => new { Type = parts[0], Level = parts[1] })
                .Where(x => Enum.TryParse<UpgradeType>(x.Type, out _) && int.TryParse(x.Level, out _))
                .Select(x => new BState(int.Parse(x.Level), Enum.Parse<UpgradeType>(x.Type)))
                .ToList();
        }

        return states;
    }
}
