using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public class Git
    {
        #region 右键菜单

        [MenuItem("Assets/Git/Commit", false, 0)]
        public static async void Commit()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:commit /path:" + GetSelection() + " /closeonend:0");
        }

        [MenuItem("Assets/Git/Push", false, 1)]
        public static async void Push()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:push /path:" + GetSelection() + " /closeonend:0");
        }

        [MenuItem("Assets/Git/Pull", false, 1)]
        public static async void Pull()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:pull /path:" + GetSelection() + " /closeonend:0");
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Git/Revert", false, 2)]
        public static async void Revert()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:revert /path:" + GetSelection() + " /closeonend:0");
        }

        [MenuItem("Assets/Git/Log", false, 51)]
        public static async void Log()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:log /path:" + GetSelection() + " /closeonend:0");
        }

        [MenuItem("Assets/Git/Blame", false, 52)]
        public static async void Blame()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:blame /path:" + GetSelection() + " /closeonend:0");
        }

        [MenuItem("Assets/Git/Merge", false, 53)]
        public static async void Merge()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:merge /path:" + GetSelection() + " /closeonend:0");
        }


        #endregion

        #region 工具栏菜单项

        [MenuItem("Tools/Git/CommitAll _F4", false, 0)]
        public static async void CommitAll()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:commit /path:" + "Assets*Packages*ProjectSettings" + " /closeonend:0");
        }

        [MenuItem("Tools/Git/Fetch _F5", false, 1)]
        public static async void Fetch()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:fetch" + " /closeonend:0");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Git/PushAll _F6", false, 1)]
        public static async void PushAll()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:push /path:" + Application.dataPath + " /closeonend:0");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Git/PullAll _F7", false, 1)]
        public static async void PullAll()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:pull /path:" + Application.dataPath + " /closeonend:0");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Git/Switch _F8", false, 1)]
        public static async void Switch()
        {
            await ProcessCommandAsync("TortoiseGitProc.exe", "/command:switch /path:" + Application.dataPath + " /closeonend:0");
        }

        #endregion

        #region Terminal

        static async Task ProcessCommandAsync(string command, string argument)
        {
            await Task.Run((() =>
            {
                ProcessStartInfo info = new ProcessStartInfo(command);
                info.Arguments = argument;
                info.CreateNoWindow = true;
                info.ErrorDialog = true;
                info.UseShellExecute = true;

                if (info.UseShellExecute)
                {
                    info.RedirectStandardOutput = false;
                    info.RedirectStandardError = false;
                    info.RedirectStandardInput = false;
                }
                else
                {
                    info.RedirectStandardOutput = true;
                    info.RedirectStandardError = true;
                    info.RedirectStandardInput = true;
                    info.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    info.StandardErrorEncoding = System.Text.Encoding.UTF8;
                }

                Process process = Process.Start(info);

                if (!info.UseShellExecute)
                {
                    UnityEngine.Debug.Log(process.StandardOutput);
                    UnityEngine.Debug.Log(process.StandardError);
                }

                process.WaitForExit();

            }));
        }

        #endregion

        #region 辅助函数

        /// <summary>
        /// 获取选中路径参数
        /// </summary>
        /// <returns>路径参数</returns>
        public static string GetSelection()
        {
            string path = "Assets";
            string[] strs = Selection.assetGUIDs;
            if (strs != null)
            {
                path = "\"";
                for (int i = 0; i < strs.Length; i++)
                {
                    if (i != 0)
                        path += "*";
                    path += AssetDatabase.GUIDToAssetPath(strs[i]);
                    if (AssetDatabase.GUIDToAssetPath(strs[i]) != "Assets")
                        path += "*" + AssetDatabase.GUIDToAssetPath(strs[i]) + ".meta";
                }

                path += "\"";
            }

            return path;
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>完整路径</returns>
        public static string GetCompletePath(string path)
        {
            var completePath = "\"";
            completePath += path;
            if (path != "Assets")
                completePath += "*" + path + ".meta";
            completePath += "\"";
            return path;
        }

        #endregion
    }
}