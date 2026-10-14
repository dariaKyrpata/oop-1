using System.IO;

namespace FileStorage;

public class StreamTextFileStore : ITextFileStore
{
    public void WriteAllText(string filePath, string content)
    {
        using StreamWriter writer = new StreamWriter(
            filePath,
            false
        );

        writer.Write(content);
    }

    public string ReadAllText(string filePath)
    {
        using StreamReader reader = new StreamReader(filePath);

        return reader.ReadToEnd();
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
