    <#
        $FileStream = New-Object System.IO.FileStream($FilePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read)
        $FileByteArray = New-Object Byte[]($FileStream.Length)
        $FileStream.Read($FileByteArray, 0, $FileStream.Length) | Out-Null
        $FileStream.Close()
        $Handle = [System.Runtime.InteropServices.GCHandle]::Alloc($FileByteArray, 'Pinned')
        $PEBaseAddr = $Handle.AddrOfPinnedObject()

    #>

function Get-PEHaeder {
    [cmdletbinding(DefaultParameterSetName = 'OnDisk')]
    param(
        [Parameter(Position = 0, Mandatory = $True, ParameterSetName = 'OnDisk', ValueFromPipelineByPropertyName = $True)] 
        [Alias('FullName')]
        [String[]]$FilePath
    )
PROCESS {
        write-host ("++{0}" -f $MyInvocation.MyCommand.Name) 
    # I am going to write a short explanation of the code I borrowedfrom PowerSploit project
    # the try block trys to "output" the type [PE] piping the error output to the NUL object
    # the catch block will activate on a [Management.Automation.RuntimeException] type
    # the block defines a [string[]] $code which is an array of strings,
    # or is it a [string] $code?
    # the string contains .NET/C# definition of the structures and some constants
    # for the layout of the PE header that is the first X bytes of any PE binary
    # after the string are some instructions to invoke the .NET JIT (Just In Time compiler)
    try { [PE] | Out-Null } catch [Management.Automation.RuntimeException] {
        write-host "[PE] NOT DEFINED ..."
        $code = @"
        using System;
        using System.Runtime.InteropServices;

        public class PE
        {
            [Flags]
            public enum IMAGE_DOS_SIGNATURE : ushort
            {
                DOS_SIGNATURE = 0x5A4D, // MZ
                OS2_SIGNATURE = 0x454E, // NE
                OS2_SIGNATURE_LE = 0x454C, // LE
                VXD_SIGNATURE = 0x454C, // LE
            }
        
            [Flags]
            public enum IMAGE_NT_SIGNATURE : uint
            {
                VALID_PE_SIGNATURE = 0x00004550 // PE00
            }
        
            [Flags]
            public enum IMAGE_FILE_MACHINE : ushort
            {
                UNKNOWN = 0,
                I386 = 0x014c, // Intel 386.
                R3000 = 0x0162, // MIPS little-endian =0x160 big-endian
                R4000 = 0x0166, // MIPS little-endian
                R10000 = 0x0168, // MIPS little-endian
                WCEMIPSV2 = 0x0169, // MIPS little-endian WCE v2
                ALPHA = 0x0184, // Alpha_AXP
                SH3 = 0x01a2, // SH3 little-endian
                SH3DSP = 0x01a3,
                SH3E = 0x01a4, // SH3E little-endian
                SH4 = 0x01a6, // SH4 little-endian
                SH5 = 0x01a8, // SH5
                ARM = 0x01c0, // ARM Little-Endian
                THUMB = 0x01c2,
                ARMNT = 0x01c4, // ARM Thumb-2 Little-Endian
                AM33 = 0x01d3,
                POWERPC = 0x01F0, // IBM PowerPC Little-Endian
                POWERPCFP = 0x01f1,
                IA64 = 0x0200, // Intel 64
                MIPS16 = 0x0266, // MIPS
                ALPHA64 = 0x0284, // ALPHA64
                MIPSFPU = 0x0366, // MIPS
                MIPSFPU16 = 0x0466, // MIPS
                AXP64 = ALPHA64,
                TRICORE = 0x0520, // Infineon
                CEF = 0x0CEF,
                EBC = 0x0EBC, // EFI public byte Code
                AMD64 = 0x8664, // AMD64 (K8)
                M32R = 0x9041, // M32R little-endian
                CEE = 0xC0EE
            }
        
            [Flags]
            public enum IMAGE_FILE_CHARACTERISTICS : ushort
            {
                IMAGE_RELOCS_STRIPPED = 0x0001, // Relocation info stripped from file.
                IMAGE_EXECUTABLE_IMAGE = 0x0002, // File is executable (i.e. no unresolved external references).
                IMAGE_LINE_NUMS_STRIPPED = 0x0004, // Line nunbers stripped from file.
                IMAGE_LOCAL_SYMS_STRIPPED = 0x0008, // Local symbols stripped from file.
                IMAGE_AGGRESIVE_WS_TRIM = 0x0010, // Agressively trim working set
                IMAGE_LARGE_ADDRESS_AWARE = 0x0020, // App can handle >2gb addresses
                IMAGE_REVERSED_LO = 0x0080, // public bytes of machine public ushort are reversed.
                IMAGE_32BIT_MACHINE = 0x0100, // 32 bit public ushort machine.
                IMAGE_DEBUG_STRIPPED = 0x0200, // Debugging info stripped from file in .DBG file
                IMAGE_REMOVABLE_RUN_FROM_SWAP = 0x0400, // If Image is on removable media =copy and run from the swap file.
                IMAGE_NET_RUN_FROM_SWAP = 0x0800, // If Image is on Net =copy and run from the swap file.
                IMAGE_SYSTEM = 0x1000, // System File.
                IMAGE_DLL = 0x2000, // File is a DLL.
                IMAGE_UP_SYSTEM_ONLY = 0x4000, // File should only be run on a UP machine
                IMAGE_REVERSED_HI = 0x8000 // public bytes of machine public ushort are reversed.
            }
        
            [Flags]
            public enum IMAGE_NT_OPTIONAL_HDR_MAGIC : ushort
            {
                PE32 = 0x10b,
                PE64 = 0x20b
            }
        
            [Flags]
            public enum IMAGE_SUBSYSTEM : ushort
            {
                UNKNOWN = 0, // Unknown subsystem.
                NATIVE = 1, // Image doesn't require a subsystem.
                WINDOWS_GUI = 2, // Image runs in the Windows GUI subsystem.
                WINDOWS_CUI = 3, // Image runs in the Windows character subsystem.
                OS2_CUI = 5, // image runs in the OS/2 character subsystem.
                POSIX_CUI = 7, // image runs in the Posix character subsystem.
                NATIVE_WINDOWS = 8, // image is a native Win9x driver.
                WINDOWS_CE_GUI = 9, // Image runs in the Windows CE subsystem.
                EFI_APPLICATION = 10,
                EFI_BOOT_SERVICE_DRIVER = 11,
                EFI_RUNTIME_DRIVER = 12,
                EFI_ROM = 13,
                XBOX = 14,
                WINDOWS_BOOT_APPLICATION = 16
            }
        
            [Flags]
            public enum IMAGE_DLLCHARACTERISTICS : ushort
            {
                DYNAMIC_BASE = 0x0040, // DLL can move.
                FORCE_INTEGRITY = 0x0080, // Code Integrity Image
                NX_COMPAT = 0x0100, // Image is NX compatible
                NO_ISOLATION = 0x0200, // Image understands isolation and doesn't want it
                NO_SEH = 0x0400, // Image does not use SEH. No SE handler may reside in this image
                NO_BIND = 0x0800, // Do not bind this image.
                WDM_DRIVER = 0x2000, // Driver uses WDM model
                TERMINAL_SERVER_AWARE = 0x8000
            }
        
            [Flags]
            public enum IMAGE_SCN : uint
            {
                TYPE_NO_PAD = 0x00000008, // Reserved.
                CNT_CODE = 0x00000020, // Section contains code.
                CNT_INITIALIZED_DATA = 0x00000040, // Section contains initialized data.
                CNT_UNINITIALIZED_DATA = 0x00000080, // Section contains uninitialized data.
                LNK_INFO = 0x00000200, // Section contains comments or some other type of information.
                LNK_REMOVE = 0x00000800, // Section contents will not become part of image.
                LNK_COMDAT = 0x00001000, // Section contents comdat.
                NO_DEFER_SPEC_EXC = 0x00004000, // Reset speculative exceptions handling bits in the TLB entries for this section.
                GPREL = 0x00008000, // Section content can be accessed relative to GP
                MEM_FARDATA = 0x00008000,
                MEM_PURGEABLE = 0x00020000,
                MEM_16BIT = 0x00020000,
                MEM_LOCKED = 0x00040000,
                MEM_PRELOAD = 0x00080000,
                ALIGN_1BYTES = 0x00100000,
                ALIGN_2BYTES = 0x00200000,
                ALIGN_4BYTES = 0x00300000,
                ALIGN_8BYTES = 0x00400000,
                ALIGN_16BYTES = 0x00500000, // Default alignment if no others are specified.
                ALIGN_32BYTES = 0x00600000,
                ALIGN_64BYTES = 0x00700000,
                ALIGN_128BYTES = 0x00800000,
                ALIGN_256BYTES = 0x00900000,
                ALIGN_512BYTES = 0x00A00000,
                ALIGN_1024BYTES = 0x00B00000,
                ALIGN_2048BYTES = 0x00C00000,
                ALIGN_4096BYTES = 0x00D00000,
                ALIGN_8192BYTES = 0x00E00000,
                ALIGN_MASK = 0x00F00000,
                LNK_NRELOC_OVFL = 0x01000000, // Section contains extended relocations.
                MEM_DISCARDABLE = 0x02000000, // Section can be discarded.
                MEM_NOT_CACHED = 0x04000000, // Section is not cachable.
                MEM_NOT_PAGED = 0x08000000, // Section is not pageable.
                MEM_SHARED = 0x10000000, // Section is shareable.
                MEM_EXECUTE = 0x20000000, // Section is executable.
                MEM_READ = 0x40000000, // Section is readable.
                MEM_WRITE = 0x80000000 // Section is writeable.
            }
    
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_DOS_HEADER
            {
                public IMAGE_DOS_SIGNATURE e_magic; // Magic number
                public ushort e_cblp; // public bytes on last page of file
                public ushort e_cp; // Pages in file
                public ushort e_crlc; // Relocations
                public ushort e_cparhdr; // Size of header in paragraphs
                public ushort e_minalloc; // Minimum extra paragraphs needed
                public ushort e_maxalloc; // Maximum extra paragraphs needed
                public ushort e_ss; // Initial (relative) SS value
                public ushort e_sp; // Initial SP value
                public ushort e_csum; // Checksum
                public ushort e_ip; // Initial IP value
                public ushort e_cs; // Initial (relative) CS value
                public ushort e_lfarlc; // File address of relocation table
                public ushort e_ovno; // Overlay number
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
                public string e_res; // This will contain 'Detours!' if patched in memory
                public ushort e_oemid; // OEM identifier (for e_oeminfo)
                public ushort e_oeminfo; // OEM information; e_oemid specific
                [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=10)] // , ArraySubType=UnmanagedType.U4
                public ushort[] e_res2; // Reserved public ushorts
                public int e_lfanew; // File address of new exe header
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_FILE_HEADER
            {
                public IMAGE_FILE_MACHINE Machine;
                public ushort NumberOfSections;
                public uint TimeDateStamp;
                public uint PointerToSymbolTable;
                public uint NumberOfSymbols;
                public ushort SizeOfOptionalHeader;
                public IMAGE_FILE_CHARACTERISTICS Characteristics;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_NT_HEADERS32
            {
                public IMAGE_NT_SIGNATURE Signature;
                public _IMAGE_FILE_HEADER FileHeader;
                public _IMAGE_OPTIONAL_HEADER32 OptionalHeader;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_NT_HEADERS64
            {
                public IMAGE_NT_SIGNATURE Signature;
                public _IMAGE_FILE_HEADER FileHeader;
                public _IMAGE_OPTIONAL_HEADER64 OptionalHeader;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_OPTIONAL_HEADER32
            {
                public IMAGE_NT_OPTIONAL_HDR_MAGIC Magic;
                public byte MajorLinkerVersion;
                public byte MinorLinkerVersion;
                public uint SizeOfCode;
                public uint SizeOfInitializedData;
                public uint SizeOfUninitializedData;
                public uint AddressOfEntryPoint;
                public uint BaseOfCode;
                public uint BaseOfData;
                public uint ImageBase;
                public uint SectionAlignment;
                public uint FileAlignment;
                public ushort MajorOperatingSystemVersion;
                public ushort MinorOperatingSystemVersion;
                public ushort MajorImageVersion;
                public ushort MinorImageVersion;
                public ushort MajorSubsystemVersion;
                public ushort MinorSubsystemVersion;
                public uint Win32VersionValue;
                public uint SizeOfImage;
                public uint SizeOfHeaders;
                public uint CheckSum;
                public IMAGE_SUBSYSTEM Subsystem;
                public IMAGE_DLLCHARACTERISTICS DllCharacteristics;
                public uint SizeOfStackReserve;
                public uint SizeOfStackCommit;
                public uint SizeOfHeapReserve;
                public uint SizeOfHeapCommit;
                public uint LoaderFlags;
                public uint NumberOfRvaAndSizes;
                [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=16)]
                public _IMAGE_DATA_DIRECTORY[] DataDirectory;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_OPTIONAL_HEADER64
            {
                public IMAGE_NT_OPTIONAL_HDR_MAGIC Magic;
                public byte MajorLinkerVersion;
                public byte MinorLinkerVersion;
                public uint SizeOfCode;
                public uint SizeOfInitializedData;
                public uint SizeOfUninitializedData;
                public uint AddressOfEntryPoint;
                public uint BaseOfCode;
                public ulong ImageBase;
                public uint SectionAlignment;
                public uint FileAlignment;
                public ushort MajorOperatingSystemVersion;
                public ushort MinorOperatingSystemVersion;
                public ushort MajorImageVersion;
                public ushort MinorImageVersion;
                public ushort MajorSubsystemVersion;
                public ushort MinorSubsystemVersion;
                public uint Win32VersionValue;
                public uint SizeOfImage;
                public uint SizeOfHeaders;
                public uint CheckSum;
                public IMAGE_SUBSYSTEM Subsystem;
                public IMAGE_DLLCHARACTERISTICS DllCharacteristics;
                public ulong SizeOfStackReserve;
                public ulong SizeOfStackCommit;
                public ulong SizeOfHeapReserve;
                public ulong SizeOfHeapCommit;
                public uint LoaderFlags;
                public uint NumberOfRvaAndSizes;
                [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst=16)]
                public _IMAGE_DATA_DIRECTORY[] DataDirectory;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_DATA_DIRECTORY
            {
                public uint VirtualAddress;
                public uint Size;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_EXPORT_DIRECTORY
            {
                public uint Characteristics;
                public uint TimeDateStamp;
                public ushort MajorVersion;
                public ushort MinorVersion;
                public uint Name;
                public uint Base;
                public uint NumberOfFunctions;
                public uint NumberOfNames;
                public uint AddressOfFunctions; // RVA from base of image
                public uint AddressOfNames; // RVA from base of image
                public uint AddressOfNameOrdinals; // RVA from base of image
            }
       
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_SECTION_HEADER
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
                public string Name;
                public uint VirtualSize;
                public uint VirtualAddress;
                public uint SizeOfRawData;
                public uint PointerToRawData;
                public uint PointerToRelocations;
                public uint PointerToLinenumbers;
                public ushort NumberOfRelocations;
                public ushort NumberOfLinenumbers;
                public IMAGE_SCN Characteristics;
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_IMPORT_DESCRIPTOR
            {
                public uint OriginalFirstThunk; // RVA to original unbound IAT (PIMAGE_THUNK_DATA)
                public uint TimeDateStamp; // 0 if not bound,
                                                    // -1 if bound, and real date/time stamp
                                                    // in IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT (new BIND)
                                                    // O.W. date/time stamp of DLL bound to (Old BIND)
                public uint ForwarderChain; // -1 if no forwarders
                public uint Name;
                public uint FirstThunk; // RVA to IAT (if bound this IAT has actual addresses)
            }

            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_THUNK_DATA32
            {
                public Int32 AddressOfData; // PIMAGE_IMPORT_BY_NAME
            }

            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_THUNK_DATA64
            {
                public Int64 AddressOfData; // PIMAGE_IMPORT_BY_NAME
            }
        
            [StructLayout(LayoutKind.Sequential, Pack=1)]
            public struct _IMAGE_IMPORT_BY_NAME
            {
                public ushort Hint;
                public char Name;
            }
        }
"@
        # create an instance of the JIT compiler
        # 1) create an object for the compiler paameters
        $compileParams = New-Object System.CodeDom.Compiler.CompilerParameters
        # 2) add members 'System.dll' and 'mscorelib.dll'
        $compileParams.ReferencedAssemblies.AddRange(@('System.dll', 'mscorlib.dll'))
        # 3) tell the compiler to keep the results in memory
        $compileParams.GenerateInMemory = $True
        # 4) invoke the compiler with no output to the console
        Add-Type -TypeDefinition $code -CompilerParameters $compileParams -PassThru -WarningAction SilentlyContinue | Out-Null
   }
    $MZ_OFFSET              = 0X00000000
    $MZ_SIZE                = 0X00000002
    $PE_OFFSET_OFFSET       = 0X0000003C  # contains 4 byte integer with the offsetof the PE header in the image
    $PE_HDR_SIZE            = 0X00000004
    $COFF_HEADER_OFFSET     = 0X00000000  # TODO: need number
    $OFF_MACHINE_ID         = 0X00000000
    $OFF_SECTION_COUNT      = 0X00000002
    $OFF_NUMBER_OF_SECTIONS = 0X00000004
    $OFF_TIME_STAMP         = 0X00000006

    # SIZES OF BLOCKS
    $MACHINE_SIZE           = 0X00000002
    $NUMBER_OF_SECTIONS     = 0X00000002
    # $
    # Read the entire file to an array of bytes.
    # $bytes = [System.IO.File]::ReadAllBytes($FilePath)
    # $bytes = Get-Content -Path $FilePath -Encoding bytes -Raw
    # Decode first 12 bytes to a text assuming ASCII encoding.
    # $text = [System.Text.Encoding]::ASCII.GetString($bytes, 0, 2)

    $OnDisk = $True
    if ($OnDisk) {
    
        # read the file into a "File stream" object
        $FileStream = New-Object System.IO.FileStream($FilePath, [System.IO.FileMode]::Open, [System.IO.FileAccess]::Read)
        # convert the FileStream to a ByteArray
        # Allocate memory for the ByteArray
        $FileByteArray = New-Object Byte[]($FileStream.Length)
        # Read the FileStream into the ByteArray
        $FileStream.Read($FileByteArray, 0, $FileStream.Length) | Out-Null
        $FileStream.Close()
        # get a "Handle" (or GCHandle = Garbage Collected handle) to the byte-array
        $Handle = [System.Runtime.InteropServices.GCHandle]::Alloc($FileByteArray, 'Pinned')
        # DeReference the Handle to a "pointer"
        $PEBaseAddr = $Handle.AddrOfPinnedObject()
        
    } else {
        wirte-Error "Don't support in-memory"
    }

    # here we use the types defined above to access the byte array as structures via the "pointer"
    $DosHeader = [System.Runtime.InteropServices.Marshal]::PtrToStructure($PEBaseAddr, [Type] [PE+_IMAGE_DOS_HEADER])
    # get a pointer to the NT header
    $PointerNtHeader = [IntPtr] ($PEBaseAddr.ToInt64() + $DosHeader.e_lfanew)
    # copy the values into a custom PSObject $NTHeader
    $NtHeader = [System.Runtime.InteropServices.Marshal]::PtrToStructure($PointerNtHeader, [Type] [PE+_IMAGE_NT_HEADERS32])
    # get a string representation of the Machine architecture
    $Architecture = ($NtHeader.FileHeader.Machine).ToString()
    
    # this assumes we are using 32-bit/4-byte pointers
    $BinaryPtrWidth = 4

    # Define relevant structure types depending upon whether the binary is 32 or 64-bit
    if ($Architecture -eq 'AMD64') {
    
        $BinaryPtrWidth = 8

        $PEStruct = @{
            IMAGE_OPTIONAL_HEADER = [PE+_IMAGE_OPTIONAL_HEADER64]
            NT_HEADER = [PE+_IMAGE_NT_HEADERS64]
        }

        $ThunkDataStruct = [PE+_IMAGE_THUNK_DATA64]

        Write-Verbose "Architecture: $Architecture"
        Write-Verbose 'Proceeding with parsing a 64-bit binary.'
        
    } elseif ($Architecture -eq 'I386' -or $Architecture -eq 'ARMNT' -or $Architecture -eq 'THUMB') {
    
        $PEStruct = @{
            IMAGE_OPTIONAL_HEADER = [PE+_IMAGE_OPTIONAL_HEADER32]
            NT_HEADER = [PE+_IMAGE_NT_HEADERS32]
        }

        $ThunkDataStruct = [PE+_IMAGE_THUNK_DATA32]

        Write-Verbose "Architecture: $Architecture"
        Write-Verbose 'Proceeding with parsing a 32-bit binary.'
        
    } else {
    
        Write-Warning 'Get-PEHeader only supports binaries compiled for x86, AMD64, and ARM.'
        return
        
    }
    
    # Need to get a new NT header in case the architecture changed
    $NtHeader = [System.Runtime.InteropServices.Marshal]::PtrToStructure($PointerNtHeader, [Type] $PEStruct['NT_HEADER'])
    # Display all section headers
    $NumSections = $NtHeader.FileHeader.NumberOfSections
    $NumRva = $NtHeader.OptionalHeader.NumberOfRvaAndSizes
    $PointerSectionHeader = [IntPtr] ($PointerNtHeader.ToInt64() + [System.Runtime.InteropServices.Marshal]::SizeOf([Type] $PEStruct['NT_HEADER']))
    $SectionHeaders = New-Object PSObject[]($NumSections)

        $PEFields = @{
            Module         = $ModuleName
            DOSHeader      = $DosHeader
            PESignature    = $NTHeader.Signature
            FileHeader     = $NTHeader.FileHeader
            OptionalHeader = $NTHeader.OptionalHeader
            SectionHeaders = $SectionHeaders
            # I don't care about imports and exports so I will not do this
            # Imports        = Get-Imports
            # Exports        = Get-Exports
        }
    

        if ($Ondisk) {
            $Handle.Free()
        }
        else {
            Write-Error "In-Memory not supported"
        }

        $PEHeader = New-Object PSObject -Property $PEFields
        $PEHeader.PSObject.TypeNames.Insert(0, 'PEHeader')
    
        write-host ( "last optional header {0}" -f $PEFields.OptionalHeader[$NumSections-1])

}
    return $PEHeader
}
