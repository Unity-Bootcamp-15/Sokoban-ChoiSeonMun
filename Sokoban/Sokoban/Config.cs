using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public static class Config
    {
        public static string Wall = "#";
        public static string Player = "P";
        public static string Box = "@";
        public static string GoalHasBox = "*";
        public static string GoalHasNoBox = "O";
        public static string ClearMessage = "축하합니다! 게임을 클리어하셨습니다!";
        public static int MinLevel = 1;
        public static int MaxLevel = 3;
        public static string NextLevelNoticeMessage = "축하합니다! 다음 레벨로 이동하려면 아무 키나 누르세요.";
    }
}
