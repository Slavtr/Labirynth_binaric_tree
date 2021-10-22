using System;
using System.Collections.Generic;

namespace Лабиринт_Двоичное_дерево
{
    class Program
    {
        static void Main(string[] args)
        {
            bool key = false, door = false;
            Map map = new Map(10, 10, key, door, 4);
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
        public int x;
        public int y;
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
    /// <summary>
    /// Класс, создающий и пестующий карту. 
    /// Чтобы создать карту для задания 1 - QNum должен быть равен 1. Чтобы создать карту задания 2 - QNum должен быть равен 2.
    /// </summary>
    class Map
    {
        int stepcount = 1;
        Tile[,] map;
        public bool key;
        public bool door;
        int Quest_number;
        public List<Coords_path> path = new List<Coords_path>();
        string pathstr = "";
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
            if (Quest_number == 2)
            {
                Console.WriteLine("Где окажется персонаж, пройдя по пути: \n" + pathstr);
            }
            else if(Quest_number == 3)
            {
                Console.WriteLine("Постройте оптимальный маршрут в конечную точку: \n" + pathstr);
            }
            else if(Quest_number == 4)
            {
                Console.WriteLine("Исправьте неправильный маршрут: \n" + pathstr);
            }
            if (door == true)
            {
                if (Quest_number == 3)
                {
                    if (stepcount < path.Count)
                    {
                        Console.WriteLine("Маршрут оптимален\n");
                    }
                    else
                    {
                        Console.WriteLine("Маршрут неоптимален\n");
                    }
                }
                Console.WriteLine("Задание выполнено\n");
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
        int Goal_builder(int[,] bones, int xstart, int ystart, List<Coords_path> coords_Paths)
        {
            Random r = new Random();
            int xlast = 0, ylast = 0, d = -1;
            int[,] temp = new int[bones.GetLength(0), bones.GetLength(1)];
            for(int i = 0; i<bones.GetLength(0); i++)
            {
                for(int j = 0; j<bones.GetLength(1); j++)
                {
                    temp[i, j] = bones[i, j];
                }
            }
            do
            {
                xlast = r.Next(1, bones.GetLength(0) - 1);
                ylast = r.Next(1, bones.GetLength(1) - 1);
            } while (bones[xlast, ylast] == 1);
            temp[xstart, ystart] = d;
            while(temp[xlast, ylast] == 0)
            {
                for(int i = 1; i < temp.GetLength(0); i++)
                {
                    for(int j = 1; j<temp.GetLength(1); j++)
                    {
                        if (temp[i, j] == d)
                        {
                            if (temp[i + 1, j] == 0)
                            {
                                temp[i + 1, j] = d - 1;
                            }
                            if (temp[i, j + 1] == 0)
                            {
                                temp[i, j + 1] = d - 1;
                            }
                            if (temp[i - 1, j] == 0)
                            {
                                temp[i - 1, j] = d - 1;
                            }
                            if (temp[i, j - 1] == 0)
                            {
                                temp[i, j - 1] = d - 1;
                            }
                        }
                    }
                }
                d--;
            }
            Coords_path n = new Coords_path(xlast, ylast);
            coords_Paths.Add(n);
            Coords_path usl = new Coords_path(xstart, ystart);
            int count = 0;
            while (d != -1)   
            {
                if(count == 50)
                {
                    return -1;
                }
                if (temp[xlast - 1, ylast] == d + 1)
                {
                    xlast--;
                    temp[xlast + 1, ylast] = 0;
                    n = new Coords_path(xlast, ylast);
                    coords_Paths.Add(n);
                    d++;
                    count = 0;
                }
                else if (temp[xlast, ylast - 1] == d + 1)
                {
                    ylast--;
                    temp[xlast, ylast + 1] = 0;
                    n = new Coords_path(xlast, ylast);
                    coords_Paths.Add(n);
                    d++;
                    count = 0;
                }
                else if (temp[xlast + 1, ylast] == d + 1)
                {
                    xlast++;
                    temp[xlast + 1, ylast] = 0;
                    n = new Coords_path(xlast, ylast);
                    coords_Paths.Add(n);
                    d++;
                    count = 0;
                }
                else if (temp[xlast, ylast + 1] == d + 1)
                {
                    ylast++;
                    temp[xlast, ylast + 1] = 0;
                    n = new Coords_path(xlast, ylast);
                    coords_Paths.Add(n);
                    d++;
                    count = 0;
                }
                count++;
            }
            return 0;
        }
        string Path_to_string(List<Coords_path> coords_Paths)
        {
            string retstr = "";
            List<List<Coords_path>> rebra = new List<List<Coords_path>>();
            for(int j = 1; j<coords_Paths.Count; j++)
            {
                rebra.Add(new List<Coords_path>());
                rebra[j - 1].Add(coords_Paths[j - 1]);
                rebra[j - 1].Add(coords_Paths[j]);
            }
            int i = 0;
            while (i < rebra.Count - 1) 
            {
                int count = 0;
                if (rebra[i][0].x < rebra[i][1].x)
                {
                    while (rebra[i][0].x < rebra[i][1].x && i < rebra.Count - 1)
                    {
                        count++;
                        i++;
                    }
                    retstr += "вниз_" + count.ToString() + ";\n";
                    count = 0;
                }
                if(rebra[i][0].x > rebra[i][1].x)
                {
                    while (rebra[i][0].x > rebra[i][1].x && i < rebra.Count - 1)
                    {
                        count++;
                        i++;
                    }
                    retstr += "вверх_" + count.ToString() + ";\n";
                    count = 0;
                }
                if (rebra[i][0].y > rebra[i][1].y)
                {
                    while (rebra[i][0].y > rebra[i][1].y && i < rebra.Count - 1)
                    {
                        count++;
                        i++;
                    }
                    retstr += "влево_" + count.ToString() + ";\n";
                    count = 0;
                }
                if (rebra[i][0].y < rebra[i][1].y)
                {
                    while (rebra[i][0].y < rebra[i][1].y && i < rebra.Count - 1)
                    {
                        count++;
                        i++;
                    }
                    retstr += "вправо_" + count.ToString() + ";\n";
                    count = 0;
                }
                if(rebra[i][0].x == rebra[i][1].x && rebra[i][0].y == rebra[i][1].y)
                {
                    i++;
                }
            }
            if (rebra[i][0].x < rebra[i][1].x)
            {
                retstr += "вниз_1;\n";
            }
            if (rebra[i][0].x > rebra[i][1].x)
            {
                retstr += "вверх_1;\n";
            }
            if (rebra[i][0].y > rebra[i][1].y)
            {
                retstr += "влево_1;\n";
            }
            if (rebra[i][0].y < rebra[i][1].y)
            {
                retstr += "вправо_1;\n";
            }
            return retstr;
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
            int o = 0;
            List<Coords_path> p2;
            List<Coords_path> p1;
            switch (Quest_number)
            {
                case 1:
                    bones = Path_builder(bones);
                    break;
                case 2:
                    o = 0;
                    do
                    {
                        o = Goal_builder(bones, 1, 1, path);
                    } while (o != 0);
                    path.Reverse();
                    pathstr = Path_to_string(path);
                    break;
                case 3:
                    o = 0;
                    do
                    {
                        p1 = new List<Coords_path>();
                        o = Goal_builder(bones, 1, 1, p1);
                    } while (o != 0);
                    p1.Reverse();
                    do
                    {
                        p2 = new List<Coords_path>();
                        o = Goal_builder(bones, p1[p1.Count - 1].x, p1[p1.Count - 1].y, p2);
                    } while (o != 0);
                    p2.Reverse();
                    foreach (Coords_path cp in p1) path.Add(cp);
                    foreach (Coords_path cp in p2) path.Add(cp);
                    pathstr = Path_to_string(path);
                    break;
                case 4:
                    o = 0;
                    do
                    {
                        p1 = new List<Coords_path>();
                        o = Goal_builder(bones, 1, 1, p1);
                    } while (o != 0);
                    p1.Reverse();
                    do
                    {
                        p2 = new List<Coords_path>();
                        o = Goal_builder(bones, p1[p1.Count - 1].x, p1[p1.Count - 1].y, p2);
                    } while (o != 0);
                    p2.Reverse();
                    foreach (Coords_path cp in p1) path.Add(cp);
                    foreach (Coords_path cp in p2) path.Add(cp);
                    pathstr = Path_to_string(path);
                    bones = Path_builder(bones);
                    break;
            }
            return bones;
        }
        public void Step_into(ref int x, ref int y, int character, int prev_x, int prev_y, ref Tile nxTile, ref Tile curTile, bool interact)
        {
            if(Quest_number == 3)
            {
                stepcount++;
            }
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
                else if(x == path[path.Count-1].x && y == path[path.Count-1].y && Quest_number == 3)
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
