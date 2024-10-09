namespace ReleaseCreator.CommandLine.Util
{
    internal interface IFileService
    {
        void AppendLine(string filePath, string content);
    }
}