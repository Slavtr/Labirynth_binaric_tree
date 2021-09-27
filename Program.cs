using System;

namespace Лабиринт_Двоичное_дерево
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map(10, 10);
            map.Show();
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
    class Map
    {
        Tile[,] map;
        public Map(int x, int y)
        {
            x *= 3;
            y *= 3;
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
        public void Show()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j].Select_type.ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine(map.GetLength(0) + " " + map.GetLength(1));
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
            bones = Path_builder(bones);
            return bones;
        }
    }
}
