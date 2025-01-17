﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToo
{
    public class Stone
    {
        // 盤面に何も置いていない状態は、EmptyのStoneが置いてあると考える
        public static readonly Stone Black = new Stone { Value = 'X' };
        public static readonly Stone White = new Stone { Value = 'O' };
        public static readonly Stone Empty = new Stone { Value = '.' };

        public char Value { get; private set; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Board : BoardBase<Stone>
    {
        public Board() : base(3, 3)
        {
            foreach (var ix in GetAllIndexes())
                this[ix] = Stone.Empty;
        }

        // 石を置けるか
        public bool CanPut(int index)
        {
            return this[index] == Stone.Empty;
        }

        // 終了しかた？
        public bool IsFin()
        {
            if (Judge() != Stone.Empty)
                return true;
            return this.GetVacantIndexes().Count() == 0;
        }

        // 空いている場所を列挙する
        public IEnumerable<int> GetVacantIndexes()
        {
            return GetAllIndexes().Where(x => this[x] == Stone.Empty);
        }

        // 全ての場所を列挙する
        public override IEnumerable<int> GetAllIndexes()
        {
            for (int y = 1; y <= 3; y++)
                for (int x = 1; x <= 3; x++)
                    yield return ToIndex(x, y);
        }

        // 縦横斜めすべての筋を列挙する
        public IEnumerable<List<int>> GetLineIndexes()
        {
            // 横
            for (int y = 1; y <= 3; y++)
            {
                var p = ToIndex(1, y);
                yield return new List<int> { p, p + 1, p + 2 };
            }
            // 縦
            for (int x = 1; x <= 3; x++)
            {
                var p = ToIndex(x, 1);
                var d = ToDirection(0, 1);
                yield return new List<int> { p, p + d, p + d + d };
            }
            // 斜め 左上から右下
            {
                var p = ToIndex(1, 1);
                var d = ToDirection(1, 1);
                yield return new List<int> { p, p + d, p + d + d };
            }
            // 斜め 右上から左下
            {
                var p = ToIndex(3, 1);
                var d = ToDirection(-1, 1);
                yield return new List<int> { p, p + d, p + d + d };
            }
        }

        // どちらが勝ったか？
        public Stone Judge()
        {
            foreach (var list in this.GetLineIndexes().ToList())
            {

                if (list.All(x => this[x] == Stone.White))
                    return Stone.White;
                if (list.All(x => this[x] == Stone.Black))
                    return Stone.Black;
            }
            return Stone.Empty;
        }
    }
}