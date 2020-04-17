; Variables
#define MyAppName "Calculate Primes Multithreaded"
#define MyAppVersion "1.1.3"
#define MyAppPublisher "Swerik"
#define MyAppURL "https://github.com/TheSwerik/CalcPrimesMultiThread"   
#define MyAppExeName "CalculatePrimesMultithreaded.exe"

[Setup]
AppId={{320386D8-5081-479D-AA54-FC03D4F6FED2}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL} 
AppVerName={#MyAppName}
DefaultDirName={autopf}\{#MyAppName}   
DefaultGroupName={#MyAppName} 
Compression=lzma2   
SolidCompression=yes   
WizardStyle=modern
OutputBaseFilename={#MyAppName}
OutputDir=Installer
SourceDir=..\       
ArchitecturesInstallIn64BitMode=x64  
AllowNoIcons=yes  
ShowLanguageDialog=auto
CloseApplications=yes
CloseApplicationsFilter=*.*

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked   

[Files]
Source: "Publish\bin\64bit\*"; DestDir: "{app}\bin\64bit"; Excludes:"*.pdb"; Check: Is64BitInstallMode;     Flags: ignoreversion recursesubdirs
Source: "Publish\bin\32bit\*"; DestDir: "{app}\bin\32bit"; Excludes:"*.pdb"; Check: not Is64BitInstallMode; Flags: ignoreversion recursesubdirs solidbreak   

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Flags: createonlyiffileexists;
Name: "{group}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Flags: createonlyiffileexists; 
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Tasks: desktopicon; Flags: createonlyiffileexists;
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Tasks: desktopicon; Flags: createonlyiffileexists; 
Name: "{app}\{#MyAppName}"; Filename: "{app}\bin\64bit\{#MyAppExeName}"; Flags: createonlyiffileexists;
Name: "{app}\{#MyAppName}"; Filename: "{app}\bin\32bit\{#MyAppExeName}"; Flags: createonlyiffileexists; 

[Run] 
Filename: "{app}\bin\64bit\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Check: IsWin64; Flags: nowait postinstall skipifsilent
Filename: "{app}\bin\32bit\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Check: not IsWin64; Flags: nowait postinstall skipifsilent