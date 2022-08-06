namespace AmbientCGMaterialDownloader
{
    internal static class App
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MaterialDownloaderForm());
        }
    }
}