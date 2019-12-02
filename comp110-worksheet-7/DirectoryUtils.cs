using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ImportantValues
{
	public int fileNo = new int();
	public int fileDepth = new int();
	public long[] allSizes = new long[0];
	public long fileSize = new long();
	public string[] fileDirectories = new string[0];
}
namespace comp110_worksheet_7
{

	public static class DirectoryUtils
	{
		private static ImportantValues valuesReference; 
		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
			valuesReference = new ImportantValues();
			void sizeRead() //reads the size of each file in a directory
			{
				string[] files = new string[0];
				files = Directory.GetFiles(directory); //Assigns each file in the directory to an array
				foreach (string file in files)
				{
					valuesReference.fileSize += new FileInfo(file).Length;

					valuesReference.fileDirectories[0] = valuesReference.fileDirectories[0] + file;
					valuesReference.allSizes[0] = valuesReference.allSizes[0] + valuesReference.fileSize;
					valuesReference.fileNo = valuesReference.fileNo + 1;
					if(IsDirectory(file) != true)
					{
						valuesReference.fileDepth = valuesReference.fileDepth + 1;
						Recursion(files[0], file);
					}
				}
			}
			void Recursion(string files, string file)

			{
				foreach (var elem in files)
					valuesReference.fileSize += GetFileSize(file);
					if(IsDirectory(file))
					{
						sizeRead();
					}
			}
			return valuesReference.fileSize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
			return valuesReference.fileNo;
		}

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
			return valuesReference.fileDepth;
		}

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
			return new Tuple<string, long>(valuesReference.fileDirectories.Min(), valuesReference.allSizes.Min());
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
		{
			return new Tuple<string, long>(valuesReference.fileDirectories.Max(), valuesReference.allSizes.Max());
		}

		// Get all files whose size is equal to the given value (in bytes) below the given directory
		public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
			if (valuesReference.allSizes.Contains(size))
			{
				return new IEnumerable<string>(valuesReference.fileDirectories);
			}

		}
	}
}