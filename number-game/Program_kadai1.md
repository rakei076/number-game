using System;
class Number_game
{
    public void Start()
    {
        int level = SelectLevel();  
        int number = GenerateNumber(level);
        int count=0;
        int guess =0;
        while(guess!=number)
        {
            guess = ReadPlayerInput();
            if (guess == -1)
            {
                continue;
            }
            count++;    
            if (CheckAnswer(number, guess,count))
            {
                break;
            }
            
        }

    }

    
    int SelectLevel()
    {
        

    Console.WriteLine("難易度を選ぶください、easy,medium,hard");
    string level = Console.ReadLine();
    int numberr =0;
    if(level=="easy")
    {
        numberr=10;

    }
    else if(level=="medium")
    {
        numberr=100;
    }
    else if(level=="hard")
    {
        numberr=1000;
    }
    else{
        Console.WriteLine("無効な難易度です．easy, medium, hardのいずれかを入力してください．");
        return -1;
    }
    return numberr;
    }




    int GenerateNumber(int numberr){

    Console.WriteLine("数当てゲームを始めます１から"+numberr+"の数字を当ててください");
    int number =new Random().Next(1,numberr+1);
    return number;

    }







    int ReadPlayerInput()
    {
        int guess =0;
            Console.WriteLine("数字を入力してください");
            guess =Convert.ToInt32(Console.ReadLine());
            if(guess<=-1){
                Console.WriteLine("無効な入力です．数字を入力してください．");
                return -1;
            }
            else{
            return guess;
            }
    }



    bool CheckAnswer(int number,int guess,int count){

        

        if(guess==number)
        {
            Console.WriteLine("正解！おめでとうございます！");
            Console.WriteLine("正解！おめでとうございます！"+count+"回目で正解しました！");
        }
        else if(guess<number)
        {
            
            Console.WriteLine("不正解"+"正解は入力した数字よりも大きいです．");
        }
        else if(guess>number)
        {
            
            Console.WriteLine("不正解"+"正解は入力した数字よりも小さいです．");
        }
        if(count==6)
        {
            Console.WriteLine("ゲームオーバー！正解は"+number+"でした");
            return true;
        }
        return false;
    }

}
    
class Program{
static void Main(string[] args){
        Number_game game =new Number_game();
        game.Start();
    }
}
    
//dotnet run
