using System;
using System.Collections.Generic;
using System.Threading;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace AmbientCGMaterialDownloader
{
    internal class MaterialDownloader
    {
        private const string ambientCgViewLink = "https://ambientcg.com/view?id=";
        private const string ambientCgDownloadLink = "https://ambientcg.com/get?file=";

        Thread _thread;

        Dictionary<string, int> _categories;
        string[] _sizes;
        string[] _formats;
        char[] _postfixes;

        string _outDirPath;

        bool _unzip;
        bool _delPreviewImg;
        bool _delUsdaFile;
        bool _delUsdcFile;

        int _progress = 0;
        int _totalMaterialCount;

        bool _requestedCancelDownload = false;

        public Thread _Thread => _thread;
        public int Progress => _progress * 100 / _totalMaterialCount;


        public MaterialDownloader(Dictionary<string, int> categories, string[] sizes, string[] formats, char[] postfixes, string outDirPath, bool unzip, bool delPreviewImg, bool delUsdaFile, bool delUsdcFile)
        {
            _categories = categories;
            _sizes = sizes;
            _formats = formats;
            _postfixes = postfixes;
            _outDirPath = outDirPath;
            _unzip = unzip;
            _delPreviewImg = delPreviewImg;
            _delUsdaFile = delUsdaFile;
            _delUsdcFile = delUsdcFile;
            _totalMaterialCount = GetTotalMaterialCount();

            _thread = new Thread(new ThreadStart(this.DowloadMaterials));
            _thread.Start();
        }

        ~MaterialDownloader()
        {
            _thread.Interrupt();
            _categories.Clear();
        }

   
        private void DowloadMaterials()
        {
            foreach (KeyValuePair<string, int> category in _categories)
            {
                Regex regex = new Regex("\\s+");
                string viewCategory = regex.Replace(category.Key, "");
                string snakeCaseCategory = category.Key.Replace(" ", "_").Trim().ToLower();

                for (int id = 1; id <= category.Value; id++)
                {
                    foreach (char postfix in _postfixes)
                    {
                        string viewId = postfix == ' ' ? id.ToString("000") : id.ToString("000") + postfix;
                        string snakeCaseId = postfix == ' ' ? id.ToString("000") : id.ToString("000") + "_" + char.ToLower(postfix);

                        foreach (string size in _sizes)
                        {
                            string snakeCaseSize = size.ToLower();

                            foreach (string format in _formats)
                            {
                                if (_requestedCancelDownload) { return ; }

                                string snakeCaseFormat = format.ToLower();

                                string viewMatName = string.Format("{0}{1}", viewCategory, viewId);
                                string snakeCaseMatName = string.Format("{0}_{1}", snakeCaseCategory, snakeCaseId);

                                string zipMatFileName = string.Format("{0}_{1}_{2}.zip", snakeCaseMatName, snakeCaseSize, snakeCaseFormat);
                                string unzipMatFileName = string.Format("{0}_{1}_{2}", snakeCaseMatName, snakeCaseSize, snakeCaseFormat);

                                string zipMatFilePath = Path.Combine(_outDirPath, zipMatFileName);
                                string unzipMatFilePath = Path.Combine(_outDirPath, unzipMatFileName);

                                if (Directory.Exists(unzipMatFilePath))
                                {
                                    continue;
                                }

                                {
                                    string matViewLink = string.Format("{0}{1}", ambientCgViewLink, viewMatName);
                                    string matDownloadLink = string.Format("{0}{1}_{2}-{3}.zip", ambientCgDownloadLink, viewMatName, size, format);

                                    if (!DowloadMaterial(matViewLink, matDownloadLink, zipMatFilePath)) { continue; }
                                }

                                if (!_unzip) { continue; }


                                if (UnzipMaterial(zipMatFilePath, unzipMatFilePath))
                                {
                                    RenameMaterialMaps(unzipMatFilePath, snakeCaseMatName, snakeCaseSize, snakeCaseFormat);
                                }
                            }       
                        }
                    }

                    _progress++;
                }

            }
        }

        private bool DowloadMaterial(string matViewLink, string matDownloadLink, string matFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();
                Task<HttpResponseMessage> task = client.GetAsync(matViewLink);
                task.Wait();

                HttpResponseMessage response = task.Result;
                if (!response.IsSuccessStatusCode) { return false; }

                task = client.GetAsync(matDownloadLink);
                task.Wait();

                Task<string> strTask = task.Result.Content.ReadAsStringAsync();
                strTask.Wait();

                if (strTask.Result.Length < 24) { return false; }

                if (File.Exists(matFilePath)) { File.Delete(matFilePath); }

                FileStream file = new FileStream(matFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                task.Result.Content.CopyToAsync(file).Wait();
                file.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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

            foreach (string mapFilePath in mapFilePaths)
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
                else if (mapFilePath.Contains("Metallness"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "metalnes", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Metalness"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "metalnes", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Emission"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "emissive", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Specular"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "specular", snakeCaseMatSize, snakeCaseMatFormat);
                    string newMapFilePath = Path.Combine(matDirPath, newMapName);

                    File.Move(mapFilePath, newMapFilePath, true);
                }
                else if (mapFilePath.Contains("Opacity"))
                {
                    string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, "opacity", snakeCaseMatSize, snakeCaseMatFormat);
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
                else if (_delPreviewImg && mapFilePath.Contains("PREVIEW"))
                {
                    File.Delete(mapFilePath);
                }
                else if (_delUsdaFile && mapFilePath.Contains(".usda"))
                {
                    File.Delete(mapFilePath);
                }
                else if (_delUsdcFile && mapFilePath.Contains(".usdc"))
                {
                    File.Delete(mapFilePath);
                }

                for(int varIdx = 1; varIdx < 10; varIdx++)
                {
                    string variation = String.Format("var{0}", varIdx);
                    if (mapFilePath.Contains(variation))
                    {
                        string snakeCaseVariation = String.Format("var_{0}", varIdx).ToLower();
                        string newMapName = string.Format("{0}_{1}_{2}.{3}", snakeCaseMatName, snakeCaseVariation, snakeCaseMatSize, snakeCaseMatFormat);
                        string newMapFilePath = Path.Combine(matDirPath, newMapName);

                        File.Move(mapFilePath, newMapFilePath, true);
                    }
                }
            }

            return true;
        }

        private int GetTotalMaterialCount()
        {

            int totalMaterialCount = 0;
            foreach (KeyValuePair<string, int> category in _categories) { totalMaterialCount += category.Value; }

            return totalMaterialCount;

        }
    
        public void CancelDownload()
        {
            _requestedCancelDownload = true;
        }
    }
}
