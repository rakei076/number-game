using System;

// 这个例子展示：static方式 vs 面向对象方式的区别

// ====== 方式1：Static方式（你现在使用的方式）======
class StaticExample
{
    // 每次调用函数都需要传递棋盘数据
    public static void PrintBoard_Static(int[,] board)
    {
        Console.WriteLine("=== Static方式 ===");
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Console.Write($"[{board[i,j]}]");
            }
            Console.WriteLine();
        }
    }
    
    public static void PlacePiece_Static(int[,] board, int row, int col, int player)
    {
        board[row, col] = player;
        Console.WriteLine($"Static方式：放置了棋子在({row},{col})");
    }
}

// ====== 方式2：面向对象方式 ======
class BoardClass
{
    // 棋盘数据属于这个类，不需要到处传递
    private int[,] board;
    
    // 构造函数：创建对象时初始化
    public BoardClass()
    {
        board = new int[3,3];
        Console.WriteLine("BoardClass对象被创建了！");
    }
    
    // 不需要传递board参数，因为board是这个类的数据
    public void PrintBoard()
    {
        Console.WriteLine("=== 面向对象方式 ===");
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Console.Write($"[{board[i,j]}]");
            }
            Console.WriteLine();
        }
    }
    
    public void PlacePiece(int row, int col, int player)
    {
        board[row, col] = player;
        Console.WriteLine($"面向对象方式：放置了棋子在({row},{col})");
    }
}

// ====== 主程序：展示两种方式的差异 ======
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("让我们比较两种方式的差异：\n");
        
        // ========== Static方式 ==========
        Console.WriteLine("【Static方式】");
        int[,] staticBoard = new int[3,3];  // 必须手动创建棋盘
        
        // 每次调用函数都要传递board参数
        StaticExample.PrintBoard_Static(staticBoard);
        StaticExample.PlacePiece_Static(staticBoard, 0, 0, 1);
        StaticExample.PrintBoard_Static(staticBoard);  // 又要传递board
        
        Console.WriteLine("\n" + "=".PadLeft(40, '=') + "\n");
        
        // ========== 面向对象方式 ==========
        Console.WriteLine("【面向对象方式】");
        BoardClass myBoard = new BoardClass();  // 创建对象，自动初始化
        
        // 不需要传递board参数，因为board属于myBoard对象
        myBoard.PrintBoard();
        myBoard.PlacePiece(0, 0, 1);
        myBoard.PrintBoard();  // 简洁！不需要传递参数
        
        Console.WriteLine("\n【总结差异】");
        Console.WriteLine("Static方式：");
        Console.WriteLine("  - 每次调用函数都要传递board参数");
        Console.WriteLine("  - BoardPrint(board), BoardUpdate(row,col,board)");
        Console.WriteLine("  - 数据和函数是分离的");
        
        Console.WriteLine("\n面向对象方式：");
        Console.WriteLine("  - 数据(board)和函数(PrintBoard)在一起");
        Console.WriteLine("  - myBoard.PrintBoard() - 简洁！");
        Console.WriteLine("  - 数据被封装在对象里");
    }
} 