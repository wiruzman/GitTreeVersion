﻿using System.Linq;
using GitTreeVersion.Git;
using GitTreeVersion.Paths;

namespace GitTreeVersion.VersionStrategies
{
    public class MajorFileBumpVersionStrategy : IVersionStrategy
    {
        public VersionComponent GetVersionComponent(AbsoluteDirectoryPath versionRootPath, AbsoluteDirectoryPath[] relevantPaths, string? range)
        {
            var gitDirectory = new GitDirectory(versionRootPath);

            var majorVersionFiles = gitDirectory.GitFindFiles(new[] { ":(glob).version/major/*" });

            foreach (var file in majorVersionFiles)
            {
                Log.Debug($"Major version file: {file}");
            }

            if (majorVersionFiles.Any())
            {
                string[] majorVersionCommits = gitDirectory.GitCommits(null, new[] { ":(glob).version/major/*" }, diffFilter: "A");

                foreach (var majorVersionCommit in majorVersionCommits)
                {
                    Log.Debug($"Major version commit: {majorVersionCommit}");
                }

                range = $"{majorVersionCommits.First()}..";
            }

            return new VersionComponent(majorVersionFiles.Length, range);
        }
    }
}