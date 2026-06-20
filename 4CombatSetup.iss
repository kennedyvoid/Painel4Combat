#define MyAppName "4Combat"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "4Combat"
#define MyAppExeName "CombatScore.UI.exe"

[Setup]
AppId={{A8D2B84B-7C55-4FC9-BB87-4COMBAT001}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}Combat
DefaultGroupName=4Combat
OutputDir=installer-output
OutputBaseFilename=4CombatSetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
DisableProgramGroupPage=yes
UninstallDisplayName=4Combat
PrivilegesRequired=admin

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Atalhos:"; Flags: checkedonce

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\4Combat"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\4Combat"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir 4Combat"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
