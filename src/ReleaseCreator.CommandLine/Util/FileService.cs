namespace ReleaseCreator.CommandLine.Util
{
    internal class FileService : IFileService
    {
        public void AppendLine(string filePath, string content) => File.AppendAllLines(filePath, [content]);
    }
}
