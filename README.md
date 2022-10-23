AKI N64 ROM Identifier
======================
This is a C# Windows Forms GUI program that analyzes a ROM file in an attempt
to determine if it is an unmodified Z64 format ROM for one of AKI Corporation's
wrestling games on the Nintendo 64.

This program was previously called "Virtual Pro-Wrestling 2 ROM Identifier".
Version 2.0 expands the game database to all known AKI wrestling games on
Nintendo 64, including revision and regional variants.

I am (still) pre-emptively declining to answer any questions about why this program was made.

Usage
-----
Click the "Browse" button to select a ROM file. Alternatively, you can type
the path to the ROM file in the text box. Relative paths work if the ROM is
in the same folder as the .exe.

Once this is done, press the "Verify ROM" button, and wait for the results.
It will take a few seconds, especially for SHA-256 and SHA-512 verification.
The "Output" box will provide any information in case you get a negative result.

The Checks
----------
Multiple checks are performed to ensure the selected ROM file is an unmodified
Z64 format ROM for one of AKI Corporation's N64 wrestling games. These steps
are outlined here in case you wish to independently verify the correct results.

- The first four bytes of the file are read and analyzed. If they do not match
  `80 37 12 40`, it is not a Z64 format ROM. Patches and editing utilities
  typically require Z64 format ROMs.

- The game/product code (four ASCII characters located at file offset `0x3B`)
  is checked for various possible values from the internal database.

  Previous versions (which only checked for valid Virtual Pro-Wrestling 2 ROMs)
  explicitly failed on `NA2E` (implying an English translation has been patched
  already), and `NVF*` (which implies VPW2 freem Edition; it cannot be "upgrade"
  patched and _must_ be patched to an unmodified original VPW2 ROM).

  Version 2.0 of this program does not specifically check for these values,
  but will still fail if they are found.

- The Mask ROM Version/Revision value located at file offset `0x3F` is checked
  to ensure the correct revision of the game. This is a new check in Version 2.0,
  since WCW vs. nWo - World Tour and WWF No Mercy have multiple revisions.

- The two internal checksum values are also checked:
  - Internal checksum #1 ("Checksum1") is found at file offset `0x10`.
  - Internal checksum #2 ("Checksum2") is found at file offset `0x14`.

- Finally, the contents of the ROM are hashed using multiple algorithms and
  compared to hash values of an unmodified Z64 format ROM for various AKI games.

  The hash algorithms used are MD5, SHA-1, SHA-256, and SHA-512.

  Some of those might be overkill. However, MD5 and SHA-1 hash collision
  is possible, so I feel it's worth it to check with SHA-256 and SHA-512.

If ***all*** of these checks pass, we can be reasonably certain that the input ROM
file is indeed, an unmodified Z64 format ROM for one of AKI Corporation's wrestling
games.

Otherwise, you probably need to convert the ROM to the correct format, or obtain
a new ROM. Any help with that is beyond the scope of this project. Don't ask me
where to get ROMs. Buy a backup device and dump your own cartridges if you want
to be 100% sure they're correct.

Game Database
-------------
Lazily copy-and-pasted from the source code:

```
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
```

General Notes
-------------
This is not a paragon of good code. It is a program hacked together to quickly solve
a problem. I am not responsible for anything that may happen from using this program.
