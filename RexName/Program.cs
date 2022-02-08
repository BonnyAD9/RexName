namespace RexName;

class Program
{
    static bool NoOp = false;
    static string SearchPattern = "";
    static string ReplacePattern = "";
    static string Directory = "./";

    static void Main(string[] args)
    {
        //args = new string[] { @"(.*?)( - Copy)?( \(.*\))?.txt", @"<1> ({i}).txt", @"-noop", @"-d", @"TestFolder" };
        if (args.Length < 2)
        {
            Console.WriteLine("Invalid number of arguments");
            return;
        }
        SearchPattern = args[0];
        ReplacePattern = args[1];
        for (int i = 2; i < args.Length; i++)
        {
            switch (args[i].ToLower())
            {
                case "-noop":
                    NoOp = true;
                    break;
                case "--directory" or "-d":
                    i++;
                    Directory = args[i];
                    break;
                default:
                    Console.WriteLine($"Invalid argument '{args[i]}'");
                    return;
            }
        }

        RegexRename rr;
        try
        {
            rr = new(Directory);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Couldn't get files from directory '{Directory}'");
            Console.WriteLine(ex.Message);
            return;
        }

        int index = 0;
        rr.GetVariable = (name, tag) =>
        {
            int i = name.IndexOf(':');

            if (i < 0) return name switch
            {
                "i" => index++.ToString(),
                "tag.artist" => tag.Value.Artist,
                "tag.album" => tag.Value.Album,
                "tag.title" => tag.Value.Title,
                "tag.track" => tag.Value.Track.ToString(),
                "tag.year" => tag.Value.Year,
                "tag.genre" => tag.Value.Genre,
                "tag.comment" => tag.Value.Comment,
                _ => null,
            };

            return name[..i] switch
            {
                "i" => index++.ToString(name[(i + 1)..]),
                "tag.track" => tag.Value.Track.ToString(name[(i + 1)..]),
                _ => null,
            };
        };

        int c;
        try
        {
            c = rr.Match(SearchPattern, ReplacePattern);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Invalid regex pattern");
            Console.WriteLine(ex.Message);
            return;
        }

        if (c < 0)
        {
            Console.WriteLine("Replace pattern was incorrect");
            return;
        }

        Console.WriteLine($"Found: {c}");
        if (NoOp)
        {
            foreach ((var file, var name) in rr)
            {
                Console.WriteLine($"{file.Name} -> {name}");
            }
            return;
        }

        rr.Rename();
    }
}