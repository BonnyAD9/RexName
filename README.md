# RexName
Command line tool that renames files using regex patterns

## Example
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

- first argument is regex pattern that will be matched to the file names
- second argument specifies the new name
  - if a string is in `<>` braces it is taken from the regex pattern (numbers can be also used e.g. `<0>` will take the whole file name, `<1>` will take contents of the first braces)
  - if a string is in `{}` braces it is a special variable, for now the only option is `i` which will result in index of the item, this also supports formats after `:` that format will be passed as format to the `ToString` method of that variable (`i` is int)

than there are optional flags:
- `-d` or `--directory` specifies directory in whlich the files will be searched (in this case *TestFolder*)
  - when it is not specified the current working directory will be used
- `-noop` specifies that the program should only display preview of what would be renamed and not actually rename anything
  - if there is no `-noop` flag the program will only show number of matches and rename the files (no preview will be shown)

### Contents of TestFolder
```
New Text Document - Copy (2).txt
New Text Document - Copy (3).ahoj
New Text Document - Copy (4).txt
New Text Document - Copy (5).txt
New Text Document - Copy (6).txt
New Text Document - Copy.txt
New Text Document.txt
```
