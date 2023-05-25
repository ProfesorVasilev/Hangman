namespace Hangman
{
    internal class Program
    {
        static string guess = "null";
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hangman™! Please select an option from the following menu:");
            int gamesPlayed = 0;
            int winstreak = 0;
            int wins = 0;
            int bestWinstreak = 0;
            int tries = 0;
            int menuResult;
            int totalMenuOptions = 5;
            bool gameIsBeingPlayed = true;
            string word = "unknown";
            while (gameIsBeingPlayed)
            {
                Console.WriteLine("1. Start new game ");
                Console.WriteLine("2. Check stats ");
                Console.WriteLine("3. Reset stats ");
                Console.WriteLine("4. Credits");
                Console.WriteLine("5. Exit");
                Console.Write("option: ");
                string menuOption = Console.ReadLine();
                Console.WriteLine("");
                bool menuParseSuccessful = int.TryParse(menuOption, out menuResult);
                while (menuParseSuccessful && menuResult <= totalMenuOptions && menuResult > 0)//moje s array
                {
                    switch (menuResult)
                    {
                        case 1:
                            bool validGamemode = false;
                            while (!validGamemode)
                            {
                                ChooseGamemode(ref tries, ref word, ref validGamemode);
                            }
                            gamesPlayed = gamesPlayed + 1;
                            List<string> lettersInWord = new List<string>();
                            List<string> blankWord = new List<string>();
                            PrintBlankWord(word, lettersInWord, blankWord);
                            List<string> correctlyGuessedLetters = new List<string>();
                            List<string> guessedLetters = new List<string>();
                            Console.WriteLine(word + "dumata");//debugging
                            while (tries >= 1)
                            {
                                if (lettersInWord.Count == correctlyGuessedLetters.Count)
                                {
                                    GuessedTheWord(ref winstreak, ref wins,ref bestWinstreak, word);
                                    break;
                                }

                                InvalidGuess();

                                if (lettersInWord.Contains(guess))
                                {
                                    CorrectGuess(lettersInWord, blankWord, correctlyGuessedLetters, guessedLetters);
                                }
                                else if (!lettersInWord.Contains(guess))
                                {
                                    if (!guessedLetters.Contains(guess))
                                    {
                                        tries = IncorrectGuess(tries, lettersInWord, blankWord, guessedLetters);
                                    }
                                    else
                                    {
                                        OldGuess();
                                    }
                                }

                            }
                            OutOfTries(ref winstreak, ref bestWinstreak, tries, word);
                            break;
                        case 2:
                            PrintStats(gamesPlayed, winstreak, wins, bestWinstreak);
                            break;
                        case 4:
                            PrintCredits();
                            break;
                        case 5:
                            Console.WriteLine("Are you sure? All of your progress will be lost (wins, games played, winstreak) Y/N");
                            string quitOption = Console.ReadLine();
                            if (quitOption.Equals("y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Farewell!");
                                return;
                            }
                            else if (quitOption.Equals("n", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("We're glad to hear that! Have a nice time playing!");
                            }
                            break;
                        case 3:
                            ResetStats(gamesPlayed, winstreak, wins, bestWinstreak);
                            break;
                    }
                    break;
                }
                InvalidMenuOption(totalMenuOptions, menuResult, menuParseSuccessful);
            }

        }

        private static void PrintBlankWord(string word, List<string> lettersInWord, List<string> blankWord)
        {
            int wordLetterCount = word.Length;
            Console.WriteLine("\nThe blank word is:");
            for (int i = 0; i < wordLetterCount; i++)
            {
                Console.Write("_  ");
                char letterChar = word.ElementAt(i);
                string letter = letterChar.ToString();
                lettersInWord.Add(letter);
                blankWord.Add("_ ");
            }
            Console.WriteLine("   ");
        }

        private static void ChooseGamemode(ref int tries, ref string word, ref bool validGamemode)
        {
            Console.WriteLine("Would you like to play singleplayer or multiplayer?");
            Console.Write("gamemode: ");
            string gamemode = Console.ReadLine();
            if (gamemode.Equals("Singleplayer", StringComparison.OrdinalIgnoreCase))
            {
                SetDifficulty(ref word, ref tries);
                validGamemode = true;
            }
            else if (gamemode.Equals("Multiplayer", StringComparison.OrdinalIgnoreCase))
            {
                SecondPlayerChoosesWord(out word, out tries);
                validGamemode = true;
            }
            else
            {
                PrintInvalidGamemode();
            }
        }

        private static void OldGuess()
        {
            Console.WriteLine("    ");
            Console.WriteLine("You've already guessed " + guess);
            Console.WriteLine("Please try again!");
        }

        private static int IncorrectGuess(int tries, List<string> lettersInWord, List<string> blankWord, List<string> guessedLetters)
        {
            guessedLetters.Add(guess);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("    ");
            Console.WriteLine("Try again!");
            tries--;
            Console.WriteLine($"You have {tries} tries left!");
            for (int i = 0; i < lettersInWord.Count; i++)
            {
                Console.Write(blankWord[i] + " ");
            }
            Console.WriteLine("   ");
            Console.ForegroundColor = ConsoleColor.White;
            return tries;
        }

        private static void CorrectGuess(List<string> lettersInWord, List<string> blankWord, List<string> correctlyGuessedLetters, List<string> guessedLetters)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nCorrect!");
            while (lettersInWord.Contains(guess))
            {
                guessedLetters.Add(guess);
                correctlyGuessedLetters.Add(guess);
                int indexOfGuess = lettersInWord.IndexOf(guess);
                lettersInWord.Remove(guess);
                lettersInWord.Insert(indexOfGuess, "0");
                blankWord.RemoveAt(indexOfGuess);
                blankWord.Insert(indexOfGuess, guess);
            }

            Console.WriteLine("blank word + guesses");
            for (int i = 0; i < lettersInWord.Count; i++)
            {
                Console.Write(blankWord[i] + " ");
            }
            Console.WriteLine("   ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void GuessedTheWord(ref int winstreak, ref int wins, ref int bestWinstreak, string word)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("You guessed the word!");
            Console.WriteLine("It was " + word);
            Console.WriteLine("");
            winstreak = winstreak + 1;
            wins = wins + 1;
            if (winstreak >= bestWinstreak)
            {
                bestWinstreak = winstreak;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void InvalidGuess()
        {
            bool guessIsValid = false;
            while (!guessIsValid)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nGuess a letter!");
                Console.Write("guess: ");
                string temporary = Console.ReadLine();
                guess = temporary.ToLower();
                int intGuess = 0;
                bool guessParseSuccessful = int.TryParse(guess, out intGuess);
                if (guessParseSuccessful || guess.Length != 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("");
                    Console.WriteLine("Please enter a valid guess! (1 letter)");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    guessIsValid = true;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void OutOfTries(ref int winstreak, ref int bestWinstreak, int tries, string word)
        {
            if (tries <= 0)
            {
                Console.WriteLine("  ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You ran out of tries, sorry :(");
                Console.WriteLine($"The word was !{word}!.");
                Console.ForegroundColor = ConsoleColor.White;
                if (winstreak >= bestWinstreak)
                {
                    bestWinstreak = winstreak;
                }
                winstreak = 0;
            }
        }

        private static void PrintCredits()
        {
            Console.WriteLine("This game was developed by Ivan Dinev TM. \nFor business enquires: ivandinev279@gmail.com\n ");
        }

        private static void ResetStats(int gamesPlayed, int winstreak, int wins, int bestWinstreak)
        {
            Console.WriteLine("Are you sure you want to reset your stats? They will be lost forever? Y/N");
            string resetStatsOption = Console.ReadLine();
            if (resetStatsOption.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                gamesPlayed = 0;
                winstreak = 0;
                wins = 0;
                bestWinstreak = 0;
                Console.WriteLine("Your stats were reset!");
            }
            else if (resetStatsOption.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Your stats were left as they were!");
            }
        }

        private static void PrintStats(int gamesPlayed, int winstreak, int wins, int bestWinstreak)
        {
            Console.WriteLine("Current stats are:");
            Console.WriteLine($"Games played = {gamesPlayed}.");
            Console.WriteLine($"Wins = {wins}.");
            Console.WriteLine($"Current winstreak = {winstreak}.");
            Console.WriteLine($"Best winstreak = {bestWinstreak}.\n");
        }

        private static void SecondPlayerChoosesWord(out string word, out int tries)
        {
            Console.WriteLine("Give the computer to player 2.");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Write the word you want player 1 to guess:");
            word = Console.ReadLine();
            tries = word.Length + 2;
        }

        private static void InvalidMenuOption(int totalMenuOptions, int menuResult, bool menuParseSuccessful)
        {
            if (!menuParseSuccessful || menuResult > totalMenuOptions || menuResult < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid menu option!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void PrintInvalidGamemode()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please enter a valid gamemode!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetDifficulty(ref string word, ref int tries)
        {
            bool validDifficulty = false;
            tries = 6;
            while (!validDifficulty)
            {
                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("Easy    Medium    Hard    Extreme");
                Console.Write("difficulty: ");
                string difficulty = Console.ReadLine();
                if (difficulty.Equals("Easy", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> randomWords = new List<string>();
                    EasyWordsList(randomWords);
                    word = GenerateRandomWord(randomWords);
                    validDifficulty = true;
                }
                else if (difficulty.Equals("Medium", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> randomWords = new List<string>();
                    MediumWordsList(randomWords);
                    word = GenerateRandomWord(randomWords);
                    validDifficulty = true;
                }
                else if (difficulty.Equals("Hard", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> randomWords = new List<string>();
                    HardWordsList(randomWords);
                    word = GenerateRandomWord(randomWords);
                    validDifficulty = true;
                }
                else if (difficulty.Equals("Extreme", StringComparison.OrdinalIgnoreCase))
                {
                    List<string> randomWords = new List<string>();
                    ExtremeWordsList(randomWords);
                    word = GenerateRandomWord(randomWords);
                    validDifficulty = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid difficulty!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        private static string GenerateRandomWord(List<string> randomWords)
        {
            int wordsCount = randomWords.Count;
            Random numGen = new Random();
            int wordIndex = numGen.Next(0, wordsCount);
            string word = randomWords[wordIndex];
            return word;
        }
        private static void EasyWordsList(List<string> randomWords)
        {
            randomWords.Add("tea");
            randomWords.Add("soup");
            randomWords.Add("can");
            randomWords.Add("sign");
            randomWords.Add("cup");
            randomWords.Add("pot");
            randomWords.Add("pen");
            randomWords.Add("car");
            randomWords.Add("fan");
            randomWords.Add("bot");
            randomWords.Add("fat");
            randomWords.Add("bin");
            randomWords.Add("word");
            randomWords.Add("key");
            randomWords.Add("loop");
            randomWords.Add("goal");
        }
        private static void MediumWordsList(List<string> randomWords)
        {
            randomWords.Add("apple");
            randomWords.Add("music");
            randomWords.Add("sleep");
            randomWords.Add("smile");
            randomWords.Add("river");
            randomWords.Add("flower");
            randomWords.Add("dance");
            randomWords.Add("beach");
            randomWords.Add("tiger");
            randomWords.Add("habit");
            randomWords.Add("mouse");
            randomWords.Add("witch");
            randomWords.Add("bench");
            randomWords.Add("onion");
            randomWords.Add("cloud");
            randomWords.Add("piece");
        }
        private static void HardWordsList(List<string> randomWords)
        {
            randomWords.Add("blanket");
            randomWords.Add("program");
            randomWords.Add("rainbow");
            randomWords.Add("shower");
            randomWords.Add("kitchen");
            randomWords.Add("passion");
            randomWords.Add("cupcake");
            randomWords.Add("silence");
            randomWords.Add("english");
            randomWords.Add("morning");
            randomWords.Add("school");
            randomWords.Add("freedom");
            randomWords.Add("diamond");
            randomWords.Add("witches");
            randomWords.Add("fortune");
            randomWords.Add("library");
        }
        private static void ExtremeWordsList(List<string> randomWords)
        {
            randomWords.Add("television");
            randomWords.Add("revolution");
            randomWords.Add("reflection");
            randomWords.Add("university");
            randomWords.Add("architecture");
            randomWords.Add("exploration");
            randomWords.Add("independence");
            randomWords.Add("destination");
            randomWords.Add("opportunity");
            randomWords.Add("friendship");
            randomWords.Add("technology");
            randomWords.Add("imagination");
            randomWords.Add("conversation");
            randomWords.Add("transformation");
            randomWords.Add("authenticity");
            randomWords.Add("collaboration");
        }
    }
}