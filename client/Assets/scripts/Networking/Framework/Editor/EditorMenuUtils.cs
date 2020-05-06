using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Networking.Framework.Editor
{
    /// <summary>
    /// Utils for editor menu
    /// </summary>
    public static class EditorMenuUtils
    {
        /// <summary>
        /// Should be used whenever .proto definition changes
        /// </summary>
        [MenuItem(RootPrefix + "/Regenerate protobuf")]
        public static void RegenerateProtobufFiles()
        {
            var protobufGenDir = Path.Combine(CommonDirPath, "protobuf");
            var protobufGenFileName = "generate_all.bat";
            var protobufGenFullPath = Path.Combine(protobufGenDir, protobufGenFileName);

            if (!File.Exists(protobufGenFullPath))
            {
                throw new FileNotFoundException(protobufGenFullPath);
            }

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    //When shell execute is set to false, full path is required as fileName and workingDir is not used to find executable
                    //see https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.workingdirectory?view=netframework-4.8
                    WorkingDirectory = protobufGenDir,
                    UseShellExecute = false,
                    FileName = protobufGenFullPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                Debug.Log($"Starting {protobufGenFullPath}");

                if (!process.Start())
                {
                    throw new Exception($"Failed to execute {protobufGenFileName}");
                }

                process.WaitForExit();

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(output))
                {
                    Debug.Log(output);
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error);
                }

                var exitCode = process.ExitCode;
                var exitMessage = $"{protobufGenFileName} exited with code {process.ExitCode}";

                if (exitCode != 0)
                {
                    Debug.LogError(exitMessage);
                    return;
                }

                Debug.Log(exitMessage);
            }
        }

        private static string CommonDirPath => Path.Combine(Application.dataPath, "..", "..", "common");
        private const string RootPrefix = "== Utils ==";
    }
}