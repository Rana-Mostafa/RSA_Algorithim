using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class BigInteger
    {
        private string number;

        public BigInteger()
        {

        }
        public BigInteger(string number)
        {
            this.number = number;
        }
        public string number_getter()
        {
            return this.number;
        }

        public void number_setter(string number)
        {
            this.number = number;
        }

        public string Subtract(string s1, string s2)
        {


            //Makeing sure that the bigger number is in s1
            bool swap = false;
            if (s1 == s2)
                return "0"; 

            if (s2.Length > s1.Length)
                swap = true;

            else if (s2.Length == s1.Length)
            {
                for (int i = 0; i < s1.Length; i++)
                {
                    if (s1[i] < s2[i])
                    {
                        swap = true;
                        break;
                    }
                    else if (s1[i] > s2[i])
                        break;
                }
            }


            if (swap)
            {
                string tp = s1;
                s1 = s2;
                s2 = tp;
            }


            char[] st1 = s1.ToCharArray();
            Array.Reverse(st1);
            s1 = new string(st1);


            StringBuilder FN = new StringBuilder(s1);
            char[] st2 = s2.ToCharArray();
            Array.Reverse(st2);
            s2 = new string(st2);

            StringBuilder SN = new StringBuilder(s2);


            int carry = 0;
            StringBuilder res = new StringBuilder("");

            for (int i = 0; i < SN.Length; i++)
            {
                int Current = (int)(FN[i] - '0') - (int)(SN[i] - '0') - carry;

                if (Current < 0)
                {
                    res.Append(10 + Current).ToString();
                    carry = 1;
                }

                else
                {
                    carry = 0;
                    res.Append((char)(Current + '0'));
                }

            }

            for (int i = SN.Length; i < FN.Length; i++)
            {

                int Current = ((int)(FN[i] - '0') - carry);
                if (Current < 0)
                {
                    res.Append(10 + Current).ToString();

                    carry = 1;
                }
                else
                {
                    carry = 0;
                    res.Append((char)(Current + '0'));
                }
            }
            string Result = res.ToString();
            char[] resArray = Result.ToCharArray();
            Array.Reverse(resArray);
            Result = new string(resArray);

            int index = 0;
            for (int i = 0; i < Result.Length; i++)
            {
                if (Result[i] != '0')
                    break;

                else
                    index++;

            }

            Result = Result.Substring(index, Result.Length - index);
            return Result;

        }

        //multpication of two big integer
        public string multiplication(string FirstNum, string SecondNum)
            {
            int StrLength = Math.Max(FirstNum.Length, SecondNum.Length);
            StringBuilder FirstNumber = new StringBuilder(FirstNum);
            StringBuilder SecondNumber = new StringBuilder(SecondNum);

            int x; 
            //to make the strings equivalent in length
            if (FirstNumber.Length < StrLength)
            {
                 x = StrLength - FirstNumber.Length;
                StringBuilder s = new StringBuilder("");

                for (int i = 0; i < x; i++)
                    s.Append("0");
                FirstNumber = s.Append(FirstNumber); 

            }

            if (SecondNumber.Length < StrLength)
            {
                 x = StrLength - SecondNumber.Length;
                StringBuilder s = new StringBuilder("");

                for (int i = 0; i < x; i++)
                    s.Append("0");
                SecondNumber = s.Append(SecondNumber);

            }
            FirstNum = FirstNumber.ToString();
            SecondNum = SecondNumber.ToString();


            //basecase 
            if (StrLength == 1)
                    return ((FirstNum[0] - '0') * (SecondNum[0] - '0')).ToString();

                //divide part : split first number into two halfes 
                string a = FirstNum.Substring(0, StrLength / 2);
                string b = FirstNum.Substring(StrLength / 2, StrLength - StrLength / 2);

                //split second number into two halfes
                string c = SecondNum.Substring(0, StrLength / 2);
                string d = SecondNum.Substring(StrLength / 2, StrLength - StrLength / 2);

                //conqure part : Recursivlly calculate the three multipications
                string ac = multiplication(a, c);
                string bd = multiplication(b, d);
                string z = multiplication(Addition(a, b), Addition(c, d));

                //calculating new z which equals ad+bd according to this equation ad+bd=z-(ac+bd)
                z = Subtract(z, Addition(ac, bd));

                //multipication of 10^N/2 *z and  10^N *ac  "N=StrLength"         

            StringBuilder st = new StringBuilder(ac);
            for (int i = 0; i < 2 * (StrLength - StrLength / 2); i++)
            {
                st.Append("0");
            }
            ac = st.ToString();

             st = new StringBuilder(z);
            for (int i = 0; i < (StrLength - StrLength / 2); i++)
            {
                st.Append("0");
            }
            z = st.ToString();

                string result = Addition(Addition(ac, bd), z);

                return result;
            }

        //public string FindMod(string B, string P, string M)
        //{

        //    string mod = null, res = null, temp, temp1;

        //    if (P == "0")
        //        return "1";
        //    else if (P == "1")
        //    {
        //        Division(B, M, ref res, ref mod);
        //        return mod;
        //    }
        //    else
        //    {
        //        Division(P, "2", ref res, ref mod);
        //        string bigInteger = FindMod(B, res, M);
        //        if (mod == "0")
        //        {
        //            temp = multiplication(bigInteger, bigInteger);
        //            Division(temp, M, ref res, ref mod);
        //            return mod;
        //        }
        //        else
        //        {
        //            temp = multiplication(bigInteger, bigInteger);
        //            Division(B, M, ref res, ref mod);
        //            temp1 = multiplication(temp, mod);
        //            Division(temp1, M, ref res, ref mod);
        //            return mod;
        //        }


        //    }
        //}

        public BigInteger FindMod(BigInteger E, BigInteger P, BigInteger M)
        {
            string res, temp, temp1;

            if (P.number_getter() == "0")
                return new BigInteger("1");

            else if (P.number_getter() == "1")
            {
                res = this.Mod(E.number_getter(), M.number_getter());

                return new BigInteger(res);

            }


            BigInteger bigInteger = this.FindMod(E, new BigInteger(this.Div(P.number_getter(), "2")), M);

            if (IS_Even(P))
            {
                temp = multiplication(bigInteger.number_getter(), bigInteger.number_getter());
                return new BigInteger(Mod(temp, M.number_getter()));
            }

            else
            {
                temp = multiplication(bigInteger.number_getter(), bigInteger.number_getter());
                temp1 = Mod(E.number_getter(), M.number_getter());
                res = multiplication(temp, temp1);
                return new BigInteger(Mod(res, M.number_getter()));
            }
        }

        public bool IS_Even(BigInteger P)
        {
            string res = Mod(P.number_getter(), "2");
            if (res[res.Length-1]=='0' || res[res.Length - 1] == '2' || res[res.Length - 1] == '4' || res[res.Length - 1] == '6' || res[res.Length - 1] == '8')
                return true;

            else return false;
        }

        private bool CheckZeroes(string num)
        {
            int numlength = num.Length;
            for (int i = 0; i < numlength; i++)
                if (num[i] != '0')
                    return false;
            return true;
        }

        public string Addition(string FirstNumber, string SecondNumber)
        {
            if (CheckZeroes(FirstNumber) && CheckZeroes(SecondNumber))
                return "0";
            int NumberOfZeroes;
            int FirstNumberSize = FirstNumber.Length, SecondNumberSize = SecondNumber.Length;
            StringBuilder Number;
            if (FirstNumberSize > SecondNumberSize)
            {
                NumberOfZeroes = FirstNumberSize - SecondNumberSize;
                Number = new StringBuilder("");
                //if there is any additional digit
                for (int i = 0; i < NumberOfZeroes; i++)
                    Number.Append("0");
                Number.Append(SecondNumber);
                SecondNumber = Number.ToString();
            }
            else if (FirstNumberSize < SecondNumberSize)
            {
                NumberOfZeroes = SecondNumberSize - FirstNumberSize;
                Number = new StringBuilder("");
                //if there is any additional digit
                for (int i = 0; i < NumberOfZeroes; i++)
                    Number.Append("0");
                Number.Append(FirstNumber);
                FirstNumber = Number.ToString();
            }
            //if there is any additional digit
            Number = new StringBuilder("");
            Number.Append("0");
            Number.Append(FirstNumber);
            FirstNumber = Number.ToString();
            Number = new StringBuilder("");
            Number.Append("0");
            Number.Append(SecondNumber);
            SecondNumber = Number.ToString();
            return AddingTwoStrings(FirstNumber, SecondNumber);
        }

        private string AddingTwoStrings(string FirstNumber, string SecondNumber)
        {
            StringBuilder Result = new StringBuilder("");
            byte Carry = 0;
            int Temp, FirstNumberSize = FirstNumber.Length;//Because of adding digit with zero, I don't care about size because it is the same
            for (int Digit = FirstNumberSize - 1; Digit >= 0; Digit--)
            {
                Temp = Carry + (FirstNumber[Digit] - '0') + (SecondNumber[Digit] - '0');
                if (Temp > 9)
                {
                    Temp %= 10;
                    Result.Append(Temp);
                    Carry = 1;
                }
                else
                {
                    Result.Append(Temp);
                    Carry = 0;
                }
            }
            Result.Insert(FirstNumberSize, Carry.ToString(), 1); ////////////////////insert here///////////////////////

            return RemoveLeftZeroes(ReverseString(Result.ToString()));
        }

        private string RemoveLeftZeroes(string Number)
        {
            StringBuilder Result = new StringBuilder("");
            int NumberLength = Number.Length;
            bool FirstNonZeroDigit = false;
            for (int Digit = 0; Digit < NumberLength; Digit++)
            {
                if (Number[Digit] != '0')
                    FirstNonZeroDigit = true;
                if (FirstNonZeroDigit == true)
                    Result.Append(Number[Digit]);
            }
            return Result.ToString();
        }

        private string ReverseString(string Text)
        {
            StringBuilder Result = new StringBuilder("");
            int TextSize = Text.Length;
            for (int Digit = TextSize - 1; Digit >= 0; Digit--)
            {
                Result.Append(Text[Digit]);
            }
            return Result.ToString();
        }

        //Division function takes two strings as numbers and other two variables quotiont and remainder as input 
        //and doesn't return anything
        //O(N)
        private void Division(string FirstNumber, string SecondNumber, ref string Quotient, ref string Remainder)
        {

            if (RemoveLeftZeroes(SecondNumber) == "")
            {

                return;
            }
            else
            {
                if (IsSmaller(FirstNumber, SecondNumber))
                {
                    Quotient = "0";
                    Remainder = FirstNumber;
                    return;
                }
                Quotient = FirstNumber;
                Remainder = SecondNumber;
                Division(FirstNumber, Addition(SecondNumber, SecondNumber), ref Quotient, ref Remainder);
                Quotient = Addition(Quotient, Quotient);
                if (IsSmaller(Remainder, SecondNumber))
                    return;
                else
                {
                    Quotient = Addition(Quotient, "1");
                    Remainder = Subtract(Remainder, SecondNumber);
                    return;
                }
            }
        }

        public string Div(string n, string m)
        {
            string res, q = "", r = "";
            Division(n, m, ref q, ref r);
            res = q;
            return res;
        }

        public string Mod(string n, string m)
        {
            string res, q = "", r = "";
            Division(n, m, ref q, ref r);
            res = r;
            return res;
        }

        public bool IsBigger(string num1, string num2)
            {
                if (Comparison(num1, num2) == 1)
                    return true;
                return false;
            }

        public bool IsSmaller(string num1, string num2)
        {
            if (Comparison(num1, num2) == -1)
                return true;
            return false;
        }

        public bool IsEqual(string num1, string num2)
        {
            if (Comparison(num1, num2) == 0)
                return true;
            return false;
        }

        private int Comparison(string s1, string s2)
        {
            if(s1.Length > s2.Length)
            {
                return 1;
            }
            else if (s1.Length < s2.Length)
            {
                return -1;
            }
            else
            {
                int len = s1.Length;
                for (int i = 0; i < len; i++)
                {
                    if (s1[i] > s2[i])
                    {
                        return 1;
                    }
                    else if (s1[i] < s2[i])
                    {
                        return -1;
                    }
                }
            }
            return 0;   
        }

        public int[] GenerateKeys()
        {
            List<int> AllPrimes = new List<int>();
            bool[] IsPrime = new bool[100000];
            IsPrime = Array.ConvertAll<bool, bool>(IsPrime, b => b = true); //setting all values to true 

            IsPrime[0] = false;
            IsPrime[1] = false;

            for (int i = 2; i * i < IsPrime.Length; i++)   // getting a list of primes 
            {
                if (IsPrime[i])
                {
                    AllPrimes.Add(i);

                    for (int j = i * i; j < IsPrime.Length; j += i)
                    {
                        IsPrime[j] = false;
                    }
                }

            }

            Random random = new Random();   //getting 2 random numbers
            int rand1 = random.Next(AllPrimes.Count);
            int rand2 = random.Next(AllPrimes.Count);

            while (true)                     //making sure the 2 random numbers wont be the same 
            {
                if (rand2 != rand1)
                    break;
                else
                    rand2 = random.Next(AllPrimes.Count);

            }

            int[] key_values = new int[5];
            key_values[0] = AllPrimes[rand1]; // p
            key_values[1] = AllPrimes[rand2]; //q
            key_values[2] = key_values[0] * key_values[1]; // n
            int phi = (key_values[0]-1) * (key_values[1]-1); // phi(n)
      
            // generating public key (e)
            int index=0;
            for(int i=0; i<AllPrimes.Count; i++)
            {
                if (AllPrimes[i] > phi && i!= AllPrimes.Count-1)
                {
                    index = i - 1;
                    break;
                }
                else if(i== AllPrimes.Count - 1)
                {
                    index = i;
                    break;
                }
            }
            int rand3 = random.Next(index);
            int e = AllPrimes[rand3];
            while (true)
            {
                if (e % phi != 0)
                    break;
                else
                {
                    rand3 = random.Next(index);
                     e = AllPrimes[rand3];
                }
            }
                   
              key_values[3] = e; // e

            // generating private key
            int d, k = 1;
            while (true)
            {
                k = phi + k;

                if(k%e == 0)
                {
                    d = k / e;
                    break;
                }
            }
            key_values[4] = d;


            return key_values;

        }

    }
}
