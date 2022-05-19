
internal class CommonResource
{
    public static string GetResourcePath()
    {
        return Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/"; // TODO...
    }
}