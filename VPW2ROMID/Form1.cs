using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPW2ROMID
{
	public partial class Form1 : Form
	{
		/// <summary>
		/// Internal checksum #1 values for unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly byte[] DefaultVPW2Checksum1 = { 0xCD, 0x09, 0x42, 0x35 };

		/// <summary>
		/// Internal checksum #2 values for unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly byte[] DefaultVPW2Checksum2 = { 0x88, 0x07, 0x4B, 0x62 };

		/// <summary>
		/// MD5 hash of an unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly string DefaultVPW2Hash_MD5 = "90002501777E3237739F5ED9B0E349E2";

		/// <summary>
		/// SHA-1 hash of an unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly string DefaultVPW2Hash_SHA1 = "82DD25A044689EAB57AB362FE10C0DA6388C217A";

		/// <summary>
		/// SHA-256 hash of an unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly string DefaultVPW2Hash_SHA256 = "358E9A345438155C6BD57DA4BBF0F7A9FA1B4F7D5B1B726E8076C38F0F987E52";

		/// <summary>
		/// SHA-512 hash of an unmodified Z64 format VPW2 ROM.
		/// </summary>
		private readonly string DefaultVPW2Hash_SHA512 = "55658D2B182CB68FAA27E494D7C130A43E0A45334B9E3BA1C817660578BE4413C2065DA5716EA63F0C4165D4A82D4E6E64175785E9525D553F4A0A799A5BD04B";

		/// <summary>
		/// Possible N64 ROM types
		/// </summary>
		public enum RomTypes
		{
			Z64 = 0, // big endian (native, most ideal)
			V64, // byte swapped
			LittleEndian, // little endian (least ideal)
			Invalid // unknown input, probably not a N64 ROM
		}

		/// <summary>
		/// Path to ROM file to verify.
		/// </summary>
		private string RomPath = string.Empty;

		/// <summary>
		/// FileStream used for opening the ROM file.
		/// </summary>
		FileStream fs;

		/// <summary>
		/// BinaryReader used for reading the ROM file.
		/// </summary>
		BinaryReader br;

		#region Test Results
		/// <summary>
		/// True if file extension is ".z64" when ROM path is lowercased.
		/// </summary>
		public bool HasCorrectFileExtension;

		/// <summary>
		/// ROM type based on the first four bytes.
		/// RomTypes.Z64 is ideal, other values are not.
		/// </summary>
		public RomTypes FirstFourBytesCategory;

		/// <summary>
		/// True if this is a Z64 format ROM.
		/// </summary>
		public bool FormatCheckPassed;

		/// <summary>
		/// True if game code is "NA2J".
		/// </summary>
		public bool HasCorrectGameCode;

		/// <summary>
		/// True if the first internal checksum matches the unmodified VPW2 values.
		/// </summary>
		public bool HasCorrectFirstChecksum;

		/// <summary>
		/// True if the second internal checksum matches the unmodified VPW2 values.
		/// </summary>
		public bool HasCorrectSecondChecksum;

		/// <summary>
		/// True if this ROM passes the MD5 hash check.
		/// </summary>
		public bool PassedHashCheck_MD5;

		/// <summary>
		/// True if this ROM passes the SHA-1 hash check.
		/// </summary>
		public bool PassedHashCheck_SHA1;

		/// <summary>
		/// True if this ROM passes the SHA-256 hash check.
		/// </summary>
		public bool PassedHashCheck_SHA256;

		/// <summary>
		/// True if this ROM passes the SHA-512 hash check.
		/// </summary>
		public bool PassedHashCheck_SHA512;

		/// <summary>
		/// Only true if most test results are positive.
		/// (HasCorrectFileExtension can be false as long as FormatCheckPassed is true)
		/// </summary>
		public bool CanBePatched;
		#endregion

		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Writing the same code for an error box sucks, so make a wrapper.
		/// </summary>
		/// <param name="msg">Message to show</param>
		private void ErrorBox(string msg)
		{
			MessageBox.Show(msg, "VPW2 ROM ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// rom file browser
		/// </summary>
		private void btnBrowseRom_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select Virtual Pro-Wrestling 2 ROM File";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				RomPath = Path.GetFullPath(ofd.FileName);
				tbRomFilePath.Text = RomPath;
			}
		}

		/// <summary>
		/// The heavy lifting.
		/// </summary>
		private void btnVerifyRom_Click(object sender, EventArgs e)
		{
			tbOutput.Clear();

			// let's be optimistic by default; there will be time to crush your hopes and dreams later.
			CanBePatched = true;

			// empty string? we can't verify anything!
			if (tbRomFilePath.Text.Equals(string.Empty))
			{
				ErrorBox("No file has been selected, and therefore, this can't be VPW2. Or anything, really.\n\nPlease click the Browse button and select a ROM file.");
				CanBePatched = false;
				return;
			}

			// when the user only modifies the text box instead of hitting "browse"...
			if (RomPath.Equals(string.Empty))
			{
				RomPath = tbRomFilePath.Text;
			}

			// ...and if they expect the file to be in the same directory as the program...
			if (!Path.IsPathRooted(RomPath))
			{
				// add the executable directory to the path.
				RomPath = string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), tbRomFilePath.Text);
			}

			// determine if the file actually exists, which is kind of important for the rest of the steps
			if (!File.Exists(RomPath))
			{
				ErrorBox(string.Format("The file '{0}' does not exist.\nTherefore, it can't be a VPW2 ROM, or anything else.\n\nPlease click the Browse button and select a ROM file.",RomPath));
				CanBePatched = false;
				return;
			}

			// check the file extension, which may be misleading
			HasCorrectFileExtension = VerifyCorrectFileExtension();
			if (HasCorrectFileExtension)
			{
				tbOutput.Text += "ROM has .z64 extension; this does not mean the ROM is necessarily Z64 format, though.\r\n";
			}
			else
			{
				tbOutput.Text += string.Format("ROM does not have .z64 extension; found extension '{0}'\r\n", Path.GetExtension(RomPath));
			}

			// load the ROM
			fs = new FileStream(RomPath, FileMode.Open);
			br = new BinaryReader(fs);

			// check the first four bytes for what we ACTUALLY have
			FirstFourBytesCategory = VerifyFirstFourBytes(out byte[] firstFour);

			// show the first four bytes in the output for debugging purposes
			tbOutput.Text += string.Format("First four bytes: {0:X2} {1:X2} {2:X2} {3:X2}\r\n", firstFour[0], firstFour[1], firstFour[2], firstFour[3]);

			#region ok, smartass (invalid file checks that match "known" formats)
			// .zip file
			if (firstFour[0] == 'P' && firstFour[1] == 'K')
			{
				ErrorBox("You will need to unzip this file in order to check it. I'm not writing a bunch of code to decompress zip files when you can just extract the file yourself.");
				br.Close();
				CanBePatched = false;
				return;
			}

			// .7z file
			if (firstFour[0] == '7' && firstFour[1] == 'z')
			{
				ErrorBox("You will need to un-7zip this file in order to check it. I'm not writing a bunch of code to decompress 7zip files when you can just extract the file yourself.");
				br.Close();
				CanBePatched = false;
				return;
			}

			// Windows .exe file
			if (firstFour[0] == 'M' && firstFour[1] == 'Z')
			{
				ErrorBox("Windows Portable Executable format does not run on Nintendo 64, no matter how hard you try.");
				br.Close();
				CanBePatched = false;
				return;
			}

			// Commodore 64 .prg file
			if (firstFour[0] == 0x01 && firstFour[1] == 0x08)
			{
				ErrorBox("You appear to have mixed up Nintendo and Commodore. Yes, both have 64, but this program only cares about the former.");
				br.Close();
				CanBePatched = false;
				return;
			}
			#endregion

			// the results vary based on HasCorrectFileExtension and FirstFourBytesCategory.
			if (FirstFourBytesCategory == RomTypes.Invalid)
			{
				// this is not a N64 ROM, we can stop here.
				ErrorBox("This does not appear to be a Nintendo 64 ROM of any known type. Therefore, it can't be VPW2.");
				br.Close();
				CanBePatched = false;
				return;
			}

			// everything else has to be checked.
			// I expect that MOST issues will come up with people renaming the file extension
			// while not actually converting the data, so it's important to cover ALL POSSIBLE BASES.
			if (FirstFourBytesCategory == RomTypes.Z64)
			{
				if (HasCorrectFileExtension)
				{
					// claims to be a Z64 ROM and is.
					tbOutput.Text += "This file claims to be a Z64 ROM, and it is!\r\n";
				}
				else
				{
					// this is a Z64 ROM masquerading as something else!
					tbOutput.Text += string.Format("This file has an extension of '{0}', but it's actually a Z64 ROM! Please change the file extension to '.z64' when you get a chance.\r\n", Path.GetExtension(RomPath));
				}
				FormatCheckPassed = true;
			}
			else if (FirstFourBytesCategory == RomTypes.V64)
			{
				if (HasCorrectFileExtension)
				{
					// claims to be a Z64 ROM, but is a V64 ROM
					tbOutput.Text += "This file claims to be a Z64 ROM, but it is a V64 ROM. You must convert it to Z64 format for patching and editing.\r\n";
				}
				else
				{
					// this never claimed to be a Z64 ROM, but is not what we need
					tbOutput.Text += "This file is a V64 ROM. You must convert it to Z64 format for patching and editing.\r\n";
				}
				FormatCheckPassed = false;
			}
			else if (FirstFourBytesCategory == RomTypes.LittleEndian)
			{
				if (HasCorrectFileExtension)
				{
					// claims to be a Z64 ROM, but is little endian
					tbOutput.Text += "This file claims to be a Z64 ROM, but it is in little endian format. You must convert it to Z64 format for patching and editing.\r\n";
				}
				else
				{
					// this never claimed to be a Z64 ROM, but is not what we need
					tbOutput.Text += "This file is a little endian ROM. You must convert it to Z64 format for patching and editing.\r\n";
				}
				FormatCheckPassed = false;
			}

			if (!FormatCheckPassed)
			{
				ErrorBox("This ROM did not pass the format check. Please see the Output box for further steps.");
				br.Close();
				CanBePatched = false;
				return;
			}

			// we have a Z64 format ROM.
			// it may not be VPW2, but at least we know it's the right format.

			HasCorrectGameCode = VerifyGameCode(out char[] gameCode);
			if (HasCorrectGameCode)
			{
				tbOutput.Text += "This file has the expected game code ('NA2J').\r\n";
			}
			else
			{
				if (gameCode[0] == 'N' && gameCode[1] == 'A' && gameCode[2] == '2')
				{
					// we have something that's likely VPW2, but not the original Japanese ROM.
					char lastChar = gameCode[3];
					tbOutput.Text += string.Format("This file has most of the expected game code ('NA2'), but the final character is '{0}'.\r\n", lastChar);
					if (lastChar == 'E')
					{
						tbOutput.Text += "Since the last character is 'E', this may have already been patched with an English translation.\r\n";
						tbOutput.Text += "Most patches for VPW2 target the unmodified original Japanese ROM.\r\n";
					}
					CanBePatched = false;
				}
				else if (gameCode[0] == 'N' && gameCode[1] == 'V' && gameCode[2] == 'F')
				{
					// "NVF*" implies VPW2 freem Edition, which is not designed to be patched onto itself.
					// this immediately fails the check.
					tbOutput.Text += "This file's game code starts with 'NVF', which highly implies Virtual Pro-Wrestling 2 freem Edition.\r\n";
					tbOutput.Text += "VPW2 freem Edition is not intended to be patched onto itself. It only works if you use an unmodified original Japanese VPW2 ROM.\r\n";
					CanBePatched = false;
				}
				else
				{
					// does not appear to be VPW2
					tbOutput.Text += string.Format("This ROM has a game code of '{0}{1}{2}{3}', which does not match Virtual Pro-Wrestling 2.\r\n", gameCode[0], gameCode[1], gameCode[2], gameCode[3]);
					CanBePatched = false;
				}
			}

			if (!CanBePatched)
			{
				ErrorBox("This ROM did not pass the game code check. Please see the Output box for details.");
				br.Close();
				return;
			}

			HasCorrectFirstChecksum = VerifyInternalChecksum1(out byte[] checksum1);
			HasCorrectSecondChecksum = VerifyInternalChecksum2(out byte[] checksum2);

			if (HasCorrectFirstChecksum)
			{
				tbOutput.Text += "First internal checksum matches the expected values for an unmodified Z64 format VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += string.Format("First internal checksum is {0:X2} {1:X2} {2:X2} {3:X2}. This is possibly a modified VPW2 ROM.\r\n", checksum1[0], checksum1[1], checksum1[2], checksum1[3]);
				CanBePatched = false;
			}

			if (HasCorrectSecondChecksum)
			{
				tbOutput.Text += "Second internal checksum matches the expected values for an unmodified Z64 format VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += string.Format("Second internal checksum is {0:X2} {1:X2} {2:X2} {3:X2}. This is possibly a modified VPW2 ROM.", checksum2[0], checksum2[1], checksum2[2], checksum2[3]);
				CanBePatched = false;
			}

			if (!CanBePatched)
			{
				ErrorBox("This ROM did not pass the internal checksum test.");
				br.Close();
				return;
			}

			// if the verification reaches this stage, it's very likely we have an unmodified original VPW2 ROM.
			// however, there is still the possibility that the rest of the data in the ROM could be different
			// from the original.

			// the final verification stages involve getting the hash values for the ROM data.
			PassedHashCheck_MD5 = VerifyHash_MD5(out byte[] inputRomHashMD5);
			if (PassedHashCheck_MD5)
			{
				tbOutput.Text += "The MD5 hash of the input ROM matches the unmodified VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += "MD5 hash of this rom does NOT match an unmodified VPW2 ROM.\r\n";
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashMD5), DefaultVPW2Hash_MD5);
				CanBePatched = false;
			}

			PassedHashCheck_SHA1 = VerifyHash_SHA1(out byte[] inputRomHashSHA1);
			if (PassedHashCheck_SHA1)
			{
				tbOutput.Text += "The SHA-1 hash of the input ROM matches the unmodified VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += "SHA-1 hash of this rom does NOT match an unmodified VPW2 ROM.\r\n";
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA1), DefaultVPW2Hash_SHA1);
				CanBePatched = false;
			}

			PassedHashCheck_SHA256 = VerifyHash_SHA256(out byte[] inputRomHashSHA256);
			if (PassedHashCheck_SHA256)
			{
				tbOutput.Text += "The SHA-256 hash of the input ROM matches the unmodified VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += "SHA-256 hash of this rom does NOT match an unmodified VPW2 ROM.\r\n";
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA256), DefaultVPW2Hash_SHA256);
				CanBePatched = false;
			}

			PassedHashCheck_SHA512 = VerifyHash_SHA512(out byte[] inputRomHashSHA512);
			if (PassedHashCheck_SHA512)
			{
				tbOutput.Text += "The SHA-512 hash of the input ROM matches the unmodified VPW2 ROM.\r\n";
			}
			else
			{
				tbOutput.Text += "SHA-512 hash of this rom does NOT match an unmodified VPW2 ROM.\r\n";
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA512), DefaultVPW2Hash_SHA512);
				CanBePatched = false;
			}
			br.Close();

			if (!CanBePatched)
			{
				ErrorBox("One or more of the file hash integrity checks has failed. Please see the Output box for details.");
			}
			else
			{
				MessageBox.Show("Congratulations! This appears to be an unmodified Virtual Pro-Wrestling 2 ROM in Z64 format, ready to be patched or edited.", "VPW2 ROM ID", MessageBoxButtons.OK);
			}
		}

		#region Verification Functions
		/// <summary>
		/// Verifies if the file extension of the ROM is ".z64".
		/// </summary>
		/// <returns>True if the file has an extension of ".z64", false otherwise.</returns>
		protected bool VerifyCorrectFileExtension()
		{
			// we lowercase in case someone is all "I_HAVE_ROMS.Z64";
			// if they get an error just because of the extension case, that would be bullshit.
			return Path.GetExtension(RomPath).ToLower().Equals(".z64");
		}

		/// <summary>
		/// Verifies the actual ROM format based on the first four bytes of the ROM.
		/// </summary>
		/// <param name="firstFour">The first four bytes of the ROM file.</param>
		/// <returns>A RomTypes value representing the ROM type.</returns>
		protected RomTypes VerifyFirstFourBytes(out byte[] firstFour)
		{
			br.BaseStream.Seek(0, SeekOrigin.Begin);
			firstFour = br.ReadBytes(4);

			// Z64 ROM: 80 37 12 40
			if (firstFour[0] == 0x80 && firstFour[1] == 0x37 && firstFour[2] == 0x12 && firstFour[3] == 0x40)
			{
				return RomTypes.Z64;
			}

			// V64 ROM: 37 80 40 12
			else if (firstFour[0] == 0x37 && firstFour[1] == 0x80 && firstFour[2] == 0x40 && firstFour[3] == 0x12)
			{
				return RomTypes.V64;
			}

			// little endian/least ideal format: 40 12 37 80
			else if (firstFour[0] == 0x40 && firstFour[1] == 0x12 && firstFour[2] == 0x37 && firstFour[3] == 0x80)
			{
				return RomTypes.LittleEndian;
			}

			// if we get here, it's probably not a N64 ROM
			return RomTypes.Invalid;
		}

		/// <summary>
		/// Verify the first internal checksum at file offset 0x10.
		/// </summary>
		/// <param name="checksum1">Values for the first internal checksum.</param>
		/// <returns>True if the first internal checksum matches the unmodified original VPW2 ROM's.</returns>
		protected bool VerifyInternalChecksum1(out byte[] checksum1)
		{
			br.BaseStream.Seek(0x10, SeekOrigin.Begin);
			checksum1 = br.ReadBytes(4);
			return (checksum1[0] == DefaultVPW2Checksum1[0] && checksum1[1] == DefaultVPW2Checksum1[1] && checksum1[2] == DefaultVPW2Checksum1[2] && checksum1[3] == DefaultVPW2Checksum1[3]);
		}

		/// <summary>
		/// Verify the second internal checksum at file offset 0x14.
		/// </summary>
		/// <param name="checksum2">Values for the second internal checksum.</param>
		/// <returns>True if the second internal checksum matches the unmodified original VPW2 ROM's.</returns>
		protected bool VerifyInternalChecksum2(out byte[] checksum2)
		{
			br.BaseStream.Seek(0x14, SeekOrigin.Begin);
			checksum2 = br.ReadBytes(4);
			return (checksum2[0] == DefaultVPW2Checksum2[0] && checksum2[1] == DefaultVPW2Checksum2[1] && checksum2[2] == DefaultVPW2Checksum2[2] && checksum2[3] == DefaultVPW2Checksum2[3]);
		}

		/// <summary>
		/// Check for "NA2J" (Virtual Pro-Wrestling 2's game code) at offset 0x3B.
		/// </summary>
		/// <param name="gameCode">The four characters making up the game code.</param>
		/// <returns>True if this is "NA2J", false otherwise.</returns>
		protected bool VerifyGameCode(out char[] gameCode)
		{
			br.BaseStream.Seek(0x3B, SeekOrigin.Begin);
			gameCode = br.ReadChars(4);

			if (gameCode[0] == 'N' && gameCode[1] == 'A' && gameCode[2] == '2' && gameCode[3] == 'J')
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Convert a hash to a string for easy display.
		/// Technically works for any set of input bytes, but I am only using it to display hashes.
		/// </summary>
		/// <param name="inputHash">Input hash value to convert.</param>
		/// <returns>Hex string representing the input hash.</returns>
		protected string HashToString(byte[] inputHash)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < inputHash.Length; i++)
			{
				sb.Append(inputHash[i].ToString("X2"));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Verify MD5 hash of VPW2 ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_MD5(out byte[] inputRomHash)
		{
			MD5 md5Hasher = MD5.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = md5Hasher.ComputeHash(fs);
			md5Hasher.Dispose();

			return HashToString(inputRomHash).Equals(DefaultVPW2Hash_MD5);
		}

		/// <summary>
		/// Verify SHA-1 hash of VPW2 ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_SHA1(out byte[] inputRomHash)
		{
			SHA1 sha1Hasher = SHA1.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha1Hasher.ComputeHash(fs);
			sha1Hasher.Dispose();

			return HashToString(inputRomHash).Equals(DefaultVPW2Hash_SHA1);
		}

		/// <summary>
		/// Verify SHA-256 hash of VPW2 ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_SHA256(out byte[] inputRomHash)
		{
			SHA256 sha256Hasher = SHA256.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha256Hasher.ComputeHash(fs);
			sha256Hasher.Dispose();

			return HashToString(inputRomHash).Equals(DefaultVPW2Hash_SHA256);
		}

		/// <summary>
		/// Verify SHA-512 hash of VPW2 ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_SHA512(out byte[] inputRomHash)
		{
			SHA512 sha512Hasher = SHA512.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha512Hasher.ComputeHash(fs);
			sha512Hasher.Dispose();

			return HashToString(inputRomHash).Equals(DefaultVPW2Hash_SHA512);
		}

		#endregion
	}
}
