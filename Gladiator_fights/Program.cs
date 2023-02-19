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
            const string CommandChooseWarrior = "1";
            const string CommandChooseMage = "2";
            const string CommandChooseRogue = "3";
            const string CommandChooseArcher = "4";
            const string CommandChooseSummoner = "5";
            const string CommandExit = "6";

            bool isGame = true;
            Console.WriteLine("Добро пожаловать в гладиаторские бои. Выбери двух бойцов и смотри кто победит!");
            Fighter firstFighter = null;
            Fighter secondFighter = null;
            Arena arena = new Arena();

            while (isGame)
            {
                Fighter[] fighters = { new Warrior("Воин", 400, 50, 30), new Mage("Маг", 400, 100), new Rogue("Разбойник", 350, 70),
                new Archer("Лучник", 350, 60, 2), new Summoner("Призыватель", 350, 60, new Fighter("Голем", 100, 0)) };
                Console.WriteLine($"Введите - {CommandChooseWarrior}, чтобы выбрать война. Воин носит крепкую броню и получает меньше урона");
                Console.WriteLine($"Введите - {CommandChooseMage}, чтобы выбрать мага. Маг может исцелить себя");
                Console.WriteLine($"Введите - {CommandChooseRogue}, чтобы выбрать разбойника. Разбойник может нанести критический удар из укрытия");
                Console.WriteLine($"Введите - {CommandChooseArcher}, чтобы выбрать лучника. Лучник быстро стреляет и наносит удвоенный урон");
                Console.WriteLine($"Введите - {CommandChooseSummoner}, чтобы выбрать призывателя. Призыватель может призвать голема и защититься");
                Console.WriteLine($"Введите - {CommandExit}, чтобы завершить бои.");
                Console.WriteLine("\nВыберите первого бойца");
                string chooseFirstFighter = Console.ReadLine();

                if (chooseFirstFighter == CommandExit)
                {
                    Console.WriteLine("Завершение работы...");
                    Console.ReadKey();
                    break;
                }

                if (int.TryParse(chooseFirstFighter, out int firstFigterIndex))
                {
                    firstFighter = arena.ChooseFigher(fighters, firstFigterIndex);
                }

                Console.WriteLine("\nВыберите оппонента");
                string chooseSecondFighter = Console.ReadLine();

                if (int.TryParse(chooseSecondFighter, out int secondFighterIndex))
                {
                    if (secondFighterIndex != Convert.ToInt32(CommandExit))
                    {
                        secondFighter = arena.ChooseFigher(fighters, secondFighterIndex);

                        if (secondFighter != firstFighter && secondFighter != null && firstFighter != null)
                        {
                            Console.WriteLine("Бойцы выбраны, начнём бой!");
                            arena.Fight(firstFighter, secondFighter);
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Неккоректный ввод или боец не может воевать сам с собой");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Завершение работы...");
                        Console.ReadKey();
                        break;
                    }
                }
            }
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

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public void ShowStats()
        {
            Console.WriteLine($"Я {Name}, у меня {Health} здоровья и сейчас я нанесу оппоненту {Damage} урона");
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

        public override void TakeDamage(int damage)
        {
            ShadowStrike();
            base.TakeDamage(damage);
        }

        private void ShadowStrike()
        {
            int stealthRegeneration = 50;
            int maxStealth = 100;

            if (_stealth == maxStealth)
            {
                Console.WriteLine($"Я {Name}, я ушёл в тень и нанесу критический урон.");
                Damage = Damage * 3;
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
        public Archer(string name, int health, int damage, int attackSpeed) : base(name, health, damage * attackSpeed) { }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }
    }

    class Summoner : Fighter
    {
        private Fighter _minion;

        public Summoner(string name, int health, int damage, Fighter minion) : base(name, health, damage)
        {
            _minion = minion;
        }

        public override void TakeDamage(int damage)
        {
            if (ifMasterNeedProtect() == true)
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

        private bool ifMasterNeedProtect()
        {
            int lowHealth = 200;
            return Health <= lowHealth;
        }
    }

    class Arena
    {
        public Fighter ChooseFigher(Fighter[] fighters, int userInput)
        {
            Fighter fighter;

            if (userInput > fighters.Length)
            {
                Console.WriteLine("Бойца с таким номером нет");
                return null;
            }
            else
            {
                fighter = fighters[userInput - 1];
                return fighter;
            }
        }

        public void Fight(Fighter firstFighter, Fighter secondFighter)
        {
            while (firstFighter.Health > 0 && secondFighter.Health > 0)
            {
                firstFighter.ShowStats();
                secondFighter.ShowStats();
                firstFighter.TakeDamage(secondFighter.Damage);
                secondFighter.TakeDamage(firstFighter.Damage);
                Console.WriteLine("------------------------");
            }

            if (firstFighter.Health <= 0)
            {
                Console.WriteLine($"В битве побеждает {secondFighter.Name} !");
            }
            else if (secondFighter.Health <= 0)
            {
                Console.WriteLine($"В битве побеждает {firstFighter.Name} !");
            }
        }
    }
}
