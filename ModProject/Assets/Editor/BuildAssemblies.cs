using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Assets.Editor
{
    class BuildAssemblies
    {
        [MenuItem("AssemblyBuilder/Build Assembly")]
        public static void BuildAssemblySync()
        {
            BuildModLib(true);
        }
        private static void BuildModLib(bool wait)
        {
            var gameScripts = new[] { "Assets/GameScripts/GameScriptableObject.cs", "Assets/GameScripts/GamePOC.cs" };
            var gameTempPath = "Temp/Build/Assembly-CSharp.dll";
            var gameReferences = new string[] { };
            BuildAssembly(gameScripts, gameTempPath, null, gameReferences, true);

            var modScripts = new[] { "Assets/ModScripts/DerivedScriptableObject.cs", "Assets/ModScripts/ModScriptableObject.cs", "Assets/ModScripts/ModPOC.cs" };
            var modTempPath = "Temp/Build/ModLib.dll";
            var modAssemblyPath = "../Libs/ModLib.dll";
            var modRefrences = new string[] { "Temp/Build/Assembly-CSharp.dll" };
            BuildAssembly(modScripts, modTempPath, modAssemblyPath, modRefrences, true);
        }
        public static void BuildAssembly(string[] scripts, string tempAssembly, string assemblyProjectPath, string [] assemblyReferences, bool wait)
        {

            Directory.CreateDirectory(Path.GetDirectoryName(tempAssembly));
            if(assemblyProjectPath != null) Directory.CreateDirectory(Path.GetDirectoryName(assemblyProjectPath));
            var assemblyBuilder = new AssemblyBuilder(tempAssembly, scripts);
            assemblyBuilder.additionalReferences = assemblyReferences;
            assemblyBuilder.buildStarted += delegate (string assemblyPath)
            {
                Debug.LogFormat("Assembly build started for {0}", assemblyPath);
            };
            assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
            {
                var errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                var warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Assembly build finished for {0}", assemblyPath);
                Debug.LogFormat("Warnings: {0} - Errors: {0}", errorCount, warningCount);
                foreach (var msg in compilerMessages)
                {
                    if (msg.type == CompilerMessageType.Warning)
                    {
                        Debug.LogWarning(msg);
                    }
                    if (msg.type == CompilerMessageType.Error)
                    {
                        Debug.LogError(msg.message);
                    }
                }
                if (errorCount == 0)
                {
                    if (assemblyProjectPath != null)
                    {
                        File.Copy(tempAssembly, assemblyProjectPath, true);
                    }
                }
            };
            // Start build of assembly
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("Failed to start build of assembly {0}!", assemblyBuilder.assemblyPath);
                return;
            }

            if (wait)
            {
                while (assemblyBuilder.status != AssemblyBuilderStatus.Finished)
                    System.Threading.Thread.Sleep(10);
            }
        }
    }
}
