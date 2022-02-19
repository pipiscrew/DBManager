using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Jurassic
{
    public static class CompressorDeflate
    {
        public static byte[] Compress(byte[] bytes)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                {
                    deflateStream.Write(bytes, 0, bytes.Length);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static byte[] Decompress(byte[] bytes)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream memoryStream2 = new MemoryStream(bytes))
                {
                    using (DeflateStream deflateStream = new DeflateStream(memoryStream2, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(memoryStream);
                    }
                }
                result = memoryStream.ToArray();
            }
            return result;
        }
    }

    public static class CompressorGZip
    {
        public static byte[] Compress(byte[] bytes)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static void Compress(Stream src, Stream dest)
        {
            if (src == null || dest == null)
            {
                return;
            }
            using (GZipStream gZipStream = new GZipStream(dest, CompressionMode.Compress))
            {
                src.CopyTo(gZipStream);
            }
        }

        public static void Compress(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(destPath))
            {
                return;
            }
            using (Stream stream = File.OpenRead(srcPath))
            {
                using (Stream stream2 = File.OpenWrite(destPath))
                {
                    Compress(stream, stream2);
                }
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream memoryStream2 = new MemoryStream(bytes))
                {
                    using (GZipStream gZipStream = new GZipStream(memoryStream2, CompressionMode.Decompress))
                    {
                        gZipStream.CopyTo(memoryStream);
                    }
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static void Decompress(Stream src, Stream dest)
        {
            if (src == null || dest == null)
            {
                return;
            }
            using (GZipStream gZipStream = new GZipStream(src, CompressionMode.Decompress))
            {
                gZipStream.CopyTo(dest);
            }
        }

        public static void Decompress(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(destPath))
            {
                return;
            }
            using (Stream stream = File.OpenRead(srcPath))
            {
                using (Stream stream2 = File.OpenWrite(destPath))
                {
                    Decompress(stream, stream2);
                }
            }
        }
    }

    public static class UnixTimeStamp
    {
        public static DateTime ToDateTime(long unixTimeStamp)
        {
            return UnixTimeStamp.zeroDate.AddSeconds((double)unixTimeStamp).ToLocalTime();
        }

        public static double ToUnixTimeStamp(DateTime dateTime)
        {
            return (dateTime - UnixTimeStamp.zeroDate.ToLocalTime()).TotalSeconds;
        }

        private static readonly DateTime zeroDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    }

    public static class Json
    {
        public static T FromBytes<T>(byte[] bytes)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            T result;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                result = (T)((object)dataContractJsonSerializer.ReadObject(memoryStream));
            }
            return result;
        }

        public static T FromString<T>(string json)
        {
            return Json.FromBytes<T>(Encoding.UTF8.GetBytes(json));
        }

        public static byte[] ToBytes(object data)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new DataContractJsonSerializer(data.GetType()).WriteObject(memoryStream, data);
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static string ToString(object data)
        {
            byte[] bytes = Json.ToBytes(data);
            return Encoding.UTF8.GetString(bytes);
        }

        public static T TryFromString<T>(string json)
        {
            T result;
            if (string.IsNullOrEmpty(json))
            {
                result = default(T);
                return result;
            }
            try
            {
                result = Json.FromString<T>(json);
            }
            catch
            {
                result = default(T);
            }
            return result;
        }
    }

    public static class EnumStringConverter
    {
        public static TEnum FromString<TEnum>(string type) where TEnum : struct, IConvertible
        {
            TEnum result = default(TEnum);
            if (typeof(TEnum).IsEnum && !string.IsNullOrEmpty(type))
            {
                Enum.TryParse<TEnum>(type.Trim().ToUpper(), false, out result);
            }
            return result;
        }

        public static string ToString<TEnum>(TEnum type) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                type = default(TEnum);
            }
            return type.ToString().ToLower();
        }
    }

    public static class FileHelper
    {

        public static void CopyDirectory(string srcDir, string destDir, bool overwrite)
        {
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            string[] array = Directory.GetDirectories(srcDir, "*", SearchOption.AllDirectories);
            for (int i = 0; i < array.Length; i++)
            {
                Directory.CreateDirectory(array[i].Replace(srcDir, destDir));
            }
            array = Directory.GetFiles(srcDir, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < array.Length; i++)
            {
                string expr_4E = array[i];
                File.Copy(expr_4E, expr_4E.Replace(srcDir, destDir), overwrite);
            }
        }

        public static long GetFreeDiskSpace(string driveLetter)
        {
            if (string.IsNullOrEmpty(driveLetter))
            {
                return -1L;
            }
            try
            {
                DriveInfo driveInfo = new DriveInfo(driveLetter);
                if (driveInfo.IsReady)
                {
                    return driveInfo.AvailableFreeSpace;
                }
            }
            catch
            {
            }
            return -1L;
        }

        public static string FormatBytes(long size)
        {
            double num = (double)size;
            string arg = "b";
            if (num > 1024.0)
            {
                num /= 1024.0;
                arg = "KB";
            }
            if (num > 1024.0)
            {
                num /= 1024.0;
                arg = "MB";
            }
            if (num > 1024.0)
            {
                num /= 1024.0;
                arg = "GB";
            }
            return string.Format("{0:###0.##} {1}", num, arg);
        }
		
        public static string FormatBytes(long size)
        { // https://www.c-sharpcorner.com/article/csharp-convert-bytes-to-kb-mb-gb/
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

            int counter = 0;
            decimal number = size;

            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }

            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    }

    public static class MD5Helper
    {
        public static byte[] ComputeMD5HashBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return new byte[0];
            }
            byte[] result;
            using (MD5 mD = MD5.Create())
            {
                result = mD.ComputeHash(bytes);
            }
            return result;
        }

        public static byte[] ComputeMD5HashBytes(Stream stream)
        {
            if (stream == null)
            {
                return new byte[0];
            }
            byte[] result;
            using (MD5 mD = MD5.Create())
            {
                result = mD.ComputeHash(stream);
            }
            return result;
        }

        public static string ComputeMD5HashString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }
            return MD5Helper.ToHashString(MD5Helper.ComputeMD5HashBytes(bytes));
        }

        public static string ComputeMD5HashString(Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }
            return MD5Helper.ToHashString(MD5Helper.ComputeMD5HashBytes(stream));
        }

        public static string ComputeMD5HashString(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return string.Empty;
            }
            string result;
            using (Stream stream = File.OpenRead(filePath))
            {
                result = MD5Helper.ComputeMD5HashString(stream);
            }
            return result;
        }

        public static string ToHashString(byte[] hash)
        {
            if (hash == null || hash.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }

    public static class AsymmetricEncryption
    {
        public static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "data");
            }
            if (!AsymmetricEncryption.IsKeySizeValid(keySize))
            {
                throw new ArgumentException("Key size is not valid", "keySize");
            }
            if (string.IsNullOrEmpty(publicAndPrivateKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");
            }
            byte[] result;
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(keySize))
            {
                rSACryptoServiceProvider.FromXmlString(publicAndPrivateKeyXml);
                result = rSACryptoServiceProvider.Decrypt(data, AsymmetricEncryption.optimalPadding);
            }
            return result;
        }

        public static string DecryptText(string text, Encoding encoding, int keySize, string publicAndPrivateKeyXml)
        {
            byte[] bytes = AsymmetricEncryption.Decrypt(Convert.FromBase64String(text), keySize, publicAndPrivateKeyXml);
            return encoding.GetString(bytes);
        }

        public static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "data");
            }
            int maxDataLength = AsymmetricEncryption.GetMaxDataLength(keySize);
            if (data.Length > maxDataLength)
            {
                throw new ArgumentException(string.Format("Maximum data length is {0}", maxDataLength), "data");
            }
            if (!AsymmetricEncryption.IsKeySizeValid(keySize))
            {
                throw new ArgumentException("Key size is not valid", "keySize");
            }
            if (string.IsNullOrEmpty(publicKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "publicKeyXml");
            }
            byte[] result;
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(keySize))
            {
                rSACryptoServiceProvider.FromXmlString(publicKeyXml);
                result = rSACryptoServiceProvider.Encrypt(data, AsymmetricEncryption.optimalPadding);
            }
            return result;
        }

        public static string EncryptText(string text, Encoding encoding, int keySize, string publicKeyXml)
        {
            return Convert.ToBase64String(AsymmetricEncryption.Encrypt(encoding.GetBytes(text), keySize, publicKeyXml));
        }

        public static void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(keySize))
            {
                publicKey = rSACryptoServiceProvider.ToXmlString(false);
                publicAndPrivateKey = rSACryptoServiceProvider.ToXmlString(true);
            }
        }

        public static int GetMaxDataLength(int keySize)
        {
            int num = (keySize - 384) / 8 + 7;
            if (!AsymmetricEncryption.optimalPadding)
            {
                return num + 30;
            }
            return num;
        }

        public static bool IsKeySizeValid(int keySize)
        {
            return keySize >= 384 && keySize <= 16384 && keySize % 8 == 0;
        }

        private static bool optimalPadding;
    }

    public static class SymmetricEncryption
    {
        public static byte[] Decrypt(SymmetricAlgorithm algorithm, byte[] cryptedData)
        {
            return SymmetricEncryption.encryptDecrypt(algorithm, cryptedData, new Func<ICryptoTransform>(algorithm.CreateDecryptor));
        }

        public static string Decrypt(SymmetricAlgorithm algorithm, string cryptedText, Encoding encoding)
        {
            byte[] cryptedData = Convert.FromBase64String(cryptedText);
            byte[] bytes = SymmetricEncryption.Decrypt(algorithm, cryptedData);
            return encoding.GetString(bytes);
        }

        public static byte[] Encrypt(SymmetricAlgorithm algorithm, byte[] data)
        {
            return SymmetricEncryption.encryptDecrypt(algorithm, data, new Func<ICryptoTransform>(algorithm.CreateEncryptor));
        }

        public static string Encrypt(SymmetricAlgorithm algorithm, string text, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(text);
            return Convert.ToBase64String(SymmetricEncryption.Encrypt(algorithm, bytes));
        }

        private static byte[] encryptDecrypt(SymmetricAlgorithm alg, byte[] bytes, Func<ICryptoTransform> encryptDecrypt)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptDecrypt(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.Flush();
                }
                result = memoryStream.ToArray();
            }
            return result;
        }
    }

    public static class Shuffler
    {
        public static void Shuffle<T>(T[] array)
        {
            int i = array.Length;
            while (i > 1)
            {
                int num = Shuffler.rnd.Next(i--);
                T t = array[i];
                array[i] = array[num];
                array[num] = t;
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            int i = list.Count;
            while (i > 1)
            {
                int index = Shuffler.rnd.Next(i--);
                T value = list[i];
                list[i] = list[index];
                list[index] = value;
            }
        }

        private static Random rnd = new Random();
    }

    public static class WinAPI
    {
        public static string GetWindowsVersionName()
        {
            string result;
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"))
                {
                    result = (string)registryKey.GetValue("ProductName");
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool Is64BitSystem()
        {
            string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string environmentVariable2 = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
            return (environmentVariable == "x86" && environmentVariable2 == "AMD64") || environmentVariable == "AMD64";
        }

        public static bool IsInternetConnectionAvailable()
        {
            int num;
            return WinAPI.InternetGetConnectedState(out num, 0);
        }

        public static void StartDefaultWebBrowser(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "rundll32",
                Arguments = "url.dll,FileProtocolHandler " + url,
                UseShellExecute = true
            });
        }
    }

    public class WinPermission
    {
        public static bool HasFolderWritePermission(string destDir)
        {
            if (string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir))
            {
                return false;
            }
            bool result;
            try
            {
                CommonObjectSecurity arg_2E_0 = Directory.GetAccessControl(destDir);
                SecurityIdentifier right = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                foreach (AuthorizationRule authorizationRule in arg_2E_0.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (authorizationRule.IdentityReference == right)
                    {
                        FileSystemAccessRule fileSystemAccessRule = (FileSystemAccessRule)authorizationRule;
                        if (fileSystemAccessRule.AccessControlType == AccessControlType.Allow && fileSystemAccessRule.FileSystemRights == (fileSystemAccessRule.FileSystemRights | FileSystemRights.Modify))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                result = false;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsInRole(WindowsBuiltInRole role)
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(role);
        }

        public static bool SetFolderWritePermission(string destDir)
        {
            if (WinPermission.IsAdmin)
            {
                return false;
            }
            if (string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir))
            {
                return false;
            }
            bool result;
            try
            {
                DirectorySecurity accessControl = Directory.GetAccessControl(destDir);
                SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                accessControl.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                Directory.SetAccessControl(destDir, accessControl);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsAdmin
        {
            get
            {
                return WinPermission.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }

    public static class XmlHelper
    {
        public static string escapeEntities(string txt)
        {
            if (txt == null)
            {
                return txt;
            }
            txt = txt.Replace("&", "&amp;");
            txt = txt.Replace(">", "&gt;");
            txt = txt.Replace("<", "&lt;");
            txt = txt.Replace("\"", "&quot;");
            txt = txt.Replace("&amp;#", "&#");
            return txt;
        }

        public static bool isCorrectArgName(string txt)
        {
            return txt.IndexOfAny(XmlHelper.incorrectChars) < 0;
        }

        public static string removeTAGs(string txt)
        {
            return Regex.Replace(txt, "<[^<]+?>", string.Empty);
        }

        public static string stripEntities(string txt)
        {
            return Regex.Replace(txt, "&[^;]+;", " ");
        }

        public static string stripHtml(string txt)
        {
            return XmlHelper.stripHtml(txt, string.Empty);
        }

        public static string stripHtml(string txt, string replacement)
        {
            return Regex.Replace(txt, "<[^>]+>", replacement);
        }

        public static void ToXml(XmlTextWriter xml, string name, string val)
        {
            if (val != null && val.Length > 0)
            {
                xml.WriteElementString(name, val);
            }
        }

        public static void ToXml(XmlTextWriter xml, string name, DateTime val)
        {
            XmlHelper.ToXml(xml, name, XmlConvert.ToString(val, "yyyy-MM-dd"));
        }

        public static void ToXmlFromHtml(XmlTextWriter xml, string name, string val)
        {
            if (val != null && val.Length > 0)
            {
                xml.WriteStartElement(name);
                xml.WriteRaw(val);
                xml.WriteEndElement();
            }
        }

        public static string unescapeEntities(string txt)
        {
            if (txt == null)
            {
                return txt;
            }
            txt = txt.Replace("&amp;", "&");
            txt = txt.Replace("&gt;", ">");
            txt = txt.Replace("&lt;", "<");
            txt = txt.Replace("&quot;", "\"");
            return txt;
        }

        private static char[] incorrectChars = new char[]
		{
			'>',
			'<',
			'&',
			'"',
			'&'
		};

        public const string XML_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static readonly string[] XML_DATETIME_FORMATS = new string[]
		{
			"yyyy-MM-dd HH:mm:ss",
			"yyyy-MM-dd"
		};

        public const string XML_DATE_FORMAT = "yyyy-MM-dd";

        public const string XML_DEFINITION = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
    }
	
    public class StopwatchProxy
    {
		/*  src - https://stackoverflow.com/a/10669397
			StopwatchProxy.Instance.Stopwatch.Start();
			.
			StopwatchProxy.Instance.Stopwatch.Stop();
			this.Text = StopwatchProxy.GetResult();
		*/
        private Stopwatch _stopwatch;
        private static readonly StopwatchProxy _stopwatchProxy = new StopwatchProxy();

        private StopwatchProxy()
        {
            _stopwatch = new Stopwatch();
        }

        public Stopwatch Stopwatch { get { return _stopwatch; } }

        public static StopwatchProxy Instance
        {
            get { return _stopwatchProxy; }
        }

        public static string GetResult(){
            string g =  StopwatchProxy.Instance.Stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
            StopwatchProxy.Instance.Stopwatch.Reset();
            return g;
        }
    }
	
    public class Reflect
    {	//src - https://stackoverflow.com/a/39132579
		/* TODO : Optimize w/ LINQ */
        public static void SetPrivatePropertyValue<T>(T obj, string propertyName, object newValue)
        { 
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField;
            FieldInfo field = obj.GetType().GetField(propertyName, flags);

            if (field != null)
                field.SetValue(obj, newValue);
            else
                throw new Exception("!!");

            return;
            // add a check here that the object obj and propertyName string are not null
            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (fi.Name.ToLower().Contains(propertyName.ToLower()))
                {
                    fi.SetValue(obj, newValue);
                    break;
                }
            }
        }

        public static void SetBasePrivatePropertyValue<T>(T obj, string propertyName, object newValue)
        {  /* when using inheritance access BASE private field */
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = obj.GetType().BaseType.GetField(propertyName, flags);

            if (field != null)
                field.SetValue(obj, newValue);
            else
                throw new Exception("!!");

            return;
            // add a check here that the object obj and propertyName string are not null
            foreach (FieldInfo fi in obj.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (fi.Name.ToLower().Contains(propertyName.ToLower()))
                {
                    fi.SetValue(obj, newValue);
                    break;
                }
            }
        }

        public static object GetBasePrivatePropertyValue<T>(T obj, string propertyName)
        { 
            //Manaual - https://jike.in/?qa=1114048/c%23-use-reflection-to-get-a-private-member-variable-from-a-derived-class
            //BASE https://stackoverflow.com/a/6961970

            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = obj.GetType().BaseType.GetField(propertyName, flags);

            if (field != null)
                return field.GetValue(obj);
            else
                throw new Exception("!!");
            
            return null;
            // add a check here that the object obj and propertyName string are not null
            foreach (FieldInfo fi in obj.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                Console.WriteLine(fi.Name);
                if (fi.Name.ToLower().Contains(propertyName.ToLower()))
                {
                    return fi.GetValue(obj);
                }
            }

            return null;
        }
	}
}
