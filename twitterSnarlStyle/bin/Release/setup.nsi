!include "MUI2.nsh"
!include "checkDotNet3.nsh"
!include "getDotNetDir.nsh"

!define MIN_FRA_MAJOR "3"
!define MIN_FRA_MINOR "5"
!define MIN_FRA_BUILD "*"


; The name of the installer
Name "Snarl Style Tweet it"

; The file to write
OutFile "Setup-SnarlStyleTweetIt.exe"


; The default installation directory
InstallDir "$PROGRAMFILES\Tlhan Ghun\Styles\Tweet it"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\TlhanGhun\Styles\Tweet it" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin


;--------------------------------

  !define MUI_ABORTWARNING

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "tlhanGhun.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "tlhanGhunWelcome.bmp"
!define MUI_WELCOMEPAGE_TITLE "Snarl style Tweet it"
!define MUI_WELCOMEPAGE_TEXT "This style forwards notifications having been sent to Snarl to your Twitter account instead of displaying them locally"
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "Tlhan Ghun\Styles\Tweet it"
!define MUI_ICON "blueBird.ico"
!define MUI_UNICON "uninstall.ico"


Var StartMenuFolder
; Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY

  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\TlhanGhun\Styles\Tweet it" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder

  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH




  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH





;--------------------------------




!insertmacro MUI_LANGUAGE "English"

; LoadLanguageFile "${NSISDIR}\Contrib\Language files\English.nlf"
;--------------------------------
;Version Information

  VIProductVersion "1.0.0.0"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Snarl Style Tweet it"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Tlhan Ghun"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "© 2009 Tlhan Ghun GPL v.3"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Forwards Snarl notifications to Twitter"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "1.1"







Function un.UninstallDirs
    Exch $R0 ;input string
    Exch
    Exch $R1 ;maximum number of dirs to check for
    Push $R2
    Push $R3
    Push $R4
    Push $R5
       IfFileExists "$R0\*.*" 0 +2
       RMDir "$R0"
     StrCpy $R5 0
    top:
     StrCpy $R2 0
     StrLen $R4 $R0
    loop:
     IntOp $R2 $R2 + 1
      StrCpy $R3 $R0 1 -$R2
     StrCmp $R2 $R4 exit
     StrCmp $R3 "\" 0 loop
      StrCpy $R0 $R0 -$R2
       IfFileExists "$R0\*.*" 0 +2
       RMDir "$R0"
     IntOp $R5 $R5 + 1
     StrCmp $R5 $R1 exit top
    exit:
    Pop $R5
    Pop $R4
    Pop $R3
    Pop $R2
    Pop $R1
    Pop $R0
FunctionEnd









; The stuff to install
Section "Snarl style Tweet it files"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  !insertmacro AbortIfBadFramework

  ; Put file there
  File "Changes.txt"
  File "CREDITS.txt"
  File "Documentation.URL"
  File "Dimebrain.TweetSharp.dll"
  File "Dimebrain.TweetSharp.xml"
  File "Interop.libmgraphics.dll"
  File "Interop.libSnarlStyles.dll"
  File "Interop.melon.dll"
  File "Interop.prefs_kit_d2.dll"
  File "Newtonsoft.Json.dll"
  File "LICENSE.txt"
  File "Documentation.ico"
  File "twitterSnarlStyle.dll"
  File "twitterSnarlStyle.dll.config"
  File "twitterSnarlStyle.pdb"
  File "uninstall.ico"
  File "blueBird.png"
  
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\TlhanGhun\SnarlStleTweetIt "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SnarlStleTweetIt" "DisplayName" "Snarl Style Tweet it"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SnarlStleTweetIt" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SnarlStleTweetIt" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SnarlStleTweetIt" "NoRepair" 1
  WriteUninstaller "uninstall.exe"

; Register the assembly

push "v2.0"
call GetDotNetDir
pop $R0
GetFullPathName /SHORT $1 $R0
GetFullPathName /SHORT $2 $INSTDIR
StrCpy $0 '"$1\RegAsm.exe" "$2\twitterSnarlStyle.dll" /codebase'  
DetailPrint $0
nsExec::ExecToLog $0
pop $0


  
SectionEnd


Section "Register style with Snarl"
  SetOutPath "$APPDATA\full phat\snarl\styles"
  
  File "twitterSnarlStyle.styleengine"

SectionEnd


; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

!insertmacro MUI_STARTMENU_WRITE_BEGIN Application

  CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
  CreateShortCut "$SMPROGRAMS\$StartMenuFolder\\Documentation.lnk" "$INSTDIR\Documentation.URL" "" $INSTDIR\Documentation.ico" 0
  CreateShortCut "$SMPROGRAMS\$StartMenuFolder\\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  
!insertmacro MUI_STARTMENU_WRITE_END

  
SectionEnd


;--------------------------------

; Uninstaller

Section "Uninstall"

  ; UNregister the assembly
  push "v2.0"
call un.GetDotNetDir
pop $R0
GetFullPathName /SHORT $1 $R0
GetFullPathName /SHORT $2 $INSTDIR
StrCpy $0 '"$1\RegAsm.exe" "$2\twitterSnarlStyle.dll" /unregister'
DetailPrint $0
nsExec::ExecToLog $0
pop $0

  
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\SnarlStleTweetIt"
  DeleteRegKey HKLM "Software\TlhanGhun\Styles\Tweet it"
  ; Remove files and uninstaller
  Delete $INSTDIR\*.*

  ; Remove shortcuts, if any
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
    
  Delete "$SMPROGRAMS\$StartMenuFolder\\*.*"
  


  DeleteRegKey HKCU "Software\TlhanGhun\Styles\Tweet it"


  ; Remove directories used
   ; RMDir "$SMPROGRAMS\$StartMenuFolder"
Push 10 #maximum amount of directories to remove
  Push "$SMPROGRAMS\$StartMenuFolder" #input string
    Call un.UninstallDirs

   
  ; RMDir "$INSTDIR"
  
  Push 10 #maximum amount of directories to remove
  Push $INSTDIR #input string
    Call un.UninstallDirs


  
  Delete "$APPDATA\full phat\snarl\styles\twitterSnarlStyle.styleengine"

SectionEnd
