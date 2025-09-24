#define MyAppName "深海搁浅汉化补丁"
#define MyAppVersion "1.0.38.0.29-Steam版"
#define Sponsor "我想赞助汉化组"
#define File_name "汉化补丁v1.0.38.0.29-Steam版"
#define Output_Dir ".\File"
#define License_File ".\license.txt"
#define Icon_File ".\Stranded_Deep.ico"
#define text1 "深海搁浅游戏"

[Dirs]
Name: {app}; Permissions: users-full

[Setup]
AppId={{863C2D97-AC02-4366-B85D-F5339BA9A0D5}
UsePreviousAppDir=no
AppName={#MyAppName}
SetupIconFile={#Icon_File}
AppVersion={#MyAppVersion}
DefaultDirName={src}
SetupMutex=SetupMutex{#SetupSetting("AppId")}
PrivilegesRequired=lowest
LicenseFile={#License_File}
OutputDir={#Output_Dir}
OutputBaseFilename={#File_name}
DisableDirPage=yes
DisableProgramGroupPage=yes
Compression=zip
SolidCompression=yes
WizardStyle=modern
Uninstallable=no

[Languages]
Name: "chinesesimplified"; MessagesFile: "compiler:Script\ChineseSimplified.isl"

[Files]
Source: ".\Software\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: ".\Software\Stranded_Deep_x64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Software\doorstop_config.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Software\winhttp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\Software\关于赞助.jpg"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autodesktop}\{#text1}"; Filename: "{app}\Stranded_Deep_x64.exe";

[Messages]
SetupAppRunningError=禁止多次启动本程序!

[Code]
procedure CreateCustomMessageForm;
var
  CustomForm: TSetupForm;
  OkButton: TButton;
  MsgLabel1, MsgLabel2, MsgLabel3: TNewStaticText;
begin
  CustomForm := CreateCustomForm();
  try
    // 进一步缩小窗口尺寸
    CustomForm.ClientWidth := ScaleX(250);  // 更小的宽度
    CustomForm.ClientHeight := ScaleY(120); // 更小的高度
    CustomForm.Caption := '软件错误提示';
    CustomForm.BorderStyle := bsDialog;
    CustomForm.Position := poScreenCenter;
    
    // 第一行文字
    MsgLabel1 := TNewStaticText.Create(CustomForm);
    MsgLabel1.Parent := CustomForm;
    MsgLabel1.Caption := '未找到游戏安装目录,';
    MsgLabel1.Font.Size := 15;  // 更小的字体
    MsgLabel1.Font.Color := clRed;
    MsgLabel1.Font.Style := [fsBold];
    MsgLabel1.AutoSize := True;
    MsgLabel1.Left := (CustomForm.ClientWidth - MsgLabel1.Width) div 2;
    MsgLabel1.Top := ScaleY(15);
    
    // 第二行文字
    MsgLabel2 := TNewStaticText.Create(CustomForm);
    MsgLabel2.Parent := CustomForm;
    MsgLabel2.Caption := '请确认本软件在';
    MsgLabel2.Font.Size := 14;
    MsgLabel2.Font.Color := clRed;
    MsgLabel2.Font.Style := [fsBold];
    MsgLabel2.AutoSize := True;
    MsgLabel2.Left := (CustomForm.ClientWidth - MsgLabel2.Width) div 2;
    MsgLabel2.Top := ScaleY(35);
    
    // 第三行文字
    MsgLabel3 := TNewStaticText.Create(CustomForm);
    MsgLabel3.Parent := CustomForm;
    MsgLabel3.Caption := '游戏安装目录下运行!';
    MsgLabel3.Font.Size := 16;
    MsgLabel3.Font.Color := clRed;
    MsgLabel3.Font.Style := [fsBold];
    MsgLabel3.AutoSize := True;
    MsgLabel3.Left := (CustomForm.ClientWidth - MsgLabel3.Width) div 2;
    MsgLabel3.Top := ScaleY(55);
    
    // 确定按钮
    OkButton := TButton.Create(CustomForm);
    OkButton.Parent := CustomForm;
    OkButton.Width := ScaleX(60);  // 更小的按钮
    OkButton.Height := ScaleY(22);
    OkButton.Left := (CustomForm.ClientWidth - OkButton.Width) div 2;
    OkButton.Top := CustomForm.ClientHeight - ScaleY(30);
    OkButton.Caption := '确定';
    OkButton.ModalResult := mrOk;
    OkButton.Default := True;
    
    CustomForm.ActiveControl := OkButton;
    
    // 显示窗体
    CustomForm.ShowModal();
  finally
    CustomForm.Free();
  end;
end;

procedure InitializeWizard();
begin
  WizardForm.LicenseMemo.Font.Size := 11;
  WizardForm.LicenseMemo.Height := WizardForm.LicenseMemo.Height + ScaleY(20);
  WizardForm.LicenseAcceptedRadio.Top := WizardForm.LicenseMemo.Top + WizardForm.LicenseMemo.Height + ScaleY(10);
  WizardForm.LicenseNotAcceptedRadio.Top := WizardForm.LicenseAcceptedRadio.Top + ScaleY(20);
  WizardForm.LicenseAcceptedRadio.Checked := True;
end;

function InitializeSetup(): Boolean;
var
  ExePath: string;
begin
  // 获取用户的启动程序目录
  ExePath := ExpandConstant('.\Stranded_Deep.exe');
  
  // 如果文件存在，则进行安装
  if FileExists(ExePath) then
  begin
    Result := True;
  end
  else
  begin
    // 使用自定义消息框
    CreateCustomMessageForm();
    Result := False;
  end;
end;


[Run]
Filename: "{app}\关于赞助.jpg"; Description: "{#Sponsor}"; Flags: postinstall shellexec skipifsilent
Filename: "{app}\Stranded_Deep_x64.exe"; Description: "{cm:LaunchProgram,{#StringChange(text1,'&','&&')}}"; Flags: nowait postinstall skipifsilent
