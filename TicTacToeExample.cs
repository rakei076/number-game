using System;

// 井字棋棋盘示例程序
class TicTacToeExample
{
    // 定义常量 - 表示棋盘上不同的状态
    const int EMPTY = 0;          // 空格子
    const int FIRST_PLAYER = 1;   // 先手玩家（显示○）
    const int SECOND_PLAYER = -1; // 后手玩家（显示×）
    
    static void Main()
    {
        Console.WriteLine("=== 井字棋棋盘示例 ===");
        
        // 第1步：创建3x3的二维数组
        int[,] cells = new int[3, 3];
        
        // 第2步：用循环把所有格子初始化为空（EMPTY=0）
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                cells[row, col] = EMPTY;
            }
        }
        
        Console.WriteLine("初始的空棋盘：");
        ShowBoard(cells);
        
        // 第3步：测试在棋盘上放一些棋子
        Console.WriteLine("\n放入一些棋子后：");
        cells[0, 1] = FIRST_PLAYER;   // 在(0,1)位置放先手的棋子
        cells[1, 1] = SECOND_PLAYER;  // 在(1,1)位置放后手的棋子
        cells[2, 0] = FIRST_PLAYER;   // 在(2,0)位置放先手的棋子
        
        ShowBoard(cells);
        
        // 演示如何访问数组中的值
        Console.WriteLine($"\n位置(0,1)的值是：{cells[0, 1]}");
        Console.WriteLine($"位置(1,1)的值是：{cells[1, 1]}");
        Console.WriteLine($"位置(2,2)的值是：{cells[2, 2]}");
    }
    
    // 显示棋盘的函数
    static void ShowBoard(int[,] cells)
    {
        Console.WriteLine("    0   1   2");  // 列号
        
        for (int row = 0; row < 3; row++)
        {
            Console.Write($"{row} ");  // 行号
            
            for (int col = 0; col < 3; col++)
            {
                Console.Write("[");
                
                // 根据数值显示不同符号
                if (cells[row, col] == FIRST_PLAYER)
                {
                    Console.Write("○");  // 先手显示○
                }
                else if (cells[row, col] == SECOND_PLAYER)
                {
                    Console.Write("×");  // 后手显示×
                }
                else
                {
                    Console.Write(" ");  // 空格显示空白
                }
                
                Console.Write("]");
            }
            Console.WriteLine();  // 换行
        }
    }
} 