using System.Linq.Expressions;

namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void LoadDictionary(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    dictionary = new List<SweEngGloss>(); // Empty it!
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        dictionary.Add(new SweEngGloss(line));
                    }
                }
                
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filepath}' was not found.");
            }
        }
        static void DeleteWord(string swedish, string english)
        {
            int index = dictionary.FindIndex(gloss => gloss.word_swe == swedish && gloss.word_eng == english);
            if (index != -1)
                dictionary.RemoveAt(index);
        }
        static void TranslateWord(string inputword)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == inputword)
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == inputword)
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "load") 
                {
                    string filepath = argument.Length == 2 ? "..\\..\\..\\dict\\" +argument[1] : defaultFile;
                    LoadDictionary(filepath);
                }
                else if (command == "list")  //FIXME when no list initiated gives nullreferenceexeption
                {
                    try
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            Console.WriteLine($"{gloss.word_swe,-10}  - {gloss.word_eng,-10}");
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("Error: Seems no dictionary loaded. Please load or add words first.");
                    }
                }
                else if (command == "new") //FIXME no list loaded gives nullreferencexeption
                {
                    if (argument.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(argument[1], argument[2]));
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swedish = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string english = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(swedish, english));
                    }
                }
                else if (command == "delete") //FIXME give some feedback when nothing is deleted due to for example typo
                {
                    if (argument.Length == 3)
                    {
                        DeleteWord(argument[1], argument[2]);
                    }
                    else if (argument.Length == 1) 
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string swedish = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string english = Console.ReadLine();
                        DeleteWord(swedish, english);
                    }
                }
                else if (command == "translate")
                {
                    if (argument.Length == 2)
                    {
                        TranslateWord(argument[1]);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string inputword = Console.ReadLine();
                        TranslateWord(inputword);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }
    }
}