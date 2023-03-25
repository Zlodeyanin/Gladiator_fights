using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gladiator_fights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            arena.Work();
        }
    }

    class Fighter
    {
        public Fighter(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public string Name { get; private set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }

        public virtual void Attack(Fighter fighter)
        {
            fighter.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public void ShowStats()
        {
            Console.WriteLine($"Я {Name}, у меня {Health} здоровья и {Damage} урона");
        }
    }

    class Warrior : Fighter
    {
        private int _armor;

        public Warrior(string name, int health, int damage, int armor) : base(name, health, damage)
        {
            _armor = armor;
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage - _armor;
        }

        public override void Attack(Fighter fighter)
        {
            int rage = 10;
            Damage += rage;
            base.Attack(fighter);
        }
    }

    class Mage : Fighter
    {
        private int _mana;

        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            _mana = 100;
        }

        public override void TakeDamage(int damage)
        {
            Heal();
            base.TakeDamage(damage);
        }

        private void Heal()
        {
            int healCost = 100;
            int heal = 50;
            int lowHealth = 200;

            if (Health <= lowHealth)
            {
                if (_mana > 0)
                {
                    Console.WriteLine($"Я {Name} и я исцелю себя!");
                    _mana -= healCost;
                    Health += heal;
                }
            }
        }
    }

    class Rogue : Fighter
    {
        private int _stealth;

        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            _stealth = 0;
        }

        public override void Attack(Fighter fighter)
        {
            ShadowStrike();
            base.Attack(fighter);
        }

        private void ShadowStrike()
        {
            int stealthRegeneration = 50;
            int maxStealth = 100;
            int criticalDamageModificator = 3;

            if (_stealth == maxStealth)
            {
                Console.WriteLine($"Я {Name}, я ушёл в тень и нанесу критический урон.");
                Damage *= criticalDamageModificator;
                _stealth = 0;
            }
            else
            {
                _stealth += stealthRegeneration;
            }
        }
    }

    class Archer : Fighter
    {
        public Archer(string name, int health, int damage) : base(name, health, damage) { }

        public override void Attack(Fighter fighter)
        {
            CriticalHit(fighter);
            base.Attack(fighter);
        }

        private void CriticalHit(Fighter fighter)
        {
            int criticalDamageModificator = 3;
            int opponentLowHealth = 200;

            if (fighter.Health <= opponentLowHealth)
            {
                Console.WriteLine($"Я {Name} и я нашёл уязвимые места моего соперника");
                Damage *= criticalDamageModificator;
            }
        }

    }

    class Summoner : Fighter
    {
        private Fighter _minion;

        public bool NeedProtection
        {
            get
            {
                int lowHealth = 200;
                return Health <= lowHealth;
            }
            private set { }
        }

        public Summoner(string name, int health, int damage, Fighter minion) : base(name, health, damage)
        {
            _minion = minion;
        }

        public override void TakeDamage(int damage)
        {
            if (NeedProtection == true)
            {
                if (_minion.Health > 0)
                {
                    Console.WriteLine($"Я {Name}, я призвал голема для защиты");
                    _minion.TakeDamage(damage);
                }
                else
                {
                    base.TakeDamage(damage);
                }
            }
            else
            {
                base.TakeDamage(damage);
            }
        }
    }

    class Arena
    {
        public void Work()
        {
            const string CommandChooseFirstFigter = "1";
            const string CommandChooseSecondFigter = "2";
            const string CommandStartFight = "3";
            const string CommandExit = "4";

            bool isPlaying = true;
            Console.WriteLine("Добро пожаловать в гладиаторские бои. Выбери двух бойцов и смотри кто победит!");
            Fighter firstFighter = null;
            Fighter secondFighter = null;
            Arena arena = new Arena();

            while (isPlaying)
            {
                Fighter[] fighters = { 
                    new Warrior("Воин", 400, 50, 30),
                    new Mage("Маг", 400, 100),
                    new Rogue("Разбойник", 350, 70),
                    new Archer("Лучник", 350, 100),
                    new Summoner("Призыватель", 350, 70, new Fighter("Голем", 150, 0)) };
                Console.WriteLine($"Введите - {CommandChooseFirstFigter}, чтобы выбрать первого бойца");
                Console.WriteLine($"Введите - {CommandChooseSecondFigter}, чтобы выбрать оппонента");
                Console.WriteLine($"Введите - {CommandStartFight}, чтобы начать бой");
                Console.WriteLine($"Введите - {CommandExit}, чтобы завершить бои.");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandChooseFirstFigter:
                        firstFighter = arena.ChooseFigher(fighters);
                        break;

                    case CommandChooseSecondFigter:
                        secondFighter = arena.ChooseFigher(fighters);
                        break;

                    case CommandStartFight:
                        Fight(firstFighter, secondFighter);
                        break;

                    case CommandExit:
                        Console.WriteLine("Арена закрывается..");
                        isPlaying = false;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowFighers(Fighter[] fighters)
        {
            for (int i = 0; i < fighters.Length; i++)
            {
                Console.Write($"{i + 1} - ");
                fighters[i].ShowStats();
            }
        }

        private Fighter ChooseFigher(Fighter[] fighters)
        {
            Fighter fighter;
            ShowFighers(fighters);
            Console.WriteLine("Выберите первого бойца: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int figherIndex))
            {
                if (figherIndex <= fighters.Length)
                {
                    fighter = fighters[figherIndex - 1];
                    return fighter;
                }
                else
                {
                    Console.WriteLine("Бойца с таким номером нет.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод.");
                return null;
            }
        }

        private void Fight(Fighter firstFighter, Fighter secondFighter)
        {
            if (isFighterChosen(firstFighter, secondFighter))
            {
                while (firstFighter.Health > 0 && secondFighter.Health > 0)
                {
                    firstFighter.ShowStats();
                    secondFighter.ShowStats();
                    firstFighter.Attack(secondFighter);
                    secondFighter.Attack(firstFighter);
                    Console.WriteLine("------------------------");
                }

                if (firstFighter.Health <= 0 && secondFighter.Health <= 0)
                {
                    Console.WriteLine("Ничья!");
                }
                else if (firstFighter.Health <= 0)
                {
                    Console.WriteLine($"В битве побеждает {secondFighter.Name} !");
                }
                else
                {
                    Console.WriteLine($"В битве побеждает {firstFighter.Name} !");
                }
            }
            else
            {
                Console.WriteLine("Бойцы не выбраны !");
            }
        }

        private bool isFighterChosen(Fighter firstFighter, Fighter secondFighter)
        {
            if (firstFighter == null || secondFighter == null)
            {
                return false;
            }
            else
            {
                if (firstFighter == secondFighter)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
