namespace BootStrapper.Core.Helpers;

public static class GetPathsHelper
{
    public static string GetUnityProjectsFolderPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}
