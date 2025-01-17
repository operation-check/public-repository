﻿using System;
namespace TicTacToo
{
    class Controller : IObserver<Board>
    {
        private IPlayer _player1;
        private IPlayer _player2;
        private Board _board;

        // 試合の開始
        public void Run()
        {
            DecideFirstPlayer();
            _board = new Board();
            var game = new Game(_player1, _player2, _board);
            // 購読者は自分自身
            game.Subscribe(this);
            var win = game.Start();
        }

        // 先手を決める
        private void DecideFirstPlayer()
        {
            var b = Confirm("Are you first?");
            if (b)
            {
                _player1 = new HumanPlayer(Stone.Black);
                _player2 = new PerfectPlayer(Stone.White);
            }
            else
            {
                _player1 = new PerfectPlayer(Stone.Black);
                _player2 = new HumanPlayer(Stone.White);
            }
        }

        private IPlayer GetHumanPlayer() =>
             _player1 is HumanPlayer ? _player1 : _player2;

        // 盤面を表示
        private void Print(Board board)
        {
            Console.Clear();
            Console.WriteLine("You: {0}\n", GetHumanPlayer().Stone.Value);
            for (int y = 1; y <= 3; y++)
            {
                for (int x = 1; x <= 3; x++)
                {
                    Console.Write(board[x, y].Value + " ");
                }
                Console.WriteLine();
            }
        }

        // 終了した
        public void OnCompleted()
        {
            var win = _board.Judge();
            var human = GetHumanPlayer();
            if (win == Stone.Empty)
                Console.WriteLine("Draw");
            else if (win == human.Stone)
                Console.WriteLine("You Win");
            else
                Console.WriteLine("You Lose");
        }

        // エラー発生
        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        // 状況変化
        public void OnNext(Board value)
        {
            Print(value);
        }

        // (y/n)の確認
        public static bool Confirm(string message)
        {
            Console.Write(message);
            var left = Console.CursorLeft;
            var top = Console.CursorTop;
            try
            {
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.KeyChar == 'y')
                        return true;
                    if (key.KeyChar == 'n')
                        return false;
                    Console.CursorLeft = left;
                    Console.CursorTop = top;
                }
            }
            finally
            {
                Console.WriteLine();
            }
        }
    }
}