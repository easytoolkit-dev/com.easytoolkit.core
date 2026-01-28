using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EasyToolkit.Core.Unity
{
    /// <summary>
    /// Provides static methods for version comparison based on the application's current version.
    /// </summary>
    public static class UnityVersionChecker
    {
        private static readonly Regex versionPattern = new Regex(@"^(\d+)\.(\d+)(?:\.(\d+))?(?:\.(\d+))?");

        private static int majorVersion = -1;
        private static int minorVersion = -1;
        private static int patchVersion = -1;
        private static int buildVersion = -1;
        private static bool isInitialized = false;

        /// <summary>
        /// Initializes the version checker by parsing the current application version.
        /// This method is called automatically before any comparison operation.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the application version format is invalid or cannot be parsed.
        /// </exception>
        private static void Initialize()
        {
            if (isInitialized) return;

            string currentVersion = Application.version;

            if (string.IsNullOrEmpty(currentVersion))
            {
                throw new InvalidOperationException("Application version is null or empty.");
            }

            Match match = versionPattern.Match(currentVersion);

            if (!match.Success)
            {
                throw new InvalidOperationException(
                    $"Invalid version format: '{currentVersion}'. Expected format: X.Y[.Z[.W]] where X, Y, Z, W are integers.");
            }

            if (!int.TryParse(match.Groups[1].Value, out majorVersion) ||
                !int.TryParse(match.Groups[2].Value, out minorVersion))
            {
                throw new InvalidOperationException(
                    $"Failed to parse major or minor version from: '{currentVersion}'.");
            }

            // Parse optional patch version (third segment)
            if (match.Groups[3].Success)
            {
                if (!int.TryParse(match.Groups[3].Value, out patchVersion))
                {
                    patchVersion = 0; // Default to 0 if parsing fails
                }
            }
            else
            {
                patchVersion = 0;
            }

            // Parse optional build version (fourth segment)
            if (match.Groups[4].Success)
            {
                if (!int.TryParse(match.Groups[4].Value, out buildVersion))
                {
                    buildVersion = 0; // Default to 0 if parsing fails
                }
            }
            else
            {
                buildVersion = 0;
            }

            isInitialized = true;
        }

        /// <summary>
        /// Gets the current application's major version.
        /// </summary>
        /// <returns>The major version number.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the version cannot be initialized.
        /// </exception>
        public static int GetMajorVersion()
        {
            Initialize();
            return majorVersion;
        }

        /// <summary>
        /// Gets the current application's minor version.
        /// </summary>
        /// <returns>The minor version number.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the version cannot be initialized.
        /// </exception>
        public static int GetMinorVersion()
        {
            Initialize();
            return minorVersion;
        }

        /// <summary>
        /// Gets the current application's patch version.
        /// Returns 0 if no patch version is specified in the version string.
        /// </summary>
        /// <returns>The patch version number.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the version cannot be initialized.
        /// </exception>
        public static int GetPatchVersion()
        {
            Initialize();
            return patchVersion;
        }

        /// <summary>
        /// Gets the current application's build version.
        /// Returns 0 if no build version is specified in the version string.
        /// </summary>
        /// <returns>The build version number.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the version cannot be initialized.
        /// </exception>
        public static int GetBuildVersion()
        {
            Initialize();
            return buildVersion;
        }

        /// <summary>
        /// Checks if the current application version is greater than or equal to the specified version.
        /// Only compares major and minor versions by default.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <returns>
        /// True if the current version is greater than or equal to the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        /// <example>
        /// <code>
        /// if (VersionChecker.IsVersionOrGreater(2, 1))
        /// {
        ///     // Execute feature available in version 2.1 or later
        /// }
        /// </code>
        /// </example>
        public static bool IsVersionOrGreater(int major, int minor)
        {
            Initialize();

            if (majorVersion > major) return true;
            if (majorVersion == major && minorVersion >= minor) return true;

            return false;
        }

        /// <summary>
        /// Checks if the current application version is greater than or equal to the specified version,
        /// including patch version comparison.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <param name="patch">The patch version to compare against.</param>
        /// <returns>
        /// True if the current version is greater than or equal to the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        public static bool IsVersionOrGreater(int major, int minor, int patch)
        {
            Initialize();

            if (majorVersion > major) return true;
            if (majorVersion < major) return false;

            if (minorVersion > minor) return true;
            if (minorVersion < minor) return false;

            return patchVersion >= patch;
        }

        /// <summary>
        /// Checks if the current application version is greater than or equal to the specified version,
        /// including patch and build version comparison.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <param name="patch">The patch version to compare against.</param>
        /// <param name="build">The build version to compare against.</param>
        /// <returns>
        /// True if the current version is greater than or equal to the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        public static bool IsVersionOrGreater(int major, int minor, int patch, int build)
        {
            Initialize();

            if (majorVersion > major) return true;
            if (majorVersion < major) return false;

            if (minorVersion > minor) return true;
            if (minorVersion < minor) return false;

            if (patchVersion > patch) return true;
            if (patchVersion < patch) return false;

            return buildVersion >= build;
        }

        /// <summary>
        /// Checks if the current application version is less than the specified version.
        /// Only compares major and minor versions by default.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <returns>
        /// True if the current version is less than the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        public static bool IsVersionLess(int major, int minor)
        {
            Initialize();

            if (majorVersion < major) return true;
            if (majorVersion == major && minorVersion < minor) return true;

            return false;
        }

        /// <summary>
        /// Checks if the current application version is exactly equal to the specified version.
        /// Only compares major and minor versions by default.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <returns>
        /// True if the current version exactly matches the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        public static bool IsVersionExactly(int major, int minor)
        {
            Initialize();
            return majorVersion == major && minorVersion == minor;
        }

        /// <summary>
        /// Checks if the current application version is exactly equal to the specified version,
        /// including patch version comparison.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <param name="patch">The patch version to compare against.</param>
        /// <returns>
        /// True if the current version exactly matches the specified version; otherwise, false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current application version cannot be initialized.
        /// </exception>
        public static bool IsVersionExactly(int major, int minor, int patch)
        {
            Initialize();
            return majorVersion == major && minorVersion == minor && patchVersion == patch;
        }

        /// <summary>
        /// Gets the full version string of the current application.
        /// </summary>
        /// <returns>The full version string as defined in Application.version.</returns>
        public static string GetFullVersionString()
        {
            return Application.version;
        }

        /// <summary>
        /// Gets the parsed version components as a formatted string.
        /// </summary>
        /// <returns>A string in the format "Major.Minor.Patch.Build" with all components.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the version cannot be initialized.
        /// </exception>
        public static string GetParsedVersionString()
        {
            Initialize();
            return $"{majorVersion}.{minorVersion}.{patchVersion}.{buildVersion}";
        }
    }
}
