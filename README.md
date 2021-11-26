Virtual Pro-Wrestling 2 ROM Identifier
======================================
This is a C# Windows Forms GUI program that analyzes a ROM file in an attempt
to determine if it is an unmodified Z64 format Virtual Pro-Wrestling 2 ROM file.

I am pre-emptively declining to answer any questions about why this program was made.

Usage
-----
Click the "Browse" button to select a ROM file. Alternatively, you can type the path
to the ROM file in the text box. Relative paths work, in case the ROM is in the same
folder as the .exe.

Once this is done, press the "Verify ROM" button, and wait for the results.
It will take a few seconds, especially for SHA-256 and SHA-512 verification.
The "Output" box will provide any information in case you get a negative result.

The Checks
----------
Multiple checks are performed to ensure the selected ROM file is an unmodified
Virtual Pro-Wrestling 2 ROM file in Z64 format. These steps are outlined here
in case you wish to independently verify the correct results.

- The first four bytes of the file are read and analyzed. If they do not match
  `80 37 12 40`, it is not a Z64 format ROM. Patches and editing utilities require
  Z64 format ROMs.

- The game code (four ASCII characters located at file offset `0x3B`) is checked for
  `NA2J` (hex values `4E 41 32 4A`).

  Explicit failure scenarios include `NA2E` (implying an English translation has been
  patched already), and `NVF*` (which implies VPW2 freem Edition; it cannot be "upgrade"
  patched and _must_ be patched to an unmodified original VPW2 ROM).

- The two internal checksum values are also checked:
  - Internal checksum #1 (file offset `0x10`) is `CD 09 42 35` in an unmodified Z64 format VPW2 ROM.
  - Internal checksum #2 (file offset `0x14`) is `88 07 4B 62` in an unmodified Z64 format VPW2 ROM.

- Finally, the contents of the ROM are hashed using multiple algorithms and compared to
  hash values of an unmodified original VPW2 ROM in Z64 format:
  - MD5: `90002501777E3237739F5ED9B0E349E2`
  - SHA-1: `82DD25A044689EAB57AB362FE10C0DA6388C217A`
  - SHA-256: `358E9A345438155C6BD57DA4BBF0F7A9FA1B4F7D5B1B726E8076C38F0F987E52`
  - SHA-512: `55658D2B182CB68FAA27E494D7C130A43E0A45334B9E3BA1C817660578BE4413C2065DA5716EA63F0C4165D4A82D4E6E64175785E9525D553F4A0A799A5BD04B`

If ***all*** of these checks pass, we can be reasonably certain that the input ROM
file is indeed, an unmodified Z64 format ROM of Virtual Pro-Wrestling 2.

Otherwise, you probably need to convert the ROM to the correct format, or obtain
a new ROM. Any help with that is beyond the scope of this project. Don't ask me
where to get ROMs. Buy a backup device and dump them yourself if you want to be 100% sure.

General Notes
-------------
This is not a paragon of good code. It is a program hacked together to quickly solve
a problem. I am not responsible for anything that may happen from using this program.
