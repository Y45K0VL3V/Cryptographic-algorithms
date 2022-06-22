using System;
using System.Collections.Generic;
using yakov.TI.Lab3;

namespace yakov.TI.Lab4
{
    public static class DSAParamsValidator
    {
        private delegate bool ParamValidator(DSAParams @params);

        private static Dictionary<string, ParamValidator> validators = new Dictionary<string, ParamValidator>()
        {
            ["q"] = ValidateQ,
            ["p"] = ValidateP,
            ["h"] = ValidateH,
            ["k"] = ValidateK,
            ["x"] = ValidateX,
            ["y"] = ValidateY
        };

        public static bool? Validate(DSAParams @params, string keyName)
        {
            return validators[keyName]?.Invoke(@params);
        }

        private static bool ValidateQ(DSAParams @params)
        {
            if (@params.q == 0) 
                return false;

            if (!HelpMath.IsNumberPrime(@params.q))
                throw new ArgumentException("Q must be prime.");

            return true;
        }

        private static bool ValidateP(DSAParams @params)
        {
            if (@params.p == 0)
                return false;

            try
            {
                if ((@params.p - 1) % @params.q != 0)
                    throw new ArgumentException("Q must be divider of p-1.");
            }
            catch (DivideByZeroException)
            {
                throw new ArgumentException("Q must be divider of p-1.");
            }

            return true;
        }

        private static bool ValidateH(DSAParams @params)
        {
            if (@params.h == 0)
                return false;

            if ((@params.h <= 1) || (@params.h >= @params.p - 1))
                throw new ArgumentException("H must be in range of [2, p-2]");

            return true;
        }

        private static bool ValidateK(DSAParams @params)
        {
            if (@params.k == 0)
                return false;

            if ((@params.k <= 0) || (@params.k >= @params.q))
                throw new ArgumentException("K must be in range of [1, q-1]");

            return true;
        }

        private static bool ValidateX(DSAParams @params)
        {
            if (@params.x == 0)
                return false;

            if ((@params.x <= 0) || (@params.x >= @params.q))
                throw new ArgumentException("X must be in range of [1, q-1]");

            return true;
        }

        private static bool ValidateY(DSAParams @params)
        {
            if (@params.y == 0)
                return false;

            return true;
        }
    }
}
