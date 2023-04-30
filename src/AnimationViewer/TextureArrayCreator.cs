using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TextureArrayCreator
{
    class MyProcess
    {
        public static List<String>? ChooseFiles(String? initialDirectory = null, Dictionary<String, String>? filters = null)
        {
            return ChooseFiles(true, initialDirectory, filters);
        }

        public static String? ChooseFile(String? initialDirectory = null, Dictionary<String, String>? filters = null)
        {
            var files = ChooseFiles(false, initialDirectory, filters).ToArray();
            return files?.Length < 0 ? null : files?[0];
        }

        private static List<String> ChooseFiles(bool multiselect, String? initialDirectory, Dictionary<String, String>? filters)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = multiselect;
            openFileDialog.Filter = CreateFilterString(filters);
            openFileDialog.InitialDirectory = GetFolderPath(initialDirectory);

            var fileList = new List<String>();
            if (openFileDialog.ShowDialog() == true)
            {
                fileList.AddRange(openFileDialog.FileNames);
            }

            return fileList;
        }

        void SaveAsJson() {
            // TODO
        }

        public void SaveAsKTX(String outputPath, String[] paths)
        {

            String executable = "";
            var arguments = new List<string>();
            arguments.Add("-t2");  // encode to KTX 2

            // add layers
            if (paths.Length > MaxLayerNum)
            {
                ShowError("Too many layers");
            }
            arguments.Add($"--levels {paths.Length}");
            arguments.AddRange(paths);

            // generate mip-mapps
            arguments.AddRange(new String[] { "--genmipmap", "--filter", "lanczos12" });

            // add ZIP compression
            arguments.AddRange(new String[] { "–zcmp", "22" });

            arguments.Add(outputPath);

            // add mip-mapps
            Run(executable, arguments.ToArray());

            var fileInfo = new System.IO.FileInfo(outputPath);
            System.Console.WriteLine($"Exporting {paths.Length} images as {fileInfo.FullName} ({fileInfo.Length / 1024}) KB");
        }

        public void ExportAsKTX(String[] paths)
        {
            String? outputPath = ChooseFile();
            if (outputPath == null)
            {
                ShowError("No file chosen");
                return;
            }

            ExportAsKTX(outputPath, paths);
        }


        private static String CreateFilterString(Dictionary<String, String>? filters)
        {
            if (filters == null)
            {
                return "(All  Files) *.*";
            }

            var sb = new System.Text.StringBuilder();
            int index = 0;
            foreach ((String description, String extension) in filters)
            {
                sb.Append($"{description} (*.{extension.ToLower()}");
                if (index++ == filters.Count - 1)
                {
                    sb.Append("|");
                }
            }

            return sb.ToString();
        }


        private static String GetFolderPath(String? initialDirectory)
        {
            if (initialDirectory != null)
            {
                return initialDirectory;
            }
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private static bool ShowError(String message)
        {
#if OS_WINDOWS
            string messageBoxText = message;
            string caption = "Error :()";
            System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
            System.Windows.MessageBoxImage icon = System.Windows.MessageBoxImage.Error;

            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, System.Windows.MessageBoxResult.OK);
            return result == System.Windows.MessageBoxResult.OK;
#else 
            var previousForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ForegroundColor = previousForegroundColor;
#endif
        }

        protected static void Run(string executablePath, string[] arguments)
        {
            try
            {
                using (Process process = new Process())
                {
                    var processStartInfo = new ProcessStartInfo();
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.FileName = executablePath;
                    processStartInfo.CreateNoWindow = true;
                    foreach (var argument in arguments)
                    {
                        processStartInfo.ArgumentList.Add(argument);
                    }
                    Process.Start(processStartInfo);
                }
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }


        void PrintHardwareLimist() {
            // TODO
        }

        void StatusBarTest()
        {

            var sb = new ProgressBar();
            sb.Value = 0.1;
        }

        readonly int MaxLayerNum = 2048;  // TODO
    }


}