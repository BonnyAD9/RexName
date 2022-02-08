# RexName
Command line tool that renames files using regex patterns

## Usage
```
rexname [regex search pattern] [rename pattern] [0 or more flags]
```

### Regex search pattern
regex pattern that is used in C#

### Rename pattern
- regex variables are specified in `<>` (e. g. `<0>`, `<1>`, `<variable_name>`,...)
- special variables are in `{}` (e. g. `{i}`, `{tag.title}`,...)
  - some special variables can have additional format separated by `:` (e. g. `{i:X}` (hex), `{i:00}` (two digits),...)
  - this format is the same as is used in `.ToString(string)` method in C# for the given type
  - you can use `\` to escape characters - any character immediately after the `\` won't be further interpreted

#### Available special variables
- `i`
  - returns the index of the item (numbers the items)
  - supports formats (int)
- `tag.artist`
  - returns the artist (ID3v1)
- `tag.alum`
  - returns the album (ID3v1)
- `tag.title`
  - returns the title (ID3v1)
- `tag.track`
  - returns the track number (ID3v1.1)
  - supports formats (byte)
- `tag.year`
  - returns the year (ID3v1)
- `tag.genre`
  - returns the genre (ID3v1, Winamp 5.6 extended list of genres)
- `tag.comment`
  - returns the comment (ID3v1)

### Flags
- `-d` | `--directory`
  - sets the directory in which the files will be matched and renamed
- `-noop`
  - with this flag no files will be renamed and a preview of how whould the files be renamed will be shown

## Examples

### Example 1
```
PS > rexname "(?<name>.*?)( - Copy)?( \(.*\))?.txt" "<name> {i:00}.txt" -d TestFolder -noop
Found: 6
New Text Document - Copy (2).txt -> New Text Document 00.txt
New Text Document - Copy (4).txt -> New Text Document 01.txt
New Text Document - Copy (5).txt -> New Text Document 02.txt
New Text Document - Copy (6).txt -> New Text Document 03.txt
New Text Document - Copy.txt -> New Text Document 04.txt
New Text Document.txt -> New Text Document 05.txt
```

#### Contents of TestFolder
```
New Text Document - Copy (2).txt
New Text Document - Copy (3).ahoj
New Text Document - Copy (4).txt
New Text Document - Copy (5).txt
New Text Document - Copy (6).txt
New Text Document - Copy.txt
New Text Document.txt
```

### Example 2
```
PS > rexname ".*\.mp3" "{tag.artist} - {tag.album} - {tag.track:00} {tag.title}.mp3" -d TestFolder -noop
Found: 13
01 Distorted Light Beam.mp3 -> Bastille - Give Me The Future - 01 Distorted Light Beam.mp3
02 Thelma + Louise.mp3 -> Bastille - Give Me The Future - 02 Thelma + Louise.mp3
03 No Bad Days.mp3 -> Bastille - Give Me The Future - 03 No Bad Days.mp3
04 Brave New World (Interlude).mp3 -> Bastille - Give Me The Future - 04 Brave New World (Interlude).mp3
05 Back To The Future.mp3 -> Bastille - Give Me The Future - 05 Back To The Future.mp3
06 Plug In.mp3 -> Bastille - Give Me The Future - 06 Plug In.mp3
07 Promises.mp3 -> Bastille - Give Me The Future - 07 Promises.mp3
08 Shut Off The Lights.mp3 -> Bastille - Give Me The Future - 08 Shut Off The Lights.mp3
09 Stay Awake.mp3 -> Bastille - Give Me The Future - 09 Stay Awake.mp3
10 Give Me The Future.mp3 -> Bastille - Give Me The Future - 10 Give Me The Future.mp3
11 Club 57.mp3 -> Bastille - Give Me The Future - 11 Club 57.mp3
12 Total Dissociation (Interlude).mp3 -> Bastille - Give Me The Future - 12 Total Dissociation (Interlude).mp3
13 Future Holds.mp3 -> Bastille - Give Me The Future - 13 Future Holds.mp3
```

## Dependencies
- [Bny.Tags](https://github.com/BonnyAD9/Bny.Tags) available at [NuGet](https://www.nuget.org/packages/Bny.Tags/)
