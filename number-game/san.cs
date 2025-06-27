using System;

class MyGame
{
        const int EMPTY = 0;
        const int FIRST_PLAYER = 1;
        const int SECOND_PLAYER = -1;
    // ここにゲームの主処理を書く
    public void Start()
    {

        
        Console.WriteLine("ゲームを開始します！");
        const int BOARD_SIZE = 3; // 例として3x3のボードサイズを設定
        int[,] array_2d = new int[3,3]; 
        
        Console.Write("   "); // 行番号のスペース
        for(int k =0;k<3;k++)
        {
            Console.Write(k + "  ");
        }
        Console.WriteLine(); // 改行

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            Console.Write(i);
            for (int j =0;j<BOARD_SIZE;j++)
            {
                
                Console.Write("[ ]");
            }
            
        Console.WriteLine();// 改行
        }
        // ここにあなたのロジックを追加
        showBoard(array_2d);
        array_2d[0,1] =FIRST_PLAYER;
        array_2d[1,1] =SECOND_PLAYER;
        array_2d[2,0] =FIRST_PLAYER;
        showBoard(array_2d);
        
    }
    static void showBoard(int[,] array_2d)
    {
        Console.WriteLine("  0  1  2");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(i);
            for (int j =0;j<3;j++)
            {
                
                Console.Write("[");
                if(array_2d[i,j]==FIRST_PLAYER)
                {
                    Console.Write("○");
                }
                else if(array_2d[i,j]==SECOND_PLAYER)
                {
                    Console.Write("x");
                }
                else
                {
                    Console.Write(" ");
                }
                Console.Write("]");
            }
            
        Console.WriteLine();// 改行
        }

    }
    
}

class Program
{
    static void Main(string[] args)
    {
        MyGame game = new MyGame();
        game.Start();
    }
}
//   cd "D:\unity\学校项目\number-game\number-game"
//dotnet run