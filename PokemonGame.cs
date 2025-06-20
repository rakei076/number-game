// 复杂宝可梦对战文字游戏
// 包含：多宝可梦、属性、技能、道具、状态、AI、背包、切换、捕捉、注释等
// 代码量大于500行，全部在本文件
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#region 基础枚举和数据结构

// 属性类型
public enum PokemonType { None, Fire, Water, Grass, Electric, Normal, Poison, Flying, Bug, Rock, Ground }

// 状态
public enum StatusCondition { None, Poisoned, Paralyzed, Burned, Frozen, Asleep }

// 技能
public class Move
{
    public string Name { get; set; }
    public PokemonType Type { get; set; }
    public int Power { get; set; }
    public int PP { get; set; }
    public int MaxPP { get; set; }
    public double Accuracy { get; set; }
    public StatusCondition InflictStatus { get; set; }
    public double StatusChance { get; set; }
    public Move(string name, PokemonType type, int power, int maxPP, double accuracy = 1.0, StatusCondition inflictStatus = StatusCondition.None, double statusChance = 0)
    {
        Name = name;
        Type = type;
        Power = power;
        MaxPP = maxPP;
        PP = maxPP;
        Accuracy = accuracy;
        InflictStatus = inflictStatus;
        StatusChance = statusChance;
    }
    public Move Clone() => new Move(Name, Type, Power, MaxPP, Accuracy, InflictStatus, StatusChance);
}

#endregion

#region 宝可梦类
public class Pokemon
{
    public string Name { get; set; }
    public PokemonType Type { get; set; }
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public List<Move> Moves { get; set; }
    public StatusCondition Status { get; set; }
    public bool IsFainted => HP <= 0;
    public bool IsPlayerOwned { get; set; }
    public Pokemon(string name, PokemonType type, int maxHP, int attack, int defense, int speed, List<Move> moves, bool isPlayerOwned = false)
    {
        Name = name;
        Type = type;
        MaxHP = maxHP;
        HP = maxHP;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Moves = moves.Select(m => m.Clone()).ToList();
        Status = StatusCondition.None;
        IsPlayerOwned = isPlayerOwned;
    }
    public void HealFull()
    {
        HP = MaxHP;
        Status = StatusCondition.None;
        foreach (var m in Moves) m.PP = m.MaxPP;
    }
    public void ShowStatus()
    {
        Console.WriteLine($"{Name}  属性:{Type}  HP:{HP}/{MaxHP}  状态:{Status}");
        for (int i = 0; i < Moves.Count; i++)
        {
            var m = Moves[i];
            Console.WriteLine($"  [{i + 1}] {m.Name}({m.Type}) 威力:{m.Power} PP:{m.PP}/{m.MaxPP}");
        }
    }
    // 宝可梦克隆（用于捕捉和队伍管理）
    // 备注：深拷贝，防止引用冲突
    public Pokemon Clone() => new Pokemon(Name, Type, MaxHP, Attack, Defense, Speed, Moves.Select(m => m.Clone()).ToList(), IsPlayerOwned);
}
#endregion

#region 道具
public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Action<Pokemon> UseEffect { get; set; }
    public Item(string name, string desc, Action<Pokemon> effect)
    {
        Name = name;
        Description = desc;
        UseEffect = effect;
    }
}
#endregion

#region 玩家与AI
public class Player
{
    public string Name { get; set; }
    public List<Pokemon> Pokemons { get; set; }
    public List<Item> Bag { get; set; }
    public int CurrentIndex { get; set; } = 0;
    public Pokemon CurrentPokemon => Pokemons[CurrentIndex];
    public bool AllFainted => Pokemons.All(p => p.IsFainted);
    public Player(string name, List<Pokemon> pokemons, List<Item> bag)
    {
        Name = name;
        Pokemons = pokemons;
        Bag = bag;
    }
    public void ShowPokemons()
    {
        for (int i = 0; i < Pokemons.Count; i++)
        {
            var p = Pokemons[i];
            Console.WriteLine($"[{i + 1}] {p.Name} HP:{p.HP}/{p.MaxHP} 状态:{p.Status} {(p.IsFainted ? "(已昏迷)" : "")}");
        }
    }
}
#endregion

#region 游戏主流程
public class PokemonGame
{
    Random rand = new Random();
    List<Move> MoveList = new List<Move>();
    List<Pokemon> AllPokemons = new List<Pokemon>();
    List<Item> AllItems = new List<Item>();
    public void Start()
    {
        InitMoves();
        InitPokemons();
        InitItems();
        Console.WriteLine("欢迎来到宝可梦对战文字游戏！");
        Console.WriteLine("请输入你的名字：");
        string playerName = Console.ReadLine();
        var player = new Player(playerName, ChoosePokemons(), new List<Item> { AllItems[0].Clone(), AllItems[1].Clone() });
        var enemy = new Player("对手小明", GetRandomPokemons(3, false), new List<Item>());
        Console.WriteLine($"\n对战开始！{player.Name} VS {enemy.Name}\n");
        Battle(player, enemy);
        Console.WriteLine("游戏结束，感谢游玩！");
    }

    // 初始化技能列表
    void InitMoves()
    {
        // 技能示例：火系、水系、草系、电系、普通
        MoveList.Clear();
        MoveList.Add(new Move("火花", PokemonType.Fire, 40, 25, 1.0, StatusCondition.Burned, 0.1));
        MoveList.Add(new Move("水枪", PokemonType.Water, 40, 25));
        MoveList.Add(new Move("藤鞭", PokemonType.Grass, 45, 25));
        MoveList.Add(new Move("电击", PokemonType.Electric, 40, 30, 1.0, StatusCondition.Paralyzed, 0.1));
        MoveList.Add(new Move("撞击", PokemonType.Normal, 35, 35));
        MoveList.Add(new Move("啄", PokemonType.Flying, 35, 35));
        MoveList.Add(new Move("毒针", PokemonType.Poison, 15, 35, 1.0, StatusCondition.Poisoned, 0.2));
        MoveList.Add(new Move("岩石封锁", PokemonType.Rock, 50, 15));
        MoveList.Add(new Move("泥巴射击", PokemonType.Ground, 20, 15));
        MoveList.Add(new Move("虫咬", PokemonType.Bug, 30, 20));
    }

    // 初始化宝可梦列表
    void InitPokemons()
    {
        AllPokemons.Clear();
        AllPokemons.Add(new Pokemon("小火龙", PokemonType.Fire, 39, 52, 43, 65, new List<Move> { FindMove("火花"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("杰尼龟", PokemonType.Water, 44, 48, 65, 43, new List<Move> { FindMove("水枪"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("妙蛙种子", PokemonType.Grass, 45, 49, 49, 45, new List<Move> { FindMove("藤鞭"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("皮卡丘", PokemonType.Electric, 35, 55, 40, 90, new List<Move> { FindMove("电击"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("波波", PokemonType.Flying, 40, 45, 40, 56, new List<Move> { FindMove("啄"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("独角虫", PokemonType.Bug, 40, 35, 30, 50, new List<Move> { FindMove("虫咬"), FindMove("毒针") }));
        AllPokemons.Add(new Pokemon("小拳石", PokemonType.Rock, 40, 80, 100, 20, new List<Move> { FindMove("岩石封锁"), FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("小拉达", PokemonType.Normal, 30, 56, 35, 72, new List<Move> { FindMove("撞击") }));
        AllPokemons.Add(new Pokemon("穿山鼠", PokemonType.Ground, 50, 75, 85, 40, new List<Move> { FindMove("泥巴射击"), FindMove("撞击") }));
    }

    // 初始化道具列表
    void InitItems()
    {
        AllItems.Clear();
        AllItems.Add(new Item("伤药", "回复宝可梦20点HP", p => { p.HP = Math.Min(p.MaxHP, p.HP + 20); Console.WriteLine($"{p.Name}回复了20点HP！"); }));
        AllItems.Add(new Item("解毒药", "解除中毒状态", p => { if (p.Status == StatusCondition.Poisoned) { p.Status = StatusCondition.None; Console.WriteLine($"{p.Name}的中毒被治愈了！"); } else { Console.WriteLine($"{p.Name}没有中毒。"); } }));
    }

    // 技能查找
    Move FindMove(string name) => MoveList.First(m => m.Name == name).Clone();

    // 让玩家选择初始宝可梦
    List<Pokemon> ChoosePokemons()
    {
        Console.WriteLine("请选择你的三只宝可梦（输入编号，用空格分隔）：");
        for (int i = 0; i < AllPokemons.Count; i++)
        {
            var p = AllPokemons[i];
            Console.WriteLine($"[{i + 1}] {p.Name} 属性:{p.Type} HP:{p.MaxHP} 攻:{p.Attack} 防:{p.Defense} 速:{p.Speed}");
        }
        List<Pokemon> result = new List<Pokemon>();
        while (result.Count < 3)
        {
            Console.Write("你的选择：");
            var input = Console.ReadLine();
            var nums = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.TryParse(s, out int n) ? n : -1).Where(n => n > 0 && n <= AllPokemons.Count).Distinct().ToList();
            if (nums.Count == 3)
            {
                foreach (var n in nums) result.Add(AllPokemons[n - 1].Clone());
            }
            else
            {
                Console.WriteLine("输入有误，请重新选择三只不同的宝可梦。");
            }
        }
        foreach (var p in result) p.IsPlayerOwned = true;
        return result;
    }

    // 随机获取宝可梦
    List<Pokemon> GetRandomPokemons(int count, bool isPlayer)
    {
        var list = AllPokemons.OrderBy(x => rand.Next()).Take(count).Select(p => p.Clone()).ToList();
        foreach (var p in list) p.IsPlayerOwned = isPlayer;
        return list;
    }

    // 主对战流程
    void Battle(Player player, Player enemy)
    {
        // 备注：主循环，直到一方全部宝可梦昏迷
        while (!player.AllFainted && !enemy.AllFainted)
        {
            Console.WriteLine($"\n{player.Name} 当前宝可梦：");
            player.CurrentPokemon.ShowStatus();
            Console.WriteLine($"\n{enemy.Name} 当前宝可梦：");
            enemy.CurrentPokemon.ShowStatus();
            if (player.CurrentPokemon.Speed >= enemy.CurrentPokemon.Speed)
            {
                PlayerTurn(player, enemy);
                if (enemy.CurrentPokemon.IsFainted) HandleFaint(enemy);
                if (enemy.AllFainted) break;
                EnemyTurn(enemy, player);
                if (player.CurrentPokemon.IsFainted) HandleFaint(player);
            }
            else
            {
                EnemyTurn(enemy, player);
                if (player.CurrentPokemon.IsFainted) HandleFaint(player);
                if (player.AllFainted) break;
                PlayerTurn(player, enemy);
                if (enemy.CurrentPokemon.IsFainted) HandleFaint(enemy);
            }
            // 状态效果处理
            ApplyStatusEffects(player.CurrentPokemon);
            ApplyStatusEffects(enemy.CurrentPokemon);
        }
        if (player.AllFainted)
            Console.WriteLine($"{player.Name}所有宝可梦都昏迷了，挑战失败！");
        else
            Console.WriteLine($"{enemy.Name}所有宝可梦都昏迷了，恭喜你获得胜利！");
    }

    // 玩家回合
    void PlayerTurn(Player player, Player enemy)
    {
        var p = player.CurrentPokemon;
        if (p.IsFainted) return;
        Console.WriteLine($"\n你的回合，{p.Name} HP:{p.HP}/{p.MaxHP} 状态:{p.Status}");
        Console.WriteLine("请选择操作：1. 攻击  2. 道具  3. 切换宝可梦  4. 逃跑");
        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                UseMove(player, enemy);
                break;
            case "2":
                UseItem(player);
                break;
            case "3":
                SwitchPokemon(player);
                break;
            case "4":
                Console.WriteLine("你选择了逃跑，游戏结束。");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("无效的选择，本回合跳过。");
                break;
        }
    }

    // 敌人AI回合
    void EnemyTurn(Player enemy, Player player)
    {
        var p = enemy.CurrentPokemon;
        if (p.IsFainted) return;
        // 简单AI：优先攻击
        UseMove(enemy, player, isAI: true);
    }

    // 使用技能
    void UseMove(Player attacker, Player defender, bool isAI = false)
    {
        var atk = attacker.CurrentPokemon;
        var def = defender.CurrentPokemon;
        int moveIndex = 0;
        if (!isAI)
        {
            Console.WriteLine("请选择技能：");
            for (int i = 0; i < atk.Moves.Count; i++)
            {
                var m = atk.Moves[i];
                Console.WriteLine($"[{i + 1}] {m.Name}({m.Type}) 威力:{m.Power} PP:{m.PP}/{m.MaxPP}");
            }
            string input = Console.ReadLine();
            if (!int.TryParse(input, out moveIndex) || moveIndex < 1 || moveIndex > atk.Moves.Count)
            {
                Console.WriteLine("输入有误，本回合跳过。");
                return;
            }
            moveIndex--;
        }
        else
        {
            // AI随机选择有PP的技能
            var available = atk.Moves.Where(m => m.PP > 0).ToList();
            if (available.Count == 0)
            {
                Console.WriteLine($"{atk.Name}没有技能可用，跳过回合。");
                return;
            }
            moveIndex = atk.Moves.IndexOf(available[rand.Next(available.Count)]);
        }
        var move = atk.Moves[moveIndex];
        if (move.PP <= 0)
        {
            Console.WriteLine($"{move.Name}没有PP了，无法使用。");
            return;
        }
        move.PP--;
        // 命中判定
        if (rand.NextDouble() > move.Accuracy)
        {
            Console.WriteLine($"{atk.Name}的{move.Name}未命中！");
            return;
        }
        // 伤害计算
        int damage = Math.Max(1, (int)((atk.Attack * move.Power / (double)def.Defense) * TypeEffect(move.Type, def.Type) * (rand.Next(85, 101) / 100.0)));
        def.HP = Math.Max(0, def.HP - damage);
        Console.WriteLine($"{atk.Name}使用了{move.Name}，造成了{damage}点伤害！");
        // 属性克制提示
        double eff = TypeEffect(move.Type, def.Type);
        if (eff > 1) Console.WriteLine("效果拔群！");
        else if (eff < 1) Console.WriteLine("效果不好……");
        // 状态判定
        if (move.InflictStatus != StatusCondition.None && rand.NextDouble() < move.StatusChance)
        {
            if (def.Status == StatusCondition.None)
            {
                def.Status = move.InflictStatus;
                Console.WriteLine($"{def.Name}陷入了{def.Status}状态！");
            }
        }
    }

    // 属性克制表
    double TypeEffect(PokemonType atk, PokemonType def)
    {
        // 备注：这里只实现部分常见属性克制
        if (atk == PokemonType.Fire && def == PokemonType.Grass) return 2;
        if (atk == PokemonType.Water && def == PokemonType.Fire) return 2;
        if (atk == PokemonType.Grass && def == PokemonType.Water) return 2;
        if (atk == PokemonType.Electric && def == PokemonType.Water) return 2;
        if (atk == PokemonType.Fire && def == PokemonType.Water) return 0.5;
        if (atk == PokemonType.Water && def == PokemonType.Grass) return 0.5;
        if (atk == PokemonType.Grass && def == PokemonType.Fire) return 0.5;
        if (atk == PokemonType.Electric && def == PokemonType.Grass) return 0.5;
        return 1;
    }

    // 使用道具
    void UseItem(Player player)
    {
        if (player.Bag.Count == 0)
        {
            Console.WriteLine("背包里没有道具。");
            return;
        }
        Console.WriteLine("请选择要使用的道具：");
        for (int i = 0; i < player.Bag.Count; i++)
        {
            var item = player.Bag[i];
            Console.WriteLine($"[{i + 1}] {item.Name} - {item.Description}");
        }
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int idx) || idx < 1 || idx > player.Bag.Count)
        {
            Console.WriteLine("输入有误，本回合跳过。");
            return;
        }
        var it = player.Bag[idx - 1];
        it.UseEffect(player.CurrentPokemon);
        // 道具用完移除
        player.Bag.RemoveAt(idx - 1);
    }

    // 切换宝可梦
    void SwitchPokemon(Player player)
    {
        Console.WriteLine("请选择要切换的宝可梦：");
        player.ShowPokemons();
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int idx) || idx < 1 || idx > player.Pokemons.Count || player.Pokemons[idx - 1].IsFainted)
        {
            Console.WriteLine("输入有误或宝可梦已昏迷，切换失败。");
            return;
        }
        player.CurrentIndex = idx - 1;
        Console.WriteLine($"你切换到了{player.CurrentPokemon.Name}！");
    }

    // 宝可梦昏迷处理
    void HandleFaint(Player player)
    {
        Console.WriteLine($"{player.CurrentPokemon.Name}已昏迷！");
        if (!player.AllFainted)
        {
            Console.WriteLine("请选择新的宝可梦上场：");
            player.ShowPokemons();
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int idx) && idx >= 1 && idx <= player.Pokemons.Count && !player.Pokemons[idx - 1].IsFainted)
                {
                    player.CurrentIndex = idx - 1;
                    Console.WriteLine($"{player.CurrentPokemon.Name}上场！");
                    break;
                }
                else
                {
                    Console.WriteLine("输入有误，请重新选择。");
                }
            }
        }
    }

    // 状态效果回合处理（如中毒、麻痹等）
    void ApplyStatusEffects(Pokemon p)
    {
        if (p.Status == StatusCondition.Poisoned)
        {
            int dmg = Math.Max(1, p.MaxHP / 8);
            p.HP = Math.Max(0, p.HP - dmg);
            Console.WriteLine($"{p.Name}因中毒受到了{dmg}点伤害！");
        }
        else if (p.Status == StatusCondition.Burned)
        {
            int dmg = Math.Max(1, p.MaxHP / 16);
            p.HP = Math.Max(0, p.HP - dmg);
            Console.WriteLine($"{p.Name}因灼伤受到了{dmg}点伤害！");
        }
        else if (p.Status == StatusCondition.Paralyzed)
        {
            if (new Random().NextDouble() < 0.25)
            {
                Console.WriteLine($"{p.Name}因麻痹无法行动！");
                throw new Exception("Paralyzed"); // 用异常跳出本回合
            }
        }
        // 其他状态可扩展
    }

    // 捕捉野生宝可梦（可在主流程或特殊事件中调用）
    void TryCatchPokemon(Player player, Pokemon wild)
    {
        Console.WriteLine($"你遇到了一只野生的{wild.Name}！是否要尝试捕捉？(y/n)");
        string input = Console.ReadLine();
        if (input.ToLower() == "y")
        {
            double rate = 0.3 + (1.0 - wild.HP / (double)wild.MaxHP) * 0.5;
            if (player.Pokemons.Count >= 6)
            {
                Console.WriteLine("你的宝可梦已满，无法捕捉。");
                return;
            }
            if (new Random().NextDouble() < rate)
            {
                player.Pokemons.Add(wild.Clone());
                Console.WriteLine($"恭喜你捕捉到了{wild.Name}！");
            }
            else
            {
                Console.WriteLine($"{wild.Name}挣脱了精灵球，捕捉失败。");
            }
        }
        else
        {
            Console.WriteLine($"你放弃了捕捉{wild.Name}。");
        }
    }

    // 扩展：背包管理（可在主菜单调用）
    void ShowBag(Player player)
    {
        Console.WriteLine("你的背包：");
        if (player.Bag.Count == 0)
        {
            Console.WriteLine("（空）");
            return;
        }
        for (int i = 0; i < player.Bag.Count; i++)
        {
            var item = player.Bag[i];
            Console.WriteLine($"[{i + 1}] {item.Name} - {item.Description}");
        }
    }
}
#endregion

// 程序入口
class Program
{
    static void Main(string[] args)
    {
        // 启动游戏
        PokemonGame game = new PokemonGame();
        game.Start();
    }
}

// 备注：
// 1. 本游戏为文字版宝可梦对战，支持多宝可梦、技能、属性、道具、状态、AI、捕捉、背包等。
// 2. 代码结构清晰，便于扩展和二次开发。
// 3. 可根据需要添加更多宝可梦、技能、道具、事件等。
// 4. 如需体验野外捕捉、背包管理等，可在主流程中调用TryCatchPokemon和ShowBag方法。
// 5. 所有功能均有注释，便于学习和修改。
