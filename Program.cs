using System;
using System.Collections.Generic;

namespace Лабиринт_Двоичное_дерево
{
    class Program
    {
        static void Main(string[] args)
        {
            bool key = false, door = false;
            Map map = new Map(10, 10, key, door, 2);
            Character ch = new Character(map, 0, 0);
            while (!ch.win)
            {
                ch.Moove(Console.ReadLine());
            }
        }
    }
    class Tile
    {
        int type;
        public int Select_type
        {
            get
            {
                return type;
            }
        }
        public Tile(int type)
        {
            this.type = type;
        }
    }
    struct Coords_path
    {
        int x;
        int y;
        public Coords_path(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator == (Coords_path a, Coords_path b)
        {
            if (a.x == b.x && a.y == b.y) return true;
            else return false;
        }
        public static bool operator != (Coords_path a, Coords_path b)
        {
            if (a.x != b.x && a.y != b.y) return true;
            else return false;
        }
        public override string ToString()
        {
            return "(" + x + ";" + y + ")";
        }
    }
    class Map
    {
        Tile[,] map;
        public bool key;
        public bool door;
        int Quest_number;
        public List<Coords_path> path = null;
        public Map(int x, int y, bool key, bool door, int QNum)
        {
            Quest_number = QNum;
            x *= 3;
            y *= 3;
            this.key = key;
            this.door = door;
            map = new Tile[x, y];
            int[,] bones = Map_bones(x, y);
            bones = Labirynth_builder(bones);
            Map_zap(bones);
        }
        void Map_zap(int[,] bones)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new Tile(bones[i, j]);
                }
            }
        }
        int[,] Map_bones(int x, int y)
        {
            int[,] ret = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    ret[i, j] = 0;
                }
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        ret[i, j] = 1;
                    }
                }
            }
            return ret;
        }
        public int Show()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j].Select_type.ToString());
                }
                Console.WriteLine();
            }
            if (path != null)
            {
                Console.WriteLine("Где окажется персонаж, пройдя по пути: ");
                foreach(Coords_path c in path)
                {
                    Console.WriteLine(c.ToString());
                }
            }
            if (door == true)
            {
                Console.WriteLine("Лабиринт пройден\n");
                return 1;
            }
            else return 0;
        }
        int[,] Key_place(int[,] bones)
        {
            Random r = new Random();
            int x = r.Next(2, bones.GetLength(0) - 1), y = r.Next(2, bones.GetLength(1) - 1);
            bones[x, y] = 2;
            while (true)
            {
                if (bones[x - 1, y] == 1 && bones[x, y - 1] == 1 && bones[x, y + 1] == 1 && bones[x + 1, y] == 1)
                {
                    if (r.Next() % 2 == 0)
                    {
                        if (x != bones.GetLength(0) && y != bones.GetLength(1))
                        {
                            if (r.Next() % 2 == 0)
                            {
                                bones[x + 1, y] = 0;
                                x++;
                            }
                            else
                            {
                                bones[x, y + 1] = 0;
                                y++;
                            }
                        }
                        else
                        {
                            if (r.Next() % 2 == 0)
                            {
                                bones[x - 1, y] = 0;
                                x--;
                            }
                            else
                            {
                                bones[x, y - 1] = 0;
                                y--;
                            }
                        }
                    }
                }
                else break;
            }
            return bones;
        }
        int[,] Door_place(int[,] bones)
        {
            Random r = new Random();
            int x, y;
            while (true)
            {
                x = r.Next(2, bones.GetLength(0) - 1);
                y = r.Next(2, bones.GetLength(1) - 1);
                if (bones[x, y] != 2)
                {
                    bones[x, y] = 3;
                    break;
                }
            }
            while (true)
            {
                if (bones[x - 1, y] == 1 && bones[x, y - 1] == 1 && bones[x, y + 1] == 1 && bones[x + 1, y] == 1)
                {
                    if (r.Next() % 2 == 0)
                    {
                        if (x != bones.GetLength(0) && y != bones.GetLength(1))
                        {
                            if (r.Next() % 2 == 0)
                            {
                                bones[x + 1, y] = 0;
                                x++;
                            }
                            else
                            {
                                bones[x, y + 1] = 0;
                                y++;
                            }
                        }
                        else
                        {
                            if (r.Next() % 2 == 0)
                            {
                                bones[x - 1, y] = 0;
                                x--;
                            }
                            else
                            {
                                bones[x, y - 1] = 0;
                                y--;
                            }
                        }
                    }
                }
                else break;
            }
            return bones;
        }
        int[,] Path_builder(int[,] bones)
        {
            bones = Key_place(bones);
            bones = Door_place(bones);
            return bones;
        }
        int[,] Goal_builder(int[,] bones)
        {
            path = new List<Coords_path>();
            Random r = new Random();
            for (int i = 1; i < map.GetLength(0); i++)
            {
                for (int j = 1; j < map.GetLength(1) - 2; j++)
                {
                    if (r.Next() % 100 != 0)
                    {
                        if (bones[i + 1, j] == 0)
                        {
                            path.Add(new Coords_path(i + 1, j));
                        }
                        else
                        {
                            if (bones[i, j + 1] == 0)
                            {
                                path.Add(new Coords_path(i, j + 1));
                            }
                            else return bones;
                        }
                    }
                }
            }
            return bones;
        }
        int[,] Labirynth_builder(int[,] bones)
        {
            Random r = new Random();
            for (int i = 1; i < map.GetLength(0); i++)
            {
                for (int j = 1; j < map.GetLength(1) - 2; j++)
                {
                    if (i == 1)
                    {
                        bones[i, j] = 0;
                    }
                    else
                    {
                        if (r.Next() % 2 == 0)
                        {
                            bones[i - 1, j] = 0;
                        }
                        else
                        {
                            bones[i, j + 1] = 0;
                        }
                    }
                }
            }
            int n = bones.GetLength(0);
            int m = bones.GetLength(1);
            for (int a = 0; a < m; a++)
            {
                bones[n - 1, a] = 1;
            }
            for (int b = 0; b < n; b++)
            {
                bones[b, m - 1] = 1;
            }
            switch (Quest_number)
            {
                case 1:
                    bones = Path_builder(bones);
                    break;
                case 2:
                    bones = Goal_builder(bones);
                    break;
            }
            return bones;
        }
        public void Step_into(ref int x, ref int y, int character, int prev_x, int prev_y, ref Tile nxTile, ref Tile curTile, bool interact)
        {
            if (map[x, y].Select_type != 1)
            {
                if (nxTile.Select_type == 2 && interact)
                {
                    key = true;
                    curTile = new Tile(0);
                    map[x, y] = new Tile(0);
                }
                else if (nxTile.Select_type == 3 && key && interact)
                {
                    door = true;
                }
                else
                {
                    nxTile = new Tile(map[x, y].Select_type);
                    map[x, y] = new Tile(character);
                    if (prev_x == 0 && prev_y == 0)
                    {
                        map[prev_x, prev_y] = new Tile(1);
                    }
                    else if (curTile.Select_type == 2)
                    {
                        map[prev_x, prev_y] = new Tile(0);
                    }
                    else map[prev_x, prev_y] = curTile;
                }
            }
            else
            {
                x = prev_x;
                y = prev_y;
            }
        }
        public int Check_path(int x, int y)
        {
            Coords_path cp = new Coords_path(x, y);
            if (path[path.Count - 1] == cp)
            {
                Console.WriteLine("Правильно!");
                return 1;
            }
            else Console.WriteLine("Не правильно!");
            return 0;
        }
    }
    class Character
    {
        public bool win = false;
        int cur_x, prew_x;
        int cur_y, prew_y;
        int character = 5;
        Tile curTile, nxTile;
        public int Char_type
        {
            get
            {
                return character;
            }
        }
        Map map;
        public Character(Map map, int x, int y)
        {
            this.map = map;
            this.prew_x = x;
            this.prew_y = y;
            cur_x = 1;
            cur_y = 1;
            curTile = new Tile(0);
            nxTile = new Tile(0);
            Draw_char(false);
        }
        public void Moove(string command)
        {
            string[] s = command.Split('_');
            int n;
            switch (s[0])
            {
                case "вверх":
                    n = Convert.ToInt32(s[1]);
                    while (n > 0)
                    {
                        prew_y = cur_y;
                        prew_x = cur_x;
                        cur_x--;
                        Draw_char(false);
                        n--;
                    }
                    break;
                case "вниз":
                    n = Convert.ToInt32(s[1]);
                    while (n > 0)
                    {
                        prew_y = cur_y;
                        prew_x = cur_x;
                        cur_x++;
                        Draw_char(false);
                        n--;
                    }
                    break;
                case "влево":
                    n = Convert.ToInt32(s[1]);
                    while (n > 0)
                    {
                        prew_y = cur_y;
                        prew_x = cur_x;
                        cur_y--;
                        Draw_char(false);
                        n--;
                    }
                    break;
                case "вправо":
                    n = Convert.ToInt32(s[1]);
                    while (n > 0)
                    {
                        prew_y = cur_y;
                        prew_x = cur_x;
                        cur_y++;
                        Draw_char(false);
                        n--;
                    }
                    break;
                case "использовать":
                    Draw_char(true);
                    break;
                case "проверить":
                    int a = Convert.ToInt32(Console.ReadLine());
                    int b = Convert.ToInt32(Console.ReadLine());
                    if(map.Check_path(a,b) == 1)
                    {
                        win = true;
                    }
                    break;
                default:
                    Console.WriteLine("Такой команды не предусмотрено\n");
                    break;
            }
        }
        void Draw_char(bool interact)
        {
            curTile = nxTile;
            map.Step_into(ref cur_x, ref cur_y, Char_type, prew_x, prew_y, ref nxTile, ref curTile, interact);
            if (map.Show() == 1)
            {
                win = true;
            }
        }
    }
}
