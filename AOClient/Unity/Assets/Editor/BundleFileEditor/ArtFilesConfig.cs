using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using UnityEditor.Build.Pipeline;
using System.Web.UI.WebControls;

[CreateAssetMenu(fileName = "ArtFilesConfig", menuName = "BundleFile/ArtFilesConfig")]
public class ArtFilesConfig : ScriptableObject
{
    public UnityEngine.Object GroupsFolder;

    public FileGroup[] GetFileGroups(GroupType groupType)
    {
        var artFilesConfig = this;
        var targetPath = AssetDatabase.GetAssetPath(artFilesConfig.GroupsFolder);
        var groupGUIDs = AssetDatabase.FindAssets("t:FileGroup", new string[] { targetPath });
        var groupPaths = groupGUIDs.Select(x => AssetDatabase.GUIDToAssetPath(x)).ToList();
        var groupAssets = groupPaths.Select(x => AssetDatabase.LoadAssetAtPath<FileGroup>(x)).Where(x => x.GroupType == groupType).ToArray();
        return groupAssets;
    }

    public string[] GetBundleByFileAssetPaths()
    {
        var groupAssets = GetFileGroups(GroupType.BundleByFile);
        var allAssetPaths = new List<string>();
        foreach (var fileGroup in groupAssets)
        {
            var assetPaths = fileGroup.GetAssetPaths();
            allAssetPaths.AddRange(assetPaths);
        }
        return allAssetPaths.ToArray();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArtFilesConfig))]
public class ArtFilesConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);

        if (GUILayout.Button("Copy Files to Streaming"))
        {
            Debug.Log("Copy Files to Streaming Finish.");
        }

        if (GUILayout.Button("Set Streaming Version 0"))
        {
            if (!Directory.Exists(Application.streamingAssetsPath + "/Bundles"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/Bundles");
            }
            var streamingArtVersionTxtPath = Application.streamingAssetsPath + $"/Bundles/_art_files_version.txt";
            File.WriteAllText(streamingArtVersionTxtPath, "0");
            Debug.Log("Set Streaming Version 0 Finish.");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Start Build"))
        {
            var artFilesConfig = target as ArtFilesConfig;
            if (artFilesConfig.GroupsFolder == null)
            {
                throw new Exception("GroupsTarget == null");
            }
            // ��ȡ��Ҫ����Bundle��Asset��Ϣ BundleByFile
            var assetPaths = artFilesConfig.GetBundleByFileAssetPaths();
            var assetBuilds = new List<AssetBundleBuild>();
            foreach (var assetPath in assetPaths)
            {
                var assetBuild = new AssetBundleBuild();
                assetBuild.assetBundleName = $"{ToMD5(assetPath)}";
                assetBuild.assetNames = new string[] { assetPath };
                assetBuilds.Add(assetBuild);
            }

            // ��ȡ��Ҫ����Bundle��Asset��Ϣ BundleInOne
            var groupAssets = artFilesConfig.GetFileGroups(GroupType.BundleInOne);
            foreach (var fileGroup in groupAssets)
            {
                var assetBuild = new AssetBundleBuild();
                assetBuild.assetBundleName = $"{ToMD5(fileGroup.name)}";
                var assetpaths = fileGroup.GetAssetPaths();
                assetBuild.assetNames = assetpaths;
                assetBuilds.Add(assetBuild);
            }

            // ��ʼ����AssetBundle
            BuildAssetBundles(assetBuilds.ToArray(), "Bundles/" + EditorUserBuildSettings.activeBuildTarget.ToString(), false, true, EditorUserBuildSettings.activeBuildTarget);
            Debug.Log("Build Bundles Finish.");

            var platformFolderPath = Application.dataPath + "/../Bundles/" + EditorUserBuildSettings.activeBuildTarget.ToString();

            // ��ȡ��һ����Դ�����İ汾
            var filePrefix = "_art_files";
            var fileListVersionPath = platformFolderPath + $"/{filePrefix}_version.txt";
            var version = 0;
            if (File.Exists(fileListVersionPath))
            {
                version = int.Parse(File.ReadAllText(fileListVersionPath).Trim());
            }
            version++;// ������Դ�����汾

            var artDataFolder = platformFolderPath + $"/artdata";
            if (!Directory.Exists(artDataFolder))
            {
                Directory.CreateDirectory(artDataFolder);
            }

            // ����������bundle�ļ���artdata�ļ��У����md5��׺
            var file2bundleDict = new Dictionary<string, string>();
            var addedBundleFiles = new Dictionary<string, string>();
            foreach (var assetBuild in assetBuilds)
            {
                var bundleFilePath = platformFolderPath + "/" + assetBuild.assetBundleName;
                var md5Value = ComputeMD5(bundleFilePath);
                var uploadBundleName = $"{assetBuild.assetBundleName}_{md5Value}";
                foreach (var assetName in assetBuild.assetNames)
                {
                    file2bundleDict.Add(assetName, uploadBundleName);
                }

                var uploadBundlePath = artDataFolder + "/" + uploadBundleName;
                if (File.Exists(uploadBundlePath))
                {
                    continue;
                }
                File.Copy(bundleFilePath, uploadBundlePath, false);
                addedBundleFiles.Add(bundleFilePath, uploadBundleName);
            }
            if (addedBundleFiles.Count == 0)
            {
                Debug.Log("No Added Files.");
                return;
            }

            // ����������bundle�ļ���added�ļ��У����md5��׺
            var versionArtDataAddedFolder = platformFolderPath + $"/artdata_{version}_added";
            if (!Directory.Exists(versionArtDataAddedFolder))
            {
                Directory.CreateDirectory(versionArtDataAddedFolder);
            }
            foreach (var item in addedBundleFiles)
            {
                var uploadBundlePath = versionArtDataAddedFolder + "/" + item.Value;
                var bundleFilePath = item.Key;
                File.Copy(bundleFilePath, uploadBundlePath, false);
            }

            Debug.Log("Copy Added Files Finish.");

            // ������Դ�����汾��Ϣ������°汾����bundle��Ϣ
            File.WriteAllText(fileListVersionPath, version.ToString());
            var versionListPath = platformFolderPath + $"/{filePrefix}_{version}.txt";
            var versionAddedListPath = versionArtDataAddedFolder + $"/{filePrefix}_added.txt";
            var versionListText = new StringBuilder();
            var versionAddedListText = new StringBuilder();
            foreach (var item in file2bundleDict)
            {
                versionListText.AppendLine($"{item.Value}={item.Key}");
            }
            //versionListText.AppendLine("added:");
            foreach (var item in addedBundleFiles)
            {
                var bundleFilePath = item.Key;
                var length = new FileInfo(bundleFilePath).Length;
                versionAddedListText.AppendLine(item.Value + $":{length}");
            }
            File.WriteAllText(versionListPath, versionListText.ToString());
            File.WriteAllText(versionAddedListPath, versionAddedListText.ToString());
            Debug.Log($"{filePrefix}_{version}.txt {file2bundleDict.Values.Distinct().Count()} ab");
            Debug.Log($"artdata_{version}_added/{filePrefix}_added.txt {addedBundleFiles.Count} ab");
        }
    }

    public static bool BuildAssetBundles(AssetBundleBuild[] builds, string outputPath, bool forceRebuild, bool useChunkBasedCompression, BuildTarget buildTarget)
    {
        var options = BuildAssetBundleOptions.None;
        if (useChunkBasedCompression)
            options |= BuildAssetBundleOptions.ChunkBasedCompression;

        if (forceRebuild)
            options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;

        Directory.CreateDirectory(outputPath);
        // Replaced BuildPipeline.BuildAssetBundles with CompatibilityBuildPipeline.BuildAssetBundles here
        var manifest = CompatibilityBuildPipeline.BuildAssetBundles(outputPath, builds, options, buildTarget);
        return manifest != null;
    }

    public static string ComputeMD5(string fileName)
    {
        var hashMD5 = string.Empty;
        //����ļ��Ƿ���ڣ�����ļ���������м��㣬���򷵻ؿ�ֵ
        if (File.Exists(fileName))
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                //�����ļ���MD5ֵ
                var buffer = md5.ComputeHash(fs);
                hashMD5 = ToHex(buffer, "x2");
            }//�ر��ļ���
        }//��������
        return hashMD5;
    }//ComputeMD5

    public static MD5 md5 = MD5.Create();
    public static string ToMD5(string content)
    {
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
        return ToHex(bytes, "x2");
    }

    public static string ToHex(byte[] bytes, string format)
    {
        var sb = new StringBuilder();
        foreach (var t in bytes)
        {
            sb.Append(t.ToString(format));
        }
        return sb.ToString();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Startup()
    {
        var artFilesConfig = AssetDatabase.LoadAssetAtPath<ArtFilesConfig>("Assets/Settings/ArtFilesConfig.asset");
        var assetPaths = artFilesConfig.GetBundleByFileAssetPaths();
        foreach (var assetPath in assetPaths)
        {
            AssetFile.Asset.Path2BundleNames.Add(assetPath, ToMD5(assetPath));
            AssetFile.Asset.AssetName2Paths.Add(Path.GetFileName(assetPath), assetPath);
            //Debug.Log($"{Path.GetFileName(assetPath)} {assetPath} {ToMD5(assetPath)}");
        }
    }
}
#endif
