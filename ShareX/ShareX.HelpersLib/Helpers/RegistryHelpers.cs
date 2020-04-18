﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2020 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Microsoft.Win32;
using System;
using System.IO;

namespace ShareX.HelpersLib
{
    public static class RegistryHelpers
    {
        public static void CreateRegistry(string path, string value, RegistryHive root = RegistryHive.CurrentUser)
        {
            CreateRegistry(path, null, value, root);
        }

        public static void CreateRegistry(string path, string name, string value, RegistryHive root = RegistryHive.CurrentUser)
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(root, RegistryView.Default).CreateSubKey(path))
            {
                if (rk != null)
                {
                    rk.SetValue(name, value, RegistryValueKind.String);
                }
            }
        }

        public static void CreateRegistry(string path, int value, RegistryHive root = RegistryHive.CurrentUser)
        {
            CreateRegistry(path, null, value, root);
        }

        public static void CreateRegistry(string path, string name, int value, RegistryHive root = RegistryHive.CurrentUser)
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(root, RegistryView.Default).CreateSubKey(path))
            {
                if (rk != null)
                {
                    rk.SetValue(name, value, RegistryValueKind.DWord);
                }
            }
        }

        public static void RemoveRegistry(string path, bool recursive = false, RegistryHive root = RegistryHive.CurrentUser)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (RegistryKey rk = RegistryKey.OpenBaseKey(root, RegistryView.Default))
                {
                    if (recursive)
                    {
                        rk.DeleteSubKeyTree(path, false);
                    }
                    else
                    {
                        rk.DeleteSubKey(path, false);
                    }
                }
            }
        }

        public static string GetRegistryValue(string path, string name = null, RegistryHive root = RegistryHive.CurrentUser)
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(root, RegistryView.Default).OpenSubKey(path))
            {
                if (rk != null)
                {
                    return rk.GetValue(name, null) as string;
                }
            }

            return null;
        }

        public static bool CheckRegistry(string path, string name = null, string value = null, RegistryHive root = RegistryHive.CurrentUser)
        {
            string registryValue = GetRegistryValue(path, name, root);

            return registryValue != null && (value == null || registryValue.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public static string SearchProgramPath(string fileName)
        {
            // First method: HKEY_CLASSES_ROOT\Applications\{filename}\shell\{command}\command

            string[] commands = new string[] { "open", "edit" };

            foreach (string command in commands)
            {
                string path = $@"HKEY_CLASSES_ROOT\Applications\{fileName}\shell\{command}\command";
                string value = Registry.GetValue(path, null, null) as string;

                if (!string.IsNullOrEmpty(value))
                {
                    string filePath = value.ParseQuoteString();

                    if (File.Exists(filePath))
                    {
                        DebugHelper.WriteLine("Found program with first method: " + filePath);
                        return filePath;
                    }
                }
            }

            // Second method: HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache

            using (RegistryKey programs = Registry.CurrentUser.OpenSubKey(@"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache"))
            {
                if (programs != null)
                {
                    foreach (string filePath in programs.GetValueNames())
                    {
                        string programPath = filePath;

                        if (!string.IsNullOrEmpty(programPath))
                        {
                            foreach (string trim in new string[] { ".ApplicationCompany", ".FriendlyAppName" })
                            {
                                if (programPath.EndsWith(trim, StringComparison.OrdinalIgnoreCase))
                                {
                                    programPath = programPath.Remove(programPath.Length - trim.Length);
                                }
                            }

                            if (programPath.EndsWith(fileName, StringComparison.OrdinalIgnoreCase) && File.Exists(programPath))
                            {
                                DebugHelper.WriteLine("Found program with second method: " + programPath);
                                return programPath;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}