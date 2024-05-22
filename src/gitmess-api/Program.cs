using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string GetPath(string path) =>
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Repos", path);

GitItem[] GetItems(string path, string branch)
{
    var output = RunGitCommand($"ls-tree {branch}", path);
    return output
        .Split("\n")
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            line = Regex.Replace(line, @"\s+", " ");
            var parts = line.Split(' ');
            return new GitItem(parts[1], parts[3], null, null);
        })
        .OrderByDescending(item => item.Type)
        .ThenBy(item => item.Name)
        .ToArray();
}

string RunGitCommand(string arguments, string workingDirectory)
{
    var processStartInfo = new ProcessStartInfo
    {
        FileName = "git",
        Arguments = arguments,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = workingDirectory
    };

    using var process = new Process { StartInfo = processStartInfo };
    process.Start();

    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    return string.IsNullOrEmpty(error) ? output : error;
}

// Endpoints
var group = app.MapGroup("repositories");
group.MapGet("/", () =>
{
    return Directory.GetDirectories(GetPath(""))
        .Select(dir => new GitRepository(Path.GetFileName(dir)));
});
group.MapGet("/{repo}/{branch}", (string repo, string branch = "main") =>
{
    var repoPath = GetPath(repo);
    var items = GetItems(repoPath, branch);
    return new GitItem("tree", repo, items, null);
});
group.MapGet("/{repo}/{branch}/{path}", (string repo, string branch, string path) =>
{
    path = HttpUtility.UrlDecode(path);

    var currentPath = GetPath(Path.Combine(repo, path));

    var itemAttributes = File.GetAttributes(GetPath(currentPath));
    var name = Path.GetFileName(currentPath);
    var isDirectory = itemAttributes.HasFlag(FileAttributes.Directory);
    var type = isDirectory ? "tree" : "blob";

    var content = !isDirectory ? File.ReadAllText(currentPath) : null;
    var items = isDirectory ? GetItems(currentPath, branch) : null;
    return new GitItem(type, name, items, content);
});

app.Run();

record GitRepository(string Name);
record GitItem(string Type, string Name, GitItem[]? Items, string? Content);
