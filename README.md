# TextReader

A simple and efficient desktop application for reading and searching large text files.

## Features

- **Load text data from various sources:**
  - Local TXT file
  - Web address (downloads and displays content from .txt or .html files)
  - Randomly generated long text (hundreds of thousands of lines)
- **Save loaded text to a file**
- **Text search with navigation:**
  - Search box with keyboard shortcut (Ctrl+F)
  - Navigate between results (F3 / Shift+F3)
  - Smooth scroll and focus on the found line
- **Optional filtering:**
  - Show only lines containing specific text
- **Efficient display of thousands to millions of lines:**
  - Uses virtualization (DataGridView) for smooth performance
- **Smooth scrolling and navigation:**
  - Home/End, PgUp/PgDn, and scrollbar navigation are fluid
- **No editing or text selection required**
- **No use of full-featured text editor components**

## Technologies
- .NET (WinForms)
- C#

## Requirements
- .NET 6.0 SDK or newer (for building from source)
- Windows OS

## Where to find the build

The executable file can be found in:

- `Build.zip/net8.0-windows/TextReader.exe`

or 

After building the project (via Visual Studio or `dotnet build`), the executable file can be found in:

- `TextReader/bin/Release/net8.0-windows/TextReader.exe` (for Release build)
- `TextReader/bin/Debug/net8.0-windows/TextReader.exe` (for Debug build)
- 
## Usage
- **Load text:** Use the menu to load from a file, URL, or generate random text.
- **Search:** Press Ctrl+F to open the search box. Use F3/Shift+F3 to navigate results.
- **Filter:** Enable filtering to show only lines containing the search text.
- **Save:** Use the menu to save the loaded text to a file.

## Notes
- The application is optimized for very large files and uses memory efficiently.
- No text editing or selection is supported (read-only viewer).
- If you encounter issues loading from some URLs, try using raw text files from GitHub or similar sources.

## Author
- Igor Minenko

---

*This project was created as a solution for a C# desktop programming assignment.* 
