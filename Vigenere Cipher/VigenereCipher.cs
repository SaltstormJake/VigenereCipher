using System;
using System.Collections.Generic;
using System.Text;
namespace Programming_Assignment_1___Vigenere_Cipher
{
    class VigenereCipher
    {
        //Converts a char to its position in the alphabet
        int CharToInt(char c)
        {
            int i = char.ToUpper(c) - 64;
            return i;
        }
        //Converts an int to the character it represents in the alphabet
        char IntToChar(int i)
        {
            char c = (char)(i + 64);
            if(c == '@')
                c = 'Z';
            return c;
        }
        //Encrypts a string given the string and a key
        string Encryption(string original, string key)
        {
            //Creates a new empty string
            StringBuilder newString = new StringBuilder();
            //Where the key initiates
            int keyIncrementor = 0;
            for(int i = 0; i < original.Length; i++){
                if(original[i] == ' '){
                    newString.Append(' ');
                    continue;
                }
                //Find how much the char needs to be incremented by
                int replacementIncrement = CharToInt(key[keyIncrementor]) - 1;
                //Increment the new character and add it to the string
                char newChar = IntToChar((CharToInt(original[i]) + replacementIncrement) % 26);
                newString.Append(newChar);
                //If the key will overrun next time, reset back to 0
                if(++keyIncrementor == key.Length)
                    keyIncrementor = 0;
            }
            return newString.ToString();
        }

        //Decrypts a string given the encrypted string and the key
        string Decryption(string original, string key)
        {
            //Creates a new empty string
            StringBuilder newString = new StringBuilder();
            //Where the key initiates
            int keyIncrementor = 0;
            for(int i = 0; i < original.Length; i++){
                if(original[i] == ' '){
                    newString.Append(' ');
                    continue;
                }
                //Find how much the char needs to be decremented by
                int replacementDecrement = CharToInt(key[keyIncrementor]) - 1;
                //Decrement the new character and add it to the string
                char newChar = IntToChar((CharToInt(original[i]) - replacementDecrement + 52) % 26);
                newString.Append(newChar);
                if(++keyIncrementor == key.Length)
                    keyIncrementor = 0;
            }
            return newString.ToString();
        }
        //Asks the user if they wish to encrypt or decrypt, and then does so
        void Prompt()
        {
            Console.Write("Type 'E' to encrypt, 'D' to decrypt. Press B to try to break a cypher, provided you know the key period.");
            char input = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            switch(input){
                case 'E':
                    AskForEncryption();
                    break;
                case 'D':
                    AskForDecryption();
                    break;
                case 'B':
                    AskForBreak();
                    break;
                default:
                    Console.Write("You didn't specify. I ask again:");
                    Prompt();
                    break;
            }
        }
        //Asks the user for the message to encrypt and the key
        void AskForEncryption(){
            Console.Write("Enter your initial message.");
            string message = Console.ReadLine();
            Console.Write("Enter your key.");
            string key = Console.ReadLine();
            Console.Write("Your encrypted message is " + Encryption(message,key));
            Console.ReadKey();
        }
        //Asks the user for the message to decrypt and the key
        void AskForDecryption(){
            Console.Write("Enter your encrypted message.");
            string message = Console.ReadLine();
            Console.Write("Enter your key.");
            string key = Console.ReadLine();
            Console.Write("Your decrypted message is " + Decryption(message,key));
            Console.ReadKey();
        }

        //Asks for the encrypted message, tries to break the cipher
        void AskForBreak(){
            Console.WriteLine("Copy and paste (or type, if you wish) the encrypted message.");
            string message = Console.ReadLine();
            Console.WriteLine("How long is the key, AKA its period?");
            int period = int.Parse(Console.ReadLine());
            message = BreakCipher(message, period);
            Console.WriteLine("To the best of my ability, the decrypted message is ");
            Console.WriteLine(message);
            Console.ReadKey();
        }

        double ExpectedFrequency(char c)
        {
            char thisChar = char.ToUpper(c);
            switch(thisChar){
                case 'A':
                    return 0.07984;
                case 'B':
                    return 0.01511;
                case 'C':
                    return 0.02504;
                case 'D':
                    return 0.04260;
                case 'E':
                    return 0.12452;
                case 'F':
                    return 0.02262;
                case 'G':
                    return 0.02013;
                case 'H':
                    return 0.06384;
                case 'I':
                    return 0.07000;
                case 'J':
                    return 0.00131;
                case 'K':
                    return 0.00741;
                case 'L':
                    return 0.03961;
                case 'M':
                    return 0.02629;
                case 'N':
                    return 0.06876;
                case 'O':
                    return 0.07691;
                case 'P':
                    return 0.01741;
                case 'Q':
                    return 0.00107;
                case 'R':
                    return 0.05912;
                case 'S':
                    return 0.06333;
                case 'T':
                    return 0.09058;
                case 'U':
                    return 0.02844;
                case 'V':
                    return 0.01056;
                case 'W':
                    return 0.02304;
                case 'X':
                    return 0.00159;
                case 'Y':
                    return 0.02028;
                case 'Z':
                    return 0.00057;
                default:
                    return 0;
            }
        }
        //Gives the actual frequency of a char in a string
        int ActualFrequency(string message, char c){
            int counter = 0;
            for(int i = 0; i < message.Length; i++){
                if(message[i] == c)
                    counter++;
            }
            return counter;
        }
        //Gives the chi squared value of a char
        double ChiSquared(string message, char c){
            int actualCount = ActualFrequency(message, c);
            double expectedCount = ExpectedFrequency(c) * message.Length;
            double chiSquare = Math.Pow(actualCount - expectedCount, 2) / expectedCount;
            return chiSquare;
        }
        //Adds up all the chi squared values across
        //a string
        double TotalChiSquared(string message){
            double totalChi = 0;
            for(int i = 0; i < message.Length; i++){
                totalChi += ChiSquared(message, message[i]);
            }
            return totalChi;
        }
        //Makes a substring of every 3 chars based on the
        //starting point
        string StringToCipher(string message, int startingPoint, int keySize)
        {
            StringBuilder toCipher = new StringBuilder();
            for(int i = startingPoint; i < message.Length; i += keySize){
                toCipher.Append(message[i]);
            }
            return toCipher.ToString();
        }
        //Shifts the message by 'shift' number of spaces
        string ShiftMessage(string message, int shift){
            StringBuilder newString = new StringBuilder();
            for(int i = 0; i < message.Length; i++){
                int index = CharToInt(message[i]);
                index += shift;
                index %= 26;
                newString.Append(IntToChar(index));
            }
            return newString.ToString();
        }
        //Finds the lowest chi value for a string
        string LowestChiValue(string message){
            string lowestValueString = message;
            double lowestChi = TotalChiSquared(message);
            for(int i = 1; i < 26; i++){
                string currentString = ShiftMessage(message, i);
                double chiValue = TotalChiSquared(currentString);
                if(chiValue < lowestChi){
                    lowestValueString = currentString;
                    lowestChi = chiValue;
                }
            }
            return lowestValueString;
        }
        //Puts together the strings in the parameters and returns
        //them as one string
        string StitchTogetherString(List<string> strings){ 
            StringBuilder finalString = new StringBuilder();
            for(int i = 0; i < strings[0].Length; i++){
                finalString.Append(strings[0][i]);
                for(int j = 1; j < strings.Count; j++)
                    if(strings[j].Length > i)
                        finalString.Append(strings[j][i]);
            }
            return finalString.ToString();
        }
        //Splits the message up into 3 parts, finds the lowest
        //chi value of each one, and stitches them back together
        string BreakCipher(string message, int keyLength){
            List<string> strings = new List<string>();
            for(int i = 0; i < keyLength; i++){
                string thisMessage = StringToCipher(message, i, keyLength);
                thisMessage = LowestChiValue(thisMessage);
                strings.Add(thisMessage);
            }
            string finalMessage = StitchTogetherString(strings);
            return finalMessage;
        }

        static void Main(string[] args)
        {
            VigenereCipher v = new VigenereCipher();
            v.Prompt();
        }

    }
}
