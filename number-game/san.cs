using System;

class MyGame
{
    // ここにゲームの主処理を書く
    public void Start()
    {
        Console.WriteLine("ゲームを開始します！");
        const int BOARD_SIZE = 3; // 例として3x3のボードサイズを設定
        
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j =0;j<BOARD_SIZE;j++)
            {
                Console.Write("[]");
            }
        Console.WriteLine();// 改行
        }
        // ここにあなたのロジックを追加
        
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

//dotnet run