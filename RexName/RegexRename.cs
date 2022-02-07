using System.Text;
using System.Text.RegularExpressions;
using Dir = System.IO.Directory;
using System.Collections;
using Bny.Tags;

namespace RexName;

public class RegexRename : IEnumerable<(FileInfo, string)>
{
    public string Directory { get; init; }
    private FileInfo[] Files { get; init; }
    private List<(FileInfo, string)> NewNames { get; } = new();
    public Regex? Pattern { get; private set; } = null;
    public Func<string, Lazy<Tag>, string?> GetVariable { get; set; } = static (_, _) => null;

    public (FileInfo File, string Name) this[int index] => NewNames[index];

    public RegexRename(string directory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        Directory = directory;
        
        var files = Dir.GetFiles(directory, "*", searchOption);
        Files = new FileInfo[files.Length];

        for (int i = 0; i < files.Length; i++)
            Files[i] = new(files[i]);
    }

    public int Match(string matchPattern, string replacePattern)
    {
        Regex ex = new(matchPattern);
        foreach (var file in Files)
        {
            var match = ex.Match(file.Name);
            if (!match.Success)
                continue;
            string? name = MakeName(match, replacePattern, file.FullName);
            if (name is null)
                return -1;
            NewNames.Add((file, name!));
        }
        
        return NewNames.Count;
    }

    public void Rename()
    {
        foreach ((var file, var name) in NewNames)
        {
            if (file.DirectoryName is null)
                continue;
            file.MoveTo(Path.Combine(file.DirectoryName!, name), true);
        }
    }

    private string? MakeName(Match match, string pattern, string file)
    {
        StringBuilder sb = new();

        Lazy<Tag> tag = new(() =>
        {
            Tag t = new();
            ID3v1.Read(t, file);
            return t;
        });

        for (int i = 0; i < pattern.Length; i++)
        {
            int newI = i;
            string? toAppend = null;
            switch (pattern[i])
            {
                case '<':
                    i++;
                    newI = pattern.IndexOf('>', i);
                    if (newI < 0)
                        return null;
                    toAppend = match.Groups[pattern[i..newI]].Value;
                    break;
                case '{':
                    i++;
                    newI = pattern.IndexOf('}', i);
                    if (newI < 0)
                        return null;
                    toAppend = GetVariable(pattern[i..newI], tag);
                    break;
                case '\\':
                    i++;
                    sb.Append(pattern[i]);
                    continue;
                default:
                    sb.Append(pattern[i]);
                    continue;
            }

            if (toAppend is null)
                return null;
            sb.Append(toAppend!);
            
            i = newI;
        }

        return sb.ToString();
    }

    public IEnumerator<(FileInfo, string)> GetEnumerator() => NewNames.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => NewNames.GetEnumerator();
}
