using Spectre.Console;
using System.Text;

var root = FindGitRoot(new(Directory.GetCurrentDirectory())) ?? new(Directory.GetCurrentDirectory());
var gitignore = Path.Combine(root.FullName, ".gitignore");
HashSet<string> existing = File.Exists(gitignore)
    ? [.. File.ReadAllLines(gitignore).Select(l => l.Trim())]
    : [];

List<string> targets = [];

// 1. Interactive TUI Mode (when no arguments provided)
if (args.Length == 0)
{
    AnsiConsole.Write(
        new Rule("[bold green]GitIgnoreThis[/] [grey]Interactive Mode[/]")
            .LeftJustified()
            .RuleStyle("grey"));

    AnsiConsole.MarkupLine($"[grey]Repository:[/] [dim]{Markup.Escape(root.FullName)}[/]\n");

    // Gather local unignored items
    var localItems = Directory.GetFileSystemEntries(".")
        .Select(Path.GetFileName)
        .Where(name => !string.IsNullOrEmpty(name) && name != ".git")
        .Select(name => Directory.Exists(name) ? $"{name}/" : name!)
        .Where(name => !existing.Contains(name) && !existing.Contains(name.TrimEnd('/')))
        .OrderBy(name => !name.EndsWith('/'))
        .ThenBy(name => name)
        .ToList();

    const string customOption = "[yellow]+ Enter custom pattern (e.g., *.log, .env*)[/]";
    var choices = new List<string>(localItems) { customOption };

    var selected = AnsiConsole.Prompt(
        new MultiSelectionPrompt<string>()
            .Title("Select items to add to [green].gitignore[/]:")
            .NotRequired()
            .PageSize(12)
            .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]")
            .AddChoices(choices));

    foreach (var item in selected)
    {
        if (item == customOption)
        {
            var custom = AnsiConsole.Ask<string>("Enter custom pattern (e.g., [green]*.log[/] or [green].env*[/]):");
            if (!string.IsNullOrWhiteSpace(custom))
                targets.Add(custom.Trim());
        }
        else
        {
            targets.Add(item);
        }
    }
}
else
{
    // 2. Direct CLI Mode
    targets.AddRange(args);
}

if (targets.Count == 0)
{
    AnsiConsole.MarkupLine("[yellow]No items selected. Nothing was changed.[/]");
    return 0;
}

// 3. Process targets & save
List<string> added = [];

foreach (var target in targets)
{
    var pattern = (target.Contains('*') || target.Contains('?'))
        ? target.Replace('\\', '/')
        : Path.GetRelativePath(root.FullName, Path.GetFullPath(target)).Replace('\\', '/');

    if (Directory.Exists(target) && !pattern.EndsWith('/')) pattern += "/";

    if (existing.Add(pattern))
    {
        added.Add(pattern);
        AnsiConsole.MarkupLine($"[green]✓ Added:[/] [bold]{Markup.Escape(pattern)}[/]");
    }
    else
    {
        AnsiConsole.MarkupLine($"[dim]• Already ignored:[/] {Markup.Escape(pattern)}");
    }
}

if (added.Count > 0)
{
    var text = File.Exists(gitignore) ? File.ReadAllText(gitignore) : "";
    var prefix = text.Length > 0 && !text.EndsWith('\n') ? Environment.NewLine : "";
    File.AppendAllText(gitignore, prefix + string.Join(Environment.NewLine, added) + Environment.NewLine, Encoding.UTF8);

    AnsiConsole.MarkupLine($"\n[bold green]Success![/] Saved {added.Count} entry/entries to [blue]{Markup.Escape(gitignore)}[/]");
}

return 0;

static DirectoryInfo? FindGitRoot(DirectoryInfo? dir) =>
    dir is null || Directory.Exists(Path.Combine(dir.FullName, ".git")) ? dir : FindGitRoot(dir.Parent);