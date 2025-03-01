namespace ReleaseCreator.Client.Util;

internal interface IFileService
{
    public void AppendLine(string filePath, string content);
}