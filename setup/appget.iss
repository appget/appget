; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!
#pragma include __INCLUDE__ + ";" + "setup\idp\"
#include <idp.iss>

#define AppName "AppGet"
#define AppPublisher "AppGet"
#define AppURL "https://appget.net/"
#define SupportURL "https://github.com/appget/appget/issues"
#define UpdatesURL "https://github.com/appget/appget/releases"
#define Copyright "Copyright 2019"
#define BuildNumber GetEnv('BUILD_BUILDNUMBER')
#define OutputDir GetEnv('BUILD_ARTIFACTSTAGINGDIRECTORY')

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{d9fda649-7b07-4197-a96b-5dc01ea10035}
AppName={#AppName}
AppVersion={#BuildNumber}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#SupportURL}
AppUpdatesURL={#UpdatesURL}
VersionInfoDescription='AppGet Installer'
AppCopyright={#Copyright}
DefaultDirName={commonappdata}\{#AppName}\bin
DisableDirPage=yes
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputBaseFilename=appget.{#BuildNumber}
SolidCompression=no
AllowUNCPath=no
UninstallDisplayIcon={app}\appget.exe
DisableReadyPage=yes
CompressionThreads=2
Compression=lzma2/normal
AppContact={#SupportURL}
VersionInfoVersion={#BuildNumber}
UninstallDisplayName={#AppName}
ChangesEnvironment=yes
OutputDir={#OutputDir}

;Windows 7 SP1
MinVersion=6.1sp1

PrivilegesRequired=lowest

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[CustomMessages]
IDP_FormCaption           =Downloading Microsoft .NET Framework
IDP_FormDescription       =Please wait while Setup downloads Microsoft .NET Framework...

[Files]
Source: "..\src\AppGet.Gui\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKCU; Subkey:"Environment"; ValueType:expandsz; ValueName:"Path"; ValueData:"{olddata};{app}"; Check:NeedsAddPath('{app}')

Root: HKCU; Subkey: "SOFTWARE\Classes\appget"; ValueType: "string"; ValueData: "URL:AppGet Protocol"; Flags: uninsdeletekey
Root: HKCU; Subkey: "SOFTWARE\Classes\appget"; ValueType: "string"; ValueName: "URL Protocol"; ValueData: ""
Root: HKCU; Subkey: "SOFTWARE\Classes\appget\DefaultIcon"; ValueType: "string"; ValueData: "{app}\appget.gui.exe,2"
Root: HKCU; Subkey: "SOFTWARE\Classes\appget\shell\open\command"; ValueType: "string"; ValueData: """{app}\appget.gui.exe"" ""%1"""

[Code]
function NeedsAddPath(Param: string): boolean;
var
  OrigPath: string;
begin
  if not RegQueryStringValue(HKEY_CURRENT_USER,'Environment', 'Path', OrigPath)
  then begin
    Result := True;
    exit;
  end;
  // look for the path with leading and trailing semicolon
  // Pos() returns 0 if not found
  Result := Pos(';' + Param + ';', ';' + OrigPath + ';') = 0;
end;


function DotNetInstallerExe(): string;
begin
  Result:= ExpandConstant('{tmp}\NetFrameworkInstaller.exe')
end;

function Framework45IsNotInstalled(): Boolean;
var
  bSuccess: Boolean;
  regVersion: Cardinal;
begin
  Result := True;

  bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);
  if (True = bSuccess) and (regVersion >= 394254) then begin
    Result := False;
  end;
end;

procedure InitializeWizard;
begin
  if Framework45IsNotInstalled() then
  begin
    idpAddFile('https://go.microsoft.com/fwlink/?linkid=2088631', DotNetInstallerExe());
    idpDownloadAfter(wpReady);
  end;
end;

procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Upgrading .NET Framework. This might take a few minutes...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    if not ShellExec('', DotNetInstallerExe(), '/passive /norestart /showrmui /showfinalerror', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin
      MsgBox('.NET framework installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
    DeleteFile(DotNetInstallerExe());
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  case CurStep of
    ssPostInstall:
      begin
        if Framework45IsNotInstalled() then
        begin
          InstallFramework();
        end;
      end;
  end;
end;
