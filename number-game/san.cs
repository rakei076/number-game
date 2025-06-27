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
        
        int[,] board = new int[3,3]; 
        BoardInit(board);
        
        
        // ここにあなたのロジックを追加
        BoardPrint(board);
        ReadPlayerInput(board);
        BoardPrint(board);


 
        
    }
    static void BoardPrint(int[,] board)
    {
        Console.WriteLine("  0  1  2");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(i);
            for (int j =0;j<3;j++)
            {
                
                Console.Write("[");
                if(board[i,j]==FIRST_PLAYER)
                {
                    Console.Write("○");
                }
                else if(board[i,j]==SECOND_PLAYER)
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
    static void BoardInit(int[,]board)
    {
        const int BOARD_SIZE = 3; // 例として3x3のボードサイズを設定
        
        
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
                Console.Write("[ ");
                
                board[i,j] = EMPTY;
                Console.Write("]");
            }
            
        Console.WriteLine();// 改行
        }
    }
    static int ReadPlayerInput(int[,]board){
        Console.Write("> Input Row: ");
        int row =Convert.ToInt32(Console.ReadLine());
        Console.Write("> Input Column: ");
        int column =Convert.ToInt32(Console.ReadLine());
        if(row<0||row>2||column<0||column>2)
        {
            Console.WriteLine("無効な入力");
            return -1;
            if (board[row,column]!=EMPTY){
            Console.WriteLine("その場所に数字を置いています");
            return -1;
            }
        }
        board[row,column]=FIRST_PLAYER;
        return 0;
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
//      dotnet run