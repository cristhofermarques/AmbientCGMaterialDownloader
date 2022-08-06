using System.Net.Http;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AmbientCGMaterialDownloader
{
    public partial class MaterialDownloaderForm : Form
    {
        HttpClient client = new HttpClient();
        private const string ambientCgViewLink = "https://ambientcg.com/view?id=";
        private const string ambientCgDownloadLink = "https://ambientcg.com/get?file=";

        Dictionary<string, int> _categories = new Dictionary<string, int>(100);


        // Constructors
        public MaterialDownloaderForm()
        {
            InitializeComponent();
        }


        // UI Events
        private void TextureDownloaderForm_Load(object sender, EventArgs e)
        {
            GetAmbientGCMaterialCategories();
        }

        private void unzipCbx_CheckedChanged(object sender, EventArgs e)
        {
            if(unzipCbx.Checked)
            {
                delPreviewImgCbx.Enabled = true;
                delUsdaFileCbx.Enabled = true;
                delUsdcFileCbx.Enabled = true;
            }
            else
            {
                delPreviewImgCbx.Enabled = false;
                delUsdaFileCbx.Enabled = false;
                delUsdcFileCbx.Enabled = false;
            }
        }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            if(! (outPathFbd.ShowDialog() == DialogResult.OK) && Directory.Exists(outPathFbd.SelectedPath)) { return; }

            downloadPgb.Visible = true;
            downloadPgb.Enabled = true;
            catDownloadPgb.Enabled = true;
            catDownloadPgb.Visible = true;
            downloadInfoLbl.Visible = true;

            DowloadMaterials();

            downloadInfoLbl.Visible = false;
            downloadInfoLbl.Text = "";
            downloadPgb.Visible = false;
            downloadPgb.Enabled = false;
            downloadPgb.Value = 0;
            catDownloadPgb.Enabled = false;
            catDownloadPgb.Visible = false;
            catDownloadPgb.Value = 0;
        }


        // Material Downloader Functions

        private async void GetAmbientGCMaterialCategories()
        {
            string[] ignoreCategories = new string[] { "Substance", "HDRI" };

            string rawHtml = await client.GetStringAsync("https://ambientcg.com/categories");

            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(rawHtml);

            foreach (HtmlAgilityPack.HtmlNode node in html.DocumentNode.SelectNodes("//a[@href]"))
            {
                bool ignore = false;
                foreach (string ignoreCategory in ignoreCategories)
                {
                    ignore |= node.InnerText.Contains(ignoreCategory);
                }

                if (!ignore && node.InnerText.Contains("Asset") && node.InnerText.Contains("(") && node.InnerText.Contains(")"))
                {
                    string[] catPairStr = node.InnerText.Trim().Split(" (");
                    catPairStr[0] = catPairStr[0].Trim();
                    catPairStr[1] = catPairStr[1].Remove(catPairStr[1].LastIndexOf(')')).Replace("Asset", "").Replace("s", "").Trim();
                    int catMatAmount = 0;

                    if (!int.TryParse(catPairStr[1], out catMatAmount)) { return; }

                    // Success to get category name and the ammount
                    categoryCbxList.Items.Add(catPairStr[0], true);
                    _categories.Add(catPairStr[0], catMatAmount);
                }
            }
        }

        private Dictionary<string, int> GetCategories()
        {
            Dictionary<string, int > categories = new Dictionary<string, int>(categoryCbxList.CheckedItems.Count);

            foreach(object catStrObj in categoryCbxList.CheckedItems)
            {
                if(catStrObj is string)
                {
                    string? categoryStrFromObj = catStrObj as string;

                    if(categoryStrFromObj != null && _categories.ContainsKey(categoryStrFromObj))
                    {
                        categories.Add(categoryStrFromObj, _categories[categoryStrFromObj]);
                    }
                }
            }

            return categories;
        }

        private string[] GetSizes()
        {
            List<string> sizes = new List<string>(4);

            if (_1kCbx.Checked) { sizes.Add("1K"); }
            if (_2kCbx.Checked) { sizes.Add("2K"); }
            if (_4kCbx.Checked) { sizes.Add("4K"); }
            if (_8kCbx.Checked) { sizes.Add("8K"); }

            return sizes.ToArray();
        }

        private string[] GetFormats()
        {
            List<string> formats = new List<string>(2);

            if (jpgCbx.Checked) { formats.Add("JPG"); }
            if (pngCbx.Checked) { formats.Add("PNG"); }

            return formats.ToArray();
        }

        private void DowloadMaterials()
        {

            Dictionary<string, int> categories = GetCategories();
            string[] sizes = GetSizes();
            string[] formats = GetFormats();
            char[] postfixes = new char[] { ' ', 'A', 'B', 'C' };

            downloadPgb.Maximum = categories.Count;
            downloadPgb.Value = 0;

            foreach(KeyValuePair<string, int> category in categories)
            {
                Regex regex = new Regex("\\s+");
                string viewCategory = regex.Replace(category.Key, "");
                string snakeCaseCategory = category.Key.Replace(" ", "_").Trim().ToLower();

                catDownloadPgb.Maximum = category.Value;

                for (int id = 1; id <= category.Value; id++)
                {
                    foreach(char postfix in postfixes)
                    {
                        string viewId = postfix == ' ' ? id.ToString("000") : id.ToString("000") + postfix;
                        string snakeCaseId = postfix == ' ' ? id.ToString("000") : id.ToString("000") + "_" + char.ToLower(postfix);

                        foreach(string size in sizes)
                        {
                            string snakeCaseSize = size.ToLower();

                            foreach (string format in formats)
                            {
                                string snakeCaseFormat = format.ToLower();

                                string viewMatName = string.Format("{0}{1}", viewCategory, viewId);
                                string snakeCaseMatName = string.Format("{0}_{1}", snakeCaseCategory, snakeCaseId);

                                string zipMatFilePath = Path.Combine(outPathFbd.SelectedPath, string.Format("{0}_{1}_{2}.zip", snakeCaseMatName, snakeCaseSize, snakeCaseFormat));
                                string unzipMatFilePath = Path.Combine(outPathFbd.SelectedPath, string.Format("{0}_{1}_{2}", snakeCaseMatName, snakeCaseSize, snakeCaseFormat));

                                if (Directory.Exists(unzipMatFilePath)) 
                                {
                                    downloadInfoLbl.Text = "Skip " + snakeCaseMatName;
                                    continue; 
                                }

                                { 
                                    string matViewLink = string.Format("{0}{1}", ambientCgViewLink, viewMatName);
                                    string matDownloadLink = string.Format("{0}{1}_{2}-{3}.zip", ambientCgDownloadLink, viewMatName, size, format);

                                    

                                    if (!DowloadMaterial(matViewLink, matDownloadLink, zipMatFilePath)) { continue; }
                                }

                                downloadInfoLbl.Text = string.Format("{0}_{1}_{2}", snakeCaseMatName, snakeCaseSize, snakeCaseFormat);

                                if (!unzipCbx.Checked) { continue; }
                                

                                if(UnzipMaterial(zipMatFilePath, unzipMatFilePath))
                                {
                                    RenameMaterialMaps(unzipMatFilePath, snakeCaseMatName, snakeCaseSize, snakeCaseFormat);
                                }
                            }
                        }
                    }

                    catDownloadPgb.Value = id;
                }
            }
        }

        private bool DowloadMaterial(string matViewLink, string matDownloadLink, string matFilePath)
        {
            Task<HttpResponseMessage> task = client.GetAsync(matViewLink);
            task.Wait();

            HttpResponseMessage response = task.Result;
            if (!response.IsSuccessStatusCode) { return false; }

            task = client.GetAsync(matDownloadLink);
            task.Wait();

            Task<string> strTask = task.Result.Content.ReadAsStringAsync();
            strTask.Wait();

            if(strTask.Result.Length < 24) { return false; }

            if (File.Exists(matFilePath)) { File.Delete(matFilePath); }

            FileStream file = new FileStream(matFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            task.Result.Content.CopyToAsync(file).Wait();
            file.Close();

            return true;
        }

        private bool UnzipMaterial(string zipMatFilePath, string unzipMatDirPath)
        {
            if (!File.Exists(zipMatFilePath)) { return false; }

            ZipFile.ExtractToDirectory(zipMatFilePath, unzipMatDirPath, true);
            File.Delete(zipMatFilePath);

            return true;
        }

        private bool RenameMaterialMaps(string matDirPath, string snakeCaseMatName, string snakeCaseMatSize, string snakeCaseMatFormat)
        {
            if (!Directory.Exists(matDirPath)) { return false; }

            string[] mapFilePaths = Directory.GetFiles(matDirPath);

            foreach(string mapFilePath in mapFilePaths)
            {
                if (mapFilePath.Contains("AmbientOcclusion"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "ao", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Color"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "albedo", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Displacement"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "height", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Roughness"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "roughness", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Metalness"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "metalness", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Specular"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "specular", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("NormalDX"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "normal_dx", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("NormalGL"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "normal_gl", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if ( delPreviewImgCbx.Checked && mapFilePath.Contains("PREVIEW"))
                {
                    File.Delete(mapFilePath);
                }
                else if (delUsdaFileCbx.Checked && mapFilePath.Contains(".usda"))
                {
                    File.Delete(mapFilePath);
                }
                else if (delUsdcFileCbx.Checked && mapFilePath.Contains(".usdc"))
                {
                    File.Delete(mapFilePath);
                }
            }

            return true;
        }

    }
}