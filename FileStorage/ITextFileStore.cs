namespace FileStorage;

public interface ITextFileStore
{
    void WriteAllText(string filePath, string content);

    string ReadAllText(string filePath);

    bool FileExists(string filePath);

    void DeleteFile(string filePath);
}
