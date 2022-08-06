using System.Net.Http;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AmbientCGMaterialDownloader
{
    public partial class MaterialDownloaderForm : Form
    {
        private const string ambientCgViewLink = "https://ambientcg.com/view?id=";
        private const string ambientCgDownloadLink = "https://ambientcg.com/get?file=";

        Dictionary<string, int> _categories = new Dictionary<string, int>(100);

        bool _isDownloading = false;
        MaterialDownloader[] _downloaders = new MaterialDownloader[1];

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

        private void MaterialDownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelDownload();
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
            if (_isDownloading)
            {
                CancelDownload();
                return; 
            }
            else
            {
                if(! (outPathFbd.ShowDialog() == DialogResult.OK) && Directory.Exists(outPathFbd.SelectedPath)) { return; }
                SetToDownloadMode();
                DowloadMaterials();
            }
        }

        private void downloadTimer_Tick(object sender, EventArgs e)
        {
            if (!_isDownloading) { return; }

            UpdateDownloadProgress();
        }

        private void allCbx_CheckedChanged(object sender, EventArgs e)
        {
            for(int itemIdx = 0; itemIdx < categoryCbxList.Items.Count; itemIdx++)
            {
                categoryCbxList.SetItemChecked(itemIdx, allCbx.Checked);
            }
        }

        // Material Downloader Functions

        private async void GetAmbientGCMaterialCategories()
        {
            HttpClient client = new HttpClient();
            string[] ignoreCategories = new string[] { "Substance", "Sbustance", "HDRI" };

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
                    categoryCbxList.Items.Add(catPairStr[0], allCbx.Checked);
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

        private Dictionary<string, int>[] DistributeCategories(Dictionary<string, int> categories, int targetDownloadersCount)
        {
            if(categories.Count == 1) { return new Dictionary<string, int>[1] { categories }; }

            bool targetIsMajor = false;
            if(targetDownloadersCount >= categories.Count) { targetDownloadersCount = categories.Count; targetIsMajor = true; }

            int totalMaterialCount = 0;
            foreach (KeyValuePair<string, int> category in categories) { totalMaterialCount += category.Value; }


            int targetPerDownloaderMaterialCount = totalMaterialCount / targetDownloadersCount;


            // To get the real downloader count
            int totalDistributedMaterialCount = 0;

            int currDownloaderMaterialIndex = 0;
            int currDownloaderMaterialCount = 0;

            foreach (KeyValuePair<string, int> category in categories) 
            {
                if(totalDistributedMaterialCount >= totalMaterialCount) { break; }

                currDownloaderMaterialCount += category.Value;
                totalDistributedMaterialCount += category.Value;

                if(currDownloaderMaterialCount >= targetPerDownloaderMaterialCount)
                {
                    currDownloaderMaterialIndex++;
                    currDownloaderMaterialCount = 0;
                }
            }


            // To create the distributed array
            int downloadersCount = targetIsMajor ? currDownloaderMaterialIndex : currDownloaderMaterialIndex + 1;  

            Dictionary<string, int>[] distributedCategories = new Dictionary<string, int>[downloadersCount];

            for(int distCatIdx = 0; distCatIdx < downloadersCount; distCatIdx++)
            {
                distributedCategories[distCatIdx] = new Dictionary<string, int>(targetPerDownloaderMaterialCount);
            }

            totalDistributedMaterialCount = 0;
            currDownloaderMaterialIndex = 0;
            currDownloaderMaterialCount = 0;

            foreach (KeyValuePair<string, int> category in categories)
            {
                if (totalDistributedMaterialCount >= totalMaterialCount) { break; }

                currDownloaderMaterialCount += category.Value;
                totalDistributedMaterialCount += category.Value;

                if(!distributedCategories[currDownloaderMaterialIndex].ContainsKey(category.Key))
                {
                    distributedCategories[currDownloaderMaterialIndex].Add(category.Key, category.Value);
                }

                if (currDownloaderMaterialCount >= targetPerDownloaderMaterialCount)
                {
                    currDownloaderMaterialIndex++;
                    currDownloaderMaterialCount = 0;
                }
            }

            return distributedCategories;
        }


        private void DowloadMaterials()
        {
            Dictionary<string, int>[] distributedCategories = DistributeCategories(GetCategories(), Environment.ProcessorCount * 4);
            string[] sizes = GetSizes();
            string[] formats = GetFormats();
            char[] postfixes = new char[] { ' ', 'A', 'B', 'C' };

            GC.Collect();

            _downloaders = new MaterialDownloader[distributedCategories.Length];
            Console.WriteLine(_downloaders.Length);

            for (int dIdx = 0; dIdx < _downloaders.Length; dIdx++)
            {
                _downloaders[dIdx] = new MaterialDownloader(distributedCategories[dIdx], sizes, formats, postfixes, outPathFbd.SelectedPath, unzipCbx.Checked, delPreviewImgCbx.Checked, delUsdaFileCbx.Checked, delUsdcFileCbx.Checked);
            }
        }

        private void CancelDownload()
        {
            if(_isDownloading)
            {
                foreach(MaterialDownloader downloader in _downloaders)
                {
                    downloader.CancelDownload();
                }


                SetToIdleMode();
            }
        }

        private void UpdateDownloadProgress()
        {
            int totalDownloadProgress = 0;

            bool allDone = true;
            int sumOfAllProgresses = 0;

            for (int dIdx = 0; dIdx < _downloaders.Length; dIdx++)
            {
                int currDownloaderProgress = _downloaders[dIdx].Progress;

                sumOfAllProgresses += currDownloaderProgress;
                allDone &= currDownloaderProgress >= 100;
            }

            totalDownloadProgress = sumOfAllProgresses * 100 / (_downloaders.Length * 100);
            downloadPgb.Value = totalDownloadProgress;

            if (allDone) { SetToIdleMode(); }
        }
    
        private void SetToDownloadMode()
        { 
            _isDownloading = true;

            downloadTimer.Start();
            downloadPgb.Visible = true;
            downloadPgb.Enabled = true;
            downloadPgb.Value = 0;
            downloadPgb.Maximum = 100;
            downloadBtn.Text = "Cancel";
        }

        private void SetToIdleMode()
        {
            _isDownloading = false;

            downloadTimer.Stop();
            downloadPgb.Visible = false;
            downloadPgb.Enabled = false;
            downloadPgb.Value = 0;
            downloadBtn.Text = "Download";
        }

    }
}