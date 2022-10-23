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
	public struct ValidValues
	{
		/// <summary>
		/// Internal checksum value #1 (file offset 0x10)
		/// </summary>
		public byte[] Checksum1;

		/// <summary>
		/// Internal checksum value #2 (file offset 0x14)
		/// </summary>
		public byte[] Checksum2;

		/// <summary>
		/// MD5 hash of full ROM data
		/// </summary>
		public string HashMD5;

		/// <summary>
		/// SHA-1 hash of full ROM data
		/// </summary>
		public string HashSHA1;

		/// <summary>
		/// SHA-256 hash of full ROM data
		/// </summary>
		public string HashSHA256;

		/// <summary>
		/// SHA-512 hash of full ROM data
		/// not really *needed*, but if we're going the whole way...
		/// </summary>
		public string HashSHA512;

		/// <summary>
		/// Game product code, e.g. "NA2J"
		/// </summary>
		public string ProductCode;

		/// <summary>
		/// Mask ROM revision number; usually 0, but sometimes not
		/// </summary>
		public byte MaskRevision;
	};

	public partial class Form1 : Form
	{
		public Dictionary<string, ValidValues> GameValues = new Dictionary<string, ValidValues>()
		{
			{
				"WCW vs. nWo - World Tour (NTSC-U v1.0)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x2C, 0x3E, 0x19, 0xBD },
					Checksum2 = new byte[]{ 0x51, 0x13, 0xEE, 0x5E },
					HashMD5 = "203C3BBFDD10C5A0B7C5D0CDB085D853",
					HashSHA1 = "5AD2D8359058C8BB71F08E3D3433B7A50D3BB645",
					HashSHA256 = "852DC6478E55C13DA60080C1ED468ECF6AA2F370C1BB8BA16720622FA931763A",
					HashSHA512 = "4B2F7154E3ACFB5C5D0939BED500DF4063611CE8479165FC45B3AD9E175D61535710982A1F2E42CE67866B18E93E1DF2E36E59903B35BBF7C006AFE094746C69",
					ProductCode = "NWNE",
					MaskRevision = 0
				}
			},
			{
				"WCW vs. nWo - World Tour (PAL)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x8B, 0xDB, 0xAF, 0x68 },
					Checksum2 = new byte[]{ 0x34, 0x5B, 0x4B, 0x36 },
					HashMD5 = "553D8D5347969C66E5D91C3FE35208B9",
					HashSHA1 = "840B8B6303ACACEB49619EB1533CB4221CDEC474",
					HashSHA256 = "177B179B78C86B97AC5EE21D7EED543DDD1A1A71A41D74ADCB4C7BBDFFF5F479",
					HashSHA512 = "F4A972A4CF5821E9419ADBC05D029D7DD6C2B2542EFDAB52E950BD4A8DAB512DB00B7D8F7FF853523EFE018324C47CCCF06A1D0A5066050042A61E91662E2CE4",
					ProductCode = "NWNP",
					MaskRevision = 0
				}
			},
			{
				"WCW vs. nWo - World Tour (NTSC-U v1.1)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x71, 0xBE, 0x60, 0xB9 },
					Checksum2 = new byte[]{ 0x1D, 0xDB, 0xFB, 0x3C },
					HashMD5 = "B7A220B59303D47F3BEAE233CA868CFD",
					HashSHA1 = "60814FB3AD2DD206BADBEEE47C07636357DD0D7E",
					HashSHA256 = "3711C8838B18374D1C7BE0192AD824082D33978F00B886D145BC55A782325D4D",
					HashSHA512 = "6395EC957D9F777A107097044D3EAB63A41824FB5152783A17AE554979BC2F8A24E9AC8F77FC385F88CC89C6173F2CDF853D470856953CBCC0622B9BB00C8F72",
					ProductCode = "NWNE",
					MaskRevision = 1
				}
			},
			{
				"Virtual Pro-Wrestling 64",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x04, 0x5C, 0x08, 0xC4 },
					Checksum2 = new byte[]{ 0x4A, 0xFD, 0x79, 0x8B },
					HashMD5 = "5E6202200AF40A8F026780EDFE1E15D0",
					HashSHA1 = "F9E9FA2ED819C3A39DB5CB6AFECA186F021DB5ED",
					HashSHA256 = "8B7191AE5489FC71C7A18C05A0AFBA035A88D8F22A2B047253DD9C524FB88921",
					HashSHA512 = "740C64323527737828703E9B1F933AC2BE1F79BF7FC035421CC966C40684970545A621ECA948FEA2AE43B5980E5409526A3D0806234F1E60B766D90B2C9B8C37",
					ProductCode = "NVPJ",
					MaskRevision = 0
				}
			},
			{
				"WCW/nWo Revenge (NTSC-U)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0xDE, 0xE5, 0x96, 0xAB },
					Checksum2 = new byte[]{ 0xAF, 0x3B, 0x7A, 0xE7 },
					HashMD5 = "C1384F3637D7A381B29341FED3EF3CEB",
					HashSHA1 = "E1711A2511394B9357B5F1AC8CA5CC17BD674836",
					HashSHA256 = "66C137D326565C6F31F992DABA8F67C0AEE7F025A142DD249D27019708014B60",
					HashSHA512 = "4ED0C9C4AEC92DCCB0FB8E52E4FE081666A2B31D1E4E2D7EF2222DB1E2EF5624300988CC2A4D886A9964F904CC3BEE03BC86A817E9B19D025D6A81995026DB06",
					ProductCode = "NW2E",
					MaskRevision = 0
				}
			},
			{
				"WCW/nWo Revenge (PAL)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x68, 0xE8, 0xA8, 0x75 },
					Checksum2 = new byte[]{ 0x0C, 0xE7, 0xA4, 0x86 },
					HashMD5 = "30C6676EC1D62122F4E7607EF3ABBD41",
					HashSHA1 = "C59315A3E7A6E8124495477656A77E4619DAC104",
					HashSHA256 = "E4BD4F49D6E2217294CD2F9178488349EABD44155E4AD0436A9D1DDEF58C53DA",
					HashSHA512 = "51028440CC8FE7E670A97DB064BB633AFC5DEDD8CAEE3CF1EDF58BF94DCD89447547424978A331C7736D82A5142143241361D57C04F157AD9BAFD0454AB2C657",
					ProductCode = "NW2P",
					MaskRevision = 0
				}
			},
			{
				"WWF WrestleMania 2000 (NTSC-U)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x90, 0xA5, 0x90, 0x03 },
					Checksum2 = new byte[]{ 0x31, 0x08, 0x98, 0x64 },
					HashMD5 = "D9030CA30E4D1AF805ACCE1BFED988CC",
					HashSHA1 = "442D417A52ED672CA1A47E7261A5414DEBB1E27A",
					HashSHA256 = "CBD44033868D3747241F5028206056E64836D744C756B5C39DC7B3A446E8CE5B",
					HashSHA512 = "4FB3AF13602926EC2A7A3896DE9A06C86E81FA437E413543F748CEE12FED2DB06C97AFCF5888FE0DD485D19EFDB446A04676A406FC04EA57B4A25F6E22D45B37",
					ProductCode = "NWXE",
					MaskRevision = 0
				}
			},
			{
				"WWF WrestleMania 2000 (PAL)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0xC7, 0x13, 0x53, 0xBE },
					Checksum2 = new byte[]{ 0xAA, 0x09, 0xA6, 0xEE },
					HashMD5 = "B75149F87CC5F3A508643AC377F2FCC9",
					HashSHA1 = "D37C4100D967CB8B71852993B947619467FFCAF7",
					HashSHA256 = "8F759C230E6C261EB77CFF7906740173132C7051FCAED10DEA71D78E375E45C7",
					HashSHA512 = "B234743E0647FA58E7D58D722FB269155C8FFBAB3F51F7C980D0FCDDD9B04EC2BAEC1E452E9CA2F3A9ECFBF9EFB36041A08CA6B03A59A9796D214154D8FB1F3E",
					ProductCode = "NWXP",
					MaskRevision = 0
				}
			},
			{
				"WWF WrestleMania 2000 (NTSC-J)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x12, 0x73, 0x7D, 0xA5 },
					Checksum2 = new byte[]{ 0x23, 0x96, 0x91, 0x59 },
					HashMD5 = "11EEE2F34BF8DA05A1B8F4FB9FE9F74C",
					HashSHA1 = "E020C26DEDE0C349181CF08A3541816DC47F63A8",
					HashSHA256 = "3E3114266D5FAEED1ED385528B0E4DBE268FDF64778A657895B31BAB9DE9AD85",
					HashSHA512 = "889FBE5838B0B0C55E23B76A3D979AB30D62AB8246ED95486663B04EBD948EA025A66788061C4439AEE1F47F240F29907EB224ADD3D79B6FF9843810808717F6",
					ProductCode = "NWXJ",
					MaskRevision = 0
				}
			},
			{
				"Virtual Pro-Wrestling 2",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0xCD, 0x09, 0x42, 0x35 },
					Checksum2 = new byte[]{ 0x88, 0x07, 0x4B, 0x62 },
					HashMD5 = "90002501777E3237739F5ED9B0E349E2",
					HashSHA1 = "82DD25A044689EAB57AB362FE10C0DA6388C217A",
					HashSHA256 = "358E9A345438155C6BD57DA4BBF0F7A9FA1B4F7D5B1B726E8076C38F0F987E52",
					HashSHA512 = "55658D2B182CB68FAA27E494D7C130A43E0A45334B9E3BA1C817660578BE4413C2065DA5716EA63F0C4165D4A82D4E6E64175785E9525D553F4A0A799A5BD04B",
					ProductCode = "NA2J",
					MaskRevision = 0
				}
			},
			{
				"WWF No Mercy (NTSC-U, v1.0)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x4E, 0x4B, 0x06, 0x40 },
					Checksum2 = new byte[]{ 0x1B, 0x49, 0xBC, 0xFB },
					HashMD5 = "04C492BE7F89FC6F425238BD67629544",
					HashSHA1 = "3566D9B5723291937582453AD0453CE517CFC358",
					HashSHA256 = "28BADB169848AA814C35C242C48C1E95CE2540D6663E67079F97768096C17A1F",
					HashSHA512 = "DDAF6F8B5FAE1BB9423AD95798131B7F892D6A4A8BD8475D0F655CDA380DB517E4536E24A05E40875E81A5C1ACF6AB192E1B7FBC0FC20F66723B9415F3F5374B",
					ProductCode = "NW4E",
					MaskRevision = 0
				}
			},
			{
				"WWF No Mercy (PAL, v1.0)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x6D, 0x8D, 0xF0, 0x8E },
					Checksum2 = new byte[]{ 0xD0, 0x08, 0xC3, 0xCF },
					HashMD5 = "D94A8F78178473D4BA4BED62FA8E2E66",
					HashSHA1 = "15AD0A48DBB2DA32B39601E6BFB5D7B381853529",
					HashSHA256 = "9FB50974A14E834BA540DF59891D359D2C9B846359BB26D58DB71FB8E28FF289",
					HashSHA512 = "E25C9DD129096B258888B1BB77DEACFA69D075D9D1F309312005F681A367E79B1E9817917AF023C7F562AE056AF6524EDE5C71B300C0F5E4E5CAC9CF5C52611E",
					ProductCode = "NW4P",
					MaskRevision = 0
				}
			},
			{
				"WWF No Mercy (NTSC-U, v1.1)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x6C, 0x80, 0xF1, 0x3B },
					Checksum2 = new byte[]{ 0x42, 0x7E, 0xDE, 0xAA },
					HashMD5 = "66B8EC24557A50514A814F15429BD559",
					HashSHA1 = "91CEE3D096F4A76644D8B35B9AEAD6448909ABD1",
					HashSHA256 = "FC561FCE443010B114CC8EA41226772B7B2A2D33055BE163FAD91AC9C3B096CB",
					HashSHA512 = "C8E39A709DCF2E74A67E819EC79A437ABD27ADD3021AE8B09339F5DA7F223AEF9655EE0363497E571C6A669EC01481BAE603A9B4D961961B60D7D585326745B5",
					ProductCode = "NW4E",
					MaskRevision = 1
				}
			},
			{
				"WWF No Mercy (PAL, v1.1)",
				new ValidValues()
				{
					Checksum1 = new byte[]{ 0x8C, 0xDB, 0x94, 0xC2 },
					Checksum2 = new byte[]{ 0xCB, 0x46, 0xC6, 0xF0 },
					HashMD5 = "400A14F132D993F5544F8B008EC136FA",
					HashSHA1 = "CC8B9863C0E96D1B6A611D6AC2238E38B247EA5D",
					HashSHA256 = "381D46CFDF108122D8E41A3E0A45B2C8FB9D0429F130DDE5D6D58E6D81DA07C0",
					HashSHA512 = "AC5F357DDEFA5CBD9739A91294553CFF54E3C77B724889E8A60C115FC24BF7ADDEF6BAE407D19C5AA1E13B74A22378978CFC3E2EB636FAB9E62E09ABAAF492DF",
					ProductCode = "NW4P",
					MaskRevision = 1
				}
			}
		};

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

		#region Base Test Results
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
		/// Only true if most test results are positive.
		/// (HasCorrectFileExtension can be false as long as FormatCheckPassed is true)
		/// </summary>
		public bool CanBePatched;
		#endregion

		#region Game-Specific Test Results
		/// <summary>
		/// True if game code matches a value in the database.
		/// </summary>
		public bool HasCorrectGameCode;

		/// <summary>
		/// True if the first internal checksum matches the unmodified values.
		/// </summary>
		public bool HasCorrectFirstChecksum;

		/// <summary>
		/// True if the second internal checksum matches the unmodified values.
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
			MessageBox.Show(msg, "AKI N64 ROM ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// rom file browser
		/// </summary>
		private void btnBrowseRom_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select N64 ROM File";
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
				ErrorBox("No file has been selected, and therefore, this can't be anything, really.\n\nPlease click the Browse button and select a ROM file.");
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
				ErrorBox(string.Format("The file '{0}' does not exist.\nTherefore, it can't be anything.\n\nPlease click the Browse button and select a ROM file.",RomPath));
				CanBePatched = false;
				return;
			}

			// check the file extension, which may be misleading
			HasCorrectFileExtension = VerifyCorrectFileExtension();
			if (HasCorrectFileExtension)
			{
				tbOutput.Text += "ROM has .z64 extension; this does not necessarily mean the ROM is Z64 format, though.\r\n";
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
				ErrorBox("This does not appear to be a Nintendo 64 ROM of any known type.");
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
					tbOutput.Text += "This file claims to be a Z64 format ROM, and it is!\r\n";
				}
				else
				{
					// this is a Z64 ROM masquerading as something else!
					tbOutput.Text += string.Format("This file has an extension of '{0}', but it's actually a Z64 format ROM! Please change the file extension to '.z64' when you get a chance.\r\n", Path.GetExtension(RomPath));
				}
				FormatCheckPassed = true;
			}
			else if (FirstFourBytesCategory == RomTypes.V64)
			{
				if (HasCorrectFileExtension)
				{
					// claims to be a Z64 ROM, but is a V64 ROM
					tbOutput.Text += "This file claims to be a Z64 format ROM, but it is a V64 format ROM. You must convert it to Z64 format for patching and editing.\r\n";
				}
				else
				{
					// this never claimed to be a Z64 ROM, but is not what we need
					tbOutput.Text += "This file is a V64 format ROM. You must convert it to Z64 format for patching and editing.\r\n";
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

			// we have a Z64 format ROM. time to find out what it actually is...
			bool FoundAnyValidGameCode = false;
			bool FoundSpecificGameCode = false;
			char[] gameCode = new char[4];
			byte gameRevision = 0; // fuckings to whatever is requiring me to set a value here
			string claimedGame = string.Empty;
			foreach (KeyValuePair<string, ValidValues> gameVals in GameValues)
			{
				if (VerifyGameCode(gameVals.Value, out gameCode))
				{
					FoundAnyValidGameCode = true;
					if (VerifyMaskRevision(gameVals.Value, out gameRevision))
					{
						claimedGame = gameVals.Key;
						FoundSpecificGameCode = true;
					}
				}
			}

			if (FoundAnyValidGameCode)
			{
				tbOutput.Text += string.Format("Game code '{0}{1}{2}{3}' matches game code for '{4}'.\r\n", gameCode[0], gameCode[1], gameCode[2], gameCode[3], claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("This ROM has a game code of '{0}{1}{2}{3}', which does not match any game in the database.\r\n", gameCode[0], gameCode[1], gameCode[2], gameCode[3]);
				CanBePatched = false;
			}

			if (FoundSpecificGameCode)
			{
				tbOutput.Text += string.Format("Mask ROM Version value '{0}' matches value for '{1}'.\r\n", gameRevision, claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("This ROM has a Mask ROM Version value of '{0}'; The database entry for '{1}' expects a Mask ROM Version value of '{2}'.\r\n", gameRevision, claimedGame, GameValues[claimedGame].MaskRevision);
				CanBePatched = false;
			}

			if (!CanBePatched)
			{
				ErrorBox("This ROM did not pass the game code and/or mask rom version checks. Please see the Output box for details.");
				br.Close();
				return;
			}

			HasCorrectFirstChecksum = VerifyInternalChecksum1(GameValues[claimedGame], out byte[] checksum1);
			HasCorrectSecondChecksum = VerifyInternalChecksum2(GameValues[claimedGame], out byte[] checksum2);

			if (HasCorrectFirstChecksum)
			{
				tbOutput.Text += string.Format("First internal checksum matches the expected values for unmodified Z64 format ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("First internal checksum is {0:X2} {1:X2} {2:X2} {3:X2}. This is possibly a modified ROM of '{4}'.\r\n", checksum1[0], checksum1[1], checksum1[2], checksum1[3], claimedGame);
				CanBePatched = false;
			}

			if (HasCorrectSecondChecksum)
			{
				tbOutput.Text += string.Format("Second internal checksum matches the expected values for unmodified Z64 format ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("Second internal checksum is {0:X2} {1:X2} {2:X2} {3:X2}. This is possibly a modified ROM of {4}.", checksum2[0], checksum2[1], checksum2[2], checksum2[3], claimedGame);
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
			PassedHashCheck_MD5 = VerifyHash_MD5(GameValues[claimedGame], out byte[] inputRomHashMD5);
			if (PassedHashCheck_MD5)
			{
				tbOutput.Text += string.Format("The MD5 hash of the input ROM matches the unmodified ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("MD5 hash of this rom does NOT match an unmodified ROM of '{0}'.\r\n", claimedGame);
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashMD5), GameValues[claimedGame].HashMD5) ;
				CanBePatched = false;
			}

			PassedHashCheck_SHA1 = VerifyHash_SHA1(GameValues[claimedGame], out byte[] inputRomHashSHA1);
			if (PassedHashCheck_SHA1)
			{
				tbOutput.Text += string.Format("The SHA-1 hash of the input ROM matches the unmodified ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("SHA-1 hash of this rom does NOT match an unmodified ROM of '{0}'.\r\n", claimedGame);
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA1), GameValues[claimedGame].HashSHA1);
				CanBePatched = false;
			}

			PassedHashCheck_SHA256 = VerifyHash_SHA256(GameValues[claimedGame], out byte[] inputRomHashSHA256);
			if (PassedHashCheck_SHA256)
			{
				tbOutput.Text += string.Format("The SHA-256 hash of the input ROM matches the unmodified ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("SHA-256 hash of this rom does NOT match an unmodified ROM of '{0}'.\r\n", claimedGame);
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA256), GameValues[claimedGame].HashSHA256);
				CanBePatched = false;
			}

			PassedHashCheck_SHA512 = VerifyHash_SHA512(GameValues[claimedGame], out byte[] inputRomHashSHA512);
			if (PassedHashCheck_SHA512)
			{
				tbOutput.Text += string.Format("The SHA-512 hash of the input ROM matches the unmodified ROM of '{0}'.\r\n", claimedGame);
			}
			else
			{
				tbOutput.Text += string.Format("SHA-512 hash of this rom does NOT match an unmodified ROM of '{0}'.\r\n", claimedGame);
				tbOutput.Text += string.Format("Found: {0}\r\nExpected: {1}\r\n", HashToString(inputRomHashSHA512), GameValues[claimedGame].HashSHA512);
				CanBePatched = false;
			}
			br.Close();

			if (!CanBePatched)
			{
				ErrorBox("One or more of the file hash integrity checks has failed. Please see the Output box for details.");
			}
			else
			{
				MessageBox.Show(string.Format("Congratulations! This appears to be an unmodified Z64 format ROM for '{0}', ready to be patched or edited.",claimedGame), "AKI N64 ROM ID", MessageBoxButtons.OK);
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
		/// <returns>True if the first internal checksum matches the unmodified original ROM's.</returns>
		protected bool VerifyInternalChecksum1(ValidValues expected, out byte[] checksum1)
		{
			br.BaseStream.Seek(0x10, SeekOrigin.Begin);
			checksum1 = br.ReadBytes(4);
			return (checksum1[0] == expected.Checksum1[0] && checksum1[1] == expected.Checksum1[1] && checksum1[2] == expected.Checksum1[2] && checksum1[3] == expected.Checksum1[3]);
		}

		/// <summary>
		/// Verify the second internal checksum at file offset 0x14.
		/// </summary>
		/// <param name="checksum2">Values for the second internal checksum.</param>
		/// <returns>True if the second internal checksum matches the unmodified original ROM's.</returns>
		protected bool VerifyInternalChecksum2(ValidValues expected, out byte[] checksum2)
		{
			br.BaseStream.Seek(0x14, SeekOrigin.Begin);
			checksum2 = br.ReadBytes(4);
			return (checksum2[0] == expected.Checksum2[0] && checksum2[1] == expected.Checksum2[1] && checksum2[2] == expected.Checksum2[2] && checksum2[3] == expected.Checksum2[3]);
		}

		/// <summary>
		/// Checks for an expected Product Code at ROM offset 0x3B.
		/// </summary>
		/// <param name="expected">ValidValues containing the expected product code</param>
		/// <param name="gameCode">The four characters making up the game code.</param>
		/// <returns>True if the ROM's product code matches the expected product code, false otherwise.</returns>
		protected bool VerifyGameCode(ValidValues expected, out char[] gameCode)
		{
			br.BaseStream.Seek(0x3B, SeekOrigin.Begin);
			gameCode = br.ReadChars(4);

			if (gameCode[0] == expected.ProductCode[0] && gameCode[1] == expected.ProductCode[1] && gameCode[2] == expected.ProductCode[2] && gameCode[3] == expected.ProductCode[3])
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks for an expected Mask ROM version byte at ROM offset 0x3F.
		/// </summary>
		/// <param name="expected">ValidValues containing the expected mask version</param>
		/// <param name="gameVer">Value of the Mask ROM version byte found in the ROM</param>
		/// <returns>True if the ROM's version byte matches the expected MaskRevision value, false otherwise</returns>
		protected bool VerifyMaskRevision(ValidValues expected, out byte gameVer)
		{
			br.BaseStream.Seek(0x3F, SeekOrigin.Begin);
			gameVer = br.ReadByte();

			if (gameVer == expected.MaskRevision)
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
		/// Verify MD5 hash of input ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_MD5(ValidValues expected, out byte[] inputRomHash)
		{
			MD5 md5Hasher = MD5.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = md5Hasher.ComputeHash(fs);
			md5Hasher.Dispose();

			return HashToString(inputRomHash).Equals(expected.HashMD5);
		}

		/// <summary>
		/// Verify SHA-1 hash of input ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_SHA1(ValidValues expected, out byte[] inputRomHash)
		{
			SHA1 sha1Hasher = SHA1.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha1Hasher.ComputeHash(fs);
			sha1Hasher.Dispose();

			return HashToString(inputRomHash).Equals(expected.HashSHA1);
		}

		/// <summary>
		/// Verify SHA-256 hash of input ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>
		protected bool VerifyHash_SHA256(ValidValues expected, out byte[] inputRomHash)
		{
			SHA256 sha256Hasher = SHA256.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha256Hasher.ComputeHash(fs);
			sha256Hasher.Dispose();

			return HashToString(inputRomHash).Equals(expected.HashSHA256);
		}

		/// <summary>
		/// Verify SHA-512 hash of input ROM against known unmodified ROM hash
		/// </summary>
		/// <param name="inputRomHash"></param>
		/// <returns></returns>

		protected bool VerifyHash_SHA512(ValidValues expected, out byte[] inputRomHash)
		{
			SHA512 sha512Hasher = SHA512.Create();
			fs.Seek(0, SeekOrigin.Begin);
			inputRomHash = sha512Hasher.ComputeHash(fs);
			sha512Hasher.Dispose();

			return HashToString(inputRomHash).Equals(expected.HashSHA512);
		}

		#endregion
	}
}
