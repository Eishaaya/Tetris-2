using Microsoft.Xna.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tetris
{
    static class Extensions
    {
        public static Random random = new Random();
        public static int Previous(this Random random, int min, int max)
        {
            switch (random.Next(0, 2))
            {
                case 0:
                    return random.Next(int.MinValue, min);
                default:
                    return random.Next(max + 1, int.MaxValue);
            }
        }

        public static float Slope(Vector2 point1, Vector2 point2)
        {
            return (point2.Y - point1.Y) / (point2.X - point1.X);
        }

        public static float PointAt(this Vector2 start, Vector2 target)
        {
            var difference = target - start;
            return (float)Math.Atan2(difference.X, difference.Y);
        }

        public static ulong Factorial(this ulong number)
        {
            ulong result = 1;
            while (number > 1)
            {
                result *= number;
                number--;
            }
            return result;
        }

        public static double Factorial(this double number)
        {
            double result = 1;
            while (number > 1)
            {
                result *= number;
                number--;
            }
            return result;
        }

        public static void ColorPoints(this List<Sprite> points, Color newColor)
        {
            for (int i = 0; i < points.Count; i++)
            {
                var degree = (float)Math.Pow(i / (points.Count != 1 ? (float)points.Count - 1 : 1), 1.75);
                points[i].Color = Color.Lerp(points[i].originalColor, newColor, degree);
            }
        }

        public static float AddTill(this float number, float endCondition, float amount)
        {
            return number + (float)(int)((endCondition - number) / amount + .99f) * amount;
        }

        public static float LoopCalc(this float current, float target, float max, float min = 0)
        {
            current %= max - min;
            target %= max - min;
            current.AddTill(min, max - min);
            target.AddTill(min, max - min);

            float result;

            if (current < target)
            {
                result = target - current;
                var alt = current + max - min - target;
                result = result < alt ? result : alt * -1;
            }
            else
            {
                result = current - target;
                var alt = current - max + min - target;
                result = result < alt * 1 ? result * -1 : alt;
            }

            return result;
        }

        public static double[] AllAddBy(this double[] numbers, double addend)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] += addend;
            }
            return numbers;
        }

        public static double[] InvertAgainst(this double[] numbers, double number)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = number - numbers[i];
            }
            return numbers;
        }

        public static Vector2 ConvertPos(this Vector2 location, Vector2 scale, Vector2 offset)
        {
            return (location - offset) / scale;
        }

        #region behaviors

        //public static void Idle(this Enemy enemy)
        //{

        //}

        //public static void Move(this Enemy enemy, Vector2 targetLocation, Enemy.MoveType type, params int[] moveNumber)
        //{
        //    var bounds = Game1.bounds;

        //    if (enemy.CurrentState != Enemy.EnemyState.Attacking && enemy.CurrentState != Enemy.EnemyState.Dying)
        //    {
        //        enemy.CurrentState = Enemy.EnemyState.Moving;
        //    }


        //    switch (type)
        //    {
        //        case Enemy.MoveType.Swoop:

        //            break;
        //        case Enemy.MoveType.Teleport:
        //            break;
        //        case Enemy.MoveType.Zigzag:
        //            break;
        //        case Enemy.MoveType.Charge:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion
    }
}
