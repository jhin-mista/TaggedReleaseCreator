namespace ReleaseCreator.Client.Util;

internal interface IFileService
{
    void AppendLine(string filePath, string content);
}