using System;

namespace Selectorlyzer.Qulaly.Helpers
{
    public static class NthHelper
    {
        public static bool IndexMatchesOffsetAndStep(int index, int offset, int step)
        {
            if (step == 0)
            {
                return index == offset;
            }

            if (index < offset)
            {
                return false;
            }

            return (index - offset) % step == 0;
        }

        public static (int, int) GetOffsetAndStep(string expression)
        {
            if (expression == "odd")
            {
                return (1, 2);
            }

            if (expression == "even")
            {
                return (0, 2);
            }

            var index = 0;
            var sign = 1;
            var a = 0;

            if (expression[index] == '-')
            {
                index++;
                sign = -1;
            }

            if (expression[index] == '+')
            {
                index++;
                sign = 1;
            }

            var stringNumber = string.Empty;
            while (index < expression.Length && expression[index] >= '0' && expression[index] <= '9')
            {
                stringNumber += expression[index];
                index++;
            }

            var number = stringNumber == string.Empty ? default(int?) : int.Parse(stringNumber);

            if (index < expression.Length && expression[index] == 'n')
            {
                index++;
                a = sign * (number ?? 1);

                if (index < expression.Length)
                {
                    sign = 1;
                    if (expression[index] == '-')
                    {
                        index++;
                        sign = -1;
                    }

                    if (expression[index] == '+')
                    {
                        index++;
                        sign = 1;
                    }

                    stringNumber = string.Empty;
                    while (index < expression.Length && expression[index] >= '0' && expression[index] <= '9')
                    {
                        stringNumber += expression[index];
                        index++;
                    }

                    number = stringNumber == string.Empty ? default(int?) : int.Parse(stringNumber);

                }
                else
                {
                    sign = 0;
                    number = 0;
                }
            }

            if (number == null || index < expression.Length)
            {
                throw new NullReferenceException($"Could not parse nth expression ({expression})");
            }

            return (sign * number ?? 0, a);
        }
    }
}
