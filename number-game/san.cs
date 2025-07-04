using System;

class MyGame
{
        const int EMPTY = 0;
        const int FIRST_PLAYER = 1;
        const int SECOND_PLAYER = -1;
        private static int currentplayer = FIRST_PLAYER;
        private static int turnNumber = 1;

    // ここにゲームの主処理を書く
    public void Start()
    {

        
        Console.WriteLine("ゲームを開始します！");
        
        int[,] board = new int[3,3]; 
        
        BoardInit(board);
        kaisu(board);
        
        
        // ここにあなたのロジックを追加
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
    static bool ReadPlayerInput(int[,]board){
        Console.Write("> Input Row: ");
        int row;
        bool success =int.TryParse(Console.ReadLine(),out row);
        if(!success)
        {
            Console.WriteLine("Row :invalid input");
            return false;
        }
        Console.Write("> Input Column: ");
        int column;
        success =int.TryParse(Console.ReadLine(),out column);
        if(!success)
        {
            Console.WriteLine("Column :invalid input");
            return false;
        }
        
        if (BoardUpdate(row,column,board)==false){
            return false;
        }
    
            
        board[row,column]=currentplayer;
        Console.WriteLine($"Turn:{turnNumber} Player:{(currentplayer==FIRST_PLAYER?"○":"×")}");
        
        return true;
    }
    static bool BoardUpdate(int row,int column,int[,]board)
    {
        if(row<0||row>2||column<0||column>2)
        {
            Console.WriteLine("Row :out of range");
            return false;
        }
        if (board[row,column]!=EMPTY){
            Console.WriteLine("その場所に数字を置いています");
            return false;
        }
        return true;
        
    }
    static void kaisu(int[,]board){
        if(BoardCheckWinner(board)==1){
            Console.Write($"game over 勝の方は{(currentplayer==FIRST_PLAYER?"○":"×")}");
            Console.WriteLine();
            gameover();
            return;
        }
        if (turnNumber==10){
                gameover();
                return;
            }
        currentplayer = FIRST_PLAYER;
        if(turnNumber%2==0)
        {
            currentplayer = SECOND_PLAYER;
        }
        
        if (ReadPlayerInput(board))
        {
            turnNumber++;
            BoardPrint(board);
           
            Console.Write("------------");
            Console.WriteLine();
            
        }
        kaisu(board);
    }
    static void gameover(){
        Console.Write("game over");
        Console.WriteLine();
        return;
    }
    static int BoardCheckWinner(int[,]board){
        for(int i=0;i<3;i++){
            if(board[i,0]!=EMPTY&&board[i,0]==board[i,1]&&board[i,1]==board[i,2]){
                return 1;
            }
        }
        for(int i=0;i<3;i++){
            if(board[0,i]!=EMPTY&&board[0,i]==board[1,i]&&board[1,i]==board[2,i]){
                return 1;
            }
        }
        if(board[0,0]!=EMPTY&&board[0,0]==board[1,1]&&board[1,1]==board[2,2]){
            return 1;
        }
        if(board[0,2]!=EMPTY&&board[0,2]==board[1,1]&&board[1,1]==board[2,0]){
            return 1;
        }
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